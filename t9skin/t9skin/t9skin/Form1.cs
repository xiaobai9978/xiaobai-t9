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
        }
        public static string yaml = System.Windows.Forms.Application.StartupPath + "\\data\\weasel.yaml";
        public static string back_color = ReadAllLines(yaml)[ReadAllLines(yaml).Length - 11];
        public static string border_color = ReadAllLines(yaml)[ReadAllLines(yaml).Length - 10];
        public static string text_color = ReadAllLines(yaml)[ReadAllLines(yaml).Length - 9];
        public static string hilited_text_color = ReadAllLines(yaml)[ReadAllLines(yaml).Length - 8];
        public static string hilited_back_color = ReadAllLines(yaml)[ReadAllLines(yaml).Length - 7];
        public static string hilited_candidate_text_color = ReadAllLines(yaml)[ReadAllLines(yaml).Length - 6];
        public static string hilited_candidate_back_color = ReadAllLines(yaml)[ReadAllLines(yaml).Length - 5];
        public static string candidate_text_color = ReadAllLines(yaml)[ReadAllLines(yaml).Length - 4];
        public static string comment_text_color = ReadAllLines(yaml)[ReadAllLines(yaml).Length - 3];
        public static string hilited_comment_text_color = ReadAllLines(yaml)[ReadAllLines(yaml).Length - 2];
        public static string label_color = ReadAllLines(yaml)[ReadAllLines(yaml).Length - 1];

        public static string back_colorz = "#" + Reverse( back_color.Substring(back_color.Length - 6, 6 ) );
        public static string border_colorz = "#" + Reverse(border_color.Substring(border_color.Length - 6, 6));
        public static string text_colorz = "#" + Reverse(text_color.Substring(text_color.Length - 6, 6));
        public static string hilited_text_colorz = "#" + Reverse(hilited_text_color.Substring(hilited_text_color.Length - 6, 6));
        public static string hilited_back_colorz = "#" + Reverse(hilited_back_color.Substring(hilited_back_color.Length - 6, 6));
        public static string hilited_candidate_text_colorz = "#" + Reverse(hilited_candidate_text_color.Substring(hilited_candidate_text_color.Length - 6, 6));
        public static string hilited_candidate_back_colorz = "#" + Reverse(hilited_candidate_back_color.Substring(hilited_candidate_back_color.Length - 6, 6));
        public static string candidate_text_colorz = "#" + Reverse(candidate_text_color.Substring(candidate_text_color.Length - 6, 6));
        public static string comment_text_colorz = "#" + Reverse(comment_text_color.Substring(comment_text_color.Length - 6, 6));
        public static string hilited_comment_text_colorz = "#" + Reverse(hilited_comment_text_color.Substring(hilited_comment_text_color.Length - 6, 6));
        public static string label_colorz = "#" + Reverse(label_color.Substring(label_color.Length - 6, 6));

        public static string Reverse(string text)
        {
            char[] cArray = text.ToCharArray();
            string reverse = String.Empty;
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
            return "#" + color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2");
        }

        public string ReverseHex(Color color)
        {
            return Reverse(ToHexValue(color).Substring(ToHexValue(color).Length - 6, 6));
        }

        
        private void Form1_Load(object sender, EventArgs e)
        {
            c1.BackColor = FromHtml(back_colorz); //获取c1颜色给按钮
            c2.BackColor = FromHtml(border_colorz); //获取c1颜色给按钮
            c3.BackColor = FromHtml(text_colorz); //获取c1颜色给按钮
            c4.BackColor = FromHtml(hilited_text_colorz); //获取c1颜色给按钮
            c5.BackColor = FromHtml(hilited_back_colorz); //获取c1颜色给按钮
            c6.BackColor = FromHtml(hilited_candidate_text_colorz); //获取c1颜色给按钮
            c7.BackColor = FromHtml(hilited_candidate_back_colorz); //获取c1颜色给按钮
            c8.BackColor = FromHtml(candidate_text_colorz); //获取c1颜色给按钮
            c9.BackColor = FromHtml(comment_text_colorz); //获取c1颜色给按钮
            c10.BackColor = FromHtml(hilited_comment_text_colorz); //获取c1颜色给按钮
            c11.BackColor = FromHtml(label_colorz); //获取c1颜色给按钮

            SetColor();
        }

        private void SetColor()
        {
            back.BackColor = c1.BackColor;
            border.BackColor = c2.BackColor;
            text.BackColor = c1.BackColor;
            text.ForeColor = c3.BackColor;
            hilited.BackColor = c5.BackColor;
            hilited.ForeColor = c4.BackColor;
            hilited_candidate.BackColor = c7.BackColor;
            hilited_candidate.ForeColor = c6.BackColor;
            candidate1.ForeColor = candidate2.ForeColor = candidate3.ForeColor = candidate4.ForeColor = candidate5.ForeColor = candidate6.ForeColor = c8.BackColor;
            candidate1.BackColor = candidate2.BackColor = candidate3.BackColor = candidate4.BackColor = candidate5.BackColor = candidate6.BackColor = c1.BackColor;
            comment1.ForeColor = comment2.ForeColor = comment3.ForeColor = comment4.ForeColor = comment5.ForeColor = comment6.ForeColor = c9.BackColor;
            comment1.BackColor = comment2.BackColor = comment3.BackColor = comment4.BackColor = comment5.BackColor = comment6.BackColor = c1.BackColor;
            comment0.BackColor = c7.BackColor;
            comment0.ForeColor = c10.BackColor;
            label18.BackColor = label19.BackColor = label20.BackColor = label21.BackColor = label22.BackColor = label23.BackColor = c1.BackColor;
            label18.ForeColor = label19.ForeColor = label20.ForeColor = label21.ForeColor = label22.ForeColor = label23.ForeColor = c11.BackColor;
            label17.BackColor = c7.BackColor;
            label17.ForeColor=  c6.BackColor;


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
            colorDialog1.FullOpen = true;
            colorDialog1.ShowDialog();
            button.BackColor = colorDialog1.Color;
            List<string> lines = new List<string>(ReadAllLines(yaml));
            lines.RemoveRange(lines.Count-12,11);
            File.WriteAllLines(yaml, lines.ToArray());
            FileStream fs = new FileStream(@yaml, FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            sw.BaseStream.Seek(0, SeekOrigin.End);
            sw.WriteLine(string.Format("    back_color: 0x" + ReverseHex(c1.BackColor)));
            sw.WriteLine(string.Format("    border_color: 0x" + ReverseHex(c2.BackColor)));
            sw.WriteLine(string.Format("    text_color: 0x" + ReverseHex(c3.BackColor)));
            sw.WriteLine(string.Format("    hilited_text_color: 0x" + ReverseHex(c4.BackColor)));
            sw.WriteLine(string.Format("    hilited_back_color: 0x" + ReverseHex(c5.BackColor)));
            sw.WriteLine(string.Format("    hilited_candidate_text_color: 0x" + ReverseHex(c6.BackColor)));
            sw.WriteLine(string.Format("    hilited_candidate_back_color: 0x" + ReverseHex(c7.BackColor)));
            sw.WriteLine(string.Format("    candidate_text_color: 0x" + ReverseHex(c8.BackColor)));
            sw.WriteLine(string.Format("    comment_text_color: 0x" + ReverseHex(c9.BackColor)));
            sw.WriteLine(string.Format("    hilited_comment_text_color: 0x" + ReverseHex(c10.BackColor)));
            sw.WriteLine(string.Format("    label_color: 0x" + ReverseHex(c11.BackColor)));
            sw.Flush();
            sw.Close();
            SetColor();
            RestartProgress();
        }

        private void recovery_Click(object sender, EventArgs e)
        {
            List<string> lines = new List<string>(ReadAllLines(yaml));
            lines.RemoveRange(lines.Count-12,11);
            File.WriteAllLines(yaml, lines.ToArray());
            FileStream fs = new FileStream(@yaml, FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            sw.BaseStream.Seek(0, SeekOrigin.End);
            sw.WriteLine(string.Format("    back_color: 0xeceeee"));
            sw.WriteLine(string.Format("    border_color: 0xe0e0e0"));
            sw.WriteLine(string.Format("    text_color: 0x000000"));
            sw.WriteLine(string.Format("    hilited_text_color: 0x000000"));
            sw.WriteLine(string.Format("    hilited_back_color: 0xd4d4d4"));
            sw.WriteLine(string.Format("    hilited_candidate_text_color: 0xffffff"));
            sw.WriteLine(string.Format("    hilited_candidate_back_color: 0xfa3a0a"));
            sw.WriteLine(string.Format("    candidate_text_color: 0x000000"));
            sw.WriteLine(string.Format("    comment_text_color: 0x4f4f4f"));
            sw.WriteLine(string.Format("    hilited_comment_text_color: 0xfdbdad"));
            sw.WriteLine(string.Format("    label_color: 0x4f4f4f"));
            sw.Flush();
            sw.Close();
            RestartProgress();
            Application.ExitThread();
            Application.Exit();
            Application.Restart();
        }
    }
}
