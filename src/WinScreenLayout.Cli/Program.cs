using System;
using System.Text.Json;

static int Usage()
{
    Console.WriteLine("winscreenlayout");
    Console.WriteLine("Usage:");
    Console.WriteLine("  winscreenlayout save <name>");
    Console.WriteLine("  winscreenlayout apply <name>");
    return 1;
}

if (args.Length < 2) return Usage();

var cmd = args[0].ToLowerInvariant();
var name = args[1];

return cmd switch
{
    "save"  => Save(name),
    "apply" => Apply(name),
    _       => Usage()
};

static int Save(string name)
{
    var layout = WindowCapture.Capture();
    LayoutStore.Save(name, layout);
    Console.WriteLine($"Saved layout: {name}");
    return 0;
}

static int Apply(string name)
{
    var layout = LayoutStore.Load(name);
    return WindowApply.Apply(layout);
}
