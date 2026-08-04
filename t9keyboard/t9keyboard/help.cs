
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.Win32;
namespace t9keyboard
{
    public partial class help : Form
    {
        private const int WM_HOTKEY = 0x0312;
        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);
        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);
        private bool _adCollapsed = false;
        private readonly string _adStateFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "t9keyboard_ad_state.txt");
        // 开机自启：注册表项与值名称
        private const string AutoStartRegKeyPath = "Software\\Microsoft\\Windows\\CurrentVersion\\Run";
        private const string AutoStartRegValueName = "t9keyboard";
        private bool _autoStartInitializing = false;

        public help()
        {
            InitializeComponent();
            this.KeyPreview = true;
            textBox1.KeyDown += TextBox1_KeyDown;
        }
        // WinAPI函数，用于获取按键的扫描码
        [DllImport("user32.dll")]
        public static extern int GetKeyboardState(byte[] lpKeyState);
        // WinAPI常量
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_SYSKEYDOWN = 0x0104;
        private const int MOD_CONTROL = 0x0002;
        private const int MOD_ALT = 0x0001;
        private const int MOD_SHIFT = 0x0004;
        private int ParseModifiers(string shortcut)
        {
            int modifiers = 0;
            if (shortcut.Contains("Ctrl")) modifiers |= MOD_CONTROL;
            if (shortcut.Contains("Alt")) modifiers |= MOD_ALT;
            if (shortcut.Contains("Shift")) modifiers |= MOD_SHIFT;
            return modifiers;
        }
        private int ParseKey(string shortcut)
        {
            string[] parts = shortcut.Split('+');
            string keyString = parts[parts.Length - 1];
            return (int)(Keys)Enum.Parse(typeof(Keys), keyString);
        }
        private void button4_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(MOD_CONTROL.ToString());
            //MessageBox.Show(ParseKey(textBox1.Text).ToString());
            //RegisterHotKey(Handle, 0, MOD_CONTROL, 84);
            RegisterHotKey(Handle, 0, ParseModifiers(textBox1.Text), ParseKey(textBox1.Text));
        }
        //private const int MOD_CONTROL = 0x0002; // Ctrl修饰键的标志位
        //[DllImport("user32.dll", SetLastError = true)]
        //private static extern int GetLastError();
        //private void button4_Click(object sender, EventArgs e)
        //{
        // int fsModifiers = MOD_CONTROL; // Ctrl修饰键
        // int vk = (int)Keys.T; // T键的键码
        // bool result = RegisterHotKey(Handle, 0, fsModifiers, vk);
        // if (!result)
        // {
        // int error = GetLastError();
        // MessageBox.Show("注册热键失败，错误代码：" + error.ToString());
        // }
        // else
        // {
        // MessageBox.Show("注册热键成功！");
        // }
        //}
        //响应快捷键
        public bool isHookActive = true;
        //protected override void WndProc(ref Message m)
        //{
        // base.WndProc(ref m);
        // if (m.Msg == WM_HOTKEY)
        // {
        // int virtualKey = m.WParam.ToInt32();
        // int scanCode = (m.LParam.ToInt32() >> 16) & 0xFF;
        // // 显示扫描码
        // //label9.Text= scanCode.ToString();
        // MessageBox.Show($"Virtual Key: {virtualKey}\nScan Code: {scanCode}", "按键信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
        // }
        //}
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == WM_HOTKEY)
            {
                if (isHookActive)
                {
                    _keyboardHook.Dispose();
                    //MessageBox.Show("KeyboardHook 已释放");
                    notifyIcon.BalloonTipTitle = "小白T9输入法软键盘";
                    notifyIcon.BalloonTipText = "快捷键已释放。";
                    notifyIcon.ShowBalloonTip(3000);
                    _keyboardHook.Dispose();
                }
                else
                {
                    _keyboardHook = new KeyboardHook(_checkBox1, _checkBox2, _checkBox3, _num7t, _num8t, _num9t, _num4t, _num5t, _num6t, _num1t, _num2t, _num3t, _num0t, _numlockt, _numchut, _numchengt, _numjiant, _numjiat, _numdiant, _numentert, _homet, _shangt, _pgupt, _zuot, _cleart, _yout, _endt, _xiat, _pgdnt, _inst, _delt);
                    //MessageBox.Show("KeyboardHook 已初始化");
                    notifyIcon.BalloonTipTitle = "小白T9输入法软键盘";
                    notifyIcon.BalloonTipText = "快捷键已注册！！";
                    notifyIcon.ShowBalloonTip(3000);
                }
                isHookActive = !isHookActive;
            }
        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            UnregisterHotKey(Handle, 0);
            base.OnFormClosing(e);
        }
        private KeyboardHook _keyboardHook;
        private TextBox _num7t;
        private TextBox _num8t;
        private TextBox _num9t;
        private TextBox _num4t;
        private TextBox _num5t;
        private TextBox _num6t;
        private TextBox _num1t;
        private TextBox _num2t;
        private TextBox _num3t;
        private TextBox _num0t;
        private TextBox _numlockt;
        private TextBox _numchut;
        private TextBox _numchengt;
        private TextBox _numjiant;
        private TextBox _numjiat;
        private TextBox _numdiant;
        private TextBox _numentert;
        private TextBox _homet;
        private TextBox _shangt;
        private TextBox _pgupt;
        private TextBox _zuot;
        private TextBox _cleart;
        private TextBox _yout;
        private TextBox _endt;
        private TextBox _xiat;
        private TextBox _pgdnt;
        private TextBox _inst;
        private TextBox _delt;
        private CheckBox _checkBox1;
        private CheckBox _checkBox2;
        private CheckBox _checkBox3;
        private CheckBox _checkBox1_placeholder = new CheckBox();
        private CheckBox _checkBox2_placeholder = new CheckBox();
        private CheckBox _checkBox3_placeholder = new CheckBox();
        private TextBox _num7t_placeholder = new TextBox();
        private TextBox _num8t_placeholder = new TextBox();
        private void TextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            // 清空 TextBox 中的内容
            textBox1.Clear();
            // 获取按下的键
            string key = e.KeyCode.ToString();
            // 检查是否有修饰键被按下
            if (e.Control)
                key = "Ctrl + " + key;
            if (e.Shift)
                key = "Shift + " + key;
            if (e.Alt)
                key = "Alt + " + key;
            // 在 TextBox 中显示按下的键
            textBox1.Text = key;
            // 阻止事件进一步传递
            e.Handled = true;
        }
        //public static int v = Color.Red.ToArgb();
        //public static Color sc = Color.FromArgb(172, 174, 187);
        //public static Color qc = Color.FromArgb(255, 255, 255);
        //public static Color bc = Color.FromArgb(200, 200, 200);
        //public static Color sc = Color.FromArgb(Convert.ToInt32("-5460293"));
        //public static Color qc = Color.FromArgb(Convert.ToInt32("-1"));
        //public static Color bc = Color.FromArgb(Convert.ToInt32("-3618616"));
        public static Color sc = Color.FromArgb(Convert.ToInt32(System.IO.File.ReadAllLines(System.Windows.Forms.Application.StartupPath + "\\data\\t9keyboard.txt")[System.IO.File.ReadAllLines(System.Windows.Forms.Application.StartupPath + "\\data\\t9keyboard.txt").Length - 5]));
        public static Color qc = Color.FromArgb(Convert.ToInt32(System.IO.File.ReadAllLines(System.Windows.Forms.Application.StartupPath + "\\data\\t9keyboard.txt")[System.IO.File.ReadAllLines(System.Windows.Forms.Application.StartupPath + "\\data\\t9keyboard.txt").Length - 4]));
        public static Color bc = Color.FromArgb(Convert.ToInt32(System.IO.File.ReadAllLines(System.Windows.Forms.Application.StartupPath + "\\data\\t9keyboard.txt")[System.IO.File.ReadAllLines(System.Windows.Forms.Application.StartupPath + "\\data\\t9keyboard.txt").Length - 3]));
        private bool isCheckBoxChecked;
        private void help_Load(object sender, EventArgs e)
        {
            // IsCheckBoxChecked = checkBox1.Checked;
            //MessageBox.Show(System.Windows.Forms.Application.StartupPath + "\\data\\t9keyboard.txt");
            bc1.BackColor = sc;
            bc2.BackColor = qc;
            bc3.BackColor = bc;
            //读取txt改键
            string filePath = Path.Combine(Application.StartupPath, "data", "t9keyboard01.txt");
            if (File.Exists(filePath))
            {
                var lines = File.ReadAllLines(filePath);
                var numchutLine = lines.FirstOrDefault(line => line.StartsWith("numchut="));
                if (numchutLine != null)
                {
                    numchut.Text = numchutLine.Substring("numchut=".Length).Trim();
                }
                var numchengtLine = lines.FirstOrDefault(line => line.StartsWith("numchengt="));
                if (numchengtLine != null)
                {
                    numchengt.Text = numchengtLine.Substring("numchengt=".Length).Trim();
                }
                var numjiantLine = lines.FirstOrDefault(line => line.StartsWith("numjiant="));
                if (numjiantLine != null)
                {
                    numjiant.Text = numjiantLine.Substring("numjiant=".Length).Trim();
                }
                var numjiatLine = lines.FirstOrDefault(line => line.StartsWith("numjiat="));
                if (numjiatLine != null)
                {
                    numjiat.Text = numjiatLine.Substring("numjiat=".Length).Trim();
                }
                var numdiantLine = lines.FirstOrDefault(line => line.StartsWith("numdiant="));
                if (numdiantLine != null)
                {
                    numdiant.Text = numdiantLine.Substring("numdiant=".Length).Trim();
                }
                var numentertLine = lines.FirstOrDefault(line => line.StartsWith("numentert="));
                if (numentertLine != null)
                {
                    numentert.Text = numentertLine.Substring("numentert=".Length).Trim();
                }
                var num7tLine = lines.FirstOrDefault(line => line.StartsWith("num7t="));
                if (num7tLine != null)
                {
                    num7t.Text = num7tLine.Substring("num7t=".Length).Trim();
                }
                var num8tLine = lines.FirstOrDefault(line => line.StartsWith("num8t="));
                if (num8tLine != null)
                {
                    num8t.Text = num8tLine.Substring("num8t=".Length).Trim();
                }
                var num9tLine = lines.FirstOrDefault(line => line.StartsWith("num9t="));
                if (num9tLine != null)
                {
                    num9t.Text = num9tLine.Substring("num9t=".Length).Trim();
                }
                var num4tLine = lines.FirstOrDefault(line => line.StartsWith("num4t="));
                if (num4tLine != null)
                {
                    num4t.Text = num4tLine.Substring("num4t=".Length).Trim();
                }
                var num5tLine = lines.FirstOrDefault(line => line.StartsWith("num5t="));
                if (num5tLine != null)
                {
                    num5t.Text = num5tLine.Substring("num5t=".Length).Trim();
                }
                var num6tLine = lines.FirstOrDefault(line => line.StartsWith("num6t="));
                if (num6tLine != null)
                {
                    num6t.Text = num6tLine.Substring("num6t=".Length).Trim();
                }
                var num1tLine = lines.FirstOrDefault(line => line.StartsWith("num1t="));
                if (num1tLine != null)
                {
                    num1t.Text = num1tLine.Substring("num1t=".Length).Trim();
                }
                var num2tLine = lines.FirstOrDefault(line => line.StartsWith("num2t="));
                if (num2tLine != null)
                {
                    num2t.Text = num2tLine.Substring("num2t=".Length).Trim();
                }
                var num3tLine = lines.FirstOrDefault(line => line.StartsWith("num3t="));
                if (num3tLine != null)
                {
                    num3t.Text = num3tLine.Substring("num3t=".Length).Trim();
                }
                var num0tLine = lines.FirstOrDefault(line => line.StartsWith("num0t="));
                if (num0tLine != null)
                {
                    num0t.Text = num0tLine.Substring("num0t=".Length).Trim();
                }
                var numlockLine = lines.FirstOrDefault(line => line.StartsWith("numlock="));
                if (numlockLine != null)
                {
                    bool numlockValue;
                    if (bool.TryParse(numlockLine.Substring("numlock=".Length).Trim(), out numlockValue))
                    {
                        checkBox1.Checked = numlockValue;
                    }
                    else
                    {
                        MessageBox.Show("无法解析 numlock 值");
                    }
                }
                var kjjLine = lines.FirstOrDefault(line => line.StartsWith("kjj="));
                if (kjjLine != null)
                {
                    textBox1.Text = kjjLine.Substring("kjj=".Length).Trim();
                }
                var space0Line = lines.FirstOrDefault(line => line.StartsWith("space0="));
                if (space0Line != null)
                {
                    bool space0Value;
                    if (bool.TryParse(space0Line.Substring("space0=".Length).Trim(), out space0Value))
                    {
                        checkBox2.Checked = space0Value;
                    }
                }
                var decimaltobackLine = lines.FirstOrDefault(line => line.StartsWith("decimaltoback="));
                if (decimaltobackLine != null)
                {
                    bool decimaltobackValue;
                    if (bool.TryParse(decimaltobackLine.Substring("decimaltoback=".Length).Trim(), out decimaltobackValue))
                    {
                        checkBox3.Checked = decimaltobackValue;
                    }
                }
            }
            else
            {
                MessageBox.Show("文件未找到：" + filePath);
            }

            // Load QR images
            string appDir = AppDomain.CurrentDomain.BaseDirectory;
            try { string p = Path.Combine(appDir, "wechat_qr.png"); if (System.IO.File.Exists(p)) qrWechat.Image = Image.FromFile(p); } catch { }
            try { string p = Path.Combine(appDir, "alipay_qr.png"); if (System.IO.File.Exists(p)) qrAlipay.Image = Image.FromFile(p); } catch { }
            try { string p = Path.Combine(appDir, "pdd_qrcode.png"); if (System.IO.File.Exists(p)) qrPdd.Image = Image.FromFile(p); } catch { }

            // Restore ad panel state
            try { if (System.IO.File.Exists(_adStateFile) && System.IO.File.ReadAllText(_adStateFile).Trim() == "1") _adCollapsed = true; } catch { }
            // Wire up ad panel events
            this.adPanel.Paint += adPanel_Paint;
            this.btnToggleAd.Click += btnToggleAd_Click;
            ApplyAdState();

            // 初始化开机自启勾选框状态（读取注册表，避免触发事件）
            _autoStartInitializing = true;
            try
            {
                chkAutoStart.Checked = IsAutoStartEnabled();
            }
            catch (Exception ex)
            {
                chkAutoStart.Checked = false;
                MessageBox.Show("读取开机自启设置失败：" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                _autoStartInitializing = false;
            }
        }
        // 读取注册表，判断当前是否已设置开机自启
        private bool IsAutoStartEnabled()
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(AutoStartRegKeyPath, false))
            {
                if (key != null)
                {
                    object value = key.GetValue(AutoStartRegValueName);
                    if (value != null)
                    {
                        string exePath = "\"" + Application.ExecutablePath + "\"";
                        return value.ToString().Trim() == exePath.Trim();
                    }
                }
            }
            return false;
        }
        // 设置/取消开机自启
        private void SetAutoStart(bool enable)
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(AutoStartRegKeyPath, true))
            {
                if (key == null)
                {
                    throw new Exception("无法打开注册表启动项。");
                }
                if (enable)
                {
                    // 带引号写入，防止路径含空格时启动失败
                    key.SetValue(AutoStartRegValueName, "\"" + Application.ExecutablePath + "\"");
                }
                else
                {
                    if (key.GetValue(AutoStartRegValueName) != null)
                    {
                        key.DeleteValue(AutoStartRegValueName, false);
                    }
                }
            }
        }
        // 开机自启勾选框状态改变事件
        private void chkAutoStart_CheckedChanged(object sender, EventArgs e)
        {
            // 初始化阶段仅同步状态，不写注册表
            if (_autoStartInitializing)
            {
                return;
            }
            try
            {
                SetAutoStart(chkAutoStart.Checked);
            }
            catch (Exception ex)
            {
                MessageBox.Show("设置开机自启失败：" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                // 写入失败时回滚勾选框状态，避免界面与实际不一致
                _autoStartInitializing = true;
                try
                {
                    chkAutoStart.Checked = IsAutoStartEnabled();
                }
                catch
                {
                    chkAutoStart.Checked = false;
                }
                finally
                {
                    _autoStartInitializing = false;
                }
            }
        }
        //写回txt
        private void SaveValuesToFile()
        {
            string filePath = Path.Combine(Application.StartupPath, "data", "t9keyboard01.txt");
            if (File.Exists(filePath))
            {
                var lines = File.ReadAllLines(filePath).ToList();
                ReplaceValue(lines, "numchut=", numchut.Text);
                ReplaceValue(lines, "numchengt=", numchengt.Text);
                ReplaceValue(lines, "numjiant=", numjiant.Text);
                ReplaceValue(lines, "numjiat=", numjiat.Text);
                ReplaceValue(lines, "numdiant=", numdiant.Text);
                ReplaceValue(lines, "numentert=", numentert.Text);
                ReplaceValue(lines, "num7t=", num7t.Text);
                ReplaceValue(lines, "num8t=", num8t.Text);
                ReplaceValue(lines, "num9t=", num9t.Text);
                ReplaceValue(lines, "num4t=", num4t.Text);
                ReplaceValue(lines, "num5t=", num5t.Text);
                ReplaceValue(lines, "num6t=", num6t.Text);
                ReplaceValue(lines, "num1t=", num1t.Text);
                ReplaceValue(lines, "num2t=", num2t.Text);
                ReplaceValue(lines, "num3t=", num3t.Text);
                ReplaceValue(lines, "num0t=", num0t.Text);
                ReplaceValue(lines, "numlock=", checkBox1.Checked.ToString());
                ReplaceValue(lines, "kjj=", textBox1.Text);
                ReplaceValue(lines, "space0=", checkBox2.Checked.ToString());
                ReplaceValue(lines, "decimaltoback=", checkBox3.Checked.ToString());
                File.WriteAllLines(filePath, lines);
            }
            else
            {
                MessageBox.Show("文件未找到：" + filePath);
            }
        }
        private void ReplaceValue(List<string> lines, string key, string value)
        {
            for (int i = 0; i < lines.Count; i++)
            {
                if (lines[i].StartsWith(key))
                {
                    lines[i] = key + value;
                    break;
                }
            }
        }
        public void yes_Click(object sender, EventArgs e)
        {
            sc = bc1.BackColor;
            qc = bc2.BackColor;
            bc = bc3.BackColor;
            string W = System.IO.File.ReadAllLines(System.Windows.Forms.Application.StartupPath + "\\data\\t9keyboard.txt")[System.IO.File.ReadAllLines(System.Windows.Forms.Application.StartupPath + "\\data\\t9keyboard.txt").Length - 2];
            string H = System.IO.File.ReadAllLines(System.Windows.Forms.Application.StartupPath + "\\data\\t9keyboard.txt")[System.IO.File.ReadAllLines(System.Windows.Forms.Application.StartupPath + "\\data\\t9keyboard.txt").Length - 1];
            List<string> lines = new List<string>(File.ReadAllLines(System.Windows.Forms.Application.StartupPath + "\\data\\t9keyboard.txt"));
            lines.RemoveAt(lines.Count - 1); // 删除最后一行
            lines.RemoveAt(lines.Count - 1); // 删除最后一行
            lines.RemoveAt(lines.Count - 1); // 删除最后一行
            lines.RemoveAt(lines.Count - 1); // 删除最后一行
            lines.RemoveAt(lines.Count - 1); // 删除最后一行
            File.WriteAllLines(System.Windows.Forms.Application.StartupPath + "\\data\\t9keyboard.txt", lines.ToArray());
            FileStream fs = new FileStream(@System.Windows.Forms.Application.StartupPath + "\\data\\t9keyboard.txt", FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter m_streamWriter = new StreamWriter(fs);
            m_streamWriter.BaseStream.Seek(0, SeekOrigin.End);
            m_streamWriter.WriteLine(string.Format(sc.ToArgb().ToString()));
            m_streamWriter.WriteLine(string.Format(qc.ToArgb().ToString()));
            m_streamWriter.WriteLine(string.Format(bc.ToArgb().ToString()));
            m_streamWriter.WriteLine(W);
            m_streamWriter.WriteLine(H);
            m_streamWriter.Flush();
            m_streamWriter.Close();
            //改键
            _num7t = num7t;
            _num8t = num8t;
            _num9t = num9t;
            _num4t = num4t;
            _num5t = num5t;
            _num6t = num6t;
            _num1t = num1t;
            _num2t = num2t;
            _num3t = num3t;
            _num0t = num0t;
            _numlockt = numlockt;
            _numchut = numchut;
            _numchengt = numchengt;
            _numjiant = numjiant;
            _numjiat = numjiat;
            _numdiant = numdiant;
            _numentert = numentert;
            _checkBox1 = checkBox1;
            _checkBox2 = checkBox2;
            _checkBox3 = checkBox3;
            //_homet = homet;
            //_shangt = shangt;
            //_pgupt = pgupt;
            //_zuot = zuot;
            //_cleart = cleart;
            //_yout = yout;
            //_endt = endt;
            //_xiat = xiat;
            //_pgdnt = pgdnt;
            //_inst = inst;
            //_delt = delt;
            if (_keyboardHook != null)
            {
                _keyboardHook.Dispose();
            }
            _keyboardHook = new KeyboardHook(_checkBox1, _checkBox2, _checkBox3, _num7t, _num8t, _num9t, _num4t, _num5t, _num6t, _num1t, _num2t, _num3t, _num0t, _numlockt, _numchut, _numchengt, _numjiant, _numjiat, _numdiant, _numentert, _homet, _shangt, _pgupt, _zuot, _cleart, _yout, _endt, _xiat, _pgdnt, _inst, _delt);
            //注册快捷键
            RegisterHotKey(Handle, 0, ParseModifiers(textBox1.Text), ParseKey(textBox1.Text));
            //写回txt
            SaveValuesToFile();
            // 隐藏当前窗体
            this.Hide();
            foreach (Form form in Application.OpenForms)
            {
                if (form is keyboard)
                {
                    ((keyboard)form).hide();
                }
                if (form is numboard)
                {
                    ((numboard)form).hide();
                }
                if (form is enboard)
                {
                    ((enboard)form).hide();
                }
            }
            keyboard f4 = Form2.FindOrCreateForm<keyboard>();
            f4.Show();
        }
        private void bc1_Click(object sender, EventArgs e)
        {
            colorDialog1.FullOpen = true;
            colorDialog1.ShowDialog();
            bc1.BackColor = colorDialog1.Color;
        }
        private void bc2_Click(object sender, EventArgs e)
        {
            colorDialog1.FullOpen = true;
            colorDialog1.ShowDialog();
            bc2.BackColor = colorDialog1.Color;
        }
        private void bc3_Click(object sender, EventArgs e)
        {
            colorDialog1.FullOpen = true;
            colorDialog1.ShowDialog();
            bc3.BackColor = colorDialog1.Color;
        }
        private void label5_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://xiaobai.pro/");
        }
        private void label6_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://qm.qq.com/cgi-bin/qm/qr?k=Rk9iBrUCsP4W-OG8vVT5Rx9Pvt6LLKJr&authKey=9t4fuekg6KcJM67QKAOGsWKV6W57pzOs9yDRip8sgjo1wzAwi8aImC6Xz2DYQ81s&noverify=0&group_code=387170746");
        }
        private void defaultcolor_Click(object sender, EventArgs e)
        {
            bc1.BackColor = Color.FromArgb(172, 174, 187);
            bc2.BackColor = Color.FromArgb(255, 255, 255);
            bc3.BackColor = Color.FromArgb(200, 200, 200);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            List<string> lines = new List<string>(File.ReadAllLines(System.Windows.Forms.Application.StartupPath + "\\data\\t9keyboard.txt"));
            lines.RemoveAt(lines.Count - 1);//删除最后一行
            lines.RemoveAt(lines.Count - 1);//删除最后一行
            File.WriteAllLines(System.Windows.Forms.Application.StartupPath + "\\data\\t9keyboard.txt", lines.ToArray());
            FileStream fs = new FileStream(@System.Windows.Forms.Application.StartupPath + "\\data\\t9keyboard.txt", FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter m_streamWriter = new StreamWriter(fs);
            m_streamWriter.BaseStream.Seek(0, SeekOrigin.End);
            m_streamWriter.WriteLine("459");
            m_streamWriter.WriteLine("322");
            m_streamWriter.Flush();
            m_streamWriter.Close();
            //this.Close();
            yes.PerformClick();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            // MessageBox.Show(_num7t.Text);
            //MessageBox.Show("Button2 clicked!");
        }
        private void button3_Click(object sender, EventArgs e)
        {
            _keyboardHook.Dispose();
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void adPanel_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            using (var tf = new Font("Microsoft YaHei UI", 11F, FontStyle.Bold))
            using (var tb = new SolidBrush(Color.FromArgb(200, 80, 40)))
            {
                var w = g.MeasureString("支持小白", tf).Width;
                g.DrawString("支持小白", tf, tb, (190 - w) / 2, 4);
            }
            using (var lf = new Font("Microsoft YaHei UI", 8.5F, FontStyle.Bold))
            using (var sf = new Font("Microsoft YaHei UI", 7.5F))
            using (var gb = new SolidBrush(Color.FromArgb(80, 80, 80)))
            using (var db = new SolidBrush(Color.FromArgb(50, 50, 50)))
            using (var rb = new SolidBrush(Color.FromArgb(210, 50, 20)))
            using (var pen = new Pen(Color.FromArgb(230, 210, 190), 1))
            {
                // 微信部分（位置基本不变）
                var w1 = g.MeasureString("微信打赏", lf).Width;
                g.DrawString("微信打赏", lf, db, (190 - w1) / 2, 132);
                var w1d = g.MeasureString("感谢支持小白输入法", sf).Width;
                g.DrawString("感谢支持小白输入法", sf, gb, (190 - w1d) / 2, 148);
                g.DrawLine(pen, 20, 140, 170, 140);

                // 支付宝部分（稍微往下挪一点）
                var w2 = g.MeasureString("支付宝打赏", lf).Width;
                g.DrawString("支付宝打赏", lf, db, (190 - w2) / 2, 272);
                var w2d = g.MeasureString("感谢支持小白输入法", sf).Width;
                g.DrawString("感谢支持小白输入法", sf, gb, (190 - w2d) / 2, 288);
                g.DrawLine(pen, 20, 280, 170, 280);

                // ==================== 修改后的拼多多部分 ====================
                var w3 = g.MeasureString("拼多多店铺", lf).Width;
                g.DrawString("拼多多店铺", lf, db, (190 - w3) / 2, 412);

                string secondLine = "捐助98元获赠小白T9无线键盘一台";
                var w3d = g.MeasureString(secondLine, sf).Width;
                g.DrawString(secondLine, sf, gb, (190 - w3d) / 2, 428);   // 第二行

                // 分割线移到文字下方
                g.DrawLine(pen, 20, 455, 170, 455);
            }
        }

        private void btnToggleAd_Click(object sender, EventArgs e)
        {
            _adCollapsed = !_adCollapsed;
            ApplyAdState();
            try { System.IO.File.WriteAllText(_adStateFile, _adCollapsed ? "1" : "0"); } catch { }
        }

        private void ApplyAdState()
        {
            if (_adCollapsed)
            {
                adPanel.Visible = false;
                btnToggleAd.Text = ">";
                btnToggleAd.Location = new System.Drawing.Point(734, 180);
                this.ClientSize = new System.Drawing.Size(754, 436);
            }
            else
            {
                adPanel.Visible = true;
                btnToggleAd.Text = "<";
                btnToggleAd.Location = new System.Drawing.Point(734, 180);
                this.ClientSize = new System.Drawing.Size(954, 480);
            }
        }
    }
    public class KeyboardHook : IDisposable
    {
        private int _hookId = 0;
        private LowLevelKeyboardProc _proc;
        private IntPtr _hookHandle = IntPtr.Zero;
        private TextBox _num7t;
        private TextBox _num8t;
        private TextBox _num9t;
        private TextBox _num4t;
        private TextBox _num5t;
        private TextBox _num6t;
        private TextBox _num1t;
        private TextBox _num2t;
        private TextBox _num3t;
        private TextBox _num0t;
        private TextBox _numchut;
        private TextBox _numchengt;
        private TextBox _numjiant;
        private TextBox _numjiat;
        private TextBox _numdiant;
        private TextBox _numentert;
        private TextBox _homet;
        private TextBox _shangt;
        private TextBox _pgupt;
        private TextBox _zuot;
        private TextBox _cleart;
        private TextBox _yout;
        private TextBox _endt;
        private TextBox _xiat;
        private TextBox _pgdnt;
        private TextBox _inst;
        private TextBox _delt;
        private CheckBox _checkBox1;
        private CheckBox _checkBox2;
        private CheckBox _checkBox3;
        public KeyboardHook(CheckBox checkBox1, CheckBox checkBox2, CheckBox checkBox3, TextBox num7t, TextBox num8t, TextBox num9t, TextBox num4t, TextBox num5t, TextBox num6t, TextBox num1t, TextBox num2t, TextBox num3t, TextBox num0t, TextBox numlockt, TextBox numchut, TextBox numchengt, TextBox numjiant, TextBox numjiat, TextBox numdiant, TextBox numentert, TextBox homet, TextBox shangt, TextBox pgupt, TextBox zuot, TextBox cleart, TextBox yout, TextBox endt, TextBox xiat, TextBox pgdnt, TextBox inst, TextBox delt)
        {
            _proc = new LowLevelKeyboardProc(HookCallback);
            _hookHandle = SetHook(_proc);
            _num7t = num7t;
            _num8t = num8t;
            _num9t = num9t;
            _num4t = num4t;
            _num5t = num5t;
            _num6t = num6t;
            _num1t = num1t;
            _num2t = num2t;
            _num3t = num3t;
            _num0t = num0t;
            _numchut = numchut;
            _numchengt = numchengt;
            _numjiant = numjiant;
            _numjiat = numjiat;
            _numdiant = numdiant;
            _numentert = numentert;
            _homet = homet;
            _shangt = shangt;
            _pgupt = pgupt;
            _zuot = zuot;
            _cleart = cleart;
            _yout = yout;
            _endt = endt;
            _xiat = xiat;
            _pgdnt = pgdnt;
            _inst = inst;
            _delt = delt;
            _checkBox1 = checkBox1;
            _checkBox2 = checkBox2;
            _checkBox3 = checkBox3;
        }
        ~KeyboardHook()
        {
            UnhookWindowsHookEx(_hookHandle);
        }
        public void Dispose()
        {
            UnhookWindowsHookEx(_hookHandle);
        }
        private IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
            }
        }
        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);
        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
            {
                int vkCode = Marshal.ReadInt32(lParam);
                // 新功能：小键盘0键映射为空格
                if (vkCode == (int)Keys.NumPad0) // 物理小键盘0键的虚拟键码是96
                {
                    if (_checkBox2.Checked)
                    {
                        // 模拟按下空格键
                        keybd_event((byte)Keys.Space, 0, 0, IntPtr.Zero);
                        keybd_event((byte)Keys.Space, 0, KEYEVENTF_KEYUP, IntPtr.Zero);
                        return (IntPtr)1; // 阻止原 NumPad0 事件传递
                    }
                    else
                    {
                        // 不勾选时正常输出 NumPad0，直接放行
                        return CallNextHookEx(_hookId, nCode, wParam, lParam);
                    }
                }
                // 新功能：小键盘 . (Decimal/Del) 键映射为退格键
                if (vkCode == (int)Keys.Decimal || vkCode == (int)Keys.Delete) // Decimal 和 Delete 实际上是同一个物理键
                {
                    if (_checkBox3.Checked)
                    {
                        // 模拟按下退格键 (Backspace)
                        keybd_event((byte)Keys.Back, 0, 0, IntPtr.Zero);
                        keybd_event((byte)Keys.Back, 0, KEYEVENTF_KEYUP, IntPtr.Zero);
                        return (IntPtr)1; // 阻止原按键事件
                    }
                    else
                    {
                        // 不勾选时正常处理（输出 . 或 Delete，取决于 NumLock）
                        return CallNextHookEx(_hookId, nCode, wParam, lParam);
                    }
                }
                Keys key = (Keys)vkCode;
                // MessageBox.Show(_checkBox1.Checked.ToString());
                //MessageBox.Show(key.ToString().ToLower());
                //修改小键盘关闭为大数字
                if (_checkBox1.Checked == true && key.ToString().ToLower() == "home")
                //if (_checkBox1.Checked == true && vkCode == 36) // 36 是 Home 键的虚拟键码
                {
                    key = Keys.D7;
                    // 发送模拟按键事件
                    PressNumPadKey(Keys.D7);
                    return (IntPtr)1; // 阻止原按键的默认处理
                }
                if (_checkBox1.Checked == true && key.ToString().ToLower() == "up")
                //if (_checkBox1.Checked == true && vkCode == 38) // 38 是 Up 键的虚拟键码
                {
                    key = Keys.D8;
                    // 发送模拟按键事件
                    PressNumPadKey(Keys.D8);
                    return (IntPtr)1; // 阻止原按键的默认处理
                }
                if (_checkBox1.Checked == true && key.ToString().ToLower() == "pageup")
                {
                    key = Keys.D9;
                    // 发送模拟按键事件
                    PressNumPadKey(Keys.D9);
                    return (IntPtr)1; // 阻止原按键的默认处理
                }
                if (_checkBox1.Checked == true && key.ToString().ToLower() == "left")
                {
                    key = Keys.D4;
                    // 发送模拟按键事件
                    PressNumPadKey(Keys.D4);
                    return (IntPtr)1; // 阻止原按键的默认处理
                }
                if (_checkBox1.Checked == true && key.ToString().ToLower() == "clear")
                {
                    key = Keys.D5;
                    // 发送模拟按键事件
                    PressNumPadKey(Keys.D5);
                    return (IntPtr)1; // 阻止原按键的默认处理
                }
                //MessageBox.Show(key.ToString());
                if (_checkBox1.Checked == true && key.ToString().ToLower() == "right")
                {
                    key = Keys.D6;
                    // 发送模拟按键事件
                    PressNumPadKey(Keys.D6);
                    return (IntPtr)1; // 阻止原按键的默认处理
                }
                if (_checkBox1.Checked == true && key.ToString().ToLower() == "end")
                {
                    key = Keys.D1;
                    // 发送模拟按键事件
                    PressNumPadKey(Keys.D1);
                    return (IntPtr)1; // 阻止原按键的默认处理
                }
                if (_checkBox1.Checked == true && key.ToString().ToLower() == "down")
                {
                    key = Keys.D2;
                    // 发送模拟按键事件
                    PressNumPadKey(Keys.D2);
                    return (IntPtr)1; // 阻止原按键的默认处理
                }
                if (_checkBox1.Checked == true && key.ToString().ToLower() == "next")
                {
                    key = Keys.D3;
                    // 发送模拟按键事件
                    PressNumPadKey(Keys.D3);
                    return (IntPtr)1; // 阻止原按键的默认处理
                }
                if (_checkBox1.Checked == true && key.ToString().ToLower() == "insert")
                {
                    key = Keys.D0;
                    // 发送模拟按键事件
                    PressNumPadKey(Keys.D0);
                    return (IntPtr)1; // 阻止原按键的默认处理
                }
                if (_checkBox1.Checked == true && key.ToString().ToLower() == "delete")
                {
                    key = Keys.Back;
                    // 发送模拟按键事件
                    PressNumPadKey(Keys.Back);
                    return (IntPtr)1; // 阻止原按键的默认处理
                }
                // 映射 num7t.text 中指定的按键为数字小键盘7按钮
                if (key.ToString().ToLower() == _num7t.Text.ToLower())
                {
                    key = Keys.NumPad7;
                    // 发送模拟按键事件
                    PressNumPadKey(Keys.NumPad7);
                    return (IntPtr)1; // 阻止原按键的默认处理
                }
                // 映射 num8t.text 中指定的按键为数字小键盘8按钮
                if (key.ToString().ToLower() == _num8t.Text.ToLower())
                {
                    key = Keys.NumPad8;
                    // 发送模拟按键事件
                    PressNumPadKey(Keys.NumPad8);
                    return (IntPtr)1; // 阻止原按键的默认处理
                }
                // 映射 num9t.text 中指定的按键为数字小键盘9按钮
                if (key.ToString().ToLower() == _num9t.Text.ToLower())
                {
                    key = Keys.NumPad9;
                    // 发送模拟按键事件
                    PressNumPadKey(Keys.NumPad9);
                    return (IntPtr)1; // 阻止原按键的默认处理
                }
                // 映射 num4t.text 中指定的按键为数字小键盘4按钮
                if (key.ToString().ToLower() == _num4t.Text.ToLower())
                {
                    key = Keys.NumPad4;
                    // 发送模拟按键事件
                    PressNumPadKey(Keys.NumPad4);
                    return (IntPtr)1; // 阻止原按键的默认处理
                }
                // 映射 num5t.text 中指定的按键为数字小键盘5按钮
                if (key.ToString().ToLower() == _num5t.Text.ToLower())
                {
                    key = Keys.NumPad5;
                    // 发送模拟按键事件
                    PressNumPadKey(Keys.NumPad5);
                    return (IntPtr)1; // 阻止原按键的默认处理
                }
                // 映射 num6t.text 中指定的按键为数字小键盘9按钮
                if (key.ToString().ToLower() == _num6t.Text.ToLower())
                {
                    key = Keys.NumPad6;
                    // 发送模拟按键事件
                    PressNumPadKey(Keys.NumPad6);
                    return (IntPtr)1; // 阻止原按键的默认处理
                }
                // 映射 num1t.text 中指定的按键为数字小键盘1按钮
                if (key.ToString().ToLower() == _num1t.Text.ToLower())
                {
                    key = Keys.NumPad1;
                    // 发送模拟按键事件
                    PressNumPadKey(Keys.NumPad1);
                    return (IntPtr)1; // 阻止原按键的默认处理
                }
                // 映射 num2t.text 中指定的按键为数字小键盘2按钮
                if (key.ToString().ToLower() == _num2t.Text.ToLower())
                {
                    key = Keys.NumPad2;
                    // 发送模拟按键事件
                    PressNumPadKey(Keys.NumPad2);
                    return (IntPtr)1; // 阻止原按键的默认处理
                }
                // 映射 num3t.text 中指定的按键为数字小键盘3按钮
                if (key.ToString().ToLower() == _num3t.Text.ToLower())
                {
                    key = Keys.NumPad3;
                    // 发送模拟按键事件
                    PressNumPadKey(Keys.NumPad3);
                    return (IntPtr)1; // 阻止原按键的默认处理
                }
                // 映射 num0t.text 中指定的按键为数字小键盘0按钮
                if (key.ToString().ToLower() == _num0t.Text.ToLower())
                {
                    key = Keys.NumPad0;
                    // 发送模拟按键事件
                    PressNumPadKey(Keys.NumPad0);
                    return (IntPtr)1; // 阻止原按键的默认处理
                }
                // 映射 加号
                if (key.ToString().ToLower() == _numjiat.Text.ToLower())
                {
                    key = Keys.Add;
                    // 发送模拟按键事件
                    PressNumPadKey(Keys.Add);
                    return (IntPtr)1; // 阻止原按键的默认处理
                }
                // 映射 减号
                if (key.ToString().ToLower() == _numjiant.Text.ToLower())
                {
                    key = Keys.Subtract;
                    // 发送模拟按键事件
                    PressNumPadKey(Keys.Subtract);
                    return (IntPtr)1; // 阻止原按键的默认处理
                }
                // 映射 星号
                if (key.ToString().ToLower() == _numchengt.Text.ToLower())
                {
                    key = Keys.Multiply;
                    // 发送模拟按键事件
                    PressNumPadKey(Keys.Multiply);
                    return (IntPtr)1; // 阻止原按键的默认处理
                }
                // 映射 斜杠
                if (key.ToString().ToLower() == _numchut.Text.ToLower())
                {
                    key = Keys.Divide;
                    // 发送模拟按键事件
                    PressNumPadKey(Keys.Divide);
                    return (IntPtr)1; // 阻止原按键的默认处理
                }
                // 映射 小数点
                if (key.ToString().ToLower() == _numdiant.Text.ToLower())
                {
                    key = Keys.Decimal;
                    // 发送模拟按键事件
                    PressNumPadKey(Keys.Decimal);
                    return (IntPtr)1; // 阻止原按键的默认处理
                }
                // 映射 小键盘回车键
                if (key.ToString().ToLower() == _numentert.Text.ToLower())
                {
                    key = Keys.Enter;
                    // 发送模拟按键事件
                    PressNumPadKey(Keys.Enter);
                    return (IntPtr)1; // 阻止原按键的默认处理
                }
            }
            return CallNextHookEx(_hookId, nCode, wParam, lParam);
        }
        private const int KEYEVENTF_EXTENDEDKEY = 0x0001;
        private const int KEYEVENTF_KEYUP = 0x0002;
        private void PressNumPadKey(Keys key)
        {
            keybd_event((byte)key, 0, KEYEVENTF_EXTENDEDKEY, IntPtr.Zero);
            keybd_event((byte)key, 0, KEYEVENTF_KEYUP, IntPtr.Zero);
        }
        [DllImport("user32.dll")]
        public static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, IntPtr dwExtraInfo);


        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(int hhk, int nCode, IntPtr wParam, IntPtr lParam);
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);
    }
}
