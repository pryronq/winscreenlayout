using WinScreenLayout.Core.Models;
using WinScreenLayout.Core.Native;

namespace WinScreenLayout.Core.Services;

public sealed class WindowCaptureService
{
    public IReadOnlyList<WindowInfo> CaptureVisibleWindows(bool includeEmptyTitles = false)
    {
        var shell = Win32.GetShellWindow();
        var result = new List<WindowInfo>();

        Win32.EnumWindows((hWnd, _) =>
        {
            if (hWnd == shell) return true;
            if (!Win32.IsWindowVisible(hWnd)) return true;

            var title = Win32.GetTitle(hWnd).Trim();
            if (!includeEmptyTitles && string.IsNullOrWhiteSpace(title)) return true;

            if (!Win32.GetWindowRect(hWnd, out var r)) return true;

            var w = Math.Max(0, r.Right - r.Left);
            var h = Math.Max(0, r.Bottom - r.Top);
            if (w < 50 || h < 50) return true; // noise filter

            result.Add(new WindowInfo
            {
                Title = title,
                ProcessName = Win32.GetProcessName(hWnd),
                ClassName = Win32.GetClassNameString(hWnd),
                X = r.Left,
                Y = r.Top,
                Width = w,
                Height = h
            });

            return true;
        }, IntPtr.Zero);

        return result;
    }
}
