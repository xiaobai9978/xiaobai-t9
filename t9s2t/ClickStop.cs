using System;
using System.Runtime.InteropServices;
using System.Text;
using System.IO;

class ClickStop {
    [DllImport("user32.dll",CharSet=CharSet.Auto)]
    static extern IntPtr SendMessage(IntPtr h,uint m,IntPtr w,IntPtr l);
    [DllImport("user32.dll",CharSet=CharSet.Auto)]
    static extern int GetWindowText(IntPtr h,StringBuilder s,int n);
    const uint BM_CLICK = 0x00F5;
    static void Main() {
        // Click stop button H=861702
        SendMessage((IntPtr)861702, BM_CLICK, IntPtr.Zero, IntPtr.Zero);
        System.Threading.Thread.Sleep(1000);
        // Read status
        var s1 = new StringBuilder(512);
        GetWindowText((IntPtr)1966306, s1, 512);
        File.WriteAllText("c:\\weasel\\t9s2t\\stop_result.txt", "After stop: " + s1.ToString(), Encoding.UTF8);
    }
}
