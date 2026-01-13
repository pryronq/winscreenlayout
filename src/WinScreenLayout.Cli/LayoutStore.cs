using System;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;

public static class LayoutStore
{
    static string BaseDir =>
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "winscreenlayout");

    public static void Save(string name, List<WindowInfo> layout)
    {
        Directory.CreateDirectory(BaseDir);
        var path = Path.Combine(BaseDir, $"{name}.json");
        File.WriteAllText(path, JsonSerializer.Serialize(layout, new JsonSerializerOptions { WriteIndented = true }));
    }
    public static List<WindowInfo> Load(string name)
{
    var path = Path.Combine(BaseDir, $"{name}.json");
    if (!File.Exists(path))
        throw new FileNotFoundException($"Layout not found: {path}");

    var json = File.ReadAllText(path);
    return JsonSerializer.Deserialize<List<WindowInfo>>(json)
           ?? new List<WindowInfo>();
}

}
