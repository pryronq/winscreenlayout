using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

public static class WindowApply
{
    public static int Apply(List<WindowInfo> layout)
    {
        var live = WindowCapture.CaptureWithHandles(); // new method below
        int moved = 0, missing = 0;

        foreach (var w in layout)
        {
            var match = FindBest(live, w.Title);
            if (match == null) { missing++; continue; }

            if (SetWindowPos(match.Handle, IntPtr.Zero, w.X, w.Y, w.Width, w.Height, SWP_NOZORDER | SWP_NOACTIVATE))
                moved++;
        }

        Console.WriteLine($"Apply done. moved={moved}, missing={missing}");
        return missing == 0 ? 0 : 2;
    }

    static LiveWindow? FindBest(List<LiveWindow> live, string title)
    {
        // simple: exact first, then contains
        var exact = live.FirstOrDefault(x => string.Equals(x.Title, title, StringComparison.Ordinal));
        if (exact != null) return exact;

        return live.FirstOrDefault(x => x.Title.Contains(title, StringComparison.OrdinalIgnoreCase))
            ?? live.FirstOrDefault(x => title.Contains(x.Title, StringComparison.OrdinalIgnoreCase));
    }

    const uint SWP_NOZORDER = 0x0004;
    const uint SWP_NOACTIVATE = 0x0010;

    [DllImport("user32.dll", SetLastError = true)]
    static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
}

public sealed class LiveWindow
{
    public IntPtr Handle { get; set; }
    public string Title { get; set; } = "";
}
