using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace t9s2t
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            // 程序启动时就设好 TLS，确保所有网络请求都支持 TLS 1.2
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            // 命令行意图：外部（如 t9keyboard）可以指名「把模型面板调出来」
            //   t9s2t.exe                  按现状：就绪则静默到托盘，未就绪则显示并自动下载
            //   t9s2t.exe /model           显示模型面板，但不自动开始下载（258MB，交给用户点）
            //   t9s2t.exe /model /install  显示并立刻开始下载
            //   t9s2t.exe /manage          显示模型面板并聚焦「删除模型」
            foreach (string a in args ?? new string[0])
            {
                string s = (a ?? "").Trim().ToLowerInvariant();
                if (s == "/manage" || s == "-manage")
                {
                    Form1.StartupIntent = Form1.ModelPanelIntent.Manage;
                }
                else if (s == "/install" || s == "-install")
                {
                    Form1.StartupAutoDownload = true;
                    Form1.StartupIntent = Form1.ModelPanelIntent.Install;
                }
                else if (s == "/model" || s == "-model")
                {
                    if (Form1.StartupIntent == Form1.ModelPanelIntent.ShowOnly)
                        Form1.StartupIntent = Form1.ModelPanelIntent.Install;
                }
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
