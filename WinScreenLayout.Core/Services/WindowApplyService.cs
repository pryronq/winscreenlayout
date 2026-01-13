using WinScreenLayout.Core.Models;
using WinScreenLayout.Core.Native;

namespace WinScreenLayout.Core.Services;

public sealed class WindowApplyService
{
    public void Apply(IReadOnlyList<WindowInfo> layout, bool restoreBeforeApply = true)
    {
        var live = CaptureLiveWindows();

        foreach (var target in layout)
        {
            var match = FindBestMatch(live, target);
            if (match == IntPtr.Zero) continue;

            if (restoreBeforeApply)
                Win32.ShowWindow(match, Win32.SW_RESTORE);

            Win32.SetWindowPos(match, IntPtr.Zero,
                target.X, target.Y, target.Width, target.Height,
                Win32.SWP_NOZORDER | Win32.SWP_NOACTIVATE);
        }
    }

    private static List<(IntPtr Hwnd, string Title, string? Proc, string? Class)> CaptureLiveWindows()
    {
        var shell = Win32.GetShellWindow();
        var list = new List<(IntPtr, string, string?, string?)>();

        Win32.EnumWindows((hWnd, _) =>
        {
            if (hWnd == shell) return true;
            if (!Win32.IsWindowVisible(hWnd)) return true;

            var title = Win32.GetTitle(hWnd).Trim();
            if (string.IsNullOrWhiteSpace(title)) return true;

            list.Add((hWnd, title, Win32.GetProcessName(hWnd), Win32.GetClassNameString(hWnd)));
            return true;
        }, IntPtr.Zero);

        return list;
    }

    private static IntPtr FindBestMatch(List<(IntPtr Hwnd, string Title, string? Proc, string? Class)> live, WindowInfo target)
    {
        // Priority:
        // 1) Proc + Class + exact title
        // 2) Proc + exact title
        // 3) exact title
        // 4) contains title (fallback)
        var title = target.Title?.Trim() ?? "";
        var proc = target.ProcessName?.Trim();
        var cls = target.ClassName?.Trim();

        var best =
            live.FirstOrDefault(x => !string.IsNullOrEmpty(proc) && !string.IsNullOrEmpty(cls)
                                     && string.Equals(x.Proc, proc, StringComparison.OrdinalIgnoreCase)
                                     && string.Equals(x.Class, cls, StringComparison.OrdinalIgnoreCase)
                                     && string.Equals(x.Title, title, StringComparison.OrdinalIgnoreCase));

        if (best.Hwnd != IntPtr.Zero) return best.Hwnd;

        best = live.FirstOrDefault(x => !string.IsNullOrEmpty(proc)
                                       && string.Equals(x.Proc, proc, StringComparison.OrdinalIgnoreCase)
                                       && string.Equals(x.Title, title, StringComparison.OrdinalIgnoreCase));
        if (best.Hwnd != IntPtr.Zero) return best.Hwnd;

        best = live.FirstOrDefault(x => string.Equals(x.Title, title, StringComparison.OrdinalIgnoreCase));
        if (best.Hwnd != IntPtr.Zero) return best.Hwnd;

        best = live.FirstOrDefault(x => x.Title.Contains(title, StringComparison.OrdinalIgnoreCase));
        return best.Hwnd;
    }
}
