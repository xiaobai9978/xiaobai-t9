using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using IWshRuntimeLibrary;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace t9configui
{
    public partial class config : Form
    {
        public config()
        {
            InitializeComponent();
        }

        private bool _adCollapsed = false;
        private readonly string _adStateFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "t9config_ad_state.txt");

        private static string appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        private void Form1_Load(object sender, EventArgs e)
        {
            hxsl1.Text = "9";
            //快捷方式文件的路径 = @"d:\Test.lnk";
            if (System.IO.File.Exists("C:\\ProgramData\\Microsoft\\Windows\\Start Menu\\Programs\\小白T9输入法\\【小白T9输入法】重新部署.lnk"))
            {
                WshShell shell = new WshShell();
                IWshShortcut 当前快捷方式文件IWshShortcut类 = (IWshShortcut)shell.CreateShortcut("C:\\ProgramData\\Microsoft\\Windows\\Start Menu\\Programs\\小白T9输入法\\【小白T9输入法】重新部署.lnk");
                快捷方式文件指向的路径.Text = 当前快捷方式文件IWshShortcut类.TargetPath;
                快捷方式文件指向的目标目录.Text = 当前快捷方式文件IWshShortcut类.WorkingDirectory;
               // return 当前快捷方式文件IWshShortcut类.TargetPath;
            }


            string appDir = AppDomain.CurrentDomain.BaseDirectory;
            try { string p = Path.Combine(appDir, "wechat_qr.png"); if (System.IO.File.Exists(p)) qrWechat.Image = Image.FromFile(p); } catch { }
            try { string p = Path.Combine(appDir, "alipay_qr.png"); if (System.IO.File.Exists(p)) qrAlipay.Image = Image.FromFile(p); } catch { }
            try { string p = Path.Combine(appDir, "taobao_qrcode.png"); if (System.IO.File.Exists(p)) qrTaobao.Image = Image.FromFile(p); } catch { }

            // Restore ad panel state
            try { if (System.IO.File.Exists(_adStateFile) && System.IO.File.ReadAllText(_adStateFile).Trim() == "1") _adCollapsed = true; } catch { }
            ApplyAdState();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            string strFilePath = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, 快捷方式文件指向的目标目录.Text + "\\data\\xiaobai_simp.schema.yaml");
            if (System.IO.File.Exists(strFilePath))
            {
                string strContent = System.IO.File.ReadAllText(strFilePath);
                strContent = Regex.Replace(strContent, "  spelling_hints: 0", "  spelling_hints: 99");
                System.IO.File.WriteAllText(strFilePath, strContent);
            }
            Process.Start("C:\\ProgramData\\Microsoft\\Windows\\Start Menu\\Programs\\小白T9输入法\\【小白T9输入法】重新部署.lnk");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string strFilePath = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, 快捷方式文件指向的目标目录.Text + "\\data\\xiaobai_simp.schema.yaml");
            if (System.IO.File.Exists(strFilePath))
            {
                string strContent = System.IO.File.ReadAllText(strFilePath);
                strContent = Regex.Replace(strContent, "  spelling_hints: 99", "  spelling_hints: 0");
                System.IO.File.WriteAllText(strFilePath, strContent);
            }
            Process.Start("C:\\ProgramData\\Microsoft\\Windows\\Start Menu\\Programs\\小白T9输入法\\【小白T9输入法】重新部署.lnk");

        }

        private void button3_Click(object sender, EventArgs e)
        {
             string strFilePath = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, appdata + @"\Rime\weasel.custom.yaml");
                if (System.IO.File.Exists(strFilePath))
                {
                    string strContent = System.IO.File.ReadAllText(strFilePath);
                    strContent = Regex.Replace(strContent, "  \"style/horizontal\": false", "  \"style/horizontal\": true");
                    System.IO.File.WriteAllText(strFilePath, strContent);
                }
                Process.Start("C:\\ProgramData\\Microsoft\\Windows\\Start Menu\\Programs\\小白T9输入法\\【小白T9输入法】重新部署.lnk");

            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string strFilePath = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, appdata + @"\Rime\weasel.custom.yaml");
            if (System.IO.File.Exists(strFilePath))
            {
                string strContent = System.IO.File.ReadAllText(strFilePath);
                strContent = Regex.Replace(strContent, "  \"style/horizontal\": true", "  \"style/horizontal\": false");
                System.IO.File.WriteAllText(strFilePath, strContent);
            }
            Process.Start("C:\\ProgramData\\Microsoft\\Windows\\Start Menu\\Programs\\小白T9输入法\\【小白T9输入法】重新部署.lnk");


        }

        private void button5_Click(object sender, EventArgs e)
        {
            
            FileStream stream = System.IO.File.Open(appdata+@"\Rime\weasel.custom.yaml", FileMode.OpenOrCreate, FileAccess.Write);
            stream.Seek(0, SeekOrigin.Begin);
            stream.SetLength(0);
            stream.Close();


            StreamWriter sw = new StreamWriter(appdata + @"\Rime\weasel.custom.yaml", true, Encoding.GetEncoding("UTF-8"));
            sw.WriteLine("customization:\n  distribution_code_name: Weasel\n  distribution_version: 0.14.3\n  generator: \"Weasel::UIStyleSettings\"\n  modified_time: \"Thu Oct 22 15:46:29 2020\"\n  rime_version: 1.5.3\npatch:\n  \"style/color_scheme\": dota_2\n  \"style/horizontal\": false");
            sw.Flush();
            sw.Close();
            Process.Start("C:\\ProgramData\\Microsoft\\Windows\\Start Menu\\Programs\\小白T9输入法\\【小白T9输入法】重新部署.lnk");









        }

        private void button6_Click(object sender, EventArgs e)
        {
            FileStream stream = System.IO.File.Open(appdata + @"\Rime\default.custom.yaml", FileMode.OpenOrCreate, FileAccess.Write);
            stream.Seek(0, SeekOrigin.Begin);
            stream.SetLength(0);
            stream.Close();

            StreamWriter sw = new StreamWriter(appdata + @"\Rime\default.custom.yaml", true, Encoding.GetEncoding("UTF-8"));
            sw.WriteLine("customization:\n  distribution_code_name: Weasel\n  distribution_version: 0.14.3\n  generator: \"Rime::SwitcherSettings\"\n  modified_time: \"Fri Aug  7 15:00:14 2020\"\n  rime_version: 1.5.3\npatch:\n  menu/page_size: "+ hxsl1.Text +"\n  schema_list:\n    - {schema: xiaobai_simp}");
            sw.Flush();
            sw.Close();
            Process.Start("C:\\ProgramData\\Microsoft\\Windows\\Start Menu\\Programs\\小白T9输入法\\【小白T9输入法】重新部署.lnk");


            Process.Start("C:\\ProgramData\\Microsoft\\Windows\\Start Menu\\Programs\\小白T9输入法\\【小白T9输入法】重新部署.lnk");

        }

        private void button8_Click(object sender, EventArgs e)
        {


            string text = string.Empty;
            using (FileStream fs = new FileStream(快捷方式文件指向的目标目录.Text + "\\data\\xiaobai_simp.schema.yaml", FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    text = sr.ReadToEnd();
                    text = text.Replace(@" #- derive/^([zcs])h/$1/", @" - derive/^([zcs])h/$1/");
                }
            }

            using (FileStream fs = new FileStream(快捷方式文件指向的目标目录.Text + "\\data\\xiaobai_simp.schema.yaml", FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.Write(text);
                }
            }

            Process.Start("C:\\ProgramData\\Microsoft\\Windows\\Start Menu\\Programs\\小白T9输入法\\【小白T9输入法】重新部署.lnk");
        }

        private void button7_Click(object sender, EventArgs e)
        {
            string text = string.Empty;
            using (FileStream fs = new FileStream(快捷方式文件指向的目标目录.Text + "\\data\\xiaobai_simp.schema.yaml", FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    text = sr.ReadToEnd();
                    text = text.Replace(@" - derive/^([zcs])h/$1/", @" #- derive/^([zcs])h/$1/");
                }
            }

            using (FileStream fs = new FileStream(快捷方式文件指向的目标目录.Text + "\\data\\xiaobai_simp.schema.yaml", FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.Write(text);
                }
            }

            Process.Start("C:\\ProgramData\\Microsoft\\Windows\\Start Menu\\Programs\\小白T9输入法\\【小白T9输入法】重新部署.lnk");
        }

        private void button10_Click(object sender, EventArgs e)
        {


            string text = string.Empty;
            using (FileStream fs = new FileStream(快捷方式文件指向的目标目录.Text + "\\data\\xiaobai_simp.schema.yaml", FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    text = sr.ReadToEnd();
                    text = text.Replace(@" #- derive/^([zcs])([^h])/$1h$2/", @" - derive/^([zcs])([^h])/$1h$2/");
                }
            }

            using (FileStream fs = new FileStream(快捷方式文件指向的目标目录.Text + "\\data\\xiaobai_simp.schema.yaml", FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.Write(text);
                }
            }

            Process.Start("C:\\ProgramData\\Microsoft\\Windows\\Start Menu\\Programs\\小白T9输入法\\【小白T9输入法】重新部署.lnk");

        }

        private void button9_Click(object sender, EventArgs e)
        {
            string text = string.Empty;
            using (FileStream fs = new FileStream(快捷方式文件指向的目标目录.Text + "\\data\\xiaobai_simp.schema.yaml", FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    text = sr.ReadToEnd();
                    text = text.Replace(@" - derive/^([zcs])([^h])/$1h$2/", @" #- derive/^([zcs])([^h])/$1h$2/");
                }
            }

            using (FileStream fs = new FileStream(快捷方式文件指向的目标目录.Text + "\\data\\xiaobai_simp.schema.yaml", FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.Write(text);
                }
            }

            Process.Start("C:\\ProgramData\\Microsoft\\Windows\\Start Menu\\Programs\\小白T9输入法\\【小白T9输入法】重新部署.lnk");
        }

        private void button14_Click(object sender, EventArgs e)
        {

            string text = string.Empty;
            using (FileStream fs = new FileStream(快捷方式文件指向的目标目录.Text + "\\data\\xiaobai_simp.schema.yaml", FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    text = sr.ReadToEnd();
                    text = text.Replace(@" #- derive/^n/l/", @" - derive/^n/l/");
                }
            }

            using (FileStream fs = new FileStream(快捷方式文件指向的目标目录.Text + "\\data\\xiaobai_simp.schema.yaml", FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.Write(text);
                }
            }

            Process.Start("C:\\ProgramData\\Microsoft\\Windows\\Start Menu\\Programs\\小白T9输入法\\【小白T9输入法】重新部署.lnk");


        }

        private void button13_Click(object sender, EventArgs e)
        {
            string text = string.Empty;
            using (FileStream fs = new FileStream(快捷方式文件指向的目标目录.Text + "\\data\\xiaobai_simp.schema.yaml", FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    text = sr.ReadToEnd();
                    text = text.Replace(@" - derive/^n/l/", @" #- derive/^n/l/");
                }
            }

            using (FileStream fs = new FileStream(快捷方式文件指向的目标目录.Text + "\\data\\xiaobai_simp.schema.yaml", FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.Write(text);
                }
            }

            Process.Start("C:\\ProgramData\\Microsoft\\Windows\\Start Menu\\Programs\\小白T9输入法\\【小白T9输入法】重新部署.lnk");

        }

        private void button12_Click(object sender, EventArgs e)
        {
            string text = string.Empty;
            using (FileStream fs = new FileStream(快捷方式文件指向的目标目录.Text + "\\data\\xiaobai_simp.schema.yaml", FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    text = sr.ReadToEnd();
                    text = text.Replace(@" #- derive/^l/n/", @" - derive/^l/n/");
                }
            }

            using (FileStream fs = new FileStream(快捷方式文件指向的目标目录.Text + "\\data\\xiaobai_simp.schema.yaml", FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.Write(text);
                }
            }

            Process.Start("C:\\ProgramData\\Microsoft\\Windows\\Start Menu\\Programs\\小白T9输入法\\【小白T9输入法】重新部署.lnk");
        }

        private void button11_Click(object sender, EventArgs e)
        {
            string text = string.Empty;
            using (FileStream fs = new FileStream(快捷方式文件指向的目标目录.Text + "\\data\\xiaobai_simp.schema.yaml", FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    text = sr.ReadToEnd();
                    text = text.Replace(@" - derive/^l/n/", @" #- derive/^l/n/");
                }
            }

            using (FileStream fs = new FileStream(快捷方式文件指向的目标目录.Text + "\\data\\xiaobai_simp.schema.yaml", FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.Write(text);
                }
            }

            Process.Start("C:\\ProgramData\\Microsoft\\Windows\\Start Menu\\Programs\\小白T9输入法\\【小白T9输入法】重新部署.lnk");
        }

        private void button22_Click(object sender, EventArgs e)
        {
            string text = string.Empty;
            using (FileStream fs = new FileStream(快捷方式文件指向的目标目录.Text + "\\data\\xiaobai_simp.schema.yaml", FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    text = sr.ReadToEnd();
                    text = text.Replace(@" #- derive/^r/l/", @" - derive/^r/l/");
                }
            }

            using (FileStream fs = new FileStream(快捷方式文件指向的目标目录.Text + "\\data\\xiaobai_simp.schema.yaml", FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.Write(text);
                }
            }

            Process.Start("C:\\ProgramData\\Microsoft\\Windows\\Start Menu\\Programs\\小白T9输入法\\【小白T9输入法】重新部署.lnk");
        }

        private void button21_Click(object sender, EventArgs e)
        {
            string text = string.Empty;
            using (FileStream fs = new FileStream(快捷方式文件指向的目标目录.Text + "\\data\\xiaobai_simp.schema.yaml", FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    text = sr.ReadToEnd();
                    text = text.Replace(@" - derive/^r/l/", @" #- derive/^r/l/");
                }
            }

            using (FileStream fs = new FileStream(快捷方式文件指向的目标目录.Text + "\\data\\xiaobai_simp.schema.yaml", FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.Write(text);
                }
            }

            Process.Start("C:\\ProgramData\\Microsoft\\Windows\\Start Menu\\Programs\\小白T9输入法\\【小白T9输入法】重新部署.lnk");
        }

        private void button20_Click(object sender, EventArgs e)
        {
            string text = string.Empty;
            using (FileStream fs = new FileStream(快捷方式文件指向的目标目录.Text + "\\data\\xiaobai_simp.schema.yaml", FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    text = sr.ReadToEnd();
                    text = text.Replace(@" #- derive/^ren/yin/", @" - derive/^ren/yin/");
                }
            }

            using (FileStream fs = new FileStream(快捷方式文件指向的目标目录.Text + "\\data\\xiaobai_simp.schema.yaml", FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.Write(text);
                }
            }

            Process.Start("C:\\ProgramData\\Microsoft\\Windows\\Start Menu\\Programs\\小白T9输入法\\【小白T9输入法】重新部署.lnk");
        }

        private void button19_Click(object sender, EventArgs e)
        {
            string text = string.Empty;
            using (FileStream fs = new FileStream(快捷方式文件指向的目标目录.Text + "\\data\\xiaobai_simp.schema.yaml", FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    text = sr.ReadToEnd();
                    text = text.Replace(@" - derive/^ren/yin/", @" #- derive/^ren/yin/");
                }
            }

            using (FileStream fs = new FileStream(快捷方式文件指向的目标目录.Text + "\\data\\xiaobai_simp.schema.yaml", FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.Write(text);
                }
            }

            Process.Start("C:\\ProgramData\\Microsoft\\Windows\\Start Menu\\Programs\\小白T9输入法\\【小白T9输入法】重新部署.lnk");
        }

        private void button18_Click(object sender, EventArgs e)
        {
            string text = string.Empty;
            using (FileStream fs = new FileStream(快捷方式文件指向的目标目录.Text + "\\data\\xiaobai_simp.schema.yaml", FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    text = sr.ReadToEnd();
                    text = text.Replace(@" #- derive/^r/y/", @" - derive/^r/y/");
                }
            }

            using (FileStream fs = new FileStream(快捷方式文件指向的目标目录.Text + "\\data\\xiaobai_simp.schema.yaml", FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.Write(text);
                }
            }

            Process.Start("C:\\ProgramData\\Microsoft\\Windows\\Start Menu\\Programs\\小白T9输入法\\【小白T9输入法】重新部署.lnk");
        }

        private void button17_Click(object sender, EventArgs e)
        {
            string text = string.Empty;
            using (FileStream fs = new FileStream(快捷方式文件指向的目标目录.Text + "\\data\\xiaobai_simp.schema.yaml", FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    text = sr.ReadToEnd();
                    text = text.Replace(@" - derive/^r/y/", @" #- derive/^r/y/");
                }
            }

            using (FileStream fs = new FileStream(快捷方式文件指向的目标目录.Text + "\\data\\xiaobai_simp.schema.yaml", FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.Write(text);
                }
            }

            Process.Start("C:\\ProgramData\\Microsoft\\Windows\\Start Menu\\Programs\\小白T9输入法\\【小白T9输入法】重新部署.lnk");
        }

        private void button16_Click(object sender, EventArgs e)
        {
            string text = string.Empty;
            using (FileStream fs = new FileStream(快捷方式文件指向的目标目录.Text + "\\data\\xiaobai_simp.schema.yaml", FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    text = sr.ReadToEnd();
                    text = text.Replace(@" #- derive/^hu$/fu/", @" - derive/^hu$/fu/");
                }
            }

            using (FileStream fs = new FileStream(快捷方式文件指向的目标目录.Text + "\\data\\xiaobai_simp.schema.yaml", FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.Write(text);
                }
            }

            Process.Start("C:\\ProgramData\\Microsoft\\Windows\\Start Menu\\Programs\\小白T9输入法\\【小白T9输入法】重新部署.lnk");
        }

        private void button15_Click(object sender, EventArgs e)
        {
            string text = string.Empty;
            using (FileStream fs = new FileStream(快捷方式文件指向的目标目录.Text + "\\data\\xiaobai_simp.schema.yaml", FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    text = sr.ReadToEnd();
                    text = text.Replace(@" - derive/^hu$/fu/", @" #- derive/^hu$/fu/");
                }
            }

            using (FileStream fs = new FileStream(快捷方式文件指向的目标目录.Text + "\\data\\xiaobai_simp.schema.yaml", FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.Write(text);
                }
            }

            Process.Start("C:\\ProgramData\\Microsoft\\Windows\\Start Menu\\Programs\\小白T9输入法\\【小白T9输入法】重新部署.lnk");
        }

        private void button28_Click(object sender, EventArgs e)
        {
            string text = string.Empty;
            using (FileStream fs = new FileStream(快捷方式文件指向的目标目录.Text + "\\data\\xiaobai_simp.schema.yaml", FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    text = sr.ReadToEnd();
                    text = text.Replace(@" #- derive/^hong$/feng/", @" - derive/^hong$/feng/");
                }
            }

            using (FileStream fs = new FileStream(快捷方式文件指向的目标目录.Text + "\\data\\xiaobai_simp.schema.yaml", FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.Write(text);
                }
            }

            Process.Start("C:\\ProgramData\\Microsoft\\Windows\\Start Menu\\Programs\\小白T9输入法\\【小白T9输入法】重新部署.lnk");
        }

        private void button27_Click(object sender, EventArgs e)
        {
            string text = string.Empty;
            using (FileStream fs = new FileStream(快捷方式文件指向的目标目录.Text + "\\data\\xiaobai_simp.schema.yaml", FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    text = sr.ReadToEnd();
                    text = text.Replace(@" - derive/^hong$/feng/", @" #- derive/^hong$/feng/");
                }
            }

            using (FileStream fs = new FileStream(快捷方式文件指向的目标目录.Text + "\\data\\xiaobai_simp.schema.yaml", FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.Write(text);
                }
            }

            Process.Start("C:\\ProgramData\\Microsoft\\Windows\\Start Menu\\Programs\\小白T9输入法\\【小白T9输入法】重新部署.lnk");
        }

        private void button26_Click(object sender, EventArgs e)
        {
            string text = string.Empty;
            using (FileStream fs = new FileStream(快捷方式文件指向的目标目录.Text + "\\data\\xiaobai_simp.schema.yaml", FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    text = sr.ReadToEnd();
                    text = text.Replace(@" #- derive/^hu([in])$/fe$1/", @" - derive/^hu([in])$/fe$1/");
                }
            }

            using (FileStream fs = new FileStream(快捷方式文件指向的目标目录.Text + "\\data\\xiaobai_simp.schema.yaml", FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.Write(text);
                }
            }

            Process.Start("C:\\ProgramData\\Microsoft\\Windows\\Start Menu\\Programs\\小白T9输入法\\【小白T9输入法】重新部署.lnk");
        }

        private void button25_Click(object sender, EventArgs e)
        {
            string text = string.Empty;
            using (FileStream fs = new FileStream(快捷方式文件指向的目标目录.Text + "\\data\\xiaobai_simp.schema.yaml", FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    text = sr.ReadToEnd();
                    text = text.Replace(@" - derive/^hu([in])$/fe$1/", @" #- derive/^hu([in])$/fe$1/");
                }
            }

            using (FileStream fs = new FileStream(快捷方式文件指向的目标目录.Text + "\\data\\xiaobai_simp.schema.yaml", FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.Write(text);
                }
            }

            Process.Start("C:\\ProgramData\\Microsoft\\Windows\\Start Menu\\Programs\\小白T9输入法\\【小白T9输入法】重新部署.lnk");
        }

        private void button24_Click(object sender, EventArgs e)
        {
            string text = string.Empty;
            using (FileStream fs = new FileStream(快捷方式文件指向的目标目录.Text + "\\data\\xiaobai_simp.schema.yaml", FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    text = sr.ReadToEnd();
                    text = text.Replace(@" #- derive/^f([ao])/hu$1/", @" - derive/^f([ao])/hu$1/");
                }
            }

            using (FileStream fs = new FileStream(快捷方式文件指向的目标目录.Text + "\\data\\xiaobai_simp.schema.yaml", FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.Write(text);
                }
            }

            Process.Start("C:\\ProgramData\\Microsoft\\Windows\\Start Menu\\Programs\\小白T9输入法\\【小白T9输入法】重新部署.lnk");
        }

        private void button23_Click(object sender, EventArgs e)
        {
            string text = string.Empty;
            using (FileStream fs = new FileStream(快捷方式文件指向的目标目录.Text + "\\data\\xiaobai_simp.schema.yaml", FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    text = sr.ReadToEnd();
                    text = text.Replace(@" #- derive/^f([ao])/hu$1/", @" - derive/^f([ao])/hu$1/");
                }
            }

            using (FileStream fs = new FileStream(快捷方式文件指向的目标目录.Text + "\\data\\xiaobai_simp.schema.yaml", FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.Write(text);
                }
            }

            Process.Start("C:\\ProgramData\\Microsoft\\Windows\\Start Menu\\Programs\\小白T9输入法\\【小白T9输入法】重新部署.lnk");
        }

        private void button50_Click(object sender, EventArgs e)
        {
            string text = string.Empty;
            using (FileStream fs = new FileStream(快捷方式文件指向的目标目录.Text + "\\data\\xiaobai_simp.schema.yaml", FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    text = sr.ReadToEnd();
                    text = text.Replace(@" #- derive/^([bpmf])eng$/$1ong/", @" - derive/^([bpmf])eng$/$1ong/");
                }
            }

            using (FileStream fs = new FileStream(快捷方式文件指向的目标目录.Text + "\\data\\xiaobai_simp.schema.yaml", FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.Write(text);
                }
            }

            Process.Start("C:\\ProgramData\\Microsoft\\Windows\\Start Menu\\Programs\\小白T9输入法\\【小白T9输入法】重新部署.lnk");
        }

        private void button49_Click(object sender, EventArgs e)
        {
            string text = string.Empty;
            using (FileStream fs = new FileStream(快捷方式文件指向的目标目录.Text + "\\data\\xiaobai_simp.schema.yaml", FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    text = sr.ReadToEnd();
                    text = text.Replace(@" - derive/^([bpmf])eng$/$1ong/", @" #- derive/^([bpmf])eng$/$1ong/");
                }
            }

            using (FileStream fs = new FileStream(快捷方式文件指向的目标目录.Text + "\\data\\xiaobai_simp.schema.yaml", FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.Write(text);
                }
            }

            Process.Start("C:\\ProgramData\\Microsoft\\Windows\\Start Menu\\Programs\\小白T9输入法\\【小白T9输入法】重新部署.lnk");
        }

        private void button48_Click(object sender, EventArgs e)
        {
            string text = string.Empty;
            using (FileStream fs = new FileStream(快捷方式文件指向的目标目录.Text + "\\data\\xiaobai_simp.schema.yaml", FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    text = sr.ReadToEnd();
                    text = text.Replace(@" #- derive/([ei])n$/$1ng/", @" - derive/([ei])n$/$1ng/");
                }
            }

            using (FileStream fs = new FileStream(快捷方式文件指向的目标目录.Text + "\\data\\xiaobai_simp.schema.yaml", FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.Write(text);
                }
            }

            Process.Start("C:\\ProgramData\\Microsoft\\Windows\\Start Menu\\Programs\\小白T9输入法\\【小白T9输入法】重新部署.lnk");
        }

        private void button47_Click(object sender, EventArgs e)
        {
            string text = string.Empty;
            using (FileStream fs = new FileStream(快捷方式文件指向的目标目录.Text + "\\data\\xiaobai_simp.schema.yaml", FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    text = sr.ReadToEnd();
                    text = text.Replace(@" - derive/([ei])n$/$1ng/", @" #- derive/([ei])n$/$1ng/");
                }
            }

            using (FileStream fs = new FileStream(快捷方式文件指向的目标目录.Text + "\\data\\xiaobai_simp.schema.yaml", FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.Write(text);
                }
            }

            Process.Start("C:\\ProgramData\\Microsoft\\Windows\\Start Menu\\Programs\\小白T9输入法\\【小白T9输入法】重新部署.lnk");
        }

        private void button46_Click(object sender, EventArgs e)
        {
            string text = string.Empty;
            using (FileStream fs = new FileStream(快捷方式文件指向的目标目录.Text + "\\data\\xiaobai_simp.schema.yaml", FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    text = sr.ReadToEnd();
                    text = text.Replace(@" #- derive/([ei])ng$/$1n/", @" - derive/([ei])ng$/$1n/");
                }
            }

            using (FileStream fs = new FileStream(快捷方式文件指向的目标目录.Text + "\\data\\xiaobai_simp.schema.yaml", FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.Write(text);
                }
            }

            Process.Start("C:\\ProgramData\\Microsoft\\Windows\\Start Menu\\Programs\\小白T9输入法\\【小白T9输入法】重新部署.lnk");
        }

        private void button45_Click(object sender, EventArgs e)
        {
            string text = string.Empty;
            using (FileStream fs = new FileStream(快捷方式文件指向的目标目录.Text + "\\data\\xiaobai_simp.schema.yaml", FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    text = sr.ReadToEnd();
                    text = text.Replace(@" - derive/([ei])ng$/$1n/", @" #- derive/([ei])ng$/$1n/");
                }
            }

            using (FileStream fs = new FileStream(快捷方式文件指向的目标目录.Text + "\\data\\xiaobai_simp.schema.yaml", FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.Write(text);
                }
            }

            Process.Start("C:\\ProgramData\\Microsoft\\Windows\\Start Menu\\Programs\\小白T9输入法\\【小白T9输入法】重新部署.lnk");
        }

        private void button44_Click(object sender, EventArgs e)
        {
            string text = string.Empty;
            using (FileStream fs = new FileStream(快捷方式文件指向的目标目录.Text + "\\data\\xiaobai_simp.schema.yaml", FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    text = sr.ReadToEnd();
                    text = text.Replace(@" #- derive/^un/iong/", @" - derive/^un/iong/");
                }
            }

            using (FileStream fs = new FileStream(快捷方式文件指向的目标目录.Text + "\\data\\xiaobai_simp.schema.yaml", FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.Write(text);
                }
            }

            Process.Start("C:\\ProgramData\\Microsoft\\Windows\\Start Menu\\Programs\\小白T9输入法\\【小白T9输入法】重新部署.lnk");
        }

        private void button43_Click(object sender, EventArgs e)
        {
            string text = string.Empty;
            using (FileStream fs = new FileStream(快捷方式文件指向的目标目录.Text + "\\data\\xiaobai_simp.schema.yaml", FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    text = sr.ReadToEnd();
                    text = text.Replace(@" - derive/^un/iong/", @" #- derive/^un/iong/");
                }
            }

            using (FileStream fs = new FileStream(快捷方式文件指向的目标目录.Text + "\\data\\xiaobai_simp.schema.yaml", FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.Write(text);
                }
            }

            Process.Start("C:\\ProgramData\\Microsoft\\Windows\\Start Menu\\Programs\\小白T9输入法\\【小白T9输入法】重新部署.lnk");
        }

        private void button42_Click(object sender, EventArgs e)
        {
            string text = string.Empty;
            using (FileStream fs = new FileStream(快捷方式文件指向的目标目录.Text + "\\data\\xiaobai_simp.schema.yaml", FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    text = sr.ReadToEnd();
                    text = text.Replace(@" #- derive/^iong/un/", @" - derive/^iong/un/");
                }
            }

            using (FileStream fs = new FileStream(快捷方式文件指向的目标目录.Text + "\\data\\xiaobai_simp.schema.yaml", FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.Write(text);
                }
            }

            Process.Start("C:\\ProgramData\\Microsoft\\Windows\\Start Menu\\Programs\\小白T9输入法\\【小白T9输入法】重新部署.lnk");
        }

        private void button41_Click(object sender, EventArgs e)
        {
            string text = string.Empty;
            using (FileStream fs = new FileStream(快捷方式文件指向的目标目录.Text + "\\data\\xiaobai_simp.schema.yaml", FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    text = sr.ReadToEnd();
                    text = text.Replace(@" - derive/^iong/un/", @" #- derive/^iong/un/");
                }
            }

            using (FileStream fs = new FileStream(快捷方式文件指向的目标目录.Text + "\\data\\xiaobai_simp.schema.yaml", FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.Write(text);
                }
            }

            Process.Start("C:\\ProgramData\\Microsoft\\Windows\\Start Menu\\Programs\\小白T9输入法\\【小白T9输入法】重新部署.lnk");
        }

        private void button40_Click(object sender, EventArgs e)
        {
            string text = string.Empty;
            using (FileStream fs = new FileStream(快捷方式文件指向的目标目录.Text + "\\data\\xiaobai_simp.schema.yaml", FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    text = sr.ReadToEnd();
                    text = text.Replace(@" #- derive/^an/ang/", @" - derive/^an/ang/");
                }
            }

            using (FileStream fs = new FileStream(快捷方式文件指向的目标目录.Text + "\\data\\xiaobai_simp.schema.yaml", FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.Write(text);
                }
            }

            Process.Start("C:\\ProgramData\\Microsoft\\Windows\\Start Menu\\Programs\\小白T9输入法\\【小白T9输入法】重新部署.lnk");
        }

        private void button39_Click(object sender, EventArgs e)
        {
            string text = string.Empty;
            using (FileStream fs = new FileStream(快捷方式文件指向的目标目录.Text + "\\data\\xiaobai_simp.schema.yaml", FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    text = sr.ReadToEnd();
                    text = text.Replace(@" - derive/^an/ang/", @" #- derive/^an/ang/");
                }
            }

            using (FileStream fs = new FileStream(快捷方式文件指向的目标目录.Text + "\\data\\xiaobai_simp.schema.yaml", FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.Write(text);
                }
            }

            Process.Start("C:\\ProgramData\\Microsoft\\Windows\\Start Menu\\Programs\\小白T9输入法\\【小白T9输入法】重新部署.lnk");
        }

        private void button38_Click(object sender, EventArgs e)
        {
            string text = string.Empty;
            using (FileStream fs = new FileStream(快捷方式文件指向的目标目录.Text + "\\data\\xiaobai_simp.schema.yaml", FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    text = sr.ReadToEnd();
                    text = text.Replace(@" #- derive/^ian/iang/", @" - derive/^ian/iang/");
                }
            }

            using (FileStream fs = new FileStream(快捷方式文件指向的目标目录.Text + "\\data\\xiaobai_simp.schema.yaml", FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.Write(text);
                }
            }

            Process.Start("C:\\ProgramData\\Microsoft\\Windows\\Start Menu\\Programs\\小白T9输入法\\【小白T9输入法】重新部署.lnk");
        }

        private void button37_Click(object sender, EventArgs e)
        {
            string text = string.Empty;
            using (FileStream fs = new FileStream(快捷方式文件指向的目标目录.Text + "\\data\\xiaobai_simp.schema.yaml", FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    text = sr.ReadToEnd();
                    text = text.Replace(@" - derive/^ian/iang/", @" #- derive/^ian/iang/");
                }
            }

            using (FileStream fs = new FileStream(快捷方式文件指向的目标目录.Text + "\\data\\xiaobai_simp.schema.yaml", FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.Write(text);
                }
            }

            Process.Start("C:\\ProgramData\\Microsoft\\Windows\\Start Menu\\Programs\\小白T9输入法\\【小白T9输入法】重新部署.lnk");
        }

        private void button36_Click(object sender, EventArgs e)
        {
            string text = string.Empty;
            using (FileStream fs = new FileStream(快捷方式文件指向的目标目录.Text + "\\data\\xiaobai_simp.schema.yaml", FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    text = sr.ReadToEnd();
                    text = text.Replace(@" #- derive/^uan/uang/", @" - derive/^uan/uang/");
                }
            }

            using (FileStream fs = new FileStream(快捷方式文件指向的目标目录.Text + "\\data\\xiaobai_simp.schema.yaml", FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.Write(text);
                }
            }

            Process.Start("C:\\ProgramData\\Microsoft\\Windows\\Start Menu\\Programs\\小白T9输入法\\【小白T9输入法】重新部署.lnk");
        }

        private void button35_Click(object sender, EventArgs e)
        {
            string text = string.Empty;
            using (FileStream fs = new FileStream(快捷方式文件指向的目标目录.Text + "\\data\\xiaobai_simp.schema.yaml", FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    text = sr.ReadToEnd();
                    text = text.Replace(@" - derive/^uan/uang/", @" #- derive/^uan/uang/");
                }
            }

            using (FileStream fs = new FileStream(快捷方式文件指向的目标目录.Text + "\\data\\xiaobai_simp.schema.yaml", FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.Write(text);
                }
            }

            Process.Start("C:\\ProgramData\\Microsoft\\Windows\\Start Menu\\Programs\\小白T9输入法\\【小白T9输入法】重新部署.lnk");
        }

        private void button32_Click(object sender, EventArgs e)
        {

        }

        private void button29_Click(object sender, EventArgs e)
        {
            string strFilePath = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, appdata + @"\Rime\build\weasel.yaml");
            if (System.IO.File.Exists(strFilePath))
            {
                string strContent = System.IO.File.ReadAllText(strFilePath);
                strContent = Regex.Replace(strContent, "  font_point: .*\r\n", "  font_point: "+hxdx1.Text+"\r\n");
                System.IO.File.WriteAllText(strFilePath, strContent);
            }
            Process.Start("C:\\ProgramData\\Microsoft\\Windows\\Start Menu\\Programs\\小白T9输入法\\【小白T9输入法】重新部署.lnk");

        }

        private void button31_Click(object sender, EventArgs e)
        {
            string text = string.Empty;
            using (FileStream fs = new FileStream(快捷方式文件指向的目标目录.Text + "\\data\\xiaobai_simp.schema.yaml", FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    text = sr.ReadToEnd();
                    text = text.Replace(@"- xform/;/7/", @"#- xform/;/7/");
                    text = text.Replace(@"- xform/^;/7/", @"#- xform/^;/7/");
                    text = text.Replace(@"#delimiter: ""7""", @"delimiter: ""7""");
                    text = text.Replace(@"#delimiter: ""7""", @"delimiter: ""7""");
                    text = text.Replace(@"#delimiter: ""7""", @"delimiter: ""7""");
                    text = text.Replace(@"#delimiter: ""7""", @"delimiter: ""7""");
                    text = text.Replace(@"#delimiter: ""7""", @"delimiter: ""7""");
                    text = text.Replace(@"#delimiter: ""7""", @"delimiter: ""7""");
                    text = text.Replace(@"#delimiter: ""7""", @"delimiter: ""7""");
                    text = text.Replace(@"#delimiter: ""7""", @"delimiter: ""7""");
                    text = text.Replace(@"#delimiter: ""7""", @"delimiter: ""7""");
                }
            }

            using (FileStream fs = new FileStream(快捷方式文件指向的目标目录.Text + "\\data\\xiaobai_simp.schema.yaml", FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.Write(text);
                }
            }

            Process.Start("C:\\ProgramData\\Microsoft\\Windows\\Start Menu\\Programs\\小白T9输入法\\【小白T9输入法】重新部署.lnk");

        }

        private void button30_Click(object sender, EventArgs e)
        {
            string text = string.Empty;
            using (FileStream fs = new FileStream(快捷方式文件指向的目标目录.Text + "\\data\\xiaobai_simp.schema.yaml", FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    text = sr.ReadToEnd();
                    text = text.Replace(@"#- xform/;/7/", @"- xform/;/7/");
                    text = text.Replace(@"#- xform/^;/7/", @"- xform/^;/7/");
                    text = text.Replace(@"#- xform/;/7/", @"- xform/;/7/");
                    text = text.Replace(@"#- xform/^;/7/", @"- xform/^;/7/");
                    text = text.Replace(@"#- xform/;/7/", @"- xform/;/7/");
                    text = text.Replace(@"#- xform/^;/7/", @"- xform/^;/7/");
                    text = text.Replace(@"#- xform/;/7/", @"- xform/;/7/");
                    text = text.Replace(@"#- xform/^;/7/", @"- xform/^;/7/");
                    text = text.Replace(@"#- xform/;/7/", @"- xform/;/7/");
                    text = text.Replace(@"#- xform/^;/7/", @"- xform/^;/7/");
                    text = text.Replace(@"#- xform/;/7/", @"- xform/;/7/");
                    text = text.Replace(@"#- xform/^;/7/", @"- xform/^;/7/");
                    text = text.Replace(@"#- xform/;/7/", @"- xform/;/7/");
                    text = text.Replace(@"#- xform/^;/7/", @"- xform/^;/7/");
                    text = text.Replace(@"#- xform/;/7/", @"- xform/;/7/");
                    text = text.Replace(@"#- xform/^;/7/", @"- xform/^;/7/");
                    //多删几遍,防止出现多个#号
                    text = text.Replace(@"delimiter: ""7""", @"#delimiter: ""7""");
                }
            }

            using (FileStream fs = new FileStream(快捷方式文件指向的目标目录.Text + "\\data\\xiaobai_simp.schema.yaml", FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.Write(text);
                }
            }

            Process.Start("C:\\ProgramData\\Microsoft\\Windows\\Start Menu\\Programs\\小白T9输入法\\【小白T9输入法】重新部署.lnk");
        }

        private void button33_Click(object sender, EventArgs e)
        {

            string text = string.Empty;
            using (FileStream fs = new FileStream(快捷方式文件指向的目标目录.Text + "\\data\\xiaobai_simp.schema.yaml", FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    text = sr.ReadToEnd();
                    text = text.Replace(@"- {accept: Left, send: Left, when: has_menu}", @"- {accept: Left, send: Page_Up, when: has_menu}");
                    text = text.Replace(@"- {accept: Right, send: Right, when: has_menu}", @"- {accept: Right, send: Page_Down, when: has_menu}");
                }
            }

            using (FileStream fs = new FileStream(快捷方式文件指向的目标目录.Text + "\\data\\xiaobai_simp.schema.yaml", FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.Write(text);
                }
            }

            Process.Start("C:\\ProgramData\\Microsoft\\Windows\\Start Menu\\Programs\\小白T9输入法\\【小白T9输入法】重新部署.lnk");
        }

        private void button32_Click_1(object sender, EventArgs e)
        {
            string text = string.Empty;
            using (FileStream fs = new FileStream(快捷方式文件指向的目标目录.Text + "\\data\\xiaobai_simp.schema.yaml", FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    text = sr.ReadToEnd();
                    text = text.Replace(@"- {accept: Left, send: Page_Up, when: has_menu}", @"- {accept: Left, send: Left, when: has_menu}");
                    text = text.Replace(@"- {accept: Right, send: Page_Down, when: has_menu}", @"- {accept: Right, send: Right, when: has_menu}");
                }
            }

            using (FileStream fs = new FileStream(快捷方式文件指向的目标目录.Text + "\\data\\xiaobai_simp.schema.yaml", FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.Write(text);
                }
            }

            Process.Start("C:\\ProgramData\\Microsoft\\Windows\\Start Menu\\Programs\\小白T9输入法\\【小白T9输入法】重新部署.lnk");
        }

        private void button51_Click(object sender, EventArgs e)
        {
            string text = string.Empty;
            using (FileStream fs = new FileStream(快捷方式文件指向的目标目录.Text + "\\data\\xiaobai.dict.yaml", FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    text = sr.ReadToEnd();
                    text = text.Replace(@"  #- dicts/ywz", @"  - dicts/ywz");
                }
            }

            using (FileStream fs = new FileStream(快捷方式文件指向的目标目录.Text + "\\data\\xiaobai.dict.yaml", FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.Write(text);
                }
            }

            Process.Start("C:\\ProgramData\\Microsoft\\Windows\\Start Menu\\Programs\\小白T9输入法\\【小白T9输入法】重新部署.lnk");
        }

        private void button34_Click(object sender, EventArgs e)
        {
            string text = string.Empty;
            using (FileStream fs = new FileStream(快捷方式文件指向的目标目录.Text + "\\data\\xiaobai.dict.yaml", FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    text = sr.ReadToEnd();
                    text = text.Replace(@"  - dicts/ywz", @"  #- dicts/ywz");
                }
            }

            using (FileStream fs = new FileStream(快捷方式文件指向的目标目录.Text + "\\data\\xiaobai.dict.yaml", FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.Write(text);
                }
            }

            Process.Start("C:\\ProgramData\\Microsoft\\Windows\\Start Menu\\Programs\\小白T9输入法\\【小白T9输入法】重新部署.lnk");
        }

        private void button53_Click(object sender, EventArgs e)
        {
            string text = string.Empty;
            using (FileStream fs = new FileStream(快捷方式文件指向的目标目录.Text + "\\data\\xiaobai_simp.custom.yaml", FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    text = sr.ReadToEnd();
                    text = text.Replace(@"    #simplifier@emoji_suggestion", @"    simplifier@emoji_suggestion");
                }
            }

            using (FileStream fs = new FileStream(快捷方式文件指向的目标目录.Text + "\\data\\xiaobai_simp.custom.yaml", FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.Write(text);
                }
            }

            Process.Start("C:\\ProgramData\\Microsoft\\Windows\\Start Menu\\Programs\\小白T9输入法\\【小白T9输入法】重新部署.lnk");
        }

        private void button52_Click(object sender, EventArgs e)
        {
            string text = string.Empty;
            using (FileStream fs = new FileStream(快捷方式文件指向的目标目录.Text + "\\data\\xiaobai_simp.custom.yaml", FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    text = sr.ReadToEnd();
                    text = text.Replace(@"    simplifier@emoji_suggestion", @"    #simplifier@emoji_suggestion");
                }
            }

            using (FileStream fs = new FileStream(快捷方式文件指向的目标目录.Text + "\\data\\xiaobai_simp.custom.yaml", FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.Write(text);
                }
            }

            Process.Start("C:\\ProgramData\\Microsoft\\Windows\\Start Menu\\Programs\\小白T9输入法\\【小白T9输入法】重新部署.lnk");
        }

        private void button55_Click(object sender, EventArgs e)
        {
            string text = string.Empty;
            using (FileStream fs = new FileStream(快捷方式文件指向的目标目录.Text + "\\data\\xiaobai_simp.schema.yaml", FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    text = sr.ReadToEnd();
                    text = text.Replace(@"    #- derive/^([a-z]{1})([a-z])$/$1/", @"    - derive/^([a-z]{1})([a-z])$/$1/");
                }
            }

            using (FileStream fs = new FileStream(快捷方式文件指向的目标目录.Text + "\\data\\xiaobai_simp.schema.yaml", FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.Write(text);
                }
            }

            Process.Start("C:\\ProgramData\\Microsoft\\Windows\\Start Menu\\Programs\\小白T9输入法\\【小白T9输入法】重新部署.lnk");
        }

        private void button54_Click(object sender, EventArgs e)
        {
            string text = string.Empty;
            using (FileStream fs = new FileStream(快捷方式文件指向的目标目录.Text + "\\data\\xiaobai_simp.schema.yaml", FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    text = sr.ReadToEnd();
                    text = text.Replace(@"    - derive/^([a-z]{1})([a-z])$/$1/", @"    #- derive/^([a-z]{1})([a-z])$/$1/");
                }
            }

            using (FileStream fs = new FileStream(快捷方式文件指向的目标目录.Text + "\\data\\xiaobai_simp.schema.yaml", FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.Write(text);
                }
            }

            Process.Start("C:\\ProgramData\\Microsoft\\Windows\\Start Menu\\Programs\\小白T9输入法\\【小白T9输入法】重新部署.lnk");
        }

        private void label18_MouseEnter(object sender, EventArgs e)
        {
            候选大小tip.Show("默认14", (Label)sender);
        }

        private void button29_MouseEnter(object sender, EventArgs e)
        {
            候选大小tip.Show("默认14", (Button)sender);
        }

        private void label34_MouseEnter(object sender, EventArgs e)
        {
            多一码候选tip.Show("没特殊要求不建议开启", (Label)sender);
        }

        private void button55_MouseEnter(object sender, EventArgs e)
        {
            多一码候选tip.Show("没特殊要求不建议开启", (Button)sender);
        }

        private void button57_Click(object sender, EventArgs e)
        {// 注意：如果是针对特定方案，建议修改 xiaobai_simp.custom.yaml；如果是全局，则修改 default.custom.yaml
            string text = string.Empty;
            using (FileStream fs = new FileStream(快捷方式文件指向的目标目录.Text + "\\data\\xiaobai_simp.schema.yaml", FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    text = sr.ReadToEnd();
                    text = text.Replace(@"  enable_completion: false", @"  enable_completion: true");
                }
            }

            using (FileStream fs = new FileStream(快捷方式文件指向的目标目录.Text + "\\data\\xiaobai_simp.schema.yaml", FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.Write(text);
                }
            }

            Process.Start("C:\\ProgramData\\Microsoft\\Windows\\Start Menu\\Programs\\小白T9输入法\\【小白T9输入法】重新部署.lnk");
        }

        private void button56_Click(object sender, EventArgs e)
        {
            string text = string.Empty;
            using (FileStream fs = new FileStream(快捷方式文件指向的目标目录.Text + "\\data\\xiaobai_simp.schema.yaml", FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    text = sr.ReadToEnd();
                    text = text.Replace(@"  enable_completion: true", @"  enable_completion: false");
                }
            }

            using (FileStream fs = new FileStream(快捷方式文件指向的目标目录.Text + "\\data\\xiaobai_simp.schema.yaml", FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.Write(text);
                }
            }

            Process.Start("C:\\ProgramData\\Microsoft\\Windows\\Start Menu\\Programs\\小白T9输入法\\【小白T9输入法】重新部署.lnk");
        }

        private void hxzs1_Click(object sender, EventArgs e)
        {
            string text = string.Empty;
            string filePath = 快捷方式文件指向的目标目录.Text + "\\data\\xiaobai_simp.schema.yaml";


            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    text = sr.ReadToEnd();

                    // 🎯 【核心修复】：匹配行首任意组合的空格和#号，一律替换为规范的带#号注释
                    text = Regex.Replace(text,
                        @"^[ \t#]*-\s*lua_filter@\*t9_preedit",
                        @"    #- lua_filter@*t9_preedit",
                        RegexOptions.Multiline);
                }
            }

            using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.Write(text);
                }
            }

            Process.Start("C:\\ProgramData\\Microsoft\\Windows\\Start Menu\\Programs\\小白T9输入法\\【小白T9输入法】重新部署.lnk");
        }

        private void hxzs2_Click(object sender, EventArgs e)
        {
            string text = string.Empty;
            string filePath = 快捷方式文件指向的目标目录.Text + "\\data\\xiaobai_simp.schema.yaml";


            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    text = sr.ReadToEnd();

                    // 🎯 【核心修复】：匹配行首任意组合的空格和#号，一律替换为干净放行的核心代码
                    text = Regex.Replace(text,
                        @"^[ \t#]*-\s*lua_filter@\*t9_preedit",
                        @"    - lua_filter@*t9_preedit",
                        RegexOptions.Multiline);
                }
            }

            using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.Write(text);
                }
            }

            Process.Start("C:\\ProgramData\\Microsoft\\Windows\\Start Menu\\Programs\\小白T9输入法\\【小白T9输入法】重新部署.lnk");
        }

        private void adPanel_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            using (var tf = new Font("Microsoft YaHei UI", 11F, FontStyle.Bold))
            using (var tb = new SolidBrush(Color.FromArgb(200, 80, 40)))
            {
                var w = g.MeasureString("支持小白", tf).Width;
                g.DrawString("支持小白", tf, tb, (190 - w) / 2, 8);
            }
            using (var lf = new Font("Microsoft YaHei UI", 8.5F, FontStyle.Bold))
            using (var sf = new Font("Microsoft YaHei UI", 7.5F))
            using (var gb = new SolidBrush(Color.FromArgb(80, 80, 80)))
            using (var db = new SolidBrush(Color.FromArgb(50, 50, 50)))
            using (var rb = new SolidBrush(Color.FromArgb(210, 50, 20)))
            using (var pen = new Pen(Color.FromArgb(230, 210, 190), 1))
            {
                var w1 = g.MeasureString("微信打赏", lf).Width;
                g.DrawString("微信打赏", lf, db, (190 - w1) / 2, 148);
                var w1d = g.MeasureString("感谢支持小白输入法", sf).Width;
                g.DrawString("感谢支持小白输入法", sf, gb, (190 - w1d) / 2, 168);
                g.DrawLine(pen, 20, 200, 170, 200);
                var w2 = g.MeasureString("支付宝打赏", lf).Width;
                g.DrawString("支付宝打赏", lf, db, (190 - w2) / 2, 358);
                var w2d = g.MeasureString("感谢支持小白输入法", sf).Width;
                g.DrawString("感谢支持小白输入法", sf, gb, (190 - w2d) / 2, 378);
                g.DrawLine(pen, 20, 418, 170, 418);
                var w3 = g.MeasureString("淘宝店铺", lf).Width;
                g.DrawString("淘宝店铺", lf, db, (190 - w3) / 2, 578);
                var w3a = g.MeasureString("捐助98元可赠送", sf).Width;
                g.DrawString("捐助98元可赠送", sf, gb, (190 - w3a) / 2, 598);
                var w3b = g.MeasureString("小白T9无线键盘一台!", sf).Width;
                g.DrawString("小白T9无线键盘一台!", sf, rb, (190 - w3b) / 2, 614);
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
                btnToggleAd.Location = new System.Drawing.Point(625, 300);
                this.ClientSize = new System.Drawing.Size(645, 687);
            }
            else
            {
                adPanel.Visible = true;
                btnToggleAd.Text = "<";
                btnToggleAd.Location = new System.Drawing.Point(625, 300);
                this.ClientSize = new System.Drawing.Size(845, 687);
            }
        }
    }
}
