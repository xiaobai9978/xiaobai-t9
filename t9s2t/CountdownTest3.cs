using System;
using System.Runtime.InteropServices;
using System.Text;
using System.IO;

class CountdownTest3 {
    [DllImport("user32.dll",CharSet=CharSet.Auto)]
    static extern IntPtr SendMessage(IntPtr h,uint m,IntPtr w,IntPtr l);
    [DllImport("user32.dll",CharSet=CharSet.Auto)]
    static extern int GetWindowText(IntPtr h,StringBuilder s,int n);
    const uint BM_CLICK = 0x00F5;
    const uint CB_SETCURSEL = 0x014E;
    const int CBN_SELCHANGE = 1;
    const uint WM_COMMAND = 0x0111;
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
        IntPtr mainWnd = (IntPtr)658896;
        // Stop first
        SendMessage((IntPtr)861702, BM_CLICK, IntPtr.Zero, IntPtr.Zero);
        System.Threading.Thread.Sleep(1000);
        // Select movie index 0 (蜘蛛侠：崭新之日 - no screenings)
        SendMessage(comboMovie, CB_SETCURSEL, (IntPtr)0, IntPtr.Zero);
        // Notify parent of selection change
        int wp = (CBN_SELCHANGE << 16) | (659222 & 0xFFFF);
        SendMessage(mainWnd, WM_COMMAND, (IntPtr)wp, comboMovie);
        System.Threading.Thread.Sleep(500);
        sb.AppendLine("Switched to movie index 0");
        sb.AppendLine("After switch - S1: " + ReadLabel(status1) + " | S2: " + ReadLabel(status2));
        // Start monitoring
        SendMessage((IntPtr)730622, BM_CLICK, IntPtr.Zero, IntPtr.Zero);
        sb.AppendLine("Start clicked at: " + DateTime.Now.ToString("HH:mm:ss.fff"));
        // Sample every 1s for 10s
        for(int i=1; i<=10; i++) {
            System.Threading.Thread.Sleep(1000);
            sb.AppendLine(string.Format("T+{0:D2}s ({1}): S1=[{2}]",
                i, DateTime.Now.ToString("HH:mm:ss"), ReadLabel(status1)));
        }
        // Stop monitoring
        SendMessage((IntPtr)861702, BM_CLICK, IntPtr.Zero, IntPtr.Zero);
        System.Threading.Thread.Sleep(500);
        sb.AppendLine("After final stop: S1=[" + ReadLabel(status1) + "]");
        // Restore movie to index 7 (奥德赛)
        SendMessage(comboMovie, CB_SETCURSEL, (IntPtr)7, IntPtr.Zero);
        SendMessage(mainWnd, WM_COMMAND, (IntPtr)wp, comboMovie);
        sb.AppendLine("Restored movie to index 7");
        File.WriteAllText("c:\\weasel\\t9s2t\\countdown3.txt", sb.ToString(), Encoding.UTF8);
    }
}
