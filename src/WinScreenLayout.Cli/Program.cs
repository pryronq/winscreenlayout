using WinScreenLayout.Core;

var svc = new LayoutService();

if (args.Length < 2) return Usage();

var cmd = args[0].ToLowerInvariant();
var name = args[1];

switch (cmd)
{
    case "save":
        svc.Save(name);
        return 0;

    case "apply":
        svc.Apply(name, restore: true);
        return 0;

    default:
        return Usage();
}

static int Usage()
{
    Console.WriteLine("winscreenlayout");
    Console.WriteLine("Usage:");
    Console.WriteLine("  winscreenlayout save <name>");
    Console.WriteLine("  winscreenlayout apply <name>");
    return 1;
}
