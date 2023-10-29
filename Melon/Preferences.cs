using MelonLoader;

namespace Paranoia
{
    internal static class Preferences
    {
        private static MelonPreferences_Category category = MelonPreferences.CreateCategory("Paranoia");

        public static ModPref<LoggingMode> loggingMode;

        public static ModPref<bool> baselineSchizophrenia;

        public static void Setup()
        {
            loggingMode = new ModPref<LoggingMode>(category, "LoggingMode", LoggingMode.NORMAL, "Logging Mode", "The logging mode to use: NORMAL, DEBUG");
            baselineSchizophrenia = new ModPref<bool>(category, "BaselineSchizophrenia", true, "Baseline Activation", "Whether to activate Paranoia in Baseline or not.");
            category.SaveToFile(false);
            ModConsole.Msg("Finished preferences setup", LoggingMode.DEBUG);
        }
    }
}