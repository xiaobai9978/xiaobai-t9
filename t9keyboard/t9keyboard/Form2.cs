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
using System.Collections.Specialized;
using System.Text.RegularExpressions;






namespace t9keyboard
{









    public partial class Form2 : Form
    {
        
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            this.BackColor = Color.White;
            this.TransparencyKey = Color.White;

            keyboard f1 = new keyboard();
            f1.Show();

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




        private Point offset;

        private void button1_MouseDown(object sender, MouseEventArgs e)
        {

            if (e.Button == MouseButtons.Left)
            {
  //              MessageBox.Show("left");
            }
            else
            {
                //               MessageBox.Show("right");
                contextMenuStrip1.Show(MousePosition.X, MousePosition.Y);
            }
            //右键

            if (MouseButtons.Left != e.Button) return;
            Point cur = this.PointToScreen(e.Location);
            offset = new Point(cur.X - this.Left, cur.Y - this.Top);




        }

        private void button1_MouseMove(object sender, MouseEventArgs e)
        {
            if (MouseButtons.Left != e.Button) return;
            Point cur = MousePosition;
            this.Location = new Point(cur.X - offset.X, cur.Y - offset.Y);
        }
        int i = 3;
        private void button1_Click(object sender, EventArgs e)
        {
          

            if (timer1.Enabled)

                {

                    timer1.Enabled = false;

                //以下为双击事件内容

                if (i % 2 == 1)
                {
                  //  MessageBox.Show((i%2).ToString());
                    //                    Form f1 = new keyboard();
                    foreach (Form form in Application.OpenForms)
                    {
                        if (form.GetType() == typeof(keyboard))
                        {
                            keyboard f1 = new keyboard();
                            f1 = (keyboard)form;
                            f1.hide();
                        }
                        if (form.GetType() == typeof(numboard))
                        {
                            numboard f2 = new numboard();
                            f2 = (numboard)form;
                            f2.hide();
                        }
                        if (form.GetType() == typeof(enboard))
                        {
                            enboard f3 = new enboard();
                            f3 = (enboard)form;
                            f3.hide();
                        }
                    }
                    i++;
                }
                else
                {
                 //               MessageBox.Show((i % 2).ToString());
                    keyboard f1 = new keyboard();
                    f1.Show();
                    i++;
                }

                //双击事件结束z

            }

                else

                {

                    timer1.Enabled = true;

                }

            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (timer1.Tag == "")

            {

                timer1.Tag = DateTime.Now.ToString();

            }

            else

            {

                if ((DateTime.Now - Convert.ToDateTime(timer1.Tag)).TotalSeconds > 0.5)

                {

                    timer1.Tag = "";

                    timer1.Enabled = false;

                    //以下为单击事件内容




                    //单击事件结束

                }

            }
        }

        private void 退出软键盘ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void 帮助与设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            help f1 = new help();
            f1.Show();

            f1.Location = this.Location;
        }













    }
}
