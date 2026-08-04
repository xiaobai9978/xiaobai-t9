using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Collections.Generic;
using System.Diagnostics;

class VerifyApp {
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
    const uint CB_SHOWDROPDOWN = 0x014F;
    const uint WM_GETTEXT = 0x000D;
    const uint WM_GETTEXTLENGTH = 0x000E;
    const uint BM_CLICK = 0x00F5;

    static List<IntPtr> children = new List<IntPtr>();

    static void Main(string[] args) {
        var proc = Process.GetProcessById(24696);
        IntPtr hWnd = proc.MainWindowHandle;
        Console.WriteLine("MainWindow: " + hWnd + " Title: " + proc.MainWindowTitle);

        children.Clear();
        EnumChildWindows(hWnd, (h,l) => { children.Add(h); return true; }, IntPtr.Zero);

        Console.WriteLine("\\n=== ALL CHILD CONTROLS (" + children.Count + ") ===");
        List<IntPtr> combos = new List<IntPtr>();
        List<IntPtr> buttons = new List<IntPtr>();
        List<IntPtr> edits = new List<IntPtr>();
        List<IntPtr> labels = new List<IntPtr>();

        foreach(var c in children) {
            var cls = new StringBuilder(256);
            GetClassName(c, cls, 256);
            var txt = new StringBuilder(2048);
            GetWindowText(c, txt, 2048);
            RECT r; GetWindowRect(c, out r);
            string cn = cls.ToString();
            Console.WriteLine(string.Format("  H={0} Class={1} Text=[{2}] Pos=({3},{4})-({5},{6})", c, cn, txt.ToString().Replace("\\n","|"), r.Left, r.Top, r.Right, r.Bottom));
            if(cn == "ComboBox") combos.Add(c);
            else if(cn == "Button") buttons.Add(c);
            else if(cn == "Edit" || cn == "RichEdit20W" || cn == "RichEdit50W") edits.Add(c);
            else if(cn == "Static") labels.Add(c);
        }

        Console.WriteLine("\\n=== COMBOBOXES (" + combos.Count + ") ===");
        foreach(var cb in combos) {
            int count = SendMessage(cb, CB_GETCOUNT, IntPtr.Zero, IntPtr.Zero).ToInt32();
            int sel = SendMessage(cb, CB_GETCURSEL, IntPtr.Zero, IntPtr.Zero).ToInt32();
            Console.WriteLine("  ComboBox " + cb + " Count=" + count + " SelectedIndex=" + sel);
            int show = Math.Min(count, 20);
            for(int i=0;i<show;i++) {
                var sb = new StringBuilder(512);
                SendMessage(cb, CB_GETLBTEXT, (IntPtr)i, sb);
                Console.WriteLine("    [" + i + "] " + sb.ToString());
            }
        }

        Console.WriteLine("\\n=== BUTTONS ===");
        foreach(var b in buttons) {
            var txt = new StringBuilder(256);
            GetWindowText(b, txt, 256);
            Console.WriteLine("  Button " + b + " Text=[" + txt.ToString() + "]");
        }

        Console.WriteLine("\\n=== EDIT/LOG CONTROLS (" + edits.Count + ") ===");
        foreach(var e in edits) {
            int len = SendMessage(e, WM_GETTEXTLENGTH, IntPtr.Zero, IntPtr.Zero).ToInt32();
            var sb = new StringBuilder(len + 1);
            SendMessage(e, WM_GETTEXT, (IntPtr)(len+1), sb);
            string content = sb.ToString();
            if(content.Length > 3000) content = content.Substring(content.Length - 3000);
            Console.WriteLine("  Edit " + e + " Len=" + len);
            Console.WriteLine(content);
        }

        Console.WriteLine("\\n=== STATIC LABELS ===");
        foreach(var s in labels) {
            var txt = new StringBuilder(512);
            GetWindowText(s, txt, 512);
            if(txt.Length > 0) Console.WriteLine("  Label " + s + " Text=[" + txt.ToString() + "]");
        }
    }
}
