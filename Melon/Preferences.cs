namespace NEP.Paranoia;

internal static class Preferences
{
    public static readonly MelonPreferences_Category Category = MelonPreferences.CreateCategory("Paranoia");

    public static MelonPreferences_Entry<LoggingMode> LoggingMode { get; private set; }
    public static MelonPreferences_Entry<bool> EnabledInBaseGameMaps { get; private set; }
    public static MelonPreferences_Entry<string> BaselineManager { get; private set; }
    public static MelonPreferences_Entry<string> MuseumManager { get; private set; }
    public static MelonPreferences_Entry<string> BlankboxManager { get; private set; }

    public static void Setup()
    {
        LoggingMode = Category.CreateEntry("LoggingMode", global::NEP.Paranoia.LoggingMode.Normal, "Logging Mode", "The logging mode to use: NORMAL, DEBUG");
        EnabledInBaseGameMaps = Category.CreateEntry("EnabledInBaseGameMaps", true, "Baseline/Museum Basement/Blankbox Activation", "Whether to activate Paranoia in Baseline/Museum Basement or not.");
        BaselineManager = Category.CreateEntry("BaselineManager", Pallet.Managers.BaselineManager, "Baseline Manager Barcode", "The barcode of the manager to use in Baseline. Default: NotEnoughPhotons.Paranoia.Spawnable.BaselineParanoia");
        MuseumManager = Category.CreateEntry("MuseumManager", Pallet.Managers.MuseumManager, "Museum Basement Manager Barcode", "The barcode of the manager to use in Museum Basement. Default: NotEnoughPhotons.Paranoia.Spawnable.MuseumParanoia");
        BlankboxManager = Category.CreateEntry("BlankboxManager", Pallet.Managers.BlankBoxManager, "BlankBox Manager Barcode", "The barcode of the manager to use in BlankBox. Default: NotEnoughPhotons.Paranoia.Spawnable.BlankboxParanoia");
        Category.SaveToFile(false);
        ModConsole.Msg("Finished preferences setup", global::NEP.Paranoia.LoggingMode.Debug);
    }
}