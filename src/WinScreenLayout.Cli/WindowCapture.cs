using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public static class WindowCapture
{
    public static List<WindowInfo> Capture()
    {
        var list = new List<WindowInfo>();
        EnumWindows((hWnd, _) =>
        {
            if (!IsWindowVisible(hWnd)) return true;

            GetWindowRect(hWnd, out var r);
            var title = GetTitle(hWnd);
            if (string.IsNullOrWhiteSpace(title)) return true;

            list.Add(new WindowInfo
            {
               Title = title,
                X = r.Left,
                Y = r.Top,
                Width = r.Right - r.Left,
                Height = r.Bottom - r.Top
            });
            return true;
        }, IntPtr.Zero);

        return list;
    }

    static string GetTitle(IntPtr hWnd)
    {
        var sb = new System.Text.StringBuilder(256);
        GetWindowText(hWnd, sb, sb.Capacity);
        return sb.ToString();
    }

    delegate bool EnumProc(IntPtr hWnd, IntPtr lParam);

    [DllImport("user32.dll")] static extern bool EnumWindows(EnumProc lpEnumFunc, IntPtr lParam);
    [DllImport("user32.dll")] static extern bool IsWindowVisible(IntPtr hWnd);
    [DllImport("user32.dll")] static extern int GetWindowText(IntPtr hWnd, System.Text.StringBuilder text, int max);
    [DllImport("user32.dll")] static extern bool GetWindowRect(IntPtr hWnd, out RECT rect);

    struct RECT { public int Left, Top, Right, Bottom; }
    public static List<LiveWindow> CaptureWithHandles()
{
    var list = new List<LiveWindow>();
    EnumWindows((hWnd, _) =>
    {
        if (!IsWindowVisible(hWnd)) return true;

        var title = GetTitle(hWnd);
        if (string.IsNullOrWhiteSpace(title)) return true;

        list.Add(new LiveWindow { Handle = hWnd, Title = title });
        return true;
    }, IntPtr.Zero);

    return list;
}

}
