using WinScreenLayout.Core.Services;

namespace WinScreenLayout.Core;

public sealed class LayoutService
{
    private readonly LayoutStore _store = new();
    private readonly WindowCaptureService _capture = new();
    private readonly WindowApplyService _apply = new();

    public IReadOnlyList<string> GetLayouts() => _store.ListLayouts();

    public void Save(string name)
    {
        var windows = _capture.CaptureVisibleWindows();
        _store.Save(name, windows);
    }

    public void Apply(string name, bool restore = true)
    {
        var layout = _store.Load(name);
        _apply.Apply(layout, restore);
    }

    public void Delete(string name) => _store.Delete(name);
}
