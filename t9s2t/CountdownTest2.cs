using System;
using System.Runtime.InteropServices;
using System.Text;
using System.IO;

class CountdownTest2 {
    [DllImport("user32.dll",CharSet=CharSet.Auto)]
    static extern IntPtr SendMessage(IntPtr h,uint m,IntPtr w,IntPtr l);
    [DllImport("user32.dll",CharSet=CharSet.Auto)]
    static extern int GetWindowText(IntPtr h,StringBuilder s,int n);
    const uint BM_CLICK = 0x00F5;
    static string ReadLabel(IntPtr h) {
        var s = new StringBuilder(512);
        GetWindowText(h, s, 512);
        return s.ToString();
    }
    static void Main() {
        var sb = new StringBuilder();
        IntPtr status1 = (IntPtr)1966306;
        IntPtr status2 = (IntPtr)1705546;
        // First stop
        SendMessage((IntPtr)861702, BM_CLICK, IntPtr.Zero, IntPtr.Zero);
        System.Threading.Thread.Sleep(1500);
        sb.AppendLine("After stop - S1: " + ReadLabel(status1) + " | S2: " + ReadLabel(status2));
        // Now start
        SendMessage((IntPtr)730622, BM_CLICK, IntPtr.Zero, IntPtr.Zero);
        sb.AppendLine("Start clicked at: " + DateTime.Now.ToString("HH:mm:ss.fff"));
        // Rapid sampling every 1s for 12 seconds
        for(int i=1; i<=12; i++) {
            System.Threading.Thread.Sleep(1000);
            sb.AppendLine(string.Format("T+{0:D2}s ({1}): S1=[{2}] S2=[{3}]",
                i, DateTime.Now.ToString("HH:mm:ss"), ReadLabel(status1), ReadLabel(status2)));
        }
        File.WriteAllText("c:\\weasel\\t9s2t\\countdown2.txt", sb.ToString(), Encoding.UTF8);
    }
}
