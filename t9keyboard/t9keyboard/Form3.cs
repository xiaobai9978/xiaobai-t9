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

namespace t9keyboard
{
    public partial class numboard : Form
    {













        public numboard()
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



















        private void en_Load(object sender, EventArgs e)
        {


   

            this.Resize += new EventHandler(Form1_Resize);//窗体调整大小时引发事件

            X = this.Width;//获取窗体的宽度

            Y = this.Height;//获取窗体的高度

            setTag(this);//调用方法

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
            n2e.BackColor = sc;
            n2t.BackColor = sc;
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

       //     this.Text = this.Width.ToString() + " " + this.Height.ToString();//窗体标题栏文本

        }













        private void b7_Click(object sender, EventArgs e)
        {
            new SendMsg().SendText("7");
        }

        private void b8_Click(object sender, EventArgs e)
        {
            new SendMsg().SendText("8");
        }

        private void b9_Click(object sender, EventArgs e)
        {
            new SendMsg().SendText("9");
        }

        private void b4_Click(object sender, EventArgs e)
        {
            new SendMsg().SendText("4");
        }

        private void b5_Click(object sender, EventArgs e)
        {
            new SendMsg().SendText("5");
        }

        private void b6_Click(object sender, EventArgs e)
        {
            new SendMsg().SendText("6");
        }

        private void b1_Click(object sender, EventArgs e)
        {
            new SendMsg().SendText("1");
        }

        private void b2_Click(object sender, EventArgs e)
        {
            new SendMsg().SendText("2");
        }

        private void b3_Click(object sender, EventArgs e)
        {
            new SendMsg().SendText("3");
        }

        private void b0_Click(object sender, EventArgs e)
        {
            new SendMsg().SendText("0");
        }


        public void hide()
        {
            this.Hide();
        }

        private void n2t_Click(object sender, EventArgs e)
        {
            keyboard f1 = new keyboard();

            f1.Show();

            f1.Location = this.Location;
            f1.Size = this.Size;
            this.Hide();
        }





        int open;
        int num;



       // int v = help.v;
        Color sc = help.sc;
        Color qc = help.qc;
        Color bc = help.bc;




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

        private void enter_Click(object sender, EventArgs e)
        {
            SendKeys.Send("{enter}");
        }



        private void n2e_Click(object sender, EventArgs e)
        {
            enboard f1 = new enboard();
            f1.Show();

            f1.Location = this.Location;
            f1.Size = this.Size;
            this.Hide();
        }
    }
}
