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
            int x = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Size.Width - this.Width;
            int y = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Size.Height - this.Height;
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

        private void one_Click(object sender, EventArgs e)
        {
            SendKeys.Send("{1}");
//          new SendMsg().SendText("1");
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



        private void b0_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                new SendMsg().SendText("0");
                b0.BackColor = qc;
            }
            else
            {
                SendKeys.Send(" ");
                b0.BackColor = qc;
            }
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
            numboard f1 = new numboard();
            f1.Show();

            f1.Location = this.Location;
            f1.Size = this.Size;
            this.Hide();
        }

        private void t2e_Click(object sender, EventArgs e)
        {
            enboard f1 = new enboard();
            f1.Show();

            f1.Location = this.Location;
            f1.Size = this.Size;
            this.Hide();
        }


    }
}
