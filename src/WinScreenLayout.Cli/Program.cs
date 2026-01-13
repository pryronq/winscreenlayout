using System;

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
    Console.WriteLine($"[TODO] save layout: {name}");
    return 0;
}

static int Apply(string name)
{
    Console.WriteLine($"[TODO] apply layout: {name}");
    return 0;
}
