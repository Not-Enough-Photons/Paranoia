namespace NEP.Paranoia.Melon;

/// <summary>
/// A wrapper for MelonLoader's MelonLogger.
/// <br/>Allows for logging with priority.
/// <br/>If the user has DEBUG enabled, all logging is shown for, well, debugging.
/// <br/>If the user has NORMAL (the default) enabled, only errors or important messages are shown. Good for an average user.
/// </summary>
internal static class ModConsole
{
    private static MelonLogger.Instance logger;
        
    public static void Setup(MelonLogger.Instance mlLogger)
    {
        logger = mlLogger;
    }

    public static void Msg(object obj, LoggingMode loggingMode = LoggingMode.Normal)
    {
        var msg = loggingMode == LoggingMode.Debug ? $"[DEBUG] {obj}" : obj.ToString();
        var txtcolor = loggingMode == LoggingMode.Debug ? ConsoleColor.Yellow : ConsoleColor.Gray;
        if (Preferences.LoggingMode.Value >= loggingMode)
            logger.Msg(txtcolor, msg);
    }

    public static void Msg(string txt, LoggingMode loggingMode = LoggingMode.Normal)
    {
        var msg = loggingMode == LoggingMode.Debug ? $"[DEBUG] {txt}" : txt;
        var txtcolor = loggingMode == LoggingMode.Debug ? ConsoleColor.Yellow : ConsoleColor.Gray;
        if (Preferences.LoggingMode.Value >= loggingMode)
            logger.Msg(txtcolor, msg);
    }

    public static void Msg(ConsoleColor txtcolor, object obj, LoggingMode loggingMode = LoggingMode.Normal)
    {
        var msg = loggingMode == LoggingMode.Debug ? $"[DEBUG] {obj}" : obj.ToString();
        if (Preferences.LoggingMode.Value >= loggingMode)
            logger.Msg(txtcolor, msg);
    }

    public static void Msg(ConsoleColor txtcolor, string txt, LoggingMode loggingMode = LoggingMode.Normal)
    {
        var msg = loggingMode == LoggingMode.Debug ? $"[DEBUG] {txt}" : txt;
        if (Preferences.LoggingMode.Value >= loggingMode)
            logger.Msg(txtcolor, msg);
    }

    public static void Msg(string txt, LoggingMode loggingMode = LoggingMode.Normal, params object[] args)
    {
        var msg = loggingMode == LoggingMode.Debug ? $"[DEBUG] {txt}" : txt;
        var txtcolor = loggingMode == LoggingMode.Debug ? ConsoleColor.Yellow : ConsoleColor.Gray;
        if (Preferences.LoggingMode.Value >= loggingMode)
            logger.Msg(txtcolor, msg, args);
    }

    public static void Msg(ConsoleColor txtcolor, string txt, LoggingMode loggingMode = LoggingMode.Normal, params object[] args)
    {
        var msg = loggingMode == LoggingMode.Debug ? $"[DEBUG] {txt}" : txt;
        if (Preferences.LoggingMode.Value >= loggingMode)
            logger.Msg(txtcolor, msg, args);
    }

    public static void Error(object obj, LoggingMode loggingMode = LoggingMode.Normal)
    {
        var msg = loggingMode == LoggingMode.Debug ? $"[DEBUG] {obj}" : obj.ToString();
        if (Preferences.LoggingMode.Value >= loggingMode)
            logger.Error(msg);
    }

    public static void Error(string txt, LoggingMode loggingMode = LoggingMode.Normal)
    {
        var msg = loggingMode == LoggingMode.Debug ? $"[DEBUG] {txt}" : txt;
        if (Preferences.LoggingMode.Value >= loggingMode)
            logger.Error(msg);
    }

    public static void Error(string txt, LoggingMode loggingMode = LoggingMode.Normal, params object[] args)
    {
        var msg = loggingMode == LoggingMode.Debug ? $"[DEBUG] {txt}" : txt;
        if (Preferences.LoggingMode.Value >= loggingMode)
            logger.Error(msg, args);
    }

    public static void Warning(object obj, LoggingMode loggingMode = LoggingMode.Normal)
    {
        var msg = loggingMode == LoggingMode.Debug ? $"[DEBUG] {obj}" : obj.ToString();
        if (Preferences.LoggingMode.Value >= loggingMode)
            logger.Warning(msg);
    }

    public static void Warning(string txt, LoggingMode loggingMode = LoggingMode.Normal)
    {
        var msg = loggingMode == LoggingMode.Debug ? $"[DEBUG] {txt}" : txt;
        if (Preferences.LoggingMode.Value >= loggingMode)
            logger.Warning(msg);
    }

    public static void Warning(string txt, LoggingMode loggingMode = LoggingMode.Normal, params object[] args)
    {
        var msg = loggingMode == LoggingMode.Debug ? $"[DEBUG] {txt}" : txt;
        if (Preferences.LoggingMode.Value >= loggingMode)
            logger.Warning(msg, args);
    }
    }

internal enum LoggingMode
{
    Normal,
    Debug
}