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
using System.Text.RegularExpressions;
using System.Diagnostics;
using static System.IO.File;
using static System.Drawing.ColorTranslator;

namespace t9skin
{
    public partial class t9skin : Form
    {

        


        public t9skin()
        {
            InitializeComponent();
            c1.Click += (e, a) => C_Click(c1);
            c2.Click += (e, a) => C_Click(c2);
            c3.Click += (e, a) => C_Click(c3);
            c4.Click += (e, a) => C_Click(c4);
            c5.Click += (e, a) => C_Click(c5);
            c6.Click += (e, a) => C_Click(c6);
            c7.Click += (e, a) => C_Click(c7);
            c8.Click += (e, a) => C_Click(c8);
            c9.Click += (e, a) => C_Click(c9);
            c10.Click += (e, a) => C_Click(c10);
            c11.Click += (e, a) => C_Click(c11);
            c12.Click += (e, a) => C_Click(c12);
            c13.Click += (e, a) => C_Click(c13);
        }
        public static string yaml = System.Windows.Forms.Application.StartupPath + "\\data\\weasel.yaml";

        public static string text_color = ReadAllLines(yaml)[ReadAllLines(yaml).Length - 13];
        public static string back_color = ReadAllLines(yaml)[ReadAllLines(yaml).Length - 12];
        public static string shadow_color = ReadAllLines(yaml)[ReadAllLines(yaml).Length - 11];
        public static string border_color = ReadAllLines(yaml)[ReadAllLines(yaml).Length - 10];
        public static string hilited_text_color = ReadAllLines(yaml)[ReadAllLines(yaml).Length - 9];
        public static string hilited_back_color = ReadAllLines(yaml)[ReadAllLines(yaml).Length - 8];
        public static string hilited_shadow_color = ReadAllLines(yaml)[ReadAllLines(yaml).Length - 7];
        public static string hilited_candidate_text_color = ReadAllLines(yaml)[ReadAllLines(yaml).Length - 6];
        public static string hilited_candidate_back_color = ReadAllLines(yaml)[ReadAllLines(yaml).Length - 5];
        public static string hilited_candidate_shadow_color = ReadAllLines(yaml)[ReadAllLines(yaml).Length - 4];
        public static string candidate_text_color = ReadAllLines(yaml)[ReadAllLines(yaml).Length - 3];
        public static string candidate_back_color = ReadAllLines(yaml)[ReadAllLines(yaml).Length - 2];
        public static string candidate_shadow_color = ReadAllLines(yaml)[ReadAllLines(yaml).Length - 1];

        public static string track_Bar1 = ReadAllLines(yaml)[ReadAllLines(yaml).Length - 20];
        public static string track_Bar2 = ReadAllLines(yaml)[ReadAllLines(yaml).Length - 19];
        public static string track_Bar3 = ReadAllLines(yaml)[ReadAllLines(yaml).Length - 18];


        public static string text_colorz = "#" + Reverse(text_color.Substring(text_color.Length - 8, 8 ));
        public static string back_colorz = "#" + Reverse(back_color.Substring(back_color.Length - 8, 8));
        public static string shadow_colorz = "#" + Reverse(shadow_color.Substring(shadow_color.Length - 8, 8));
        public static string border_colorz = "#" + Reverse(border_color.Substring(border_color.Length - 8, 8));
        public static string hilited_text_colorz = "#" + Reverse(hilited_text_color.Substring(hilited_text_color.Length - 8, 8));
        public static string hilited_back_colorz = "#" + Reverse(hilited_back_color.Substring(hilited_back_color.Length - 8, 8));
        public static string hilited_shadow_colorz = "#" + Reverse(hilited_shadow_color.Substring(hilited_shadow_color.Length - 8, 8));
        public static string hilited_candidate_text_colorz = "#" + Reverse(hilited_candidate_text_color.Substring(hilited_candidate_text_color.Length - 8, 8));
        public static string hilited_candidate_back_colorz = "#" + Reverse(hilited_candidate_back_color.Substring(hilited_candidate_back_color.Length - 8, 8));
        public static string hilited_candidate_shadow_colorz = "#" + Reverse(hilited_candidate_shadow_color.Substring(hilited_candidate_shadow_color.Length - 8, 8));
        public static string candidate_text_colorz = "#" + Reverse(candidate_text_color.Substring(candidate_text_color.Length - 8, 8));
        public static string candidate_back_colorz = "#" + Reverse(candidate_back_color.Substring(candidate_back_color.Length - 8, 8));
        public static string candidate_shadow_colorz = "#" + Reverse(candidate_shadow_color.Substring(candidate_shadow_color.Length - 8, 8));

        public static string Reverse(string text)
        {
            char[] cArray = text.ToCharArray();
            string reverse = String.Empty;
            reverse += cArray[6];
            reverse += cArray[7];
            reverse += cArray[4];
            reverse += cArray[5];
            reverse += cArray[2];
            reverse += cArray[3];
            reverse += cArray[0];
            reverse += cArray[1];
            return reverse;
        }

        public static string ToHexValue( Color color)
        {
            return "#" + color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2") + color.A.ToString("X2") ;
        }

        public string ReverseHex(Color color)
        {
            return Reverse(ToHexValue(color).Substring(ToHexValue(color).Length - 8, 8));
        }

        
        private void Form1_Load(object sender, EventArgs e)
        {
            //MessageBox.Show(hilited_candidate_back_colorz);
            //MessageBox.Show(hilited_candidate_back_colorz.Substring(0, 7));
            c1.BackColor = FromHtml(text_colorz.Substring(0,7)); //获取c1颜色给按钮
            c2.BackColor = FromHtml(back_colorz.Substring(0, 7)); //获取c1颜色给按钮
            c3.BackColor = FromHtml(shadow_colorz.Substring(0, 7)); //获取c1颜色给按钮
            c4.BackColor = FromHtml(border_colorz.Substring(0, 7)); //获取c1颜色给按钮
            c5.BackColor = FromHtml(hilited_text_colorz.Substring(0, 7)); //获取c1颜色给按钮
            c6.BackColor = FromHtml(hilited_back_colorz.Substring(0, 7)); //获取c1颜色给按钮
            c7.BackColor = FromHtml(hilited_shadow_colorz.Substring(0, 7)); //获取c1颜色给按钮
            c8.BackColor = FromHtml(hilited_candidate_text_colorz.Substring(0, 7)); //获取c1颜色给按钮
            c9.BackColor = FromHtml(hilited_candidate_back_colorz.Substring(0, 7)); //获取c1颜色给按钮
            c10.BackColor = FromHtml(hilited_candidate_shadow_colorz.Substring(0, 7)); //获取c1颜色给按钮
            c11.BackColor = FromHtml(candidate_text_colorz.Substring(0, 7)); //获取c1颜色给按钮
            c12.BackColor = FromHtml(candidate_back_colorz.Substring(0, 7)); //获取c1颜色给按钮
            c13.BackColor = FromHtml(candidate_shadow_colorz.Substring(0, 7)); //获取c1颜色给按钮

            //MessageBox.Show(track_Bar1.Substring(19));
            trackBar1.Value = int.Parse((track_Bar1.Substring(19)));
            trackBar2.Value = int.Parse((track_Bar2.Substring(21)));
            trackBar3.Value = int.Parse((track_Bar3.Substring(21)));

            SetColor();
        }

