using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Management;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Linq;

namespace NumKeyboardTray
{
    public partial class Form1 : Form
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern uint RegisterWindowMessage(string lpString);
        private uint wmTaskbarCreated; // 用于监听系统任务栏是否刷新
        private bool forceEnabled = false;
        private static IntPtr _hookID = IntPtr.Zero;
        private static LowLevelKeyboardProc _proc = HookCallback;
        private static DateTime lastNumPad0Time = DateTime.MinValue;
        private static int repeatCount = 0;
        private static bool isProgramShiftPressed = false;
        private static bool isProgramCtrlPressed = false;
        private Timer monitorTimer;
        private NotifyIcon notifyIcon;
        private static bool isAltPressed = false;
        private static bool isVoiceInputPressed = false;
        private static Timer altReleaseTimer;
        private static readonly string dataFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data");
        private static readonly string configFile = Path.Combine(dataFolder, "numkeyboard.txt");
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYUP = 0x0101;
        private const uint KEYEVENTF_KEYUP = 0x0002;
        private const uint SPI_GETMOUSEKEYS = 0x0036;
        private const uint SPI_SETMOUSEKEYS = 0x0037;
        private const uint SPIF_SENDCHANGE = 0x0002;
        private const int WM_HOTKEY = 0x0312;
        private const int HOTKEY_ID_BROWSER_HOME = 1001;
        private const int HOTKEY_ID_MAIL = 1002;
        private const int HOTKEY_ID_CALC = 1003;
        private const int VK_BROWSER_HOME = 0xAC;
        private const int VK_LAUNCH_MAIL = 0xB4;
        private const int VK_LAUNCH_APP2 = 0xB7;
        private const int HOTKEY_ID_FORCE_DISCONNECT = 1004;
        private const int VK_C = 0x43;
        private const uint MOD_CONTROL_ALT = 0x0003;
        private bool isForceDisconnectMode = false;

        // 【新增修改】悬浮图片窗体变量
        private ImageTooltipForm imgTooltip;

        // 【原有】控制静默启动的两个变量
        private bool isBackgroundStart = false;
        private bool isFirstShow = true;

        public Form1()
        {
            InitializeComponent();
            InitializeNotifyIcon();

            // 检查快捷方式或开机自启是否带了后台参数
            string[] args = Environment.GetCommandLineArgs();
            if (args.Contains("-background"))
            {
                isBackgroundStart = true;
            }

            this.Hide();
            this.Text = "小白T9键盘驱动";
        }

        // 重写此方法。完美的静默后台方案
        protected override void SetVisibleCore(bool value)
        {
            if (isFirstShow && isBackgroundStart)
            {
                isFirstShow = false;
                base.SetVisibleCore(false); // 第一次原生显示请求直接拦截，保持隐形
            }
            else
            {
                base.SetVisibleCore(value); // 后续用户双击托盘时，正常放行显示
            }
        }

        #region 全局消息处理
        protected override void WndProc(ref Message m)
        {
            // 解决开机时系统托盘还没准备好，导致图标丢失的问题
            if (m.Msg == wmTaskbarCreated)
            {
                if (notifyIcon != null && this.Visible)
                {
                    notifyIcon.Visible = false;
                    notifyIcon.Visible = true; // 只在窗口可见时刷新托盘图标
                }
            }

            if (m.Msg == WM_HOTKEY)
            {
                int id = m.WParam.ToInt32();
                string keyName = "";
                if (id == HOTKEY_ID_BROWSER_HOME) keyName = "BrowserHomeKey";
                else if (id == HOTKEY_ID_MAIL) keyName = "LaunchMailKey";
                else if (id == HOTKEY_ID_CALC) keyName = "LaunchApp2Key";
                else if (id == HOTKEY_ID_FORCE_DISCONNECT) { ToggleForceDisconnectMode(); m.Result = (IntPtr)1; return; }
                if (!string.IsNullOrEmpty(keyName))
                {
                    string action = GetKeyMapping(keyName);
                    if (action != "None")
                    {
                        HandleCustomKeyByType(keyName);
                    }
                    m.Result = (IntPtr)1;
                    return;
                }
            }

            const int WM_SHOWME = 0x8001;
            if (m.Msg == WM_SHOWME) { RestoreWindow(); }
            base.WndProc(ref m);
        }


































        #endregion

