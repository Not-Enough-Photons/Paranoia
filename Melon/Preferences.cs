using MelonLoader;

namespace Paranoia
{
    internal static class Preferences
    {
        public static readonly MelonPreferences_Category Category = MelonPreferences.CreateCategory("Paranoia");

        public static MelonPreferences_Entry<LoggingMode> loggingMode;

        public static MelonPreferences_Entry<bool> enabledInBaseGameMaps;

        public static void Setup()
        {
            loggingMode = Category.CreateEntry("LoggingMode", LoggingMode.NORMAL, "Logging Mode", "The logging mode to use: NORMAL, DEBUG");
            enabledInBaseGameMaps = Category.CreateEntry("EnabledInBaseGameMaps", true, "Baseline/Museum Basement Activation", "Whether to activate Paranoia in Baseline/Museum Basement or not.");
            Category.SaveToFile(false);
            ModConsole.Msg("Finished preferences setup", LoggingMode.DEBUG);
        }
    }
}