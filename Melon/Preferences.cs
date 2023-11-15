using MelonLoader;

namespace Paranoia
{
    internal static class Preferences
    {
        public static readonly MelonPreferences_Category Category = MelonPreferences.CreateCategory("Paranoia");

        public static MelonPreferences_Entry<LoggingMode> loggingMode { get; set; }
        public static MelonPreferences_Entry<bool> enabledInBaseGameMaps { get; set; }
        public static MelonPreferences_Entry<string> baselineManager { get; set; }
        public static MelonPreferences_Entry<string> museumManager { get; set; }
        public static MelonPreferences_Entry<string> blankboxManager { get; set; }

        public static void Setup()
        {
            loggingMode = Category.CreateEntry("LoggingMode", LoggingMode.NORMAL, "Logging Mode", "The logging mode to use: NORMAL, DEBUG");
            enabledInBaseGameMaps = Category.CreateEntry("EnabledInBaseGameMaps", true, "Baseline/Museum Basement/Blankbox Activation", "Whether to activate Paranoia in Baseline/Museum Basement or not.");
            baselineManager = Category.CreateEntry("BaselineManager", Pallet.Managers.BaselineManager, "Baseline Manager Barcode", "The barcode of the manager to use in Baseline. Default: NotEnoughPhotons.Paranoia.Spawnable.BaselineParanoia");
            museumManager = Category.CreateEntry("MuseumManager", Pallet.Managers.MuseumManager, "Museum Basement Manager Barcode", "The barcode of the manager to use in Museum Basement. Default: NotEnoughPhotons.Paranoia.Spawnable.MuseumParanoia");
            blankboxManager = Category.CreateEntry("BlankboxManager", Pallet.Managers.BlankBoxManager, "BlankBox Manager Barcode", "The barcode of the manager to use in BlankBox. Default: NotEnoughPhotons.Paranoia.Spawnable.BlankboxParanoia");
            Category.SaveToFile(false);
            ModConsole.Msg("Finished preferences setup", LoggingMode.DEBUG);
        }
    }
}