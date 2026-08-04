using System;
using System.Runtime.InteropServices;
using System.Text;
using System.IO;

class CountdownTest4 {
    [DllImport("user32.dll",CharSet=CharSet.Auto)]
    static extern IntPtr SendMessage(IntPtr h,uint m,IntPtr w,IntPtr l);
    [DllImport("user32.dll",CharSet=CharSet.Auto)]
    static extern IntPtr SendMessage(IntPtr h,uint m,IntPtr w,StringBuilder l);
    [DllImport("user32.dll",CharSet=CharSet.Auto)]
    static extern int GetWindowText(IntPtr h,StringBuilder s,int n);
    [DllImport("user32.dll",CharSet=CharSet.Auto)]
    static extern bool SetWindowText(IntPtr h,string s);
    const uint BM_CLICK = 0x00F5;
    const uint CB_SETCURSEL = 0x014E;
    const uint CB_GETCURSEL = 0x0147;
    const uint CB_SELECTSTRING = 0x014D;
    const uint WM_SETTEXT = 0x000C;
    const uint WM_KEYDOWN = 0x0100;
    const uint WM_KEYUP = 0x0101;
    const int VK_RETURN = 0x0D;
    static string ReadLabel(IntPtr h) {
        var s = new StringBuilder(512);
        GetWindowText(h, s, 512);
        return s.ToString();
    }
    static void Main() {
        var sb = new StringBuilder();
        IntPtr status1 = (IntPtr)1966306;
        IntPtr status2 = (IntPtr)1705546;
        IntPtr comboMovie = (IntPtr)659222;
        IntPtr comboEdit = (IntPtr)658260;
        IntPtr mainWnd = (IntPtr)658896;
        // Stop first
        SendMessage((IntPtr)861702, BM_CLICK, IntPtr.Zero, IntPtr.Zero);
        System.Threading.Thread.Sleep(1000);
        sb.AppendLine("After stop: S2=[" + ReadLabel(status2) + "]");
        // Try CB_SELECTSTRING to select 蜘蛛侠
        var result = SendMessage(comboMovie, CB_SELECTSTRING, (IntPtr)(-1), new StringBuilder("蜘蛛侠：崭新之日"));
        sb.AppendLine("CB_SELECTSTRING result: " + result.ToInt32());
        int curSel = SendMessage(comboMovie, CB_GETCURSEL, IntPtr.Zero, IntPtr.Zero).ToInt32();
        sb.AppendLine("Current selection index after CB_SELECTSTRING: " + curSel);
        System.Threading.Thread.Sleep(500);
        sb.AppendLine("After select - S2: " + ReadLabel(status2));
        // Start monitoring
        SendMessage((IntPtr)730622, BM_CLICK, IntPtr.Zero, IntPtr.Zero);
        sb.AppendLine("Start clicked at: " + DateTime.Now.ToString("HH:mm:ss.fff"));
        // Sample every 1s for 10s
        for(int i=1; i<=10; i++) {
            System.Threading.Thread.Sleep(1000);
            sb.AppendLine(string.Format("T+{0:D2}s ({1}): S1=[{2}] S2=[{3}]",
                i, DateTime.Now.ToString("HH:mm:ss"), ReadLabel(status1), ReadLabel(status2)));
        }
        // Stop
        SendMessage((IntPtr)861702, BM_CLICK, IntPtr.Zero, IntPtr.Zero);
        System.Threading.Thread.Sleep(500);
        sb.AppendLine("Stopped. S1=[" + ReadLabel(status1) + "]");
        // Restore to 奥德赛
        SendMessage(comboMovie, CB_SELECTSTRING, (IntPtr)(-1), new StringBuilder("奥德赛"));
        sb.AppendLine("Restored to 奥德赛");
        File.WriteAllText("c:\\weasel\\t9s2t\\countdown4.txt", sb.ToString(), Encoding.UTF8);
    }
}
