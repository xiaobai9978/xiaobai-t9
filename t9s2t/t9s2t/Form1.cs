using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using NAudio.Wave;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.Win32;
using t9s2t.Engines;

namespace t9s2t
{
    public partial class Form1 : Form
    {
        private ISpeechEngine engine;
        private VadStreamProcessor vadProcessor;
        private WaveInEvent waveIn;
        private bool isRecording = false;
        private readonly string modelPath = "model";
        private NotifyIcon trayIcon;
        private EngineType currentEngineType = EngineType.None;

        // 录音浮动提示窗
        private Form recordingIndicator;
        // 录音前的目标窗口句柄（防止提示窗抢焦点后无法恢复）
        private IntPtr _lastTargetWindow = IntPtr.Zero;

        // ==================== 动态 UI 控件 ====================
        private CheckBox chkAutoStart;

        // ==================== 远程模型配置地址 ====================
        private readonly string remoteModelsJsonUrl = "https://t9.xiaobai.pro/models/models.json";
        // 引擎运行时 DLL  远程配置地址
        private readonly string remoteEngineJsonUrl = "https://t9.xiaobai.pro/models/engine.json";
        // 备用方案：如果网络请求失败，使用本地内置的默认列表
        private readonly string fallbackJson = @"
        [
            {
                ""name"": ""Paraformer-large-zh (推荐)"",
                ""description"": ""中文大模型, 自动标点, 松开快捷键后出字, 识别质量最高"",
                ""url"": ""https://wp.xiaobai.pro:1111/d/%E8%BE%93%E5%85%A5%E6%B3%95/models/sherpa-onnx-paraformer-large.zip"",
                ""folder"": ""sherpa-onnx-paraformer-large"",
                ""size_hint"": ""258MB"",
                ""version"": ""2024-03-09"",
                ""published"": true
            },
            {
               ""name"": ""Paraformer-large-zh (备用)"",
                ""description"": ""中文大模型, 自动标点, 松开快捷键后出字, 识别质量最高"",
                ""url"": ""https://t9.xiaobai.pro/models/sherpa-onnx-paraformer-large.zip"",
                ""folder"": ""sherpa-onnx-paraformer-large"",
                ""size_hint"": ""258MB"",
                ""version"": ""2024-03-09"",
                ""published"": true
            }
        ]";

        // 引擎运行时 DLL 备用配置
        private readonly string fallbackEngineJson = @"
        {
            ""engine_runtimes"": [
                {
                    ""name"": ""onnxruntime.dll"",
                    ""url"": ""https://wp.xiaobai.pro:1111/d/%E8%BE%93%E5%85%A5%E6%B3%95/models/onnxruntime.dll"",
                    ""description"": ""ONNX 推理引擎 (15MB)""
                },
                {
                    ""name"": ""sherpa-onnx-c-api.dll"",
                    ""url"": ""https://wp.xiaobai.pro:1111/d/%E8%BE%93%E5%85%A5%E6%B3%95/models/sherpa-onnx-c-api.dll"",
                    ""description"": ""sherpa-onnx C API (4MB)""
                }
            ]
        }";

        // ==================== 键盘钩子 API ====================
        public delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);
        [DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(int vKey);
        [DllImport("user32.dll")]
        private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, IntPtr dwExtraInfo);
        private const uint KEYEVENTF_KEYUP = 0x0002;  // keybd_event 标志：表示按键释放

