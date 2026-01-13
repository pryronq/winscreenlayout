using System.Text.Json;
using WinScreenLayout.Core.Models;

namespace WinScreenLayout.Core.Services;

public sealed class LayoutStore
{
    private static readonly JsonSerializerOptions JsonOpts = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public string RootPath { get; } =
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "winscreenlayout");

    public LayoutStore()
    {
        Directory.CreateDirectory(RootPath);
    }

    public IReadOnlyList<string> ListLayouts()
        => Directory.EnumerateFiles(RootPath, "*.json")
            .Select(Path.GetFileNameWithoutExtension)
            .Where(n => !string.IsNullOrWhiteSpace(n))
            .OrderBy(n => n, StringComparer.OrdinalIgnoreCase)
            .ToList();

    public void Save(string name, IReadOnlyList<WindowInfo> windows)
    {
        var path = GetPath(name);
        File.WriteAllText(path, JsonSerializer.Serialize(windows, JsonOpts));
    }

    public IReadOnlyList<WindowInfo> Load(string name)
    {
        var path = GetPath(name);
        if (!File.Exists(path))
            throw new FileNotFoundException($"Layout not found: {name}", path);

        var json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<List<WindowInfo>>(json, JsonOpts) ?? new List<WindowInfo>();
    }

    public void Delete(string name)
    {
        var path = GetPath(name);
        if (File.Exists(path)) File.Delete(path);
    }

    private string GetPath(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name is empty.", nameof(name));
        foreach (var c in Path.GetInvalidFileNameChars())
            name = name.Replace(c, '_');
        return Path.Combine(RootPath, $"{name}.json");
    }
}
