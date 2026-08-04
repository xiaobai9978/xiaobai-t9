using System;
using System.Runtime.InteropServices;
using System.Text;
using System.IO;

class ReadLog {
    [DllImport("user32.dll",CharSet=CharSet.Auto)]
    static extern IntPtr SendMessage(IntPtr h,uint m,IntPtr w,IntPtr l);
    [DllImport("user32.dll",CharSet=CharSet.Auto)]
    static extern IntPtr SendMessage(IntPtr h,uint m,IntPtr w,StringBuilder l);
    const uint WM_GETTEXT = 0x000D;
    const uint WM_GETTEXTLENGTH = 0x000E;
    static void Main() {
        IntPtr logEdit = (IntPtr)8523068;
        int len = SendMessage(logEdit, WM_GETTEXTLENGTH, IntPtr.Zero, IntPtr.Zero).ToInt32();
        var sb = new StringBuilder(len + 1);
        SendMessage(logEdit, WM_GETTEXT, (IntPtr)(len+1), sb);
        string content = sb.ToString();
        // Get last 2000 chars
        if(content.Length > 2000) content = content.Substring(content.Length - 2000);
        File.WriteAllText("c:\\weasel\\t9s2t\\latest_log.txt", content, Encoding.UTF8);
    }
}
