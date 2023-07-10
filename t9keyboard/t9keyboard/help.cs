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



namespace t9keyboard
{
    public partial class help : Form
    {
        public help()
        {
            InitializeComponent();
        }



        //public static int v = Color.Red.ToArgb();
        //public static Color sc = Color.FromArgb(172, 174, 187);
        //public static Color qc = Color.FromArgb(255, 255, 255);
        //public static Color bc = Color.FromArgb(200, 200, 200);

        //public static Color sc = Color.FromArgb(Convert.ToInt32("-5460293"));
        //public static Color qc = Color.FromArgb(Convert.ToInt32("-1"));
        //public static Color bc = Color.FromArgb(Convert.ToInt32("-3618616"));

        public static Color sc = Color.FromArgb(Convert.ToInt32(System.IO.File.ReadAllLines(System.Windows.Forms.Application.StartupPath + "\\data\\t9keyboard.txt")[System.IO.File.ReadAllLines(System.Windows.Forms.Application.StartupPath + "\\data\\t9keyboard.txt").Length - 3]));
        public static Color qc = Color.FromArgb(Convert.ToInt32(System.IO.File.ReadAllLines(System.Windows.Forms.Application.StartupPath + "\\data\\t9keyboard.txt")[System.IO.File.ReadAllLines(System.Windows.Forms.Application.StartupPath + "\\data\\t9keyboard.txt").Length - 2]));
        public static Color bc = Color.FromArgb(Convert.ToInt32(System.IO.File.ReadAllLines(System.Windows.Forms.Application.StartupPath + "\\data\\t9keyboard.txt")[System.IO.File.ReadAllLines(System.Windows.Forms.Application.StartupPath + "\\data\\t9keyboard.txt").Length - 1]));


        private void help_Load(object sender, EventArgs e)
        {

            //MessageBox.Show(System.Windows.Forms.Application.StartupPath + "\\data\\t9keyboard.txt");
            bc1.BackColor = sc;
            bc2.BackColor = qc;
            bc3.BackColor = bc;
        }



        private void yes_Click(object sender, EventArgs e)
        {
            sc = bc1.BackColor;
            qc = bc2.BackColor;
            bc = bc3.BackColor;


            List<string> lines = new List<string>(File.ReadAllLines(System.Windows.Forms.Application.StartupPath + "\\data\\t9keyboard.txt"));
            lines.RemoveAt(lines.Count - 1);//删除最后一行
            lines.RemoveAt(lines.Count - 1);//删除最后一行
            lines.RemoveAt(lines.Count - 1);//删除最后一行

            File.WriteAllLines(System.Windows.Forms.Application.StartupPath + "\\data\\t9keyboard.txt", lines.ToArray());


            FileStream fs = new FileStream(@System.Windows.Forms.Application.StartupPath + "\\data\\t9keyboard.txt", FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter m_streamWriter = new StreamWriter(fs);
            m_streamWriter.BaseStream.Seek(0, SeekOrigin.End);
            m_streamWriter.WriteLine(string.Format(sc.ToArgb().ToString()));
            m_streamWriter.WriteLine(string.Format(qc.ToArgb().ToString()));
            m_streamWriter.WriteLine(string.Format(bc.ToArgb().ToString()));
            m_streamWriter.Flush();
            m_streamWriter.Close();


            this.Close();
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
            keyboard f4 = new keyboard();
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
    }
}
