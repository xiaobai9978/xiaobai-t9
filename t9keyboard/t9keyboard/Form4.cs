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
    public partial class enboard : Form
    {
       // int v = help.v;
        Color sc = help.sc;
        Color qc = help.qc;
        Color bc = help.bc;











        public enboard()
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
            backspace.BackColor = sc;
            enter.BackColor = sc;
            tab.BackColor = sc;
            capslock.BackColor = sc;
            e2t.BackColor = sc;
            e2n.BackColor = sc;
            fen.BackColor = sc;
            b0.BackColor = qc;
            ab.BackColor = qc;
            bb.BackColor = qc;
            cb.BackColor = qc;
            db.BackColor = qc;
            eb.BackColor = qc;
            fb.BackColor = qc;
            gb.BackColor = qc;
            hb.BackColor = qc;
            ib.BackColor = qc;
            jb.BackColor = qc;
            kb.BackColor = qc;
            lb.BackColor = qc;
            mb.BackColor = qc;
            nb.BackColor = qc;
            ob.BackColor = qc;
            pb.BackColor = qc;
            qb.BackColor = qc;
            rb.BackColor = qc;
            sb.BackColor = qc;
            tb.BackColor = qc;
            ub.BackColor = qc;
            vb.BackColor = qc;
            wb.BackColor = qc;
            xb.BackColor = qc;
            yb.BackColor = qc;
            zb.BackColor = qc;














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

      //      this.Text = this.Width.ToString() + " " + this.Height.ToString();//窗体标题栏文本

        }








        String qd = "Q";
        String qx = "q";



        private void ab_Click(object sender, EventArgs e)
        {
            SendKeys.Send(ab.Text);
            //new SendMsg().SendText(ab.Text);
        }

        private void bb_Click(object sender, EventArgs e)
        {
            SendKeys.Send(bb.Text);
            //new SendMsg().SendText(bb.Text);
        }
        private void cb_Click(object sender, EventArgs e)
        {
            SendKeys.Send(cb.Text);
            //new SendMsg().SendText(cb.Text);
        }
        private void db_Click(object sender, EventArgs e)
        {
            SendKeys.Send(db.Text);
            //new SendMsg().SendText(db.Text);
        }
        private void eb_Click(object sender, EventArgs e)
        {
            SendKeys.Send(eb.Text);
            //new SendMsg().SendText(eb.Text);
        }
        private void fb_Click(object sender, EventArgs e)
        {
            SendKeys.Send(fb.Text);
            //new SendMsg().SendText(fb.Text);
        }
        private void gb_Click(object sender, EventArgs e)
        {
            SendKeys.Send(gb.Text);
            //new SendMsg().SendText(gb.Text);
        }
        private void hb_Click(object sender, EventArgs e)
        {
            SendKeys.Send(hb.Text);
            //new SendMsg().SendText(hb.Text);
        }
        private void ib_Click(object sender, EventArgs e)
        {
            SendKeys.Send(ib.Text);
            //new SendMsg().SendText(ib.Text);
        }
        private void jb_Click(object sender, EventArgs e)
        {
            SendKeys.Send(jb.Text);
            //new SendMsg().SendText(jb.Text);
        }
        private void kb_Click(object sender, EventArgs e)
        {
            SendKeys.Send(kb.Text);
            //new SendMsg().SendText(kb.Text);
        }
        private void lb_Click(object sender, EventArgs e)
        {
            SendKeys.Send(lb.Text);
            // new SendMsg().SendText(lb.Text);
        }
        private void mb_Click(object sender, EventArgs e)
        {
            SendKeys.Send(mb.Text);
            //new SendMsg().SendText(mb.Text);
        }
        private void nb_Click(object sender, EventArgs e)
        {
            SendKeys.Send(nb.Text);
            //new SendMsg().SendText(nb.Text);
        }
        private void ob_Click(object sender, EventArgs e)
        {
            SendKeys.Send(ob.Text);
            //new SendMsg().SendText(ob.Text);
        }
        private void pb_Click(object sender, EventArgs e)
        {
            SendKeys.Send(pb.Text);
            //new SendMsg().SendText(pb.Text);
        }
        private void qb_Click(object sender, EventArgs e)
        {
            SendKeys.Send(qb.Text);
        // new SendMsg().SendText(qb.Text);
        }
        private void rb_Click(object sender, EventArgs e)
        {
            SendKeys.Send(rb.Text);
            //new SendMsg().SendText(rb.Text);
        }
        private void sb_Click(object sender, EventArgs e)
        {
            SendKeys.Send(sb.Text);
            // new SendMsg().SendText(sb.Text);
        }
        private void tb_Click(object sender, EventArgs e)
        {
            SendKeys.Send(tb.Text);
            //new SendMsg().SendText(tb.Text);
        }
        private void ub_Click(object sender, EventArgs e)
        {
            SendKeys.Send(ub.Text);
            // new SendMsg().SendText(ub.Text);
        }
        private void vb_Click(object sender, EventArgs e)
        {
            SendKeys.Send(vb.Text);
            //  new SendMsg().SendText(vb.Text);
        }
        private void wb_Click(object sender, EventArgs e)
        {
            SendKeys.Send(wb.Text);
            //new SendMsg().SendText(wb.Text);
        }
        private void xb_Click(object sender, EventArgs e)
        {
            SendKeys.Send(xb.Text);
            // new SendMsg().SendText(xb.Text);
        }
        private void yb_Click(object sender, EventArgs e)
        {
            SendKeys.Send(yb.Text);
            // new SendMsg().SendText(yb.Text);
        }
        private void zb_Click(object sender, EventArgs e)
        {
            SendKeys.Send(zb.Text);
            //  new SendMsg().SendText(zb.Text);
        }



        private void fen_Click(object sender, EventArgs e)
        {
            new SendMsg().SendText(";");
        }
        

        private void b0_Click(object sender, EventArgs e)
        {
            //new SendMsg().SendText(" ");
            SendKeys.SendWait(" ");
        }


        public void hide()
        {
            this.Hide();
        }

        private void n2t_Click(object sender, EventArgs e)
        {
            numboard f1 = new numboard();

            f1.Show();

            f1.Location = this.Location;
            f1.Size = this.Size;
            this.Hide();
        }





        int open;
        int time = 800;
        int num;

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            open = 0;


        }



        private void dou_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                new SendMsg().SendText("，");
                dou.BackColor = qc;
            }
            else
            {
                new SendMsg().SendText(",");
                dou.BackColor = qc;
            }
        }

        private void ju_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                new SendMsg().SendText("。");
                dou.BackColor = qc;
            }
            else
            {
                new SendMsg().SendText(".");
                dou.BackColor = qc;
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

        private void capslock_Click(object sender, EventArgs e)
        {
            if (capslock.BackColor == sc)
            {
                capslock.BackColor = qc;
                ab.Text = "A";
                bb.Text = "B";
                cb.Text = "C";
                db.Text = "D";
                eb.Text = "E";
                fb.Text = "F";
                gb.Text = "G";
                hb.Text = "H";
                ib.Text = "I";
                jb.Text = "J";
                kb.Text = "K";
                lb.Text = "L";
                mb.Text = "M";
                nb.Text = "N";
                ob.Text = "O";
                pb.Text = "P";
                qb.Text = "Q";
                rb.Text = "R";
                sb.Text = "S";
                tb.Text = "T";
                ub.Text = "U";
                vb.Text = "V";
                wb.Text = "W";
                xb.Text = "X";
                yb.Text = "Y";
                zb.Text = "Z";
            }
            else
            {
                capslock.BackColor = sc;
                ab.Text = "a";
                bb.Text = "b";
                cb.Text = "c";
                db.Text = "d";
                eb.Text = "e";
                fb.Text = "f";
                gb.Text = "g";
                hb.Text = "h";
                ib.Text = "i";
                jb.Text = "j";
                kb.Text = "k";
                lb.Text = "l";
                mb.Text = "m";
                nb.Text = "n";
                ob.Text = "o";
                pb.Text = "p";
                qb.Text = "q";
                rb.Text = "r";
                sb.Text = "s";
                tb.Text = "t";
                ub.Text = "u";
                vb.Text = "v";
                wb.Text = "w";
                xb.Text = "x";
                yb.Text = "y";
                zb.Text = "z";
            }
        }

        private void e2t_Click(object sender, EventArgs e)
        {
            keyboard f1 = new keyboard();
            f1.Show();

            f1.Location = this.Location;
            f1.Size = this.Size;
            this.Hide();
        }

        private void tab_Click(object sender, EventArgs e)
        {
            SendKeys.Send("{TAB}");
        }

        private void one_Click(object sender, EventArgs e)
        {
            new SendMsg().SendText("1");
        }

        private void two_Click(object sender, EventArgs e)
        {
            new SendMsg().SendText("2");
        }

        private void three_Click(object sender, EventArgs e)
        {
            new SendMsg().SendText("3");
        }

        private void four_Click(object sender, EventArgs e)
        {
            new SendMsg().SendText("4");
        }

        private void five_Click(object sender, EventArgs e)
        {
            new SendMsg().SendText("5");
        }

        private void six_Click(object sender, EventArgs e)
        {
            new SendMsg().SendText("6");
        }

        private void seven_Click(object sender, EventArgs e)
        {
            new SendMsg().SendText("7");
        }

        private void eight_Click(object sender, EventArgs e)
        {
            new SendMsg().SendText("8");
        }

        private void nine_Click(object sender, EventArgs e)
        {
            new SendMsg().SendText("9");
        }

        private void zero_Click(object sender, EventArgs e)
        {
            new SendMsg().SendText("0");
        }
    }
}
