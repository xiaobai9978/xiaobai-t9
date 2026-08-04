using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Collections.Generic;
using System.Diagnostics;

class VerifyApp2 {
    [DllImport("user32.dll",CharSet=CharSet.Auto)]
    static extern IntPtr SendMessage(IntPtr h,uint m,IntPtr w,IntPtr l);
    [DllImport("user32.dll",CharSet=CharSet.Auto)]
    static extern IntPtr SendMessage(IntPtr h,uint m,IntPtr w,StringBuilder l);
    [DllImport("user32.dll",CharSet=CharSet.Auto)]
    static extern int GetWindowText(IntPtr h,StringBuilder s,int n);
    [DllImport("user32.dll",CharSet=CharSet.Auto)]
    static extern int GetClassName(IntPtr h,StringBuilder s,int n);
    [DllImport("user32.dll")]
    static extern bool EnumChildWindows(IntPtr h,EnumChildProc f,IntPtr l);
    [DllImport("user32.dll")]
    static extern bool GetWindowRect(IntPtr h,out RECT r);
    delegate bool EnumChildProc(IntPtr h,IntPtr l);
    [StructLayout(LayoutKind.Sequential)]
    struct RECT { public int Left,Top,Right,Bottom; }

    const uint CB_GETCOUNT = 0x0146;
    const uint CB_GETLBTEXT = 0x0148;
    const uint CB_GETCURSEL = 0x0147;
    const uint WM_GETTEXT = 0x000D;
    const uint WM_GETTEXTLENGTH = 0x000E;

    static List<IntPtr> children = new List<IntPtr>();
    static StreamWriter w;

    static void Main(string[] args) {
        w = new StreamWriter("c:\\weasel\\t9s2t\\result.txt", false, Encoding.UTF8);
        var proc = Process.GetProcessById(24696);
        IntPtr hWnd = proc.MainWindowHandle;
        w.WriteLine("MainWindow: " + hWnd + " Title: " + proc.MainWindowTitle);

        children.Clear();
        EnumChildWindows(hWnd, (h,l) => { children.Add(h); return true; }, IntPtr.Zero);

        List<IntPtr> combos = new List<IntPtr>();
        List<IntPtr> buttons = new List<IntPtr>();

        foreach(var c in children) {
            var cls = new StringBuilder(256);
            GetClassName(c, cls, 256);
            string cn = cls.ToString();
            if(cn.Contains("COMBOBOX")) combos.Add(c);
            else if(cn.Contains("BUTTON")) buttons.Add(c);
        }

        w.WriteLine("\\n=== COMBOBOXES (" + combos.Count + ") ===");
        foreach(var cb in combos) {
            int count = SendMessage(cb, CB_GETCOUNT, IntPtr.Zero, IntPtr.Zero).ToInt32();
            int sel = SendMessage(cb, CB_GETCURSEL, IntPtr.Zero, IntPtr.Zero).ToInt32();
            var txt = new StringBuilder(512);
            GetWindowText(cb, txt, 512);
            RECT r; GetWindowRect(cb, out r);
            w.WriteLine(string.Format("  ComboBox H={0} Count={1} SelIdx={2} Text=[{3}] Pos=({4},{5})", cb, count, sel, txt.ToString(), r.Left, r.Top));
            int show = Math.Min(count, 20);
            for(int i=0;i<show;i++) {
                var sb = new StringBuilder(512);
                SendMessage(cb, CB_GETLBTEXT, (IntPtr)i, sb);
                w.WriteLine("    [" + i + "] " + sb.ToString());
            }
        }

        w.WriteLine("\\n=== BUTTONS ===");
        foreach(var b in buttons) {
            var txt = new StringBuilder(256);
            GetWindowText(b, txt, 256);
            RECT r; GetWindowRect(b, out r);
            w.WriteLine(string.Format("  Button H={0} Text=[{1}] Pos=({2},{3})", b, txt.ToString(), r.Left, r.Top));
        }

        // Read log edit control (the big one at pos 398,261)
        w.WriteLine("\\n=== LOG CONTENT ===");
        IntPtr logEdit = (IntPtr)8523068;
        int len = SendMessage(logEdit, WM_GETTEXTLENGTH, IntPtr.Zero, IntPtr.Zero).ToInt32();
        var lsb = new StringBuilder(len + 1);
        SendMessage(logEdit, WM_GETTEXT, (IntPtr)(len+1), lsb);
        w.WriteLine(lsb.ToString());

        // Read status labels
        w.WriteLine("\\n=== STATUS BAR ===");
        IntPtr status1 = (IntPtr)1966306;
        IntPtr status2 = (IntPtr)1705546;
        var s1 = new StringBuilder(512);
        var s2 = new StringBuilder(512);
        GetWindowText(status1, s1, 512);
        GetWindowText(status2, s2, 512);
        w.WriteLine("Status1: " + s1.ToString());
        w.WriteLine("Status2: " + s2.ToString());

        w.Flush();
        w.Close();
    }
}