        private void RegisterMediaHotkeys()
        {
            UnregisterAllHotkeys();
            if (GetKeyMapping("BrowserHomeKey") != "None") RegisterHotKey(this.Handle, HOTKEY_ID_BROWSER_HOME, 0, VK_BROWSER_HOME);
            if (GetKeyMapping("LaunchMailKey") != "None") RegisterHotKey(this.Handle, HOTKEY_ID_MAIL, 0, VK_LAUNCH_MAIL);
            if (GetKeyMapping("LaunchApp2Key") != "None") RegisterHotKey(this.Handle, HOTKEY_ID_CALC, 0, VK_LAUNCH_APP2);
        }

        private void UnregisterAllHotkeys()
        {
            UnregisterHotKey(this.Handle, HOTKEY_ID_BROWSER_HOME);
            UnregisterHotKey(this.Handle, HOTKEY_ID_MAIL);
            UnregisterHotKey(this.Handle, HOTKEY_ID_CALC);
            UnregisterHotKey(this.Handle, HOTKEY_ID_FORCE_DISCONNECT);
        }

        public void RestoreWindow()
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.ShowInTaskbar = true; // 允许在任务栏显示
            this.BringToFront();
            this.Activate();
            if (notifyIcon != null) notifyIcon.Visible = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Microsoft.Win32.Registry.SetValue(@"HKEY_CURRENT_USER\Control Panel\Desktop", "LowLevelHooksTimeout", 5000, Microsoft.Win32.RegistryValueKind.DWord);
            this.FormClosing += Form1_FormClosing;
            altReleaseTimer = new Timer();

            // 【修改】将原本的 MouseHover 改为 MouseEnter 绑定，消除1秒的悬停延迟
            label6.MouseEnter += Label6_MouseEnter;
            label6.MouseLeave += Label6_MouseLeave;

            // 监听 Windows 任务栏创建消息
            wmTaskbarCreated = RegisterWindowMessage("TaskbarCreated");

            // 如果检测到当前没有开启自启，则默认帮他勾上并写入注册表
            if (!IsAutoRunEnabled())
            {
                SetAutoRun(true);
            }

            checkBox1.Checked = IsAutoRunEnabled();
            checkBox1.CheckedChanged += checkBox1_CheckedChanged;

            string[] items = { "None", "ESC", "Shift", "Tab", "Ctrl", "0", ".", "复制", "粘贴", "剪切", "全选", "撤回", "退格", "空格", "逗号", "句号", "打开我的电脑", "打开计算器", "打开浏览器主页", "打开邮件", "切换输入法", "微信截图", "切换鼠标键", "语音输入" };
            comboBoxBrowserHome.Items.AddRange(items);
            comboBoxTab.Items.AddRange(items);
            comboBoxMail.Items.AddRange(items);
            comboBoxApp2.Items.AddRange(items);
            comboBoxCtrl.Items.AddRange(items);
            comboBox0.Items.AddRange(items);
            comboBoxDot.Items.AddRange(items);
            comboBoxBrowserHome.SelectedItem = LoadKeyMapping("BrowserHomeKey");
            comboBoxTab.SelectedItem = LoadKeyMapping("TabKey");
            comboBoxMail.SelectedItem = LoadKeyMapping("LaunchMailKey");
            comboBoxApp2.SelectedItem = LoadKeyMapping("LaunchApp2Key");
            comboBoxCtrl.SelectedItem = LoadKeyMapping("Single0Key");
            comboBox0.SelectedItem = LoadKeyMapping("Triple0Key");
            comboBoxDot.SelectedItem = LoadKeyMapping("DotKey");
            CleanupLegacyNumlockKey();
            comboBoxBrowserHome.SelectedIndexChanged += (s, e2) => { if (!CheckAndLaunchT9s2t(comboBoxBrowserHome, "BrowserHomeKey")) return; SaveKeyMapping("BrowserHomeKey", comboBoxBrowserHome.SelectedItem?.ToString() ?? "None"); RegisterMediaHotkeys(); };
            comboBoxTab.SelectedIndexChanged += (s, e2) => { if (!CheckAndLaunchT9s2t(comboBoxTab, "TabKey")) return; SaveKeyMapping("TabKey", comboBoxTab.SelectedItem?.ToString() ?? "None"); };
            comboBoxMail.SelectedIndexChanged += (s, e2) => { if (!CheckAndLaunchT9s2t(comboBoxMail, "LaunchMailKey")) return; SaveKeyMapping("LaunchMailKey", comboBoxMail.SelectedItem?.ToString() ?? "None"); RegisterMediaHotkeys(); };
            comboBoxApp2.SelectedIndexChanged += (s, e2) => { if (!CheckAndLaunchT9s2t(comboBoxApp2, "LaunchApp2Key")) return; SaveKeyMapping("LaunchApp2Key", comboBoxApp2.SelectedItem?.ToString() ?? "None"); RegisterMediaHotkeys(); };
            comboBoxCtrl.SelectedIndexChanged += (s, e2) => { if (!CheckAndLaunchT9s2t(comboBoxCtrl, "Single0Key")) return; SaveKeyMapping("Single0Key", comboBoxCtrl.SelectedItem?.ToString() ?? "None"); };
            comboBox0.SelectedIndexChanged += (s, e2) => { if (!CheckAndLaunchT9s2t(comboBox0, "Triple0Key")) return; SaveKeyMapping("Triple0Key", comboBox0.SelectedItem?.ToString() ?? "None"); };
            comboBoxDot.SelectedIndexChanged += (s, e2) => { if (!CheckAndLaunchT9s2t(comboBoxDot, "DotKey")) return; SaveKeyMapping("DotKey", comboBoxDot.SelectedItem?.ToString() ?? "None"); };
            RegisterMediaHotkeys();
            RegisterHotKey(this.Handle, HOTKEY_ID_FORCE_DISCONNECT, MOD_CONTROL_ALT, VK_C);

