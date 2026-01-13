using System;

public sealed class WindowInfo
{
    // Handle ist nur zur Laufzeit sinnvoll → nicht speichern
    // public IntPtr Handle { get; set; }

    public string Title { get; set; } = "";
    public int X { get; set; }
    public int Y { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
}

