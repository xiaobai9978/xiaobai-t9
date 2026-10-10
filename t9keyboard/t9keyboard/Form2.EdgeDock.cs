using System;
using System.Drawing;
using System.Windows.Forms;

namespace t9keyboard
{
    /// <summary>
    /// Form2（半透明悬浮图标）的「贴边收起」扩展，partial 类的一部分，不改动原有代码。
    ///
    /// 交互定义：
    ///   1) 拖动松手时，若窗口任一边到「当前屏幕工作区（已排除任务栏）」对应边的距离
    ///      小于 DockSnapDistance，判定为贴该边并吸附到位；
    ///   2) 吸附状态下，鼠标离开窗口满 DockCollapseDelayMs 毫秒 → 沿该边滑出屏幕，
    ///      只留 DockPeekPx 像素的窄条露在屏幕内；
    ///   3) 鼠标悬停到窄条上，或点击一下 → 立即滑回完整显示；
    ///   4) 鼠标再次离开 → 再次收起；把窗口拖离边缘即取消吸附，之后不再自动收起。
    ///
    /// 保持不变的部分：Opacity(0.4)、TransparencyKey(White)、TopMost、WS_EX_NOACTIVATE /
    /// WS_EX_TOOLWINDOW、拖动方式、单击/双击切换键盘、右键菜单。
    /// </summary>
    public partial class Form2
    {
        // ==================== 可调参数 ====================
        /// <summary>距屏幕边缘多少像素内松手，判定为贴边吸附</summary>
        private const int DockSnapDistance = 20;
        /// <summary>收起后仍露在屏幕内的窄条宽度（像素）</summary>
        private const int DockPeekPx = 6;
        /// <summary>鼠标离开窗口后多久自动收起（毫秒）</summary>
        private const int DockCollapseDelayMs = 600;
        /// <summary>滑入 / 滑出动画时长（毫秒）</summary>
        private const int DockAnimMs = 160;
        /// <summary>动画帧间隔（毫秒），约 66fps</summary>
        private const int DockAnimFrameMs = 15;
        /// <summary>
        /// 鼠标位置轮询间隔（毫秒）。
        /// 这里刻意不用 MouseEnter / MouseLeave：窗体用 TransparencyKey 把白色区域做成了
        /// 点击穿透，收起后只剩 6px 且其中约一半像素是透明的，靠鼠标事件会漏判。
        /// </summary>
        private const int DockHoverPollMs = 120;
        /// <summary>刚展开后吞掉点击的时长（毫秒），避免双击窄条被识别成「双击切换键盘」</summary>
        private const int DockClickGuardMs = 500;
        /// <summary>从托盘菜单唤出悬浮窗后，多少毫秒内不自动收起（留时间让用户把鼠标移上去或把它拖走）</summary>
        private const int DockHoldAfterRestoreMs = 8000;

        private enum DockEdge { None, Left, Right, Top, Bottom }

        // ==================== 状态 ====================
        private DockEdge _dockEdge = DockEdge.None;   // 当前吸附在哪条边
        private bool _dockCollapsed;                  // 是否处于收起（滑出）状态
        private bool _dockDragging;                   // 是否正在被拖动
        private bool _dockAnimToCollapsed;            // 当前动画的终点状态
        private bool _dockAttached;                   // 是否已挂接

        private Timer _dockAnimTimer;
        private Timer _dockHoverTimer;
        private DateTime _dockAnimStart;
        private Point _dockAnimFrom;
        private Point _dockAnimTo;
        private DateTime _dockLeaveAt = DateTime.MinValue;          // 鼠标最后一次离开窗口的时刻
        private DateTime _dockClickGuardUntil = DateTime.MinValue;  // 此时间前吞掉点击
        private string _dockScreenDevice;                           // 吸附时锁定的显示器（多屏相邻时必需）
        private DateTime _dockHoldUntil = DateTime.MinValue;        // 此时间前不自动收起（被托盘菜单唤出后用）

        // ==================== 挂接（在 Form2_Load 里调用）====================
        private void AttachEdgeDock()
        {
            if (_dockAttached) return;
            _dockAttached = true;

            // 松手判定吸附；按下/移动沿用 Form2.cs 里原有的拖动实现
            button1.MouseUp += button1_MouseUp_Dock;

            _dockAnimTimer = new Timer { Interval = DockAnimFrameMs };
            _dockAnimTimer.Tick += dockAnimTimer_Tick;

            _dockHoverTimer = new Timer { Interval = DockHoverPollMs };
            _dockHoverTimer.Tick += dockHoverTimer_Tick;
            _dockHoverTimer.Start();

            // 分辨率 / 显示器数量变化后重新贴合，避免窄条跑到屏幕外
            Microsoft.Win32.SystemEvents.DisplaySettingsChanged += OnDisplaySettingsChanged;
            this.FormClosed += (s, e) =>
            {
                Microsoft.Win32.SystemEvents.DisplaySettingsChanged -= OnDisplaySettingsChanged;
                if (_dockHoverTimer != null) { _dockHoverTimer.Stop(); _dockHoverTimer.Dispose(); }
                if (_dockAnimTimer != null) { _dockAnimTimer.Stop(); _dockAnimTimer.Dispose(); }
            };
        }

        // ==================== 拖动挂钩（由 Form2.cs 调用）====================
        /// <summary>在 button1_MouseDown 的「记录 offset 之前」调用。</summary>
        private void DockBeginDrag()
        {
            _dockDragging = true;
            StopDockAnimation();

            // 从收起状态开始拖动时，先瞬时恢复完整显示，否则窗口一半在屏幕外，拖起来很怪
            if (_dockCollapsed)
            {
                _dockCollapsed = false;
                if (_dockEdge != DockEdge.None)
                    this.Location = ExpandedLocation(DockWorkArea());
            }
        }

        // ==================== 拖动结束：吸附 ====================
        private void button1_MouseUp_Dock(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left || !_dockDragging) return;
            _dockDragging = false;

            _dockLeaveAt = DateTime.MinValue;   // 松手瞬间鼠标必然在窗口上，重新开始计时

            // 以鼠标所在显示器为目标屏（多屏时窗口可能横跨两块屏）
            Screen released = Screen.FromPoint(Cursor.Position);
            _dockScreenDevice = (released == null) ? null : released.DeviceName;

            Rectangle wa = DockWorkArea();
            Rectangle b = this.Bounds;

            // 用「有符号」距离：负数表示已经越界拖出屏幕，同样算贴边
            int dLeft = b.Left - wa.Left;
            int dRight = wa.Right - b.Right;
            int dTop = b.Top - wa.Top;
            int dBottom = wa.Bottom - b.Bottom;

            int min = Math.Min(Math.Min(dLeft, dRight), Math.Min(dTop, dBottom));
            if (min > DockSnapDistance)
            {
                _dockEdge = DockEdge.None;      // 离边缘还远，取消吸附
                _dockScreenDevice = null;
                return;
            }

            if (min == dLeft) _dockEdge = DockEdge.Left;
            else if (min == dRight) _dockEdge = DockEdge.Right;
            else if (min == dTop) _dockEdge = DockEdge.Top;
            else _dockEdge = DockEdge.Bottom;

            AnimateTo(ExpandedLocation(wa), false);   // 吸附到位（短距离，看起来就是"啪"一下贴上去）
        }

        // ==================== 悬停轮询：展开 / 收起 ====================
        private void dockHoverTimer_Tick(object sender, EventArgs e)
        {
            if (!_dockAttached || _dockDragging) return;
            if (_dockEdge == DockEdge.None) return;   // 没贴边就不自动收起
            if (_dockAnimTimer != null && _dockAnimTimer.Enabled) return;
            if (DateTime.Now < _dockHoldUntil) return;   // 刚从托盘菜单唤出，先别急着缩回去

            bool inside = this.Bounds.Contains(Cursor.Position);
            if (inside)
            {
                _dockLeaveAt = DateTime.MinValue;
                if (_dockCollapsed) ExpandFromDock();
            }
            else
            {
                if (_dockLeaveAt == DateTime.MinValue) _dockLeaveAt = DateTime.Now;
                if (!_dockCollapsed &&
                    (DateTime.Now - _dockLeaveAt).TotalMilliseconds >= DockCollapseDelayMs)
                    CollapseToDock();
            }
        }

        // ==================== 点击展开（由 Form2.cs 的 button1_Click 调用）====================
        /// <summary>返回 true 表示这次点击已被贴边逻辑消费掉，不再进入原有的单击/双击判定。</summary>
        private bool DockHandleClick()
        {
            if (DateTime.Now < _dockClickGuardUntil) return true;   // 刚展开的那一下不算点击

            if (_dockCollapsed)
            {
                ExpandFromDock();
                _dockClickGuardUntil = DateTime.Now.AddMilliseconds(DockClickGuardMs);
                return true;
            }
            return false;
        }

        // ==================== 收起 / 展开 ====================
        private void CollapseToDock()
        {
            if (_dockEdge == DockEdge.None || _dockCollapsed) return;
            AnimateTo(CollapsedLocation(DockWorkArea()), true);
        }

        private void ExpandFromDock()
        {
            if (_dockEdge == DockEdge.None || !_dockCollapsed) return;
            _dockLeaveAt = DateTime.MinValue;
            AnimateTo(ExpandedLocation(DockWorkArea()), false);
        }

        /// <summary>
        /// 供外部入口（托盘图标右键菜单）调用：把悬浮窗完整摆出来。
        /// 贴边收起状态下会先展开，并在随后 DockHoldAfterRestoreMs 内不自动收起——
        /// 否则用户刚点完菜单它又缩回边缘，等于白点。
        /// </summary>
        public void RestoreFloatingIcon()
        {
            _dockHoldUntil = DateTime.Now.AddMilliseconds(DockHoldAfterRestoreMs);
            _dockLeaveAt = DateTime.MinValue;
            if (_dockEdge != DockEdge.None && _dockCollapsed) ExpandFromDock();
            if (!this.Visible) this.Show();
            try { this.BringToFront(); } catch { }
        }

        /// <summary>贴边且完整显示时的位置（保证整体落在工作区内）</summary>
        private Point ExpandedLocation(Rectangle wa)
        {
            int x = this.Left;
            int y = this.Top;
            switch (_dockEdge)
            {
                case DockEdge.Left:
                    x = wa.Left;
                    y = Clamp(y, wa.Top, wa.Bottom - this.Height);
                    break;
                case DockEdge.Right:
                    x = wa.Right - this.Width;
                    y = Clamp(y, wa.Top, wa.Bottom - this.Height);
                    break;
                case DockEdge.Top:
                    x = Clamp(x, wa.Left, wa.Right - this.Width);
                    y = wa.Top;
                    break;
                case DockEdge.Bottom:
                    x = Clamp(x, wa.Left, wa.Right - this.Width);
                    y = wa.Bottom - this.Height;
                    break;
            }
            return new Point(x, y);
        }

        /// <summary>收起后的位置：沿吸附的那条边滑出，只留 DockPeekPx 像素在屏幕内</summary>
        private Point CollapsedLocation(Rectangle wa)
        {
            Point p = ExpandedLocation(wa);
            // 工作区比窗口还小时（极端分辨率）退化为完整显示，避免算出负数把窗口整个推出屏幕
            int peekX = Math.Min(DockPeekPx, this.Width);
            int peekY = Math.Min(DockPeekPx, this.Height);
            switch (_dockEdge)
            {
                case DockEdge.Left: p.X = wa.Left - (this.Width - peekX); break;
                case DockEdge.Right: p.X = wa.Right - peekX; break;
                case DockEdge.Top: p.Y = wa.Top - (this.Height - peekY); break;
                case DockEdge.Bottom: p.Y = wa.Bottom - peekY; break;
            }
            return p;
        }

        // ==================== 动画（ease-out 缓动，逐帧插值）====================
        private void AnimateTo(Point target, bool endsCollapsed)
        {
            if (this.Location == target)
            {
                _dockCollapsed = endsCollapsed;
                _dockAnimToCollapsed = endsCollapsed;
                return;
            }
            _dockAnimFrom = this.Location;
            _dockAnimTo = target;
            _dockAnimToCollapsed = endsCollapsed;
            _dockAnimStart = DateTime.Now;
            _dockAnimTimer.Stop();
            _dockAnimTimer.Start();
        }

        private void StopDockAnimation()
        {
            if (_dockAnimTimer != null) _dockAnimTimer.Stop();
        }

        private void dockAnimTimer_Tick(object sender, EventArgs e)
        {
            double t = (DateTime.Now - _dockAnimStart).TotalMilliseconds / DockAnimMs;
            if (t >= 1.0) t = 1.0;

            double k = 1.0 - Math.Pow(1.0 - t, 3);   // easeOutCubic：起步快、收尾稳
            int x = _dockAnimFrom.X + (int)Math.Round((_dockAnimTo.X - _dockAnimFrom.X) * k);
            int y = _dockAnimFrom.Y + (int)Math.Round((_dockAnimTo.Y - _dockAnimFrom.Y) * k);
            this.Location = new Point(x, y);

            if (t >= 1.0)
            {
                _dockAnimTimer.Stop();
                this.Location = _dockAnimTo;             // 消除浮点误差
                _dockCollapsed = _dockAnimToCollapsed;
                if (_dockCollapsed) _dockLeaveAt = DateTime.Now;
            }
        }

        // ==================== 多屏 / 分辨率 ====================
        /// <summary>取吸附目标显示器。吸附那一刻就把屏幕记下来，之后一直用它。</summary>
        /// <remarks>
        /// 不能每次都按「窗口中心」判定：窗口贴屏 B 的左边缘收起后会滑到屏幕外，
        /// 中心点正好漂进左边那块屏 A，于是工作区就取错了。
        /// </remarks>
        private Screen DockScreen()
        {
            if (!string.IsNullOrEmpty(_dockScreenDevice))
            {
                foreach (Screen s in Screen.AllScreens)
                    if (s.DeviceName == _dockScreenDevice) return s;
                _dockScreenDevice = null;   // 这块屏被拔掉了，回落重新判定
            }
            Point center = new Point(this.Left + this.Width / 2, this.Top + this.Height / 2);
            Screen sc = Screen.FromPoint(center);   // 点落在所有屏幕之外时返回最近的那块屏
            return sc ?? Screen.PrimaryScreen;
        }

        /// <summary>取吸附目标显示器的**工作区**（已排除任务栏）</summary>
        private Rectangle DockWorkArea()
        {
            return DockScreen().WorkingArea;
        }

        private void OnDisplaySettingsChanged(object sender, EventArgs e)
        {
            if (this.IsDisposed || !this.IsHandleCreated) return;
            try
            {
                // SystemEvents 在非 UI 线程触发，切回 UI 线程再动窗口
                this.BeginInvoke((MethodInvoker)(() =>
                {
                    if (this.IsDisposed || _dockEdge == DockEdge.None) return;
                    Rectangle wa = DockWorkArea();
                    StopDockAnimation();
                    this.Location = _dockCollapsed ? CollapsedLocation(wa) : ExpandedLocation(wa);
                }));
            }
            catch { }
        }

        private static int Clamp(int v, int lo, int hi)
        {
            if (hi < lo) return lo;
            if (v < lo) return lo;
            if (v > hi) return hi;
            return v;
        }
    }
}