            // 启动时：如果有按键配置为"语音输入"，检测 t9s2t 是否在运行
            CheckVoiceInputOnStartup();

            monitorTimer = new Timer { Interval = 3000 };
            monitorTimer.Tick += (s, ev) => CheckKeyboardAndHook();
            monitorTimer.Start();

            // 核心功能启动完毕
            CheckKeyboardAndHook();

            // 让界面启动时直接缩回托盘，不占任务栏
            this.WindowState = FormWindowState.Minimized;
            this.ShowInTaskbar = false;


            this.BeginInvoke(new Action(() => { this.Hide(); }));








        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing) { e.Cancel = true; this.Hide(); if (notifyIcon != null) notifyIcon.Visible = false; return; }
            UnregisterAllHotkeys();
            if (isProgramShiftPressed) keybd_event((byte)Keys.ShiftKey, 0, KEYEVENTF_KEYUP, IntPtr.Zero);
            if (isProgramCtrlPressed) keybd_event((byte)Keys.ControlKey, 0, KEYEVENTF_KEYUP, IntPtr.Zero);
            if (isVoiceInputPressed)
            {
                keybd_event(0x44, 0, KEYEVENTF_KEYUP, IntPtr.Zero);
                keybd_event((byte)Keys.Menu, 0, KEYEVENTF_KEYUP, IntPtr.Zero);
                keybd_event((byte)Keys.ControlKey, 0, KEYEVENTF_KEYUP, IntPtr.Zero);
                isVoiceInputPressed = false;
            }
            if (_hookID != IntPtr.Zero) UnhookWindowsHookEx(_hookID);
            monitorTimer?.Stop();
            notifyIcon?.Dispose();
        }

        #region 托盘和自启
        private void InitializeNotifyIcon()
        {
            notifyIcon = new NotifyIcon { Icon = numkeyboard.Properties.Resources.xiaobait9, Visible = false, Text = "小白T9键盘驱动 - 未检测到键盘" };
            var menu = new ContextMenuStrip();
            menu.Items.Add("打开设置", null, (s, e) => RestoreWindow());
            menu.Items.Add("退出", null, (s, e) => Application.Exit());
            notifyIcon.ContextMenuStrip = menu;
            notifyIcon.DoubleClick += (s, e) => RestoreWindow();
        }
        private void ShowStatus(string status)
        {
            if (notifyIcon != null)
            {
                notifyIcon.Text = status; notifyIcon.BalloonTipTitle = "小白T9键盘驱动"; notifyIcon.BalloonTipText = status; notifyIcon.ShowBalloonTip(2000);
            }
        }
        private bool IsAutoRunEnabled() { using (var key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", false)) return key?.GetValue("NumKeyboardTray") != null; }
        private void SetAutoRun(bool enable) { using (var key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true)) { if (enable) key.SetValue("NumKeyboardTray", Application.ExecutablePath); else key.DeleteValue("NumKeyboardTray", false); } }
        private void checkBox1_CheckedChanged(object sender, EventArgs e) { SetAutoRun(checkBox1.Checked); }
        #endregion

        #region 小键盘检测
        private void CheckKeyboardAndHook()
        {
            // 强制断开模式下不安装钩子
            if (isForceDisconnectMode)
            {
                if (_hookID != IntPtr.Zero) { UnhookWindowsHookEx(_hookID); _hookID = IntPtr.Zero; }
                return;
            }
            if (IsKeyboardInsertedByVIDPID()) { if (_hookID == IntPtr.Zero) { _hookID = SetHook(_proc); ShowStatus("小白T9键盘已插入！"); } }
            else { if (_hookID != IntPtr.Zero) { UnhookWindowsHookEx(_hookID); _hookID = IntPtr.Zero; ShowStatus("未检测到键盘！"); } }
        }
        private bool IsKeyboardInsertedByVIDPID()
        {
            string vid = "32C2"; string pid = "0012";
            try
            {
                var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PnPEntity");
                foreach (ManagementObject device in searcher.Get())
                {
                    string id = device["PNPDeviceID"]?.ToString() ?? "";
                    if (id.Contains("VID_" + vid) && id.Contains("PID_" + pid)) return true;
                }
            }
            catch { }
            return false;
        }
        /// <summary>
        /// 切换强制断开小键盘检测模式（Ctrl+Alt+C）
        /// </summary>
        private void ToggleForceDisconnectMode()
        {
            isForceDisconnectMode = !isForceDisconnectMode;
            if (isForceDisconnectMode)
            {
                if (_hookID != IntPtr.Zero) { UnhookWindowsHookEx(_hookID); _hookID = IntPtr.Zero; }
                monitorTimer.Stop();
                ShowStatus("【强制断开模式】小键盘改键已禁用");
                MessageBox.Show("已进入强制断开小键盘检测模式！\n\n在此模式下，即使连接了无线小键盘，也不会进行任何按键映射/改键操作。\n\n再次按下 Ctrl+Alt+C 可恢复正常改键功能。", "强制断开模式", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                monitorTimer.Start();
                CheckKeyboardAndHook();
                ShowStatus("【正常模式】小键盘改键已恢复");
                MessageBox.Show("已退出强制断开模式！\n\n小键盘改键功能已恢复正常。", "恢复正常模式", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        #endregion

        #region 键盘钩子
        private static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule) return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
        }
        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);
        [StructLayout(LayoutKind.Sequential)] private struct KBDLLHOOKSTRUCT { public uint vkCode; public uint scanCode; public uint flags; public uint time; public IntPtr dwExtraInfo; }
        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                KBDLLHOOKSTRUCT kbData = (KBDLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(KBDLLHOOKSTRUCT));
                if ((kbData.flags & 0x10) != 0) return CallNextHookEx(_hookID, nCode, wParam, lParam);
                uint vkCode = kbData.vkCode;
                if (vkCode == (int)Keys.NumPad0)
                {
                    if (wParam == (IntPtr)WM_KEYDOWN)
                    {
                        var now = DateTime.Now;
                        if ((now - lastNumPad0Time).TotalMilliseconds < 50) repeatCount++; else repeatCount = 1;
                        lastNumPad0Time = now;
                        if (repeatCount == 3) { HandleCustomKeyByType("Triple0Key"); repeatCount = 0; return (IntPtr)1; }
                        else if (repeatCount == 1)
                        {
                            string action = GetKeyMapping("Single0Key");
                            if (action == "Ctrl" && !isProgramCtrlPressed) { keybd_event((byte)Keys.ControlKey, 0, 0, IntPtr.Zero); isProgramCtrlPressed = true; }
                            else if (action == "语音输入" && !isVoiceInputPressed) { keybd_event((byte)Keys.ControlKey, 0, 0, IntPtr.Zero); keybd_event((byte)Keys.Menu, 0, 0, IntPtr.Zero); keybd_event(0x44, 0, 0, IntPtr.Zero); isVoiceInputPressed = true; }
                        }
                        return (IntPtr)1;
                    }
                    else if (wParam == (IntPtr)WM_KEYUP)
                    {
                        if (repeatCount < 3)
                        {
                            string action = GetKeyMapping("Single0Key");
                            if (action == "Ctrl" && isProgramCtrlPressed) { keybd_event((byte)Keys.ControlKey, 0, KEYEVENTF_KEYUP, IntPtr.Zero); isProgramCtrlPressed = false; }
                            else if (action == "语音输入" && isVoiceInputPressed) { keybd_event(0x44, 0, KEYEVENTF_KEYUP, IntPtr.Zero); keybd_event((byte)Keys.Menu, 0, KEYEVENTF_KEYUP, IntPtr.Zero); keybd_event((byte)Keys.ControlKey, 0, KEYEVENTF_KEYUP, IntPtr.Zero); isVoiceInputPressed = false; }
                            else if (action != "Ctrl" && action != "Shift" && action != "语音输入" && action != "None" && repeatCount == 1) HandleCustomKeyByType("Single0Key");
                        }
                        if ((DateTime.Now - lastNumPad0Time).TotalMilliseconds >= 50) repeatCount = 0;
                        return (IntPtr)1;
                    }
                }
                if (wParam == (IntPtr)WM_KEYDOWN || wParam == (IntPtr)WM_KEYUP)
                {
                    string keyName = "";
                    if (vkCode == (int)Keys.Tab) keyName = "TabKey";
                    else if (vkCode == (int)Keys.Decimal) keyName = "DotKey";
                    if (!string.IsNullOrEmpty(keyName))
                    {
                        string action = GetKeyMapping(keyName);
                        if (action == "Shift" || action == "Ctrl" || action == "语音输入")
                        {
                            bool isDown = wParam == (IntPtr)WM_KEYDOWN;
                            if (action == "Shift")
                            {
                                if (isDown && !isProgramShiftPressed) { keybd_event((byte)Keys.ShiftKey, 0, 0, IntPtr.Zero); isProgramShiftPressed = true; }
                                else if (!isDown && isProgramShiftPressed) { keybd_event((byte)Keys.ShiftKey, 0, KEYEVENTF_KEYUP, IntPtr.Zero); isProgramShiftPressed = false; }
                            }
                            else if (action == "Ctrl")
                            {
                                if (isDown && !isProgramCtrlPressed) { keybd_event((byte)Keys.ControlKey, 0, 0, IntPtr.Zero); isProgramCtrlPressed = true; }
                                else if (!isDown && isProgramCtrlPressed) { keybd_event((byte)Keys.ControlKey, 0, KEYEVENTF_KEYUP, IntPtr.Zero); isProgramCtrlPressed = false; }
                            }
                            else if (action == "语音输入")
                            {
                                if (isDown && !isVoiceInputPressed)
                                {
                                    keybd_event((byte)Keys.ControlKey, 0, 0, IntPtr.Zero);
                                    keybd_event((byte)Keys.Menu, 0, 0, IntPtr.Zero);      // Alt
                                    keybd_event(0x44, 0, 0, IntPtr.Zero);                 // D 键
                                    isVoiceInputPressed = true;
                                }
                                else if (!isDown && isVoiceInputPressed)
                                {
                                    keybd_event(0x44, 0, KEYEVENTF_KEYUP, IntPtr.Zero);               // D up
                                    keybd_event((byte)Keys.Menu, 0, KEYEVENTF_KEYUP, IntPtr.Zero);    // Alt up
                                    keybd_event((byte)Keys.ControlKey, 0, KEYEVENTF_KEYUP, IntPtr.Zero); // Ctrl up
                                    isVoiceInputPressed = false;
                                }
                            }
                            return (IntPtr)1;
                        }
                        if (wParam == (IntPtr)WM_KEYDOWN) HandleCustomKeyByType(keyName);
                        return (IntPtr)1;
                    }
                }
            }
            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }
        #endregion

        #region 映射处理
        private void SaveKeyMapping(string keyName, string value)
        {
            if (!Directory.Exists(dataFolder)) Directory.CreateDirectory(dataFolder);
            var lines = File.Exists(configFile) ? File.ReadAllLines(configFile).ToList() : new List<string>();
            int idx = lines.FindIndex(l => l.StartsWith(keyName + "="));
            if (idx != -1) lines[idx] = keyName + "=" + value; else lines.Add(keyName + "=" + value);
            File.WriteAllLines(configFile, lines);
        }
        private string LoadKeyMapping(string keyName)
        {
            if (!File.Exists(configFile)) return "None";
            var line = File.ReadAllLines(configFile).FirstOrDefault(l => l.StartsWith(keyName + "="));
            return line?.Substring((keyName + "=").Length) ?? "None";
        }
        private static string GetKeyMapping(string keyName)
        {
            if (!File.Exists(configFile)) return "None";
            var line = File.ReadAllLines(configFile).FirstOrDefault(l => l.StartsWith(keyName + "="));
            return line?.Substring((keyName + "=").Length) ?? "None";
        }
        [StructLayout(LayoutKind.Sequential)] private struct MOUSEKEYS { public uint cbSize; public uint dwFlags; public uint iMaxSpeed; public uint iTimeToMaxSpeed; public uint iCtrlSpeed; public uint dwReserved1; public uint dwReserved2; }
        [DllImport("user32.dll")] private static extern bool SystemParametersInfo(uint uiAction, uint uiParam, ref MOUSEKEYS pvParam, uint fWinIni);
        private void ToggleMouseKeys()
        {
            MOUSEKEYS mk = new MOUSEKEYS { cbSize = (uint)Marshal.SizeOf(typeof(MOUSEKEYS)) };
            if (SystemParametersInfo(SPI_GETMOUSEKEYS, mk.cbSize, ref mk, 0))
            {
                if ((mk.dwFlags & 0x1) != 0) mk.dwFlags &= ~0x1U; else mk.dwFlags |= 0x1U | 0x2U;
                SystemParametersInfo(SPI_SETMOUSEKEYS, mk.cbSize, ref mk, SPIF_SENDCHANGE);
                ShowStatus("鼠标键状态已切换");
            }
        }
        private void CleanupLegacyNumlockKey()
        {
            if (!File.Exists(configFile)) return;
            var lines = File.ReadAllLines(configFile).ToList();
            if (lines.RemoveAll(l => l.StartsWith("NumlockKey=") && !l.EndsWith("=None")) > 0) File.WriteAllLines(configFile, lines);
        }

        /// <summary>
        /// 启动时检测：如果配置文件中有任何按键设为"语音输入"，
        /// 检查 t9s2t.exe 是否在运行，未运行则提示用户。
        /// </summary>
        private void CheckVoiceInputOnStartup()
        {
            // 检查所有可配置按键中是否有"语音输入"
            string[] allKeys = { "BrowserHomeKey", "TabKey", "LaunchMailKey", "LaunchApp2Key", "Single0Key", "Triple0Key", "DotKey" };
            bool hasVoiceInput = false;
            foreach (string key in allKeys)
            {
                if (LoadKeyMapping(key) == "语音输入")
                {
                    hasVoiceInput = true;
                    break;
                }
            }
            if (!hasVoiceInput) return;  // 没有按键设为语音输入，跳过检测

            // 检测 t9s2t.exe 是否在运行
            if (Process.GetProcessesByName("t9s2t").Length > 0) return;

            // 未运行，询问用户是否启动
            string exePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "t9s2t", "t9s2t.exe");
            DialogResult result = MessageBox.Show(
                "检测到有按键设置为\"语音输入\"，但 t9s2t.exe 未在运行。\n\n是否立即启动 t9s2t.exe？",
                "语音输入提示",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                if (File.Exists(exePath))
                {
                    try
                    {
                        Process.Start(new ProcessStartInfo
                        {
                            FileName = exePath,
                            WorkingDirectory = Path.GetDirectoryName(exePath),
                            UseShellExecute = true
                        });
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("启动 t9s2t.exe 失败：" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show(
                        "未找到文件：" + exePath + "\n\n请确认 t9s2t 文件夹和 t9s2t.exe 存在于程序目录下。",
                        "文件不存在",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
            }
        }

        /// <summary>
        /// 当用户在下拉菜单选择"语音输入"时，检测t9s2t.exe是否在运行，
        /// 如果没有运行则询问是否启动。返回true表示允许保存，false表示需要回退选择。
        /// </summary>
        private bool CheckAndLaunchT9s2t(ComboBox comboBox, string keyName)
        {
            if (comboBox.SelectedItem?.ToString() != "语音输入") return true;

            // 检测t9s2t.exe是否在运行
            var running = Process.GetProcessesByName("t9s2t");
            if (running.Length > 0) return true;

            // 没在运行，询问用户
            string exePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "t9s2t", "t9s2t.exe");
            DialogResult result = MessageBox.Show(
                "检测到 t9s2t.exe 未在后台运行，语音输入功能需要该程序支持。\n\n是否立即启动 t9s2t.exe？",
                "语音输入提示",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                if (File.Exists(exePath))
                {
                    try
                    {
                        Process.Start(new ProcessStartInfo
                        {
                            FileName = exePath,
                            WorkingDirectory = Path.GetDirectoryName(exePath),
                            UseShellExecute = true
                        });
                        return true; // 允许保存"语音输入"选择
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("启动 t9s2t.exe 失败：" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        RevertComboBox(comboBox, keyName);
                        return false;
                    }
                }
                else
                {
                    MessageBox.Show(
                        "未找到文件：" + exePath + "\n\n请确认 t9s2t 文件夹和 t9s2t.exe 存在于程序目录下。",
                        "文件不存在",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    RevertComboBox(comboBox, keyName);
                    return false;
                }
            }
            else
            {
                // 用户选择"否"，回退到之前的选项
                RevertComboBox(comboBox, keyName);
                return false;
            }
        }

        /// <summary>
        /// 回退ComboBox到配置文件中保存的上一个选项
        /// </summary>
        private void RevertComboBox(ComboBox comboBox, string keyName)
        {
            string prevValue = LoadKeyMapping(keyName);
            comboBox.SelectedItem = prevValue; // 直接恢复，不会再次触发SelectedIndexChanged（因为值变了）
        }

        private static void HandleCustomKeyByType(string keyName)
        {
            string value = GetKeyMapping(keyName);
            switch (value)
            {
                case "None": break;

                case "Shift":
                    keybd_event((byte)Keys.ShiftKey, 0, 0, IntPtr.Zero);
                    keybd_event((byte)Keys.ShiftKey, 0, KEYEVENTF_KEYUP, IntPtr.Zero);
                    break;
                case "Ctrl":
                    keybd_event((byte)Keys.ControlKey, 0, 0, IntPtr.Zero);
                    keybd_event((byte)Keys.ControlKey, 0, KEYEVENTF_KEYUP, IntPtr.Zero);
                    break;

                case "微信截图":
                    keybd_event((byte)Keys.Menu, 0, 0, IntPtr.Zero);
                    keybd_event((byte)Keys.A, 0, 0, IntPtr.Zero);
                    keybd_event((byte)Keys.A, 0, KEYEVENTF_KEYUP, IntPtr.Zero);
                    keybd_event((byte)Keys.Menu, 0, KEYEVENTF_KEYUP, IntPtr.Zero);
                    break;

                case "切换鼠标键": (Application.OpenForms["Form1"] as Form1)?.ToggleMouseKeys(); break;
                case "打开我的电脑": Process.Start("explorer.exe"); break;
                case "打开计算器": Process.Start("calc.exe"); break;
                case "打开浏览器主页": Process.Start("http://www.google.com"); break;
                case "打开邮件": Process.Start("mailto:"); break;
                case "切换输入法":
                    keybd_event((byte)Keys.ControlKey, 0, 0, IntPtr.Zero);
                    keybd_event((byte)Keys.ShiftKey, 0, 0, IntPtr.Zero);
                    keybd_event((byte)Keys.ShiftKey, 0, KEYEVENTF_KEYUP, IntPtr.Zero);
                    keybd_event((byte)Keys.ControlKey, 0, KEYEVENTF_KEYUP, IntPtr.Zero);
                    break;
                case "复制":
                    keybd_event((byte)Keys.ControlKey, 0, 0, IntPtr.Zero);
                    keybd_event((byte)Keys.C, 0, 0, IntPtr.Zero);
                    keybd_event((byte)Keys.C, 0, KEYEVENTF_KEYUP, IntPtr.Zero);
                    keybd_event((byte)Keys.ControlKey, 0, KEYEVENTF_KEYUP, IntPtr.Zero);
                    break;
                case "粘贴":
                    keybd_event((byte)Keys.ControlKey, 0, 0, IntPtr.Zero);
                    keybd_event((byte)Keys.V, 0, 0, IntPtr.Zero);
                    keybd_event((byte)Keys.V, 0, KEYEVENTF_KEYUP, IntPtr.Zero);
                    keybd_event((byte)Keys.ControlKey, 0, KEYEVENTF_KEYUP, IntPtr.Zero);
                    break;
                case "剪切":
                    keybd_event((byte)Keys.ControlKey, 0, 0, IntPtr.Zero);
                    keybd_event((byte)Keys.X, 0, 0, IntPtr.Zero);
                    keybd_event((byte)Keys.X, 0, KEYEVENTF_KEYUP, IntPtr.Zero);
                    keybd_event((byte)Keys.ControlKey, 0, KEYEVENTF_KEYUP, IntPtr.Zero);
                    break;
                case "全选":
                    keybd_event((byte)Keys.ControlKey, 0, 0, IntPtr.Zero);
                    keybd_event((byte)Keys.A, 0, 0, IntPtr.Zero);
                    keybd_event((byte)Keys.A, 0, KEYEVENTF_KEYUP, IntPtr.Zero);
                    keybd_event((byte)Keys.ControlKey, 0, KEYEVENTF_KEYUP, IntPtr.Zero);
                    break;
                case "撤回":
                    keybd_event((byte)Keys.ControlKey, 0, 0, IntPtr.Zero);
                    keybd_event((byte)Keys.Z, 0, 0, IntPtr.Zero);
                    keybd_event((byte)Keys.Z, 0, KEYEVENTF_KEYUP, IntPtr.Zero);
                    keybd_event((byte)Keys.ControlKey, 0, KEYEVENTF_KEYUP, IntPtr.Zero);
                    break;
                case "ESC": keybd_event((byte)Keys.Escape, 0, 0, IntPtr.Zero); keybd_event((byte)Keys.Escape, 0, KEYEVENTF_KEYUP, IntPtr.Zero); break;
                case "退格": keybd_event((byte)Keys.Back, 0, 0, IntPtr.Zero); keybd_event((byte)Keys.Back, 0, KEYEVENTF_KEYUP, IntPtr.Zero); break;
                case "空格": keybd_event((byte)Keys.Space, 0, 0, IntPtr.Zero); keybd_event((byte)Keys.Space, 0, KEYEVENTF_KEYUP, IntPtr.Zero); break;
                case "逗号": keybd_event((byte)Keys.Oemcomma, 0, 0, IntPtr.Zero); keybd_event((byte)Keys.Oemcomma, 0, KEYEVENTF_KEYUP, IntPtr.Zero); break;
                case "句号": keybd_event((byte)Keys.OemPeriod, 0, 0, IntPtr.Zero); keybd_event((byte)Keys.OemPeriod, 0, KEYEVENTF_KEYUP, IntPtr.Zero); break;
                case "Tab": keybd_event((byte)Keys.Tab, 0, 0, IntPtr.Zero); keybd_event((byte)Keys.Tab, 0, KEYEVENTF_KEYUP, IntPtr.Zero); break;
                case "0": keybd_event((byte)Keys.NumPad0, 0, 0, IntPtr.Zero); keybd_event((byte)Keys.NumPad0, 0, KEYEVENTF_KEYUP, IntPtr.Zero); break;
                case ".": keybd_event((byte)Keys.Decimal, 0, 0, IntPtr.Zero); keybd_event((byte)Keys.Decimal, 0, KEYEVENTF_KEYUP, IntPtr.Zero); break;
            }
        }
        #endregion

        #region WinAPI
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, IntPtr dwExtraInfo);
        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);
        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);
        #endregion

        private void label6_Click(object sender, EventArgs e) { Process.Start("https://t9.xiaobai.pro/?p=508"); }
        private void button1_Click(object sender, EventArgs e)
        {
            forceEnabled = !forceEnabled;
            if (forceEnabled)
            {
                monitorTimer.Stop();
                if (_hookID == IntPtr.Zero) _hookID = SetHook(_proc);
                ShowStatus("【强制模式已开启】"); button1.Text = "关闭强制模式";
            }
            else
            {
                if (_hookID != IntPtr.Zero) { UnhookWindowsHookEx(_hookID); _hookID = IntPtr.Zero; }
                monitorTimer.Start();
                ShowStatus("【强制模式已关闭】"); button1.Text = "强制开启改键";
            }
        }

        // 【修改】换成灵敏无延迟的 MouseEnter 事件
        private void Label6_MouseEnter(object sender, EventArgs e)
        {
            if (imgTooltip == null || imgTooltip.IsDisposed)
            {
                // 🎯 注意：请把下面的 "你的图片名字" 替换成你项目中真正的资源图片名称
                imgTooltip = new ImageTooltipForm(numkeyboard.Properties.Resources.numkeyboard);
            }

            // 获取 label6 在屏幕上的绝对坐标
            Point screenPoint = label6.PointToScreen(Point.Empty);

            // 计算悬浮窗的显示位置：显示在 label6 的正上方，且水平居中对齐
            int x = screenPoint.X + (label6.Width / 2) - (imgTooltip.Width / 2);
            int y = screenPoint.Y - imgTooltip.Height - 8; // 8 像素的间距，防止鼠标误触

            // 强制指定坐标
            imgTooltip.StartPosition = FormStartPosition.Manual;
            imgTooltip.Location = new Point(x, y);
            imgTooltip.TopMost = true; // 确保图片显示在最上层

            imgTooltip.Show();
        }

        // 【修改】离开 label6 时隐藏图片窗体
        private void Label6_MouseLeave(object sender, EventArgs e)
        {
            if (imgTooltip != null && !imgTooltip.IsDisposed)
            {
                imgTooltip.Hide();
            }
        }
    }
}