        // ==================== 窗口控制与防多开 API ====================
        [DllImport("user32.dll")] private static extern bool SetForegroundWindow(IntPtr hWnd);
        [DllImport("user32.dll")] private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        [DllImport("user32.dll")] private static extern bool IsIconic(IntPtr hWnd);
        [DllImport("user32.dll")] private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll")] private static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll")] private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);
        [DllImport("user32.dll")] private static extern bool AttachThreadInput(uint idAttach, uint idAttachTo, bool fAttach);

        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYUP = 0x0101;
        private const int WM_SYSKEYDOWN = 0x0104;  // Alt 按住时的 KEYDOWN
        private const int WM_SYSKEYUP = 0x0105;    // Alt 按住时的 KEYUP
        private const int VK_D = 0x44;             // D 键（语音触发键）
        private const int VK_CONTROL = 0x11;
        private const int VK_MENU = 0x12;          // Alt 键
        private const int SW_RESTORE = 9;
        private const int SW_SHOW = 5;

        private static readonly int WM_SHOWAPP = RegisterWindowMessage("WM_SHOWAPP_T9S2T_UNIQUE");
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int RegisterWindowMessage(string lpString);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

        private IntPtr _hookID = IntPtr.Zero;
        private LowLevelKeyboardProc _proc;
        private Mutex _mutex;

        // 模型数据结构
        public class ModelInfo
        {
            public string name { get; set; }
            public string description { get; set; }
            public string url { get; set; }
            public string folder { get; set; }
            public string size_hint { get; set; }
            public string version { get; set; }
            public bool published { get; set; } = true;  // 默认 true，兼容旧 JSON

            // 新增：用于自动显示的格式化属性
            public string DisplayText => $"[{size_hint}] {name} - {description}";
        }

        public Form1()
        {
            InitializeComponent();
            this.Load += Form1_Load;
            this.FormClosing += Form1_FormClosing;
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            bool createdNew;
            _mutex = new Mutex(true, "Global\\t9s2t_single_instance_mutex_v3", out createdNew);

            if (!createdNew)
            {
                IntPtr existingHandle = FindWindow(null, this.Text);
                if (existingHandle != IntPtr.Zero) SendMessage(existingHandle, WM_SHOWAPP, IntPtr.Zero, IntPtr.Zero);
                Environment.Exit(0);
                return;
            }

            trayIcon = new NotifyIcon { Icon = this.Icon, Visible = true, Text = "t9s2t 启动中..." };
            var menu = new ContextMenuStrip();
            menu.Items.Add("显示主界面", null, (s, ev) => ForceShowMainWindow());
            menu.Items.Add("退出", null, (s, ev) => Application.Exit());
            trayIcon.ContextMenuStrip = menu;
            trayIcon.DoubleClick += (s, ev) => ForceShowMainWindow();

            chkAutoStart = new CheckBox
            {
                Text = "开机自动启动",
                Location = new Point(30, 162),
                Size = new Size(300, 24),
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(80, 80, 80),
                Checked = IsAutoStartEnabled()
            };
            chkAutoStart.CheckedChanged += ChkAutoStart_CheckedChanged;
            this.Controls.Add(chkAutoStart);
            chkAutoStart.BringToFront();
            await Task.Delay(200);

            // 检查引擎 DLL 和模型是否就绪
            UpdateEngineButtonState();
            bool engineReady = AreEngineDllsReady();
            bool modelReady = false;

            if (engineReady)
            {
                currentEngineType = EngineDetector.Detect(modelPath);
                modelReady = (currentEngineType != EngineType.None);
            }

            if (engineReady && modelReady)
            {
                // 一切就绪：隐藏到托盘后台运行
                this.WindowState = FormWindowState.Minimized;
                this.Hide();
                this.ShowInTaskbar = false;
                trayIcon.ShowBalloonTip(3000, "t9s2t 已就绪", "程序已在后台运行，按 Ctrl+Alt+D 说话", ToolTipIcon.Info);
                await LoadEngine();
            }
            else
            {
                // 组件或模型未就绪：保持窗口可见，让用户操作
                if (!engineReady)
                    lblStatus.Text = "⚠️ 缺少引擎组件，请点击上方按钮下载";
                else
                    lblStatus.Text = "❌ 模型未找到，请点击下载按钮";
                this.Show();
                this.BringToFront();
            }

            _proc = HookCallback;
            _hookID = SetHook(_proc);
            InitMicrophone();
            SetupDynamicResources();














            SetupDynamicResources();
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_SHOWAPP) ForceShowMainWindow();
            base.WndProc(ref m);
        }

        private void ForceShowMainWindow()
        {
            if (this.InvokeRequired) { this.Invoke(new Action(ForceShowMainWindow)); return; }
            // 确保尺寸正确（最小化/隐藏时 ClientSize.Height 可能被置 0）
            int correctWidth = _adCollapsed ? AdCollapsedWidth : AdExpandedWidth;
            if (this.ClientSize.Width != correctWidth || this.ClientSize.Height != FormHeight)
                this.ClientSize = new Size(correctWidth, FormHeight);
            IntPtr foreHandle = GetForegroundWindow();
            uint foreThreadId = GetWindowThreadProcessId(foreHandle, out _);
            uint thisThreadId = GetWindowThreadProcessId(this.Handle, out _);
            if (foreThreadId != thisThreadId) AttachThreadInput(foreThreadId, thisThreadId, true);
            if (IsIconic(this.Handle)) ShowWindow(this.Handle, SW_RESTORE); else ShowWindow(this.Handle, SW_SHOW);
            this.ShowInTaskbar = true; this.Visible = true; this.BringToFront();
            SetForegroundWindow(this.Handle); this.Activate();
            if (foreThreadId != thisThreadId) AttachThreadInput(foreThreadId, thisThreadId, false);
        }

        private void ChkAutoStart_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true))
                {
                    if (chkAutoStart.Checked) key.SetValue("t9s2t", Application.ExecutablePath);
                    else key.DeleteValue("t9s2t", false);
                }
            }
            catch (Exception ex) { MessageBox.Show("设置开机自启失败: " + ex.Message); chkAutoStart.Checked = !chkAutoStart.Checked; }
        }

        private bool IsAutoStartEnabled()
        {
            try { using (var key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", false)) return key.GetValue("t9s2t") != null; }
            catch { return false; }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true; this.Hide(); this.ShowInTaskbar = false;
                trayIcon.ShowBalloonTip(2000, "t9s2t", "程序已最小化到系统托盘", ToolTipIcon.Info); return;
            }
            if (_hookID != IntPtr.Zero) UnhookWindowsHookEx(_hookID);
            _mutex?.ReleaseMutex(); waveIn?.Dispose(); engine?.Dispose(); trayIcon?.Dispose();
        }

        private void SetupDynamicResources()
        {
            if (DesignMode) return;
            try { string p = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "mic_icon.png"); if (File.Exists(p)) appIcon.Image = Image.FromFile(p); } catch { }

            // 加载二维码图片
            string appDir = AppDomain.CurrentDomain.BaseDirectory;
            try { string p = Path.Combine(appDir, "wechat_qr.png"); if (File.Exists(p)) qrWechat.Image = Image.FromFile(p); } catch { }
            try { string p = Path.Combine(appDir, "alipay_qr.png"); if (File.Exists(p)) qrAlipay.Image = Image.FromFile(p); } catch { }
            try { string p = Path.Combine(appDir, "pdd_qrcode.png"); if (File.Exists(p)) qrPdd.Image = Image.FromFile(p); } catch { }

            // 恢复广告面板折叠状态
            RestoreAdPanelState();
        }

        private void adPanel_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

            // 标题 - 居中
            using (var titleFont = new Font("Microsoft YaHei UI", 12F, FontStyle.Bold))
            using (var titleBrush = new SolidBrush(Color.FromArgb(200, 80, 40)))
            {
                g.DrawString("支持小白", titleFont, titleBrush, 210, 8);
            }

            // 二维码下方标签
            using (var labelFont = new Font("Microsoft YaHei UI", 8.5F))
            using (var smallFont = new Font("Microsoft YaHei UI", 7.5F))
            using (var grayBrush = new SolidBrush(Color.FromArgb(80, 80, 80)))
            using (var darkBrush = new SolidBrush(Color.FromArgb(50, 50, 50)))
            using (var redBrush = new SolidBrush(Color.FromArgb(210, 50, 20)))
            using (var pen = new Pen(Color.FromArgb(220, 200, 180), 1))
            {
                // 分割线
                g.DrawLine(pen, 175, 40, 175, 240);
                g.DrawLine(pen, 350, 40, 350, 240);

                // 微信打赏
                g.DrawString("微信打赏", labelFont, darkBrush, 42, 160);
                g.DrawString("感谢支持小白输入法", smallFont, grayBrush, 18, 178);

                // 支付宝打赏
                g.DrawString("支付宝打赏", labelFont, darkBrush, 213, 160);
                g.DrawString("感谢支持小白输入法", smallFont, grayBrush, 193, 178);

                // 拼多多店铺
                g.DrawString("拼多多店铺", labelFont, darkBrush, 388, 160);
                g.DrawString("捐助98元可赠送", smallFont, grayBrush, 393, 178);
                g.DrawString("小白T9无线键盘一台!", smallFont, redBrush, 380, 194);

                // 分隔线上方小标题
                using (var tipFont = new Font("Microsoft YaHei UI", 7F))
                using (var tipBrush = new SolidBrush(Color.FromArgb(150, 120, 90)))
                {
                    g.DrawString("扫码打赏", tipFont, tipBrush, 48, 30);
                    g.DrawString("扫码打赏", tipFont, tipBrush, 223, 30);
                    g.DrawString("扫码进店", tipFont, tipBrush, 398, 30);
                }
            }
        }

        #region 广告面板折叠

        private bool _adCollapsed = false;
        private const int AdExpandedWidth = 900;
        private const int AdCollapsedWidth = 365;
        private const int FormHeight = 232;  // 与 Designer 中的 ClientSize.Height 保持一致

        private void btnToggleAd_Click(object sender, EventArgs e)
        {
            _adCollapsed = !_adCollapsed;
            SetAdPanelCollapsed(_adCollapsed);
            SaveAdPanelState();
        }

        private void SetAdPanelCollapsed(bool collapsed)
        {
            if (collapsed)
            {
                adPanel.Visible = false;
                btnToggleAd.Text = ">";
                btnToggleAd.Location = new Point(340, 110);
                this.ClientSize = new Size(AdCollapsedWidth, FormHeight);
            }
            else
            {
                adPanel.Visible = true;
                btnToggleAd.Text = "<";
                btnToggleAd.Location = new Point(340, 110);
                this.ClientSize = new Size(AdExpandedWidth, FormHeight);
            }
        }

        private void SaveAdPanelState()
        {
            try
            {
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ad_state.txt");
                File.WriteAllText(path, _adCollapsed ? "collapsed" : "expanded");
            }
            catch { }
        }

        private void RestoreAdPanelState()
        {
            try
            {
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ad_state.txt");
                if (File.Exists(path))
                {
                    string state = File.ReadAllText(path).Trim();
                    _adCollapsed = (state == "collapsed");
                }
            }
            catch { }

            // 默认展开
            if (!_adCollapsed) return;
            SetAdPanelCollapsed(true);
        }

        #endregion

        private IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
        }

        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                int vkCode = Marshal.ReadInt32(lParam);
                if (vkCode == VK_D)
                {
                    // Alt 按住时 Windows 发送 WM_SYSKEYDOWN/WM_SYSKEYUP 而非 WM_KEYDOWN/WM_KEYUP
                    bool isKeyDown = wParam == (IntPtr)WM_KEYDOWN || wParam == (IntPtr)WM_SYSKEYDOWN;
                    bool isKeyUp   = wParam == (IntPtr)WM_KEYUP   || wParam == (IntPtr)WM_SYSKEYUP;

                    if (isKeyDown)
                    {
                        bool isCtrlDown = (GetAsyncKeyState(VK_CONTROL) & 0x8000) != 0;
                        bool isAltDown  = (GetAsyncKeyState(VK_MENU)    & 0x8000) != 0;

                        if (isCtrlDown && isAltDown && !isRecording)
                        {
                            // Ctrl+Alt+D：开始录音，拦截按键
                            StartRecording();
                            return (IntPtr)1;
                        }
                        if (isRecording)
                        {
                            // 录音中：拦截 D 键防止字符输入
                            return (IntPtr)1;
                        }
                        // 非 Ctrl+Alt+D 且非录音中：放行，正常打字
                    }
                    else if (isKeyUp && isRecording)
                    {
                        // D 键松开：停止录音（不检查修饰键，因为松开顺序不确定）
                        StopRecording();
                        return (IntPtr)1;
                    }
                }
            }
            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        // ==================== 引擎 DLL 按需下载 ====================
        private static readonly string[] RequiredEngineDlls = { "onnxruntime.dll", "sherpa-onnx-c-api.dll" };

        /// <summary>
        /// 检查引擎 DLL 是否已存在且有效（大小 > 0）
        /// </summary>
        private bool AreEngineDllsReady()
        {
            string appDir = AppDomain.CurrentDomain.BaseDirectory;
            foreach (var dll in RequiredEngineDlls)
            {
                string dllPath = Path.Combine(appDir, dll);
                if (!File.Exists(dllPath) || new FileInfo(dllPath).Length == 0)
                {
                    if (File.Exists(dllPath)) try { File.Delete(dllPath); } catch { }
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 根据引擎 DLL 是否存在，设置按钮文字和颜色
        /// </summary>
        private void UpdateEngineButtonState()
        {
            if (AreEngineDllsReady())
            {
                // 引擎就绪：恢复正常的下载模型按钮
                btnDownloadModel.Text = "⬇️ 下载模型";
                btnDownloadModel.BackColor = SystemColors.Control;
                btnDownloadModel.ForeColor = SystemColors.ControlText;
            }
            else
            {
                // 引擎缺失：按钮变为“下载组件”，橙色警示
                btnDownloadModel.Text = "⚙️ 下载引擎组件";
                btnDownloadModel.BackColor = Color.FromArgb(255, 152, 0);  // 橙色
                btnDownloadModel.ForeColor = Color.White;
                btnDownloadModel.Enabled = true;
                lblStatus.Text = "⚠️ 缺少引擎组件，请先点击上方按钮下载";
                lblStatus.ForeColor = Color.FromArgb(200, 100, 0);
            }
        }

        /// <summary>
        /// 从服务器下载引擎 DLL 文件
        /// </summary>
        private async Task<bool> DownloadEngineDllsAsync()
        {
            string appDir = AppDomain.CurrentDomain.BaseDirectory;
            btnDownloadModel.Enabled = false;
            lblStatus.Text = "⚙️ 正在下载引擎组件..."; lblStatus.Refresh();

            List<EngineDllInfo> engineDlls = await FetchEngineDllsAsync();
            if (engineDlls == null || engineDlls.Count == 0)
            {
                lblStatus.Text = "❌ 无法获取引擎组件下载地址"; lblStatus.Refresh();
                btnDownloadModel.Enabled = true;
                return false;
            }

            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                using (var client = new WebClient())
                {
                    client.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64)");
                    int total = engineDlls.Count, current = 0;
                    foreach (var dll in engineDlls)
                    {
                        string dllPath = Path.Combine(appDir, dll.name);
                        if (File.Exists(dllPath) && new FileInfo(dllPath).Length > 0) continue;
                        current++;
                        lblStatus.Text = $"⬇️ 正在下载引擎组件 ({current}/{total}): {dll.description}"; lblStatus.Refresh();
                        await client.DownloadFileTaskAsync(new Uri(dll.url), dllPath);
                    }
                }

                if (AreEngineDllsReady())
                {
                    lblStatus.Text = "✅ 引擎组件下载完成"; lblStatus.Refresh();
                    UpdateEngineButtonState();
                    return true;
                }
                else
                {
                    lblStatus.Text = "❌ 引擎组件下载不完整"; lblStatus.Refresh();
                    btnDownloadModel.Enabled = true;
                    return false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[t9s2t] 引擎 DLL 下载失败: {ex.Message}");
                foreach (var dll in RequiredEngineDlls)
                {
                    string dllPath = Path.Combine(appDir, dll);
                    if (File.Exists(dllPath) && new FileInfo(dllPath).Length == 0)
                        try { File.Delete(dllPath); } catch { }
                }
                lblStatus.Text = $"❌ 下载失败: {ex.Message}"; lblStatus.Refresh();
                btnDownloadModel.Enabled = true;
                return false;
            }
        }

        private async Task<List<EngineDllInfo>> FetchEngineDllsAsync()
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                using (var client = new WebClient())
                {
                    client.Headers.Add("User-Agent", "Mozilla/5.0");
                    string json = await client.DownloadStringTaskAsync(remoteEngineJsonUrl);
                    var obj = JObject.Parse(json);
                    var arr = obj["engine_runtimes"] as JArray;
                    if (arr != null)
                        return arr.ToObject<List<EngineDllInfo>>();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[t9s2t] 远程引擎配置加载失败: {ex.Message}");
            }

            // 使用内置备用配置
            try
            {
                var obj = JObject.Parse(fallbackEngineJson);
                var arr = obj["engine_runtimes"] as JArray;
                if (arr != null)
                    return arr.ToObject<List<EngineDllInfo>>();
            }
            catch { }
            return null;
        }

        public class EngineDllInfo
        {
            public string name { get; set; }
            public string url { get; set; }
            public string description { get; set; }
        }

        private async Task AutoCheckModel()
        {
            // 先检查引擎 DLL
            if (!AreEngineDllsReady())
            {
                UpdateEngineButtonState();
                lblEngine.Text = "引擎: 未就绪";
                return;
            }

            lblStatus.Text = "🔍 正在检测模型..."; lblStatus.Refresh();

            currentEngineType = EngineDetector.Detect(modelPath);

            if (currentEngineType != EngineType.None)
            {
                btnDownloadModel.Enabled = false; btnDownloadModel.Text = "✅ 模型已就绪";
                btnDownloadModel.BackColor = SystemColors.Control;
                btnDownloadModel.ForeColor = SystemColors.ControlText;
                btnDeleteModel.Enabled = true;
                lblEngine.Text = $"引擎: {EngineDetector.GetDisplayName(currentEngineType)}";
                lblStatus.Text = $"✅ {EngineDetector.GetDisplayName(currentEngineType)} 模型已存在，正在加载..."; lblStatus.Refresh();
                await LoadEngine();
            }
            else
            {
                btnDownloadModel.Enabled = true; btnDownloadModel.Text = "⬇️ 下载模型";
                btnDownloadModel.BackColor = SystemColors.Control;
                btnDownloadModel.ForeColor = SystemColors.ControlText;
                btnDeleteModel.Enabled = false;
                lblEngine.Text = "引擎: 未加载";
                lblStatus.Text = "❌ 模型未找到，请点击下方按钮下载";
                lblStatus.ForeColor = SystemColors.ControlText;
            }
        }


        private async Task LoadEngine()
        {
            try
            {
                engine?.Dispose();
                engine = EngineDetector.DetectAndCreate(modelPath);

                if (engine == null)
                {
                    this.Invoke((MethodInvoker)delegate {
                        lblStatus.Text = "❌ 无法识别模型类型";
                        btnDownloadModel.Enabled = true;
                        btnDownloadModel.Text = "⬇️ 重新下载模型";
                    });
                    return;
                }

                await engine.LoadAsync(modelPath);

                // ==================== 流式处理设置 ====================
                if (engine.SupportsStreaming)
                {
                    if (engine is SherpaEngine) // 1GB Paraformer 走原生流式
                    {
                        vadProcessor = null; // 不使用 VAD
                    }
                    else // SenseVoice 等走 VAD 模拟流式
                    {
                        vadProcessor = new VadStreamProcessor(
                            engine,
                            onResult: (text) => SimulateKeyboardInput(text, isFinal: false), // 实时出字
                            onPartial: (partial) => {
                                try
                                {
                                    this.Invoke((MethodInvoker)delegate {
                                        lblStatus.Text = $"🎤 {partial}";
                                    });
                                }
                                catch { }
                            }
                        );
                    }
                }
                else
                {
                    vadProcessor = null;
                }

                string streamingHint = engine.SupportsStreaming ? " (流式识别)" : "";

                this.Invoke((MethodInvoker)delegate {
                    var sherpa = engine as SherpaEngine;
                    string extraHint = "";
                    if (sherpa != null)
                    {
                        if (sherpa.HasPunctuation) extraHint += " +标点";
                        if (sherpa.HasVad) extraHint += " +VAD";
                    }
                    lblEngine.Text = $"引擎: {engine.EngineName}{streamingHint}{extraHint}";
                    lblEngine.ForeColor = Color.FromArgb(40, 140, 80);
                    lblStatus.Text = $"✅ {engine.EngineName}{streamingHint}{extraHint} 就绪！按住 Ctrl+Alt+D 说话";
                    lblStatus.ForeColor = Color.FromArgb(40, 53, 147);
                    trayIcon.Text = $"t9s2t ({engine.EngineName})";
                });

                Debug.WriteLine($"[LoadEngine] Success: {engine.EngineName}, Streaming={engine.SupportsStreaming}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[LoadEngine Error] {ex}");
                this.Invoke((MethodInvoker)delegate {
                    lblStatus.Text = "❌ 加载失败: " + ex.Message;
                    btnDownloadModel.Enabled = true;
                    btnDownloadModel.Text = "⬇️ 重新下载模型";
                });
            }
        }

        // ==================== 核心：动态获取模型列表并弹窗选择 ====================
        private async void btnDownloadModel_Click(object sender, EventArgs e)
        {
            // 如果引擎组件未就绪，先下载引擎组件
            if (!AreEngineDllsReady())
            {
                bool success = await DownloadEngineDllsAsync();
                if (success)
                {
                    // 引擎就绪后，自动继续模型检测
                    await AutoCheckModel();
                }
                return;
            }

            btnDownloadModel.Enabled = false;
            lblStatus.Text = "🔄 正在获取最新模型列表..."; lblStatus.Refresh();

            List<ModelInfo> models = await FetchModelsAsync();
            if (models == null || models.Count == 0)
            {
                MessageBox.Show("无法获取模型列表，请检查网络连接。", "错误");
                btnDownloadModel.Enabled = true;
                lblStatus.Text = "❌ 获取模型列表失败";
                return;
            }

            // 动态创建模型选择窗口
            using (Form selectForm = new Form())
            {
                selectForm.Text = "请选择要下载的语音模型";
                selectForm.Size = new Size(720, 400);
                selectForm.StartPosition = FormStartPosition.CenterParent;
                selectForm.FormBorderStyle = FormBorderStyle.FixedDialog;
                selectForm.MaximizeBox = false; selectForm.MinimizeBox = false;

                Label lblTip = new Label
                {
                    Text = "官方更新了模型？这里会自动显示最新列表：",
                    Location = new Point(12, 10),
                    Size = new Size(680, 20),
                    Font = new Font("Segoe UI", 9F)
                };

                ListView lvModels = new ListView
                {
                    Location = new Point(12, 36),
                    Size = new Size(680, 270),
                    Font = new Font("Segoe UI", 9.5F),
                    View = View.Details,
                    FullRowSelect = true,
                    GridLines = true,
                    MultiSelect = false,
                    HideSelection = false
                };
                lvModels.Columns.Add("大小", 70, HorizontalAlignment.Left);
                lvModels.Columns.Add("名称", 180, HorizontalAlignment.Left);
                lvModels.Columns.Add("说明", 410, HorizontalAlignment.Left);

                foreach (var m in models)
                {
                    var item = new ListViewItem(m.size_hint ?? "");
                    item.SubItems.Add(m.name ?? "");
                    item.SubItems.Add(m.description ?? "");
                    item.Tag = m; // 保存原始对象用于后续取值
                    lvModels.Items.Add(item);
                }
                if (lvModels.Items.Count > 0) lvModels.Items[0].Selected = true;

                Button btnOK = new Button
                {
                    Text = "开始下载选中模型",
                    Location = new Point(270, 316),
                    Size = new Size(160, 34),
                    DialogResult = DialogResult.OK
                };

                selectForm.Controls.AddRange(new Control[] { lblTip, lvModels, btnOK });
                selectForm.AcceptButton = btnOK;

                if (selectForm.ShowDialog(this) == DialogResult.OK)
                {
                    ModelInfo selected = null;
                    if (lvModels.SelectedItems.Count > 0)
                        selected = (ModelInfo)lvModels.SelectedItems[0].Tag;
                    if (selected != null)
                    {
                        await StartDownloadProcess(selected);
                        return; // AutoCheckModel 已设置好按钮状态
                    }
                }
                // 用户取消或未选择，恢复按钮
                btnDownloadModel.Enabled = true;
            }
        }
        private async Task<List<ModelInfo>> FetchModelsAsync()
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                using (var client = new WebClient())
                {
                    client.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) t9s2t-client");
                    client.Encoding = System.Text.Encoding.UTF8;
                    // 尝试从远程获取最新列表
                    string json = await client.DownloadStringTaskAsync(remoteModelsJsonUrl);

                    // 清理 BOM（字节顺序标记）和其他不可见字符
                    if (json.Length > 0 && json[0] == '\uFEFF')
                        json = json.Substring(1);
                    json = json.Trim();

                    Debug.WriteLine($"[t9s2t] 原始 JSON 前 100 字符: {json.Substring(0, Math.Min(100, json.Length))}");

                    var models = JsonConvert.DeserializeObject<List<ModelInfo>>(json);
                    // 只保留 published=true 的模型，过滤掉测试/占位条目
                    models = models?.FindAll(m => m.published);
                    Debug.WriteLine($"[t9s2t] 远程模型列表获取成功，共 {models?.Count ?? 0} 个已发布模型");
                    return models;
                }
            }
            catch (Exception ex)
            {
                // 如果网络不通或链接失效，使用内置的备用列表
                Debug.WriteLine($"[t9s2t] 远程模型列表获取失败，使用本地备用列表: {ex.Message}");
                var models = JsonConvert.DeserializeObject<List<ModelInfo>>(fallbackJson);
                return models?.FindAll(m => m.published);
            }
        }

        private async Task StartDownloadProcess(ModelInfo selectedModel)
        {
            btnDownloadModel.Enabled = false;
            btnDeleteModel.Enabled = false;

            ProgressBar progressBar = new ProgressBar
            {
                Location = new Point(29, 90),
                Size = new Size(300, 16),
                Minimum = 0,
                Maximum = 100,
                Style = ProgressBarStyle.Blocks
            };
            this.Controls.Add(progressBar);
            progressBar.BringToFront();

            lblStatus.Text = $"⬇️ 正在下载 {selectedModel.name}...";
            await DownloadWithProgress(progressBar, selectedModel.url, selectedModel.folder);

            this.Controls.Remove(progressBar);
            await AutoCheckModel();
        }

        private async Task DownloadWithProgress(ProgressBar progressBar, string url, string targetFolder)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                using (var client = new WebClient())
                {
                    client.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36");
                    client.Headers.Add("Accept", "*/*");

                    client.DownloadProgressChanged += (s, ev) =>
                    {
                        if (ev.TotalBytesToReceive > 0)
                        {
                            progressBar.Style = ProgressBarStyle.Blocks;
                            progressBar.Value = ev.ProgressPercentage;
                            lblStatus.Text = $"⬇️ 正在下载... {ev.ProgressPercentage}% ({ev.BytesReceived / 1048576}MB / {ev.TotalBytesToReceive / 1048576}MB)";
                        }
                        else
                        {
                            progressBar.Style = ProgressBarStyle.Marquee;
                            lblStatus.Text = $"⬇️ 正在下载... 已下载 {ev.BytesReceived / 1048576} MB";
                        }
                        lblStatus.Refresh();
                    };

                    // 1. 清理可能存在的旧模型文件夹
                    if (Directory.Exists(modelPath)) Directory.Delete(modelPath, true);
                    if (Directory.Exists(targetFolder)) Directory.Delete(targetFolder, true);

                    // 下载文件
                    string archivePath = Path.Combine(Path.GetTempPath(), "model_temp_" + Guid.NewGuid().ToString("N") + ".bin");
                    await client.DownloadFileTaskAsync(new Uri(url), archivePath);

                    progressBar.Style = ProgressBarStyle.Blocks;
                    progressBar.Value = 100;
                    lblStatus.Text = "📦 下载完成，正在解压...";
                    lblStatus.Refresh();

                    // 2. 根据文件内容（魔数）判断压缩格式
                    bool isTarBz2File = TarBz2Extractor.IsTarBz2File(archivePath);
                    if (isTarBz2File)
                    {
                        await TarBz2Extractor.ExtractAsync(archivePath, ".");
                    }
                    else
                    {
                        ZipFile.ExtractToDirectory(archivePath, ".");
                    }
                    File.Delete(archivePath);

                    // 3. 智能查找解压后的模型目录
                    string extractedFolder = null;

                    // 情况 A：解压出了 JSON 中指定的 folder 目录
                    if (Directory.Exists(targetFolder))
                    {
                        extractedFolder = targetFolder;
                    }
                    // 情况 B：寻找任何包含已知模型文件的目录
                    foreach (var dir in Directory.GetDirectories("."))
                    {
                        // SenseVoice / Paraformer 模型
                        if (File.Exists(Path.Combine(dir, "model.int8.onnx")) ||
                            File.Exists(Path.Combine(dir, "encoder.int8.onnx")) ||
                            File.Exists(Path.Combine(dir, "tokens.txt")))
                        { extractedFolder = dir; break; }
                    }

                    // 4. 将找到的目录重命名为 "model"
                    if (extractedFolder != null && Directory.Exists(extractedFolder))
                    {
                        Directory.Move(extractedFolder, modelPath);
                        lblStatus.Text = "✅ 模型安装完成！";
                    }
                    else
                    {
                        throw new DirectoryNotFoundException("解压后未找到有效的模型目录结构。\n请检查压缩包是否正确。");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"下载或解压失败: {ex.Message}\n\n排查建议：\n1. 检查链接是否已过期\n2. .tar.bz2 需要 Windows 10+ 自带 tar.exe 或安装 7-Zip", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblStatus.Text = "❌ 下载失败";
                btnDownloadModel.Enabled = true;
                btnDownloadModel.Text = "⬇️ 重新下载模型";
            }
        }




        private void btnDeleteModel_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(modelPath))
            {
                if (MessageBox.Show("确定删除模型吗？", "确认", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    Directory.Delete(modelPath, true);
                    engine?.Dispose(); engine = null; vadProcessor = null; currentEngineType = EngineType.None;
                    lblEngine.Text = "引擎: 未加载";
                    lblEngine.ForeColor = Color.FromArgb(120, 120, 120);
                    _ = AutoCheckModel();
                }
            }
        }



        private readonly object _inputLock = new object();  // 保护键盘/剪贴板/引擎操作的线程安全锁
        private DateTime _lastPartialTime = DateTime.MinValue;  // partial 更新时间戳（节流用）
        private const int PartialThrottleMs = 200;  // partial 更新最小间隔（毫秒）
        private string _lastDisplayedPartial = "";  // 上次显示的 partial 文本（去重用）

        private void SimulateKeyboardInput(string text, bool isFinal = false)
        {
            if (string.IsNullOrWhiteSpace(text)) return;

            if (!isFinal)
            {
                // partial：不模拟到目标窗口（避免"BACKSPACE删除-粘贴"肉眼可见的闪烁）
                // 实时进度只在托盘图标 tooltip 上显示，让用户能看到识别中但不会出现闪烁
                try
                {
                    this.BeginInvoke((MethodInvoker)delegate {
                        trayIcon.Text = $"🎤 {text}";
                    });
                }
                catch { }
                return;
            }

            // final：原子化粘贴最终结果到目标窗口（一次性出现，不闪烁）
            lock (_inputLock)
            {
                try
                {
                    ForceForegroundWindow();

                    // 关键修复：如果 Alt 仍按着，先临时释放，避免 Alt+Space 触发系统菜单
                    // 场景：用户松开 D 时钩子拦截了 D 的 KEYUP，但 Ctrl/Alt 可能仍按着
                    // 此时发送 Space 会被系统识别为 Alt+Space，弹出窗口控制菜单
                    bool altDown = (GetAsyncKeyState(VK_MENU) & 0x8000) != 0;
                    if (altDown)
                    {
                        keybd_event((byte)Keys.Menu, 0, KEYEVENTF_KEYUP, IntPtr.Zero);
                        Thread.Sleep(20);
                    }

                    Clipboard.SetText(text);
                    Thread.Sleep(50);  // 增加等待时间，避免剪贴板未就绪就发送 ^v
                    SendKeys.SendWait("^v");
                    SendKeys.SendWait(" ");  // 最终结果加空格

                    // 不主动恢复 Alt：让用户自己控制 Alt 状态，避免干扰用户的实际输入
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"[SimulateKeyboardInput Error] {ex.Message}");
                    try { FallbackTypeText(text); } catch { }
                }
            }

            // UI 更新在 lock 外
            try
            {
                this.BeginInvoke((MethodInvoker)delegate {
                    lblStatus.Text = $"✅ 已输入: {text}";
                    trayIcon.Text = $"✅ 已输入: {text}";
                });
            }
            catch { }
        }

        // DeletePreviousPartial 已废弃：partial 不再模拟到目标窗口，无需删除中间结果



        private void InitMicrophone()
        {
            if (WaveInEvent.DeviceCount < 1) { lblStatus.Text = "❌ 未检测到麦克风！"; return; }
            waveIn = new WaveInEvent { DeviceNumber = 0, WaveFormat = new WaveFormat(16000, 1) };
            waveIn.DataAvailable += WaveIn_DataAvailable;
        }

        private void WaveIn_DataAvailable(object sender, WaveInEventArgs e)
        {
            if (!isRecording || engine == null) return;

            lock (_inputLock)
            {
                if (!isRecording) return;  // double-check：获取锁后可能已停止录音

                if (engine.SupportsStreaming && engine is SherpaEngine sherpaEngine)
                {
                    // 1GB Paraformer 流式处理
                    sherpaEngine.AcceptAudio(e.Buffer, e.BytesRecorded);

                    if (sherpaEngine.IsEndpoint())
                    {
                        // 端点检测（停顿分句）：获取结果并重置 stream（不调用 InputFinished）
                        string finalText = sherpaEngine.GetResultAndReset();
                        if (!string.IsNullOrWhiteSpace(finalText))
                            SimulateKeyboardInput(finalText, isFinal: true);
                    }
                    else
                    {
                        string partial = sherpaEngine.GetPartialResult();
                        // 节流+去重：仅当文本变化且距上次更新超过 200ms 时才更新
                        if (!string.IsNullOrEmpty(partial) && partial != _lastDisplayedPartial)
                        {
                            var now = DateTime.Now;
                            if ((now - _lastPartialTime).TotalMilliseconds >= PartialThrottleMs)
                            {
                                _lastPartialTime = now;
                                _lastDisplayedPartial = partial;
                                SimulateKeyboardInput(partial, isFinal: false);
                            }
                        }
                    }
                }
                else if (vadProcessor != null)
                {
                    // SenseVoice 等使用 VAD
                    vadProcessor.ProcessAudio(e.Buffer, e.BytesRecorded);
                }
                else if (engine != null && !engine.SupportsStreaming)
                {
                    // 离线模式（Paraformer-large 等）：积累音频，松开后一次性识别
                    engine.AcceptAudio(e.Buffer, e.BytesRecorded);
                }
            }
        }


        private void ForceForegroundWindow()
        {
            try
            {
                IntPtr targetWnd = GetForegroundWindow();

                // 如果前台窗口是自己的窗口（主窗体或录音提示窗），回退到录音前保存的目标窗口
                if (targetWnd == this.Handle ||
                    (recordingIndicator != null && targetWnd == recordingIndicator.Handle))
                {
                    targetWnd = _lastTargetWindow;
                }

                if (targetWnd == IntPtr.Zero || targetWnd == this.Handle) return;

                uint foreThread = GetWindowThreadProcessId(targetWnd, out _);
                uint thisThread = GetWindowThreadProcessId(this.Handle, out _);

                if (foreThread != thisThread)
                    AttachThreadInput(foreThread, thisThread, true);

                SetForegroundWindow(targetWnd);

                if (foreThread != thisThread)
                    AttachThreadInput(foreThread, thisThread, false);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[ForceForegroundWindow] {ex.Message}");
            }
        }

        public void StartRecording()
        {
            if (isRecording || engine == null || waveIn == null) return;

            lock (_inputLock)
            {
                if (isRecording) return;  // double-check
                isRecording = true;
                engine.Reset();
                vadProcessor?.Reset();
                waveIn.StartRecording();
            }

            // 在显示录音提示窗之前，保存当前目标窗口句柄（防止提示窗抢焦点）
            _lastTargetWindow = GetForegroundWindow();

            string modeHint = engine.SupportsStreaming ? " (流式)" : "";
            try
            {
                this.BeginInvoke((MethodInvoker)delegate {
                    lblStatus.Text = $"🎤 {engine.EngineName} 正在录音{modeHint}... (松开 Ctrl+Alt+D 结束)";
                    lblStatus.ForeColor = Color.FromArgb(200, 50, 50);
                    trayIcon.Text = $"🎤 {engine.EngineName} 录音中...";
                    ShowRecordingIndicator();
                });
            }
            catch { }
        }

        // ==================== 录音浮动提示 ====================

        private void ShowRecordingIndicator()
        {
            if (recordingIndicator == null)
            {
                recordingIndicator = new NonActivatingForm
                {
                    FormBorderStyle = FormBorderStyle.None,
                    StartPosition = FormStartPosition.Manual,
                    ShowInTaskbar = false,
                    TopMost = true,
                    Width = 180,
                    Height = 44,
                    BackColor = Color.FromArgb(230, 57, 70),
                    Opacity = 0.92,
                    ControlBox = false,
                    Text = ""
                };

                var lbl = new Label
                {
                    Text = "● 正在识别...(松开结束)",
                    ForeColor = Color.White,
                    Font = new Font("Microsoft YaHei UI", 11, FontStyle.Bold),
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleCenter
                };
                recordingIndicator.Controls.Add(lbl);

                // 圆角
                recordingIndicator.Region = System.Drawing.Region.FromHrgn(
                    CreateRoundRectRgn(0, 0, recordingIndicator.Width, recordingIndicator.Height, 16, 16));
            }

            // 定位到屏幕底部居中
            var screen = Screen.PrimaryScreen.WorkingArea;
            recordingIndicator.Location = new Point(
                screen.X + (screen.Width - recordingIndicator.Width) / 2,
                screen.Y + screen.Height - recordingIndicator.Height - 80);

            recordingIndicator.Show();
        }

        private void HideRecordingIndicator()
        {
            if (recordingIndicator != null && recordingIndicator.Visible)
                recordingIndicator.Hide();
        }

        [DllImport("gdi32.dll")]
        private static extern IntPtr CreateRoundRectRgn(int x1, int y1, int x2, int y2, int cx, int cy);

        public void StopRecording()
        {
            if (!isRecording) return;

            // 先停止录音设备（会触发最后的 DataAvailable），再在锁内设置 isRecording=false
            // 这样最后几帧音频不会被丢弃
            waveIn.StopRecording();

            // 获取 lock，确保录音线程的 DataAvailable 已完成
            lock (_inputLock)
            {
                isRecording = false;

                if (engine.SupportsStreaming && engine is SherpaEngine sherpaEngine)
                {
                    string text = sherpaEngine.GetFinalResult();
                    if (!string.IsNullOrWhiteSpace(text))
                        SimulateKeyboardInput(text, isFinal: true);  // reentrant lock，一次性粘贴
                }
                else if (vadProcessor != null && engine.SupportsStreaming)
                {
                    vadProcessor.Flush();
                }
                else if (engine != null && !engine.SupportsStreaming && vadProcessor == null)
                {
                    // 离线模型（Paraformer-large）：录音结束后一次性识别整段音频
                    string text = engine.GetFinalResult();
                    if (!string.IsNullOrWhiteSpace(text))
                        SimulateKeyboardInput(text, isFinal: true);
                }
            }

            _lastDisplayedPartial = "";  // 重置去重状态

            // UI 更新用 BeginInvoke 避免阻塞
            try
            {
                this.BeginInvoke((MethodInvoker)delegate {
                    lblStatus.Text = $"✅ {engine.EngineName} 识别完成，等待下次输入";
                    lblStatus.ForeColor = Color.FromArgb(40, 53, 147);
                    trayIcon.Text = $"t9s2t ({engine.EngineName}) 按住Ctrl+Alt+D";
                    HideRecordingIndicator();
                });
            }
            catch { }
        }




        // 最终保底方案：逐字符模拟输入（最慢但最稳）
        private void FallbackTypeText(string text)
        {
            try
            {
                foreach (char c in text)
                {
                    SendKeys.SendWait(c.ToString());
                    Thread.Sleep(8); // 适当延时
                }
            }
            catch { }
        }

        // ==================== 非激活窗体（录音提示用） ====================
        /// <summary>
        /// 不会抢夺焦点的窗体，用于录音提示悬浮窗。
        /// ShowWithoutActivate=true 阻止 Show() 激活窗口；
        /// WS_EX_NOACTIVATE(0x08000000) 阻止鼠标点击激活窗口。
        /// </summary>
        private class NonActivatingForm : Form
        {
            protected override CreateParams CreateParams
            {
                get
                {
                    var cp = base.CreateParams;
                    cp.ExStyle |= 0x08000000; // WS_EX_NOACTIVATE：阻止窗口被激活
                    return cp;
                }
            }
        }






    }
}
