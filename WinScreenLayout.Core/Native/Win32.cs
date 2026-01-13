using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace WinScreenLayout.Core.Native;

internal static class Win32
{
    internal delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

    [DllImport("user32.dll")] internal static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);
    [DllImport("user32.dll")] internal static extern bool IsWindowVisible(IntPtr hWnd);
    [DllImport("user32.dll", SetLastError = true)] internal static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);
    [DllImport("user32.dll")] internal static extern int GetWindowTextLength(IntPtr hWnd);
    [DllImport("user32.dll")] internal static extern IntPtr GetShellWindow();
    [DllImport("user32.dll")] internal static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

    [DllImport("user32.dll")] internal static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter,
        int X, int Y, int cx, int cy, uint uFlags);

    [DllImport("user32.dll")] internal static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

    [DllImport("user32.dll", SetLastError = true)]
    internal static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

    [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    internal static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

    internal const uint SWP_NOZORDER = 0x0004;
    internal const uint SWP_NOACTIVATE = 0x0010;

    internal const int SW_RESTORE = 9;

    [StructLayout(LayoutKind.Sequential)]
    internal struct RECT
    {
        public int Left, Top, Right, Bottom;
    }

    internal static string GetTitle(IntPtr hWnd)
    {
        var len = GetWindowTextLength(hWnd);
        if (len <= 0) return "";
        var sb = new StringBuilder(len + 1);
        GetWindowText(hWnd, sb, sb.Capacity);
        return sb.ToString();
    }

    internal static string? GetProcessName(IntPtr hWnd)
    {
        try
        {
            GetWindowThreadProcessId(hWnd, out var pid);
            if (pid == 0) return null;
            return Process.GetProcessById((int)pid).ProcessName;
        }
        catch { return null; }
    }

    internal static string? GetClassNameString(IntPtr hWnd)
    {
        try
        {
            var sb = new StringBuilder(256);
            var r = GetClassName(hWnd, sb, sb.Capacity);
            return r > 0 ? sb.ToString() : null;
        }
        catch { return null; }
    }
}
