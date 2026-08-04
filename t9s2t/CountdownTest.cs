using System;
using System.Runtime.InteropServices;
using System.Text;
using System.IO;

class CountdownTest {
    [DllImport("user32.dll",CharSet=CharSet.Auto)]
    static extern IntPtr SendMessage(IntPtr h,uint m,IntPtr w,IntPtr l);
    [DllImport("user32.dll",CharSet=CharSet.Auto)]
    static extern int GetWindowText(IntPtr h,StringBuilder s,int n);
    const uint BM_CLICK = 0x00F5;
    static void Main() {
        var sb = new StringBuilder();
        // Click start button H=730622
        SendMessage((IntPtr)730622, BM_CLICK, IntPtr.Zero, IntPtr.Zero);
        sb.AppendLine("Clicked start at: " + DateTime.Now.ToString("HH:mm:ss"));
        // Wait 6 seconds for first check to complete
        System.Threading.Thread.Sleep(6000);
        // First reading
        var s1 = new StringBuilder(512);
        GetWindowText((IntPtr)1966306, s1, 512);
        sb.AppendLine("Reading1 (" + DateTime.Now.ToString("HH:mm:ss") + "): " + s1.ToString());
        // Wait 3 seconds
        System.Threading.Thread.Sleep(3000);
        // Second reading
        var s2 = new StringBuilder(512);
        GetWindowText((IntPtr)1966306, s2, 512);
        sb.AppendLine("Reading2 (" + DateTime.Now.ToString("HH:mm:ss") + "): " + s2.ToString());
        // Wait 3 more seconds
        System.Threading.Thread.Sleep(3000);
        // Third reading for extra confirmation
        var s3 = new StringBuilder(512);
        GetWindowText((IntPtr)1966306, s3, 512);
        sb.AppendLine("Reading3 (" + DateTime.Now.ToString("HH:mm:ss") + "): " + s3.ToString());
        File.WriteAllText("c:\\weasel\\t9s2t\\countdown_result.txt", sb.ToString(), Encoding.UTF8);
    }
}