        private void SetColor()
        {
            //候选
            text.ForeColor = c1.BackColor;
            text.BackColor = c2.BackColor;
            back.BackColor = c2.BackColor;

            border.BackColor = c4.BackColor;


            //编码
            hilited.ForeColor = c5.BackColor;
            hilited.BackColor = c6.BackColor;


            //高亮

            hilited_candidate.ForeColor = c8.BackColor;
            comment0.BackColor = label17.BackColor=hilited_candidate.BackColor = c9.BackColor;


            //非高亮
            candidate1.ForeColor = candidate2.ForeColor = candidate3.ForeColor = candidate4.ForeColor = candidate5.ForeColor = candidate6.ForeColor = c11.BackColor;
            Color originalColor = c11.BackColor;
            int RR = c11.BackColor.R/2;
            int GG = c11.BackColor.G/2;
            int BB = c11.BackColor.B/2;
            if (RR == 0) RR = -128; if (GG == 0) GG = -128; if (BB == 0) BB = -128;
            Color newColor = Color.FromArgb(originalColor.A, originalColor.R-RR, originalColor.G-GG, originalColor.B-BB);
            label17.ForeColor = label18.ForeColor = label19.ForeColor = label20.ForeColor = label21.ForeColor = label22.ForeColor = label23.ForeColor = comment0.ForeColor = comment1.ForeColor = comment2.ForeColor = comment3.ForeColor = comment4.ForeColor = comment5.ForeColor = comment6.ForeColor =  newColor;
            label18.BackColor = label19.BackColor = label20.BackColor = label21.BackColor = label22.BackColor = label23.BackColor = comment1.BackColor = comment2.BackColor = comment3.BackColor = comment4.BackColor = comment5.BackColor = comment6.BackColor = candidate1.BackColor = candidate2.BackColor = candidate3.BackColor = candidate4.BackColor = candidate5.BackColor = candidate6.BackColor = c12.BackColor;



        }

        public void RestartProgress()
        {
           Process.Start("C:\\ProgramData\\Microsoft\\Windows\\Start Menu\\Programs\\小白T9输入法\\【小白T9输入法】重新部署.lnk");
        }
        public void CC_Click(Object sender, EventArgs e)
        {

        }


        private void C_Click(Button button)
        {
            Form2 form2 = new Form2();
            if (form2.ShowDialog() == DialogResult.OK)
            {
                button.BackColor = form2.tsbColor; // 将 Form2 的颜色值设置为 Form1 的颜色
            }
            form2.Dispose(); // 释放 Form2 的资源
            //colorDialog1.ShowDialog();
            //button.BackColor = colorDialog1.Color;
            List<string> lines = new List<string>(ReadAllLines(yaml));
            lines.RemoveRange(lines.Count-13,13);
            File.WriteAllLines(yaml, lines.ToArray());
            FileStream fs = new FileStream(@yaml, FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            sw.BaseStream.Seek(0, SeekOrigin.End);
            sw.WriteLine(string.Format("    text_color: 0x" + ReverseHex(c1.BackColor)));
            sw.WriteLine(string.Format("    back_color: 0x" + ReverseHex(c2.BackColor)));
            sw.WriteLine(string.Format("    shadow_color: 0x" + ReverseHex(c3.BackColor)));
            sw.WriteLine(string.Format("    border_color: 0x" + ReverseHex(c4.BackColor)));
            sw.WriteLine(string.Format("    hilited_text_color: 0x" + ReverseHex(c5.BackColor)));
            sw.WriteLine(string.Format("    hilited_back_color: 0x" + ReverseHex(c6.BackColor)));
            sw.WriteLine(string.Format("    hilited_shadow_color: 0x" + ReverseHex(c7.BackColor)));
            sw.WriteLine(string.Format("    hilited_candidate_text_color: 0x" + ReverseHex(c8.BackColor)));
            sw.WriteLine(string.Format("    hilited_candidate_back_color: 0x" + ReverseHex(c9.BackColor)));
            sw.WriteLine(string.Format("    hilited_candidate_shadow_color: 0x" + ReverseHex(c10.BackColor)));
            sw.WriteLine(string.Format("    candidate_text_color: 0x" + ReverseHex(c11.BackColor)));
            sw.WriteLine(string.Format("    candidate_back_color: 0x" + ReverseHex(c12.BackColor)));
            sw.WriteLine(string.Format("    candidate_shadow_color: 0x" + ReverseHex(c13.BackColor)));
            sw.Flush();
            sw.Close();
            SetColor();
            RestartProgress();
        }

        private void recovery_Click(object sender, EventArgs e)
        {
            List<string> lines = new List<string>(ReadAllLines(yaml));
            lines.RemoveRange(lines.Count-20,20);
            File.WriteAllLines(yaml, lines.ToArray());
            FileStream fs = new FileStream(@yaml, FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            sw.BaseStream.Seek(0, SeekOrigin.End);

            sw.WriteLine(string.Format("    shadow_radius: 0"));
            sw.WriteLine(string.Format("    shadow_offset_x: 4"));
            sw.WriteLine(string.Format("    shadow_offset_y: 12"));
            sw.WriteLine(string.Format(""));
            sw.WriteLine(string.Format("preset_color_schemes:"));
            sw.WriteLine(string.Format("  xiaobai:"));
            sw.WriteLine(string.Format("    name: 小白输入法／xiaobai"));

            sw.WriteLine(string.Format("    text_color: 0xFF000000"));
            sw.WriteLine(string.Format("    back_color: 0xFFeceeee"));
            sw.WriteLine(string.Format("    shadow_color: 0xFF000000"));
            sw.WriteLine(string.Format("    border_color: 0xeFF0e0e0"));
            sw.WriteLine(string.Format("    hilited_text_color: 0xFF000000"));
            sw.WriteLine(string.Format("    hilited_back_color: 0xFFd4d4d4"));
            sw.WriteLine(string.Format("    hilited_shadow_color: 0xFF000000"));
            sw.WriteLine(string.Format("    hilited_candidate_text_color: 0xFFffffff"));
            sw.WriteLine(string.Format("    hilited_candidate_back_color: 0xFFfa3a0a"));
            sw.WriteLine(string.Format("    hilited_candidate_shadow_color: 0xFF000000"));
            sw.WriteLine(string.Format("    candidate_text_color: 0xFF000000"));
            sw.WriteLine(string.Format("    candidate_back_color: 0xFFeceeee"));
            sw.WriteLine(string.Format("    candidate_shadow_color: 0xFF000000"));
            sw.Flush();
            sw.Close();


            RestartProgress();
            Application.ExitThread();
            Application.Exit();
            Application.Restart();
        }

        private void trackBar1_MouseUp(object sender, MouseEventArgs e)
        {
            List<string> lines = new List<string>(ReadAllLines(yaml));
            string pattern = @"shadow_radius: \d+";
            for (int i = 0; i < lines.Count; i++)
            {
                if (Regex.IsMatch(lines[i], pattern))
                {
                    lines[i] = Regex.Replace(lines[i], pattern, "shadow_radius: "+ trackBar1.Value);
                }
            }
            File.WriteAllLines(yaml, lines.ToArray());
            RestartProgress();


        }

        private void trackBar2_MouseUp(object sender, MouseEventArgs e)
        {
            List<string> lines = new List<string>(ReadAllLines(yaml));
            string pattern = @"shadow_offset_x: \d+";
            for (int i = 0; i < lines.Count; i++)
            {
                if (Regex.IsMatch(lines[i], pattern))
                {
                    lines[i] = Regex.Replace(lines[i], pattern, "shadow_offset_x: " + trackBar2.Value);
                }
            }
            File.WriteAllLines(yaml, lines.ToArray());
            RestartProgress();
        }

        private void trackBar3_MouseUp(object sender, MouseEventArgs e)
        {
            List<string> lines = new List<string>(ReadAllLines(yaml));
            string pattern = @"shadow_offset_y: \d+";
            for (int i = 0; i < lines.Count; i++)
            {
                if (Regex.IsMatch(lines[i], pattern))
                {
                    lines[i] = Regex.Replace(lines[i], pattern, "shadow_offset_y: " + trackBar3.Value);
                }
            }
            File.WriteAllLines(yaml, lines.ToArray());
            RestartProgress();
        }
    }
}
