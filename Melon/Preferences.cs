using MelonLoader;

namespace Paranoia
{
    internal static class Preferences
    {
        private static MelonPreferences_Category category = MelonPreferences.CreateCategory("Barcode");

        public static ModPref<LoggingMode> loggingMode;

        public static ModPref<bool> enabledInBaseGameMaps;

        public static void Setup()
        {
            loggingMode = new ModPref<LoggingMode>(category, "LoggingMode", LoggingMode.NORMAL, "Logging Mode", "The logging mode to use: NORMAL, DEBUG");
            enabledInBaseGameMaps = new ModPref<bool>(category, "EnabledInBaseGameMaps", true, "Baseline/Museum Basement Activation", "Whether to activate Barcode in Baseline/Museum Basement or not.");
            category.SaveToFile(false);
            ModConsole.Msg("Finished preferences setup", LoggingMode.DEBUG);
        }
    }
}