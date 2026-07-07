using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace DeviceTest
{
    public class MainForm : Form
    {
        #region WinAPI

        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYUP = 0x0101;
        private const int WM_INPUT = 0x00FF;
        private const int RIM_TYPEKEYBOARD = 1;
        private const int RIDI_DEVICENAME = 0x20000007;
        private const uint RIDEV_INPUTSINK = 0x00000100;
        private const uint KEYEVENTF_KEYUP = 0x0002;

        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("user32.dll")]
        private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, IntPtr dwExtraInfo);

        [StructLayout(LayoutKind.Sequential)]
        private struct KBDLLHOOKSTRUCT
        {
            public uint vkCode;
            public uint scanCode;
            public uint flags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct RAWINPUTDEVICE
        {
            public ushort UsagePage;
            public ushort Usage;
            public uint Flags;
            public IntPtr WindowHandle;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct RAWINPUTHEADER
        {
            public uint Type;
            public uint Size;
            public IntPtr Device;
            public IntPtr WParam;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct RAWKEYBOARD
        {
            public ushort MakeCode;
            public ushort Flags;
            public ushort Reserved;
            public ushort VKey;
            public uint Message;
            public uint ExtraInformation;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct RAWINPUT
        {
            public RAWINPUTHEADER Header;
            public RAWKEYBOARD Keyboard;
        }

        [DllImport("user32.dll", SetLastError = true)]
        static extern uint RegisterRawInputDevices(RAWINPUTDEVICE[] pRawInputDevices, uint uiNumDevices, uint cbSize);

        [DllImport("user32.dll", SetLastError = true)]
        static extern uint GetRawInputData(IntPtr hRawInput, uint uiCommand, IntPtr pData, ref uint pcbSize, uint cbSizeHeader);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        static extern uint GetRawInputDeviceInfoW(IntPtr hDevice, uint uiCommand, StringBuilder pData, ref uint pcbSize);

        #endregion

        #region 字段

        private TextBox logBox;
        private Label statusLabel;
        private IntPtr _hookID = IntPtr.Zero;
        private LowLevelKeyboardProc _proc;
        private Dictionary<IntPtr, string> deviceCache = new Dictionary<IntPtr, string>();

        private const string T9_VID = "32C2";
        private const string T9_PID = "0012";

        // 记录当前按下的 NumPad0 的扫描码和对应设备
        private uint pendingScanCode = 0;
        private IntPtr pendingDevice = IntPtr.Zero;
        private bool isCtrlDown = false;

        #endregion

        public MainForm()
        {
            _proc = HookCallback;

            this.Text = "设备识别改键 v3";
            this.Size = new System.Drawing.Size(700, 500);

            statusLabel = new Label
            {
                Dock = DockStyle.Top,
                Height = 70,
                Text = "测试规则：T9键盘按0 → 发送Ctrl | 普通键盘按0 → 正常输出0\r\n请先打开记事本，然后分别在两个键盘上按NumPad0",
                Font = new System.Drawing.Font("Microsoft YaHei", 10),
                ForeColor = System.Drawing.Color.DarkBlue
            };
            this.Controls.Add(statusLabel);

            logBox = new TextBox
            {
                Dock = DockStyle.Fill,
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                ReadOnly = true,
                Font = new System.Drawing.Font("Consolas", 10)
            };
            this.Controls.Add(logBox);

            this.Shown += (s, e) => Start();
            this.FormClosing += (s, e) => Stop();
        }

        private void Start()
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                _hookID = SetWindowsHookEx(WH_KEYBOARD_LL, _proc, GetModuleHandle(curModule.ModuleName), 0);
            }

            if (_hookID == IntPtr.Zero)
            {
                Log("WH_KEYBOARD_LL 钩子注册失败！");
                return;
            }

            RAWINPUTDEVICE[] rid = new RAWINPUTDEVICE[1];
            rid[0].UsagePage = 0x01;
            rid[0].Usage = 0x06;
            rid[0].Flags = RIDEV_INPUTSINK;
            rid[0].WindowHandle = this.Handle;

            uint result = RegisterRawInputDevices(rid, 1, (uint)Marshal.SizeOf(typeof(RAWINPUTDEVICE)));
            if (result == 0)
            {
                Log("Raw Input 注册失败！");
                return;
            }

            Log("设备识别改键 v3 已启动");
            Log("========================================\r\n");
        }

        private void Stop()
        {
            if (_hookID != IntPtr.Zero)
            {
                UnhookWindowsHookEx(_hookID);
                _hookID = IntPtr.Zero;
            }
            if (isCtrlDown)
            {
                keybd_event((byte)Keys.ControlKey, 0, KEYEVENTF_KEYUP, IntPtr.Zero);
                isCtrlDown = false;
            }
        }

        #region WH_KEYBOARD_LL

        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                KBDLLHOOKSTRUCT kbData = Marshal.PtrToStructure<KBDLLHOOKSTRUCT>(lParam);
                bool isKeyDown = wParam == (IntPtr)WM_KEYDOWN;
                bool isKeyUp = wParam == (IntPtr)WM_KEYUP;
                uint vkCode = kbData.vkCode;

                if (vkCode == (uint)Keys.NumPad0)
                {
                    if (isKeyDown)
                    {
                        // 按下时：先记录扫描码，延迟判断设备
                        pendingScanCode = kbData.scanCode;
                        pendingDevice = IntPtr.Zero; // 清空，等 Raw Input 来填充

                        // 启动延迟检测，50ms 后判断
                        Timer delayTimer = new Timer { Interval = 50 };
                        delayTimer.Tick += (s, e) =>
                        {
                            delayTimer.Stop();
                            delayTimer.Dispose();
                            CheckAndAct();
                        };
                        delayTimer.Start();

                        Log($"NumPad0按下 scanCode={kbData.scanCode}，等待设备识别...");
                        return (IntPtr)1; // 先拦截
                    }
                    else if (isKeyUp)
                    {
                        Log($"NumPad0抬起");
                        if (isCtrlDown)
                        {
                            keybd_event((byte)Keys.ControlKey, 0, KEYEVENTF_KEYUP, IntPtr.Zero);
                            isCtrlDown = false;
                            Log("  → 发送 Ctrl 抬起");
                        }
                        return (IntPtr)1;
                    }
                }
            }

            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        /// <summary>
        /// 延迟后检查设备，决定行为
        /// </summary>
        private void CheckAndAct()
        {
            bool isT9 = pendingDevice != IntPtr.Zero && IsT9Device(pendingDevice);

            if (isT9)
            {
                // T9 键盘：发送 Ctrl
                if (!isCtrlDown)
                {
                    keybd_event((byte)Keys.ControlKey, 0, 0, IntPtr.Zero);
                    isCtrlDown = true;
                    Log("  → T9设备！发送 Ctrl 按下");
                }
            }
            else
            {
                // 普通键盘：放行原始的 0
                keybd_event((byte)Keys.NumPad0, 0, 0, IntPtr.Zero);
                keybd_event((byte)Keys.NumPad0, 0, KEYEVENTF_KEYUP, IntPtr.Zero);
                Log("  → 普通键盘，放行 0");
            }

            pendingScanCode = 0;
            pendingDevice = IntPtr.Zero;
        }

        #endregion

        #region Raw Input

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_INPUT)
            {
                RecordRawInput(m.LParam);
            }
            base.WndProc(ref m);
        }

        private void RecordRawInput(IntPtr hRawInput)
        {
            uint dwSize = 0;
            uint result = GetRawInputData(hRawInput, 0x10000003, IntPtr.Zero, ref dwSize, (uint)Marshal.SizeOf(typeof(RAWINPUTHEADER)));

            if (result == 0 && dwSize > 0)
            {
                IntPtr pData = Marshal.AllocHGlobal((int)dwSize);
                try
                {
                    result = GetRawInputData(hRawInput, 0x10000003, pData, ref dwSize, (uint)Marshal.SizeOf(typeof(RAWINPUTHEADER)));

                    if (result != unchecked((uint)(-1)))
                    {
                        RAWINPUT rawInput = Marshal.PtrToStructure<RAWINPUT>(pData);

                        if (rawInput.Header.Type == RIM_TYPEKEYBOARD && rawInput.Keyboard.VKey == (uint)Keys.NumPad0)
                        {
                            // 只要收到 NumPad0 的 Raw Input，就记录设备
                            if (pendingScanCode > 0 && pendingDevice == IntPtr.Zero)
                            {
                                pendingDevice = rawInput.Header.Device;
                                Log($"  Raw Input 设备确认: {(IsT9Device(rawInput.Header.Device) ? "T9" : "普通")}");
                            }
                        }
                    }
                }
                finally
                {
                    Marshal.FreeHGlobal(pData);
                }
            }
        }

        private bool IsT9Device(IntPtr hDevice)
        {
            if (deviceCache.ContainsKey(hDevice))
                return deviceCache[hDevice].Contains("T9");

            uint bufferSize = 0;
            GetRawInputDeviceInfoW(hDevice, RIDI_DEVICENAME, null, ref bufferSize);

            if (bufferSize > 0)
            {
                StringBuilder sb = new StringBuilder((int)bufferSize);
                uint nameResult = GetRawInputDeviceInfoW(hDevice, RIDI_DEVICENAME, sb, ref bufferSize);

                if (nameResult != unchecked((uint)(-1)))
                {
                    string deviceName = sb.ToString();
                    bool isT9 = deviceName.Contains($"VID_{T9_VID}") && deviceName.Contains($"PID_{T9_PID}");
                    deviceCache[hDevice] = isT9 ? $"T9: {deviceName}" : $"普通: {deviceName}";
                    return isT9;
                }
            }

            return false;
        }

        #endregion

        private void Log(string message)
        {
            if (logBox.InvokeRequired)
            {
                logBox.Invoke(new Action(() => Log(message)));
                return;
            }
            logBox.AppendText($"[{DateTime.Now:HH:mm:ss.fff}] {message}\r\n");
        }
    }
}
