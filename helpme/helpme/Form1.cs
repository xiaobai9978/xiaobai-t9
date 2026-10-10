using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace helpme
{
    public partial class Form1 : Form
    {
        // ================= API 声明 =================
        [DllImport("user32.dll", SetLastError = true)]
        private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelMouseProc lpfn, IntPtr hMod, uint dwThreadId);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);
        [DllImport("user32.dll")] private static extern IntPtr GetForegroundWindow();
        [DllImport("imm32.dll")] private static extern IntPtr ImmGetContext(IntPtr hwnd);
        [DllImport("imm32.dll")] private static extern bool ImmReleaseContext(IntPtr hwnd, IntPtr hIMC);
        [DllImport("imm32.dll")] private static extern bool ImmGetConversionStatus(IntPtr hIMC, out uint lpdw, out uint lpdw2);

        private const uint IME_CMODE_NATIVE = 0x0001;
        private const byte VK_LWIN = 0x5B, VK_SPACE = 0x20, VK_SHIFT = 0x10, VK_NUMLOCK = 0x90, VK_CAPITAL = 0x14;

        private const uint KEYEVENTF_EXTENDEDKEY = 0x0001;
        private const uint KEYEVENTF_KEYUP = 0x0002;
        private const byte VK_NUMPAD0 = 0x60;
        private const int WH_MOUSE_LL = 14, WM_LBUTTONDOWN = 0x0201, WM_RBUTTONDOWN = 0x0204;

        private delegate IntPtr LowLevelMouseProc(int nCode, IntPtr wParam, IntPtr lParam);
        private LowLevelMouseProc _mouseProc;
        private IntPtr _hookID = IntPtr.Zero;

        private System.Windows.Forms.Timer actionTimer;
        private bool isWinPressed = false;
        private bool isHandlingClick = false;

        // 箭头覆盖层与闪烁定时器
        private ArrowOverlay arrowOverlay;
        private System.Windows.Forms.Timer blinkTimer;
        private int blinkTickCount = 0;

        // ================= 广告面板状态 =================
        private bool _adCollapsed = false;
        private readonly string _adStateFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "helpme_ad_state.txt");
        private Image _imgWechat, _imgAlipay, _imgTaobao;

        public Form1()
        {
            InitializeComponent();
            InitializeAdPanel();
            InitializeCustomTimer();
            InitializeArrowOverlay();

            _mouseProc = HookCallback;

            this.FormClosing += Form1_FormClosing;
            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;

            StyleUI();
            AppendLog("系统初始化成功。请点击左侧按钮启动切换流程.");
        }

        // ================= 广告面板方法 =================
        private void InitializeAdPanel()
        {
            var asm = System.Reflection.Assembly.GetExecutingAssembly();
            try { using (var s = asm.GetManifestResourceStream("helpme.Properties.wechat_qr.png")) { if (s != null) _imgWechat = Image.FromStream(s); } } catch { }
            try { using (var s = asm.GetManifestResourceStream("helpme.Properties.alipay_qr.png")) { if (s != null) _imgAlipay = Image.FromStream(s); } } catch { }
            try { using (var s = asm.GetManifestResourceStream("helpme.Properties.taobao_qrcode.png")) { if (s != null) _imgTaobao = Image.FromStream(s); } } catch { }

            try { if (File.Exists(_adStateFile) && File.ReadAllText(_adStateFile).Trim() == "1") _adCollapsed = true; } catch { }
            ApplyAdState();
        }

        private void adPanel_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            int pw = adPanel.ClientSize.Width;
            int ph = adPanel.ClientSize.Height;

            // 按比例计算
            int imgSize = (int)(pw * 0.42);
            int imgX = (pw - imgSize) / 2;
            float secH = ph / 3f; // 每个区块高度

            // 标题
            using (var tf = new Font("Microsoft YaHei UI", 10F, FontStyle.Bold))
            using (var tb = new SolidBrush(Color.FromArgb(200, 80, 40)))
            {
                var w = g.MeasureString("支持小白", tf).Width;
                g.DrawString("支持小白", tf, tb, (pw - w) / 2, ph * 0.01f);
            }

            // 文字与分割线
            using (var lf = new Font("Microsoft YaHei UI", 8F, FontStyle.Bold))
            using (var sf = new Font("Microsoft YaHei UI", 7F))
            using (var db = new SolidBrush(Color.FromArgb(50, 50, 50)))
            using (var gb = new SolidBrush(Color.FromArgb(80, 80, 80)))
            using (var rb = new SolidBrush(Color.FromArgb(210, 50, 20)))
            using (var pen = new Pen(Color.FromArgb(230, 210, 190), 1))
            {
                // 区块1: 微信
                int y1img = (int)(secH * 0.15f);
                int y1txt = y1img + imgSize + 2;
                if (_imgWechat != null) g.DrawImage(_imgWechat, imgX, y1img, imgSize, imgSize);
                var w1 = g.MeasureString("微信打赏", lf).Width;
                g.DrawString("微信打赏", lf, db, (pw - w1) / 2, y1txt);
                g.DrawLine(pen, pw * 0.1f, secH * 0.97f, pw * 0.9f, secH * 0.97f);

                // 区块2: 支付宝
                int y2img = (int)(secH + secH * 0.15f);
                int y2txt = y2img + imgSize + 2;
                if (_imgAlipay != null) g.DrawImage(_imgAlipay, imgX, y2img, imgSize, imgSize);
                var w2 = g.MeasureString("支付宝打赏", lf).Width;
                g.DrawString("支付宝打赏", lf, db, (pw - w2) / 2, y2txt);
                g.DrawLine(pen, pw * 0.1f, secH * 1.97f, pw * 0.9f, secH * 1.97f);

                // 区块3: 淘宝
                int y3img = (int)(secH * 2 + secH * 0.1f);
                int y3txt = y3img + imgSize + 2;
                if (_imgTaobao != null) g.DrawImage(_imgTaobao, imgX, y3img, imgSize, imgSize);
                var w3 = g.MeasureString("淘宝店铺", lf).Width;
                g.DrawString("淘宝店铺", lf, db, (pw - w3) / 2, y3txt);
                var w3a = g.MeasureString("捐助98元送小白T9无线键盘一台", sf).Width;
                g.DrawString("捐助98元送小白T9无线键盘一台", sf, rb, (pw - w3a) / 2, y3txt + 16);
            }
        }










































        private void btnToggleAd_Click(object sender, EventArgs e)
        {
            _adCollapsed = !_adCollapsed;
            ApplyAdState();
            try { File.WriteAllText(_adStateFile, _adCollapsed ? "1" : "0"); } catch { }
        }

        private void ApplyAdState()
        {
            int h = this.ClientSize.Height;
            int adW = adPanel.Width;
            if (_adCollapsed)
            {
                adPanel.Location = new Point(3000, 0);
                btnToggleAd.Text = ">";
                this.ClientSize = new Size(this.ClientSize.Width - adW, h);
                if (arrowOverlay != null) arrowOverlay.RightOffset = 0;
            }
            else
            {
                adPanel.Location = new Point(this.ClientSize.Width, 0);
                btnToggleAd.Text = "<";
                this.ClientSize = new Size(this.ClientSize.Width + adW, h);
                if (arrowOverlay != null) arrowOverlay.RightOffset = adW;
            }
            btnToggleAd.Location = new Point(this.ClientSize.Width - btnToggleAd.Width, btnToggleAd.Location.Y);
            btnToggleAd.BringToFront();
        }




































        private void InitializeCustomTimer()
        {
            actionTimer = new System.Windows.Forms.Timer();
            actionTimer.Interval = 15000;
            actionTimer.Tick += ActionTimer_Tick;
        }

        // ================= 大箭头覆盖层逻辑 =================
        private void InitializeArrowOverlay()
        {
            arrowOverlay = new ArrowOverlay();
            arrowOverlay.Dock = DockStyle.Fill;
            arrowOverlay.BackColor = Color.Transparent;
            arrowOverlay.Visible = false;
            // 放入 panelRight 内部，避免与 form 上其他 Dock=Fill 控件冲突
            panelRight.Controls.Add(arrowOverlay);
            arrowOverlay.BringToFront();

            blinkTimer = new System.Windows.Forms.Timer();
            blinkTimer.Interval = 40;
            blinkTimer.Tick += (s, e) =>
            {
                blinkTickCount++;
                double rad = (blinkTickCount % 20) / 20.0 * Math.PI * 2;
                int alpha = (int)(150 + 105 * Math.Sin(rad));
                arrowOverlay.ArrowAlpha = alpha;
                arrowOverlay.Invalidate();

                if (blinkTickCount > 250) HideArrow();
            };
        }

        private void ShowArrow()
        {
            blinkTickCount = 0;
            arrowOverlay.Visible = true;
            arrowOverlay.BringToFront();
            blinkTimer.Start();
        }

        private void HideArrow()
        {
            blinkTimer.Stop();
            arrowOverlay.Visible = false;
        }

        // ================= 业务逻辑 =================
        private void AppendLog(string message)
        {
            if (txtLog == null) return;
            if (txtLog.InvokeRequired) { txtLog.BeginInvoke(new Action(() => AppendLog(message))); return; }
            txtLog.AppendText($"[{DateTime.Now:HH:mm:ss}] {message}{Environment.NewLine}");
        }

        private void EnsureT9s2tRunning()
        {
            try
            {
                if (Process.GetProcessesByName("t9s2t").Length > 0)
                {
                    AppendLog("【正常】t9s2t.exe 已经在运行中。");
                    return;
                }

                string exePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "t9s2t", "t9s2t.exe");
                if (File.Exists(exePath))
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = exePath,
                        WorkingDirectory = Path.GetDirectoryName(exePath),
                        UseShellExecute = true
                    });
                    AppendLog("【启动】t9s2t.exe 已自动拉起。");
                    System.Threading.Thread.Sleep(2000);
                }
                else
                {
                    AppendLog("【警告】未找到 t9s2t.exe，路径: " + exePath);
                }
            }
            catch (Exception ex)
            {
                AppendLog($"【失败】启动 t9s2t.exe 异常: {ex.Message}");
            }
        }

        private void btnSwitch_Click_1(object sender, EventArgs e)
        {
            EnsureT9s2tRunning();

            ResetRunningState();
            isHandlingClick = false;
            AppendLog("已唤醒系统输入法侧边栏，等待用户选择...");

            try
            {
                string numkeyboardPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "numkeyboard.exe");
                if (File.Exists(numkeyboardPath))
                {
                    if (Process.GetProcessesByName("numkeyboard").Length == 0)
                    {
                        Process.Start(numkeyboardPath);
                        AppendLog("【启动】numkeyboard.exe 已拉起。");
                    }
                    else
                    {
                        AppendLog("【提示】numkeyboard.exe 已经在运行中。");
                    }
                }
                else
                {
                    AppendLog("【警告】未在当前路径下找到 numkeyboard.exe，请检查文件是否存在。");
                }
            }
            catch (Exception ex)
            {
                AppendLog($"【失败】拉起 numkeyboard.exe 异常: {ex.Message}");
            }

            keybd_event(VK_LWIN, 0x5B, KEYEVENTF_EXTENDEDKEY, UIntPtr.Zero);
            keybd_event(VK_SPACE, 0, 0, UIntPtr.Zero);
            keybd_event(VK_SPACE, 0, KEYEVENTF_KEYUP, UIntPtr.Zero);
            isWinPressed = true;

            _hookID = SetHook(_mouseProc);
            actionTimer.Start();

            ShowArrow();
        }

        // ================= 核心延时流修复 =================
        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && (wParam == (IntPtr)WM_LBUTTONDOWN || wParam == (IntPtr)WM_RBUTTONDOWN))
            {
                if (!isHandlingClick)
                {
                    isHandlingClick = true;
                    actionTimer.Stop();

                    this.BeginInvoke((MethodInvoker)HideArrow);
                    AppendLog("检测到鼠标点击，已放行点击。正在安全排队延时...");

                    System.Threading.Tasks.Task.Run(async () =>
                    {
                        await System.Threading.Tasks.Task.Delay(200);

                        this.BeginInvoke((MethodInvoker)delegate
                        {
                            if (_hookID != IntPtr.Zero)
                            {
                                UnhookWindowsHookEx(_hookID);
                                _hookID = IntPtr.Zero;
                                AppendLog("鼠标钩子已安全异步释放.");
                            }
                        });

                        ReleaseWinKey();

                        await System.Threading.Tasks.Task.Delay(300);

                        this.BeginInvoke((MethodInvoker)delegate
                        {
                            this.Activate();
                            if (textBox1 != null) { textBox1.Focus(); AppendLog("焦点已成功强制锁定回测试框。"); }
                        });

                        await System.Threading.Tasks.Task.Delay(200);

                        this.BeginInvoke((MethodInvoker)delegate
                        {
                            isHandlingClick = false;
                            ExecutePostSwitchTasks();
                        });
                    });
                }
            }
            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        private void ActionTimer_Tick(object sender, EventArgs e)
        {
            AppendLog("超时未检测到点击，自动收回并执行环境初始化...");
            ResetRunningState();
            ReleaseWinKey();
            HideArrow();

            this.Activate();
            if (textBox1 != null) textBox1.Focus();
            ExecutePostSwitchTasks();
        }

        private void ResetRunningState()
        {
            actionTimer.Stop();
            if (_hookID != IntPtr.Zero) { UnhookWindowsHookEx(_hookID); _hookID = IntPtr.Zero; }
        }

        private void ReleaseWinKey()
        {
            keybd_event(VK_LWIN, 0x5B, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, UIntPtr.Zero);
            isWinPressed = false;
            AppendLog("底层 Win 键已安全释放。");
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            ResetRunningState();
            ReleaseWinKey();
            blinkTimer?.Stop();
            _imgWechat?.Dispose(); _imgWechat = null;
            _imgAlipay?.Dispose(); _imgAlipay = null;
            _imgTaobao?.Dispose(); _imgTaobao = null;
        }

        private void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            keybd_event(VK_LWIN, 0x5B, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, UIntPtr.Zero);
        }

        private IntPtr SetHook(LowLevelMouseProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_MOUSE_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private void ExecutePostSwitchTasks()
        {
            try
            {
                string weaselPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "WeaselServer.exe");
                if (File.Exists(weaselPath) && Process.GetProcessesByName("WeaselServer").Length == 0)
                {
                    Process.Start(weaselPath);
                    AppendLog("【启动】WeaselServer.exe 已拉起。");
                }

                AppendLog("开始执行自动化环境核验...");
                if (!Console.NumberLock) { keybd_event(VK_NUMLOCK, 0, 0, UIntPtr.Zero); keybd_event(VK_NUMLOCK, 0, KEYEVENTF_KEYUP, UIntPtr.Zero); AppendLog("【调整】小键盘已开启。"); }
                else AppendLog("【正常】小键盘已开启。");

                if (Console.CapsLock) { keybd_event(VK_CAPITAL, 0, 0, UIntPtr.Zero); keybd_event(VK_CAPITAL, 0, KEYEVENTF_KEYUP, UIntPtr.Zero); AppendLog("【调整】大写锁已关闭。"); }
                else AppendLog("【正常】大写锁已关闭。");

                EnsureChineseMode();

                System.Threading.Thread.Sleep(150);

                SimulateNumpadInput("64486");

                AppendLog("====== 自动化环境调整全部完成 ======");
            }
            catch (Exception ex) { AppendLog($"【失败】异常: {ex.Message}"); }
        }

        private void EnsureChineseMode()
        {
            IntPtr hwnd = GetForegroundWindow();
            IntPtr hIMC = ImmGetContext(hwnd);
            bool isChinese = false;
            if (hIMC != IntPtr.Zero)
            {
                uint conversion, sentence;
                if (ImmGetConversionStatus(hIMC, out conversion, out sentence)) isChinese = (conversion & IME_CMODE_NATIVE) != 0;
                ImmReleaseContext(hwnd, hIMC);
            }
            if (!isChinese)
            {
                keybd_event(VK_SHIFT, 0, 0, UIntPtr.Zero);
                keybd_event(VK_SHIFT, 0, KEYEVENTF_KEYUP, UIntPtr.Zero);
                AppendLog("【调整】已发送 SHIFT 激活中文。");
            }
            else AppendLog("【正常】已是中文模式。");
        }

        private void SimulateNumpadInput(string digits)
        {
            AppendLog($"【输入】开始自动输入小键盘序列: '{digits}'");
            foreach (char c in digits)
            {
                byte vk = (byte)(VK_NUMPAD0 + (c - '0'));
                keybd_event(vk, 0, 0, UIntPtr.Zero);
                System.Threading.Thread.Sleep(30);
                keybd_event(vk, 0, KEYEVENTF_KEYUP, UIntPtr.Zero);
                System.Threading.Thread.Sleep(60);
            }
        }

        private void StyleUI()
        {
            this.Font = new Font("微软雅黑", 10F);
            btnSwitch.FlatStyle = FlatStyle.Flat;
            btnSwitch.BackColor = Color.FromArgb(0, 122, 204);
            btnSwitch.FlatAppearance.BorderSize = 0;
            btnSwitch.Cursor = Cursors.Hand;
        }
    }

    // ================= 自定义 GDI+ 箭头绘制控件 =================
    public class ArrowOverlay : Panel
    {
        public int ArrowAlpha { get; set; } = 255;
        public int RightOffset { get; set; } = 0;
        public ArrowOverlay()
        {
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            int w = this.Width - RightOffset; // 减去广告宽度
            int h = this.Height;

            PointF start = new PointF(w * 0.25f, h * 0.25f);
            PointF end = new PointF(w * 0.80f, h * 0.80f);

            using (Pen pen = new Pen(Color.FromArgb(ArrowAlpha, 220, 20, 20), 22f))
            {
                pen.StartCap = LineCap.Round;
                pen.CustomEndCap = new AdjustableArrowCap(4.5f, 4.5f, true);
                g.DrawLine(pen, start, end);
            }

            using (Font font = new Font("微软雅黑", 26f, FontStyle.Bold))
            using (Brush brush = new SolidBrush(Color.FromArgb(ArrowAlpha, 220, 20, 20)))
            {
                string text = " \n\n右下方请选择！！\n\"小白T9输入法\"！！";
                g.DrawString(text, font, brush, w * 0.30f, h * 0.50f);
            }
        }
    }
}
