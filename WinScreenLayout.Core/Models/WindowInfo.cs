namespace WinScreenLayout.Core.Models;

public sealed class WindowInfo
{
    public string Title { get; set; } = "";
    public string? ProcessName { get; set; }
    public string? ClassName { get; set; }

    public int X { get; set; }
    public int Y { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }

    public bool IsMaximized { get; set; }
    public bool IsMinimized { get; set; }
}
