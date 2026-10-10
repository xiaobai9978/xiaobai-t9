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

            // 启动时显示键盘
            ShowKeyboard();

            // 显示帮助
            // help f2 = new help(); // removed to avoid duplicate tray icon
            //f2.Show();
            // f2.yes.PerformClick(); // 建议把这行逻辑放在help窗体内部处理，或者确保yes按钮存在

            // 贴边收起：拖动吸附 / 滑出收起 / 悬停展开（实现见 Form2.EdgeDock.cs）
            AttachEdgeDock();
        }

        // 窗口拖动相关变量
        private Point offset;

        // 窗口样式重写 (保持不变)
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

        private void button1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                DockBeginDrag();   // 贴边收起：从收起状态开始拖动时先恢复完整显示
                Point cur = this.PointToScreen(e.Location);
                offset = new Point(cur.X - this.Left, cur.Y - this.Top);
            }
            else if (e.Button == MouseButtons.Right)
            {
                contextMenuStrip1.Show(MousePosition.X, MousePosition.Y);
            }
        }

        private void button1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            Point cur = MousePosition;
            this.Location = new Point(cur.X - offset.X, cur.Y - offset.Y);
        }

        // 用一个布尔值记录键盘显示状态，比 int i 更加准确
        bool isKeyboardVisible = true;

        private void button1_Click(object sender, EventArgs e)
        {
            // 贴边收起：处于收起状态时，这一下点击只用来展开，不参与双击判定
            if (DockHandleClick()) return;

            // 核心修改：双击逻辑判定
            if (timer1.Enabled)
            {
                // --- 这里是双击事件 ---
                timer1.Enabled = false;
                timer1.Tag = null; // 【关键修复】重置Tag，防止下次判定错误

                if (isKeyboardVisible)
                {
                    HideKeyboard(); // 隐藏
                }
                else
                {
                    ShowKeyboard(); // 显示
                }
                // 切换状态
                isKeyboardVisible = !isKeyboardVisible;
            }
            else
            {
                // --- 这里是第一次点击 ---
                timer1.Enabled = true;
                timer1.Tag = DateTime.Now; // 立即记录时间
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // 检查是否超时 (0.4秒作为双击间隔比较合适，0.5秒略长)
            if (timer1.Tag != null)
            {
                DateTime startTime = (DateTime)timer1.Tag;
                if ((DateTime.Now - startTime).TotalMilliseconds > 400)
                {
                    // --- 这里是单击事件（超时未双击） ---
                    timer1.Enabled = false;
                    timer1.Tag = null;

                    // 如果你需要单击做点什么，写在这里
                    // Console.WriteLine("单击触发");
                }
            }
        }

        // 通用方法：查找或创建指定类型的窗体（防止重复创建）
        public static T FindOrCreateForm<T>() where T : Form, new()
        {
            foreach (Form form in Application.OpenForms)
            {
                if (form is T)
                {
                    return (T)form;
                }
            }
            return new T();
        }

        // 封装：隐藏键盘逻辑
        private void HideKeyboard()
        {
            foreach (Form form in Application.OpenForms)
            {
                // 隐藏指定类型的窗体，而不是关闭
                if (form is keyboard || form is numboard || form is enboard)
                {
                    form.Hide();
                }
            }
        }

        // 封装：显示键盘逻辑
        private void ShowKeyboard()
        {
            // 查找已存在的键盘窗体（keyboard/numboard/enboard）
            Form firstFound = null;
            var formsToClose = new System.Collections.Generic.List<Form>();

            foreach (Form form in Application.OpenForms)
            {
                if (form is keyboard || form is numboard || form is enboard)
                {
                    if (firstFound == null)
                    {
                        // 第一个找到的窗体：显示它
                        firstFound = form;
                        form.Show();
                    }
                    else
                    {
                        // 后续重复的窗体：标记为待关闭
                        formsToClose.Add(form);
                    }
                }
            }

            // 关闭所有重复的键盘窗体，防止积累
            foreach (Form form in formsToClose)
            {
                form.Close();
            }

            // 如果没找到任何键盘窗体，才创建新的
            if (firstFound == null)
            {
                keyboard f1 = new keyboard();
                f1.Show();
            }
        }

        private void 退出软键盘ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void 帮助与设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form form in Application.OpenForms)
            {
                if (form is help)
                {
                    form.Show();
                    form.WindowState = FormWindowState.Normal;
                    form.BringToFront();
                    form.Activate();
                    return;
                }
            }
            help f1 = new help();
            f1.Show();
            f1.Location = new Point(this.Location.X + 60, this.Location.Y); // 稍微错开一点位置
        }
    }
}
