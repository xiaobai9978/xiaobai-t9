using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using RFID;
using System.Threading;
using System.IO;
using System.Diagnostics;




namespace t9keyboard
{









    public partial class keyboard : Form
    {


        public keyboard()
        {
            InitializeComponent();
        }


        float X;
        float Y;



        private const int WS_EX_TOOLWINDOW = 0x00000080;
        private const int WS_EX_NOACTIVATE = 0x08000000;
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= (WS_EX_NOACTIVATE | WS_EX_TOOLWINDOW);
                cp.Parent = IntPtr.Zero;
                return cp;
            }
        }






        private void Form1_Load(object sender, EventArgs e)
        {

            int sizex = int.Parse(System.IO.File.ReadAllLines(System.Windows.Forms.Application.StartupPath + "\\data\\t9keyboard.txt")[System.IO.File.ReadAllLines(System.Windows.Forms.Application.StartupPath + "\\data\\t9keyboard.txt").Length - 2]);
            int sizey = int.Parse(System.IO.File.ReadAllLines(System.Windows.Forms.Application.StartupPath + "\\data\\t9keyboard.txt")[System.IO.File.ReadAllLines(System.Windows.Forms.Application.StartupPath + "\\data\\t9keyboard.txt").Length - 1]);


            int x = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Size.Width - sizex;
            int y = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Size.Height - sizey;



            this.SetDesktopLocation(x, y);
            qc = help.qc;
            sc = help.sc;
            bc = help.bc;

            this.BackColor = bc;
            one.BackColor = sc;
            two.BackColor = sc;
            three.BackColor = sc;
            four.BackColor = sc;
            five.BackColor = sc;
            six.BackColor = sc;
            seven.BackColor = sc;
            eight.BackColor = sc;
            nine.BackColor = sc;
            zero.BackColor = sc;
            dou.BackColor = sc;
            ju.BackColor = sc;
            wen.BackColor = sc;
            gan.BackColor = sc;
            backspace.BackColor = sc;
            enter.BackColor = sc;
            jia.BackColor = sc;
            jian.BackColor = sc;
            cheng.BackColor = sc;
            chu.BackColor = sc;
            t2e.BackColor = sc;
            t2n.BackColor = sc;
            b1.BackColor = qc;
            b2.BackColor = qc;
            b3.BackColor = qc;
            b4.BackColor = qc;
            b5.BackColor = qc;
            b6.BackColor = qc;
            b7.BackColor = qc;
            b8.BackColor = qc;
            b9.BackColor = qc;
            b0.BackColor = qc;





            this.Resize += new EventHandler(Form1_Resize);//窗体调整大小时引发事件

            X = this.Width;//获取窗体的宽度

            Y = this.Height;//获取窗体的高度

            setTag(this);//调用方法


            this.Width = sizex;
            this.Height = sizey;
           // this.one.Click -= new System.EventHandler(this.one_Click); // 彻底移除原有的Click响应
            this.one.MouseUp -= new System.Windows.Forms.MouseEventHandler(this.one_MouseUp); // 防止重复绑定
            this.one.MouseUp += new System.Windows.Forms.MouseEventHandler(this.one_MouseUp); // 绑定新的 MouseUp 事件

        }






        private void setTag(Control cons)

        {
            //遍历窗体中的控件

            foreach (Control con in cons.Controls)

            {
                con.Tag = con.Width + ":" + con.Height + ":" + con.Left + ":" + con.Top + ":" + con.Font.Size;

                if (con.Controls.Count > 0)

                    setTag(con);

            }

        }




        private void setControls(float newx, float newy, Control cons)

        {
            //遍历窗体中的控件，重新设置控件的值

            foreach (Control con in cons.Controls)

            {
                string[] mytag = con.Tag.ToString().Split(new char[] { ':' });//获取控件的Tag属性值，并分割后存储字符串数组

                float a = Convert.ToSingle(mytag[0]) * newx;//根据窗体缩放比例确定控件的值，宽度

                con.Width = (int)a;//宽度

                a = Convert.ToSingle(mytag[1]) * newy;//高度

                con.Height = (int)(a);

                a = Convert.ToSingle(mytag[2]) * newx;//左边距离

                con.Left = (int)(a);

                a = Convert.ToSingle(mytag[3]) * newy;//上边缘距离

                con.Top = (int)(a);

                Single currentSize = Convert.ToSingle(mytag[4]) * newy;//字体大小

                con.Font = new Font(con.Font.Name, currentSize, con.Font.Style, con.Font.Unit);

                if (con.Controls.Count > 0)

                {
                    setControls(newx, newy, con);

                }

            }

        }


        


        void Form1_Resize(object sender, EventArgs e)

        {
            float newx = (this.Width) / X; //窗体宽度缩放比例

            float newy = this.Height / Y;//窗体高度缩放比例

            setControls(newx, newy, this);//随窗体改变控件大小

            //  this.Text = this.Width.ToString() + " " + this.Height.ToString();//窗体标题栏文本

            List<string> lines = new List<string>(File.ReadAllLines(System.Windows.Forms.Application.StartupPath + "\\data\\t9keyboard.txt"));
            lines.RemoveAt(lines.Count - 1);//删除最后一行
            lines.RemoveAt(lines.Count - 1);//删除最后一行
            File.WriteAllLines(System.Windows.Forms.Application.StartupPath + "\\data\\t9keyboard.txt", lines.ToArray());


            FileStream fs = new FileStream(@System.Windows.Forms.Application.StartupPath + "\\data\\t9keyboard.txt", FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter m_streamWriter = new StreamWriter(fs);
            m_streamWriter.BaseStream.Seek(0, SeekOrigin.End);

            m_streamWriter.WriteLine(this.Width);
            m_streamWriter.WriteLine(this.Height);
            m_streamWriter.Flush();
            m_streamWriter.Close();

        }






        [DllImport("User32.dll")]
        public static extern void keybd_event(Byte bVk, Byte bScan, Int32 dwFlags, Int32 dwExtraInfo);
        byte VK_NUM1 = 97;
        byte VK_NUM2 = 98;
        byte VK_NUM3 = 99;
        byte VK_NUM4 = 100;
        byte VK_NUM5 = 101;
        byte VK_NUM6 = 102;
        byte VK_NUM7 = 103;
        byte VK_NUM8 = 104;
        byte VK_NUM9 = 105;
        byte VK_NUM0 = 96;
        private const int KEYEVENTF_KEYUP = 0x0002;





        //private void backspace_Click(object sender, EventArgs e)
        //{
        //    SendKeys.Send("{BACKSPACE}");
        //}

        private void enter_Click(object sender, EventArgs e)
        {
            SendKeys.Send("{enter}");
        }

        //private void chu_Click(object sender, EventArgs e)
        //{
        //    SendKeys.Send("{DIVIDE}");
        //}

        //private void jia_Click(object sender, EventArgs e)
        //{
        //    SendKeys.Send("{ADD}");
        //}

        //private void jian_Click(object sender, EventArgs e)
        //{
        //    SendKeys.Send("{SUBTRACT}");
        //}

        //private void cheng_Click(object sender, EventArgs e)
        //{
        //    SendKeys.Send("{MULTIPLY}");
        //}

        private void one_MouseUp(object sender, MouseEventArgs e)
        {
            // 检查是否是鼠标左键
            if (e.Button == MouseButtons.Left)
            {
                SendKeys.Send("{1}");
            }
            // 检查是否是鼠标右键
            else if (e.Button == MouseButtons.Right)
            {
                SendKeys.Send("{ESC}"); // 触发 ESC 键
            }
        }

        private void two_Click(object sender, EventArgs e)
        {
            SendKeys.Send("{2}");
            //            new SendMsg().SendText("2");
        }

        private void three_Click(object sender, EventArgs e)
        {
            SendKeys.Send("{3}");
            //          new SendMsg().SendText("3");
        }

        private void four_Click(object sender, EventArgs e)
        {
            SendKeys.Send("{4}");
            //  new SendMsg().SendText("4");
        }

        private void five_Click(object sender, EventArgs e)
        {
            SendKeys.Send("{5}");
            // new SendMsg().SendText("5");
        }

        private void six_Click(object sender, EventArgs e)
        {
            SendKeys.Send("{6}");
            //  new SendMsg().SendText("6");
        }

        private void seven_Click(object sender, EventArgs e)
        {

            SendKeys.Send("{7}");

        }

        private void eight_Click(object sender, EventArgs e)
        {
            SendKeys.Send("{8}");
            // new SendMsg().SendText("8");
        }

        private void nine_Click(object sender, EventArgs e)
        {
            SendKeys.Send("{9}");
            //  new SendMsg().SendText("9");
        }

        private void zero_Click(object sender, EventArgs e)
        {
            SendKeys.Send("{0}");
            //   new SendMsg().SendText("0");
        }

 






       // int v = help.v;
        Color sc = help.sc;
        Color qc = help.qc;
        Color bc = help.bc;


        private void dou_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                new SendMsg().SendText(",");
                gan.BackColor = sc;
            }
            else
            {
                new SendMsg().SendText("，");
                gan.BackColor = sc;
            }
        }


        private void ju_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                new SendMsg().SendText(".");
                gan.BackColor = sc;
            }
            else
            {
                new SendMsg().SendText("。");
                gan.BackColor = sc;
            }
        }


        private void wen_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                new SendMsg().SendText("?");
                gan.BackColor = sc;
            }
            else
            {
                new SendMsg().SendText("？");
                gan.BackColor = sc;
            }
        }


        private void gan_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                new SendMsg().SendText("!");
                gan.BackColor = sc;
            }
            else
            {
                new SendMsg().SendText("！");
                gan.BackColor = sc;
            }
        }

        




        private void jia_MouseDown(object sender, MouseEventArgs e)
        {
            // 短按（左键）= 方向键↓，长按（右键）= 运算符 +
            if (e.Button == MouseButtons.Right)
            {
                new SendMsg().SendText("+");
                jia.BackColor = sc;
            }
            else
            {
                SendKeys.Send("{DOWN}");
                jia.BackColor = sc;
            }
        }


        private void jian_MouseDown(object sender, MouseEventArgs e)
        {
            // 短按（左键）= 方向键↑，长按（右键）= 运算符 -
            if (e.Button == MouseButtons.Right)
            {
                new SendMsg().SendText("-");
                jian.BackColor = sc;
            }
            else
            {
                SendKeys.Send("{UP}");
                jian.BackColor = sc;
            }
        }


        private void cheng_MouseDown(object sender, MouseEventArgs e)
        {
            // 短按（左键）= 方向键→，长按（右键）= 运算符 *
            if (e.Button == MouseButtons.Right)
            {
                new SendMsg().SendText("*");
                cheng.BackColor = sc;
            }
            else
            {
                SendKeys.Send("{RIGHT}");
                cheng.BackColor = sc;
            }
        }


        private void chu_MouseDown(object sender, MouseEventArgs e)
        {
            // 短按（左键）= 方向键←，长按（右键）= 运算符 /
            if (e.Button == MouseButtons.Right)
            {
                new SendMsg().SendText("/");
                chu.BackColor = sc;
            }
            else
            {
                SendKeys.Send("{LEFT}");
                chu.BackColor = sc;
            }
        }



























        int open;
        int num;

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            open = 0;

            switch (num)
            {
                case 9:
                    b9.BackColor = sc;
                    break;
            }
            switch (num)
            {
                case 8:
                    b8.BackColor = sc;
                    break;
            }
            switch (num)
            {
                case 7:
                    b7.BackColor = sc;
                    break;
            }
            switch (num)
            {
                case 6:
                    b6.BackColor = sc;
                    break;
            }
            switch (num)
            {
                case 5:
                    b5.BackColor = sc;
                    break;
            }
            switch (num)
            {
                case 4:
                    b4.BackColor = sc;
                    break;
            }
            switch (num)
            {
                case 3:
                    b3.BackColor = sc;
                    break;
            }
            switch (num)
            {
                case 2:
                    b2.BackColor = sc;
                    break;
            }
            switch (num)
            {
                case 1:
                    b1.BackColor = sc;
                    break;
            }
            switch (num)
            {
                case 0:
                    b0.BackColor = sc;
                    break;
            }
            switch (num)
            {
                case 11:
                    dou.BackColor = qc;
                    break;
            }
            switch (num)
            {
                case 22:
                    ju.BackColor = qc;
                    break;
            }
            switch (num)
            {
                case 33:
                    wen.BackColor = qc;
                    break;
            }
            switch (num)
            {
                case 44:
                    gan.BackColor = qc;
                    break;
            }
            switch (num)
            {
                case 55:
                    jia.BackColor = qc;
                    break;
            }
            switch (num)
            {
                case 66:
                    jian.BackColor = qc;
                    break;
            }
            switch (num)
            {
                case 77:
                    cheng.BackColor = qc;
                    break;
            }
            switch (num)
            {
                case 88:
                    chu.BackColor = qc;
                    break;
            }

        }



















        private void b9_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                new SendMsg().SendText("9");
                b9.BackColor = qc;
            }
            else
            {
                keybd_event(VK_NUM9, 0, 0, 0);
                b9.BackColor = qc;
            }
        }


        private void b8_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                new SendMsg().SendText("8");
                b8.BackColor = qc;
            }
            else
            {
                keybd_event(VK_NUM8, 0, 0, 0);
                b8.BackColor = qc;
            }
        }

        



        private void b7_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                new SendMsg().SendText("7");
                b7.BackColor = qc;
            }
            else
            {
                keybd_event(VK_NUM7, 0, 0, 0);
                b7.BackColor = qc;
            }
        }


        private void b6_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                new SendMsg().SendText("6");
                b6.BackColor = qc;
            }
            else
            {
                keybd_event(VK_NUM6, 0, 0, 0);
                b6.BackColor = qc;
            }
        }

        private void b5_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                new SendMsg().SendText("5");
                b5.BackColor = qc;
            }
            else
            {
                keybd_event(VK_NUM5, 0, 0, 0);
                b5.BackColor = qc;
            }
        }


        private void b4_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                new SendMsg().SendText("4");
                b4.BackColor = qc;
            }
            else
            {
                keybd_event(VK_NUM4, 0, 0, 0);
                b4.BackColor = qc;
            }
        }



        private void b3_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                new SendMsg().SendText("3");
                b3.BackColor = qc;
            }
            else
            {
                keybd_event(VK_NUM3, 0, 0, 0);
                b3.BackColor = qc;
            }
        }


        private void b2_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                new SendMsg().SendText("2");
                b2.BackColor = qc;
            }
            else
            {
                keybd_event(VK_NUM2, 0, 0, 0);
                b2.BackColor = qc;
            }
        }


        private void b1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                new SendMsg().SendText("1");
                b1.BackColor = qc;
            }
            else
            {
                keybd_event(VK_NUM1, 0, 0, 0);
                b1.BackColor = qc;
            }
        }



        // 标记：右键是否正在触发语音输入
        private bool _voiceInputActive = false;

        private void b0_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                // 右键：语音输入（Ctrl+Alt+D），先检测 t9s2t 是否在运行
                if (CheckAndLaunchT9s2t())
                {
                    // 只发送按下事件，松开右键时在 MouseUp 中发送释放事件
                    // t9s2t 钩子逻辑：D down 开始录音，D up 停止录音
                    keybd_event(0x11, 0, 0, 0);   // Ctrl down
                    keybd_event(0x12, 0, 0, 0);   // Alt down
                    keybd_event(0x44, 0, 0, 0);   // D down -> t9s2t 开始录音
                    _voiceInputActive = true;
                }
                b0.BackColor = qc;
            }
            else
            {
                SendKeys.Send(" ");
                b0.BackColor = qc;
            }
        }

        private void b0_MouseUp(object sender, MouseEventArgs e)
        {
            // 仅处理右键且确实在语音输入中
            if (e.Button == MouseButtons.Right && _voiceInputActive)
            {
                // 松开右键：发送释放事件，t9s2t 收到 D up 停止录音并输出文字
                keybd_event(0x44, 0, KEYEVENTF_KEYUP, 0);   // D up
                keybd_event(0x12, 0, KEYEVENTF_KEYUP, 0);   // Alt up
                keybd_event(0x11, 0, KEYEVENTF_KEYUP, 0);   // Ctrl up
                _voiceInputActive = false;
                b0.BackColor = qc;
            }
        }

        /// <summary>
        /// 检测 t9s2t.exe 是否在运行，未运行则询问用户是否启动。
        /// 返回 true 表示可以执行语音输入，false 表示无法执行。
        /// </summary>
        private bool CheckAndLaunchT9s2t()
        {
            try
            {
                string exePath = Path.Combine(Application.StartupPath, "t9s2t", "t9s2t.exe");

                // ① 已经在运行：再问一句"模型到底能不能用"（t9s2t 会把状态写到注册表）
                //    以前这里只要进程活着就 return true，模型还在下载时用户按语音键会"没反应"
                if (Process.GetProcessesByName("t9s2t").Length > 0)
                {
                    if (ReadT9s2tModelReady() == false)
                    {
                        DialogResult r0 = MessageBox.Show(
                            "t9s2t 已在运行，但语音模型还没有准备好，暂时无法语音输入。\n\n" +
                            "点「是」打开它的模型管理界面（首次使用需下载约 258MB）。",
                            "语音模型尚未就绪", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (r0 == DialogResult.Yes) LaunchT9s2t(exePath, "/model");
                        return false;
                    }
                    return true;   // 已就绪，或读不到状态（读不到时保持原有的乐观放行）
                }

                // ② 没在运行：问过用户再启动
                DialogResult result = MessageBox.Show(
                    "语音输入需要 t9s2t 支持，它当前没有运行。\n\n" +
                    "点「是」启动它（会请求一次管理员权限）。\n" +
                    "如果还没装过语音模型，还需要下载约 258MB。",
                    "语音输入",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    if (File.Exists(exePath))
                    {
                        try
                        {
                            // 带 /model：让它把模型面板摆出来（模型已就绪时该参数只是显示主界面）
                            LaunchT9s2t(exePath, "/model");
                            // 等待片刻让 t9s2t 启动完成
                            Thread.Sleep(2000);
                            return true;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("启动 t9s2t.exe 失败：" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                        return false;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("检测 t9s2t 状态时出错：" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        /// <summary>启动 t9s2t 并带上意图参数（/model = 把模型面板摆出来）</summary>
        private static void LaunchT9s2t(string exePath, string arguments)
        {
            if (!File.Exists(exePath)) return;
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = exePath,
                    Arguments = arguments ?? "",
                    WorkingDirectory = Path.GetDirectoryName(exePath),
                    UseShellExecute = true
                });
            }
            catch { }
        }

        /// <summary>
        /// 读 t9s2t 写入的就绪状态（HKCU\Software\Rime\Weasel\T9S2T）。
        /// 返回 null 表示读不到（旧版本没写 / 注册表不可用），调用方按"无法判断"处理。
        /// </summary>
        private static bool? ReadT9s2tModelReady()
        {
            try
            {
                using (var k = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"Software\Rime\Weasel\T9S2T", false))
                {
                    if (k == null) return null;
                    object v = k.GetValue("ModelReady");
                    if (v == null) return null;
                    return Convert.ToInt32(v) != 0;
                }
            }
            catch { return null; }
        }


        private void backspace_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                timer2.Enabled = true;
            }
            else
            {
                SendKeys.Send("{BACKSPACE}");
            }


        }

        private void backspace_MouseUp(object sender, MouseEventArgs e)
        {
            timer2.Enabled = false;
            timer3.Enabled = false;
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            timer3.Interval = 50;
            timer3.Enabled = true;
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            SendKeys.Send("{BACKSPACE}");
            timer2.Enabled = true;
        }











        public void hide()
        {
            this.Hide();
        }

        private void t2n_Click(object sender, EventArgs e)
        {
            numboard f1 = Form2.FindOrCreateForm<numboard>();
            f1.Show();

            f1.Location = this.Location;
            f1.Size = this.Size;
            this.Hide();
        }

        private void t2e_Click(object sender, EventArgs e)
        {
            enboard f1 = Form2.FindOrCreateForm<enboard>();
            f1.Show();

            f1.Location = this.Location;
            f1.Size = this.Size;
            this.Hide();
        }


    }
}
