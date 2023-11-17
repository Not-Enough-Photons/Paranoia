namespace Paranoia;

internal static class ModStats
{
    private const string StatsCategory = "Paranoia";
        
    public static async Task IncrementLaunch()
    {
        var prefix = Jevil.Utilities.IsPlatformQuest() ? "Quest" : "PCVR";
        ModConsole.Msg($"Sending stats request for {prefix} platform launch!");
        var success = await StatsEntry.IncrementValueAsync(StatsCategory, prefix + "Launches");
        ModConsole.Msg($"Sending stats request for {prefix} platform launch {(success ? "succeeded" : "failed")}");
    }
        
    public static async Task IncrementUser()
    {
        var prefix = Jevil.Utilities.IsPlatformQuest() ? "Quest" : "PCVR";
        ModConsole.Msg($"Sending stats request for {prefix} platform user!");
        var success = await StatsEntry.IncrementValueAsync(StatsCategory, prefix + "Users");
        ModConsole.Msg($"Sending stats request for {prefix} platform user {(success ? "succeeded" : "failed")}");
    }

    public static void IncrementEntry(string entry)
    {
        StatsEntry.IncrementValueAsync(StatsCategory, entry);
        ModConsole.Msg($"Sending stats request for {entry}", LoggingMode.Debug);
    }
}