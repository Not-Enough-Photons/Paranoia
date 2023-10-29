using MelonLoader;
using System;

namespace Paranoia
{
    /// <summary>
    /// A wrapper for MelonLoader's MelonLogger.
    /// <br/>Allows for logging with priority.
    /// <br/>If the user has DEBUG enabled, all logging is shown for, well, debugging.
    /// <br/>If the user has NORMAL (the default) enabled, only errors or important messages are shown. Good for an average user.
    /// </summary>
    internal static class ModConsole
    {
        private static MelonLogger.Instance logger;
        
        public static void Setup(MelonLogger.Instance logger)
        {
            ModConsole.logger = logger;
        }

        public static void Msg(object obj, LoggingMode loggingMode = LoggingMode.NORMAL)
        {
            var msg = loggingMode == LoggingMode.DEBUG ? $"[DEBUG] {obj}" : obj.ToString();
            var txtcolor = loggingMode == LoggingMode.DEBUG ? ConsoleColor.Yellow : ConsoleColor.Gray;
            if (Preferences.loggingMode >= loggingMode)
                logger.Msg(txtcolor, msg);
        }

        public static void Msg(string txt, LoggingMode loggingMode = LoggingMode.NORMAL)
        {
            var msg = loggingMode == LoggingMode.DEBUG ? $"[DEBUG] {txt}" : txt;
            var txtcolor = loggingMode == LoggingMode.DEBUG ? ConsoleColor.Yellow : ConsoleColor.Gray;
            if (Preferences.loggingMode >= loggingMode)
                logger.Msg(txtcolor, msg);
        }

        public static void Msg(ConsoleColor txtcolor, object obj, LoggingMode loggingMode = LoggingMode.NORMAL)
        {
            var msg = loggingMode == LoggingMode.DEBUG ? $"[DEBUG] {obj}" : obj.ToString();
            if (Preferences.loggingMode >= loggingMode)
                logger.Msg(txtcolor, msg);
        }

        public static void Msg(ConsoleColor txtcolor, string txt, LoggingMode loggingMode = LoggingMode.NORMAL)
        {
            var msg = loggingMode == LoggingMode.DEBUG ? $"[DEBUG] {txt}" : txt;
            if (Preferences.loggingMode >= loggingMode)
                logger.Msg(txtcolor, msg);
        }

        public static void Msg(string txt, LoggingMode loggingMode = LoggingMode.NORMAL, params object[] args)
        {
            var msg = loggingMode == LoggingMode.DEBUG ? $"[DEBUG] {txt}" : txt;
            var txtcolor = loggingMode == LoggingMode.DEBUG ? ConsoleColor.Yellow : ConsoleColor.Gray;
            if (Preferences.loggingMode >= loggingMode)
                logger.Msg(txtcolor, msg, args);
        }

        public static void Msg(ConsoleColor txtcolor, string txt, LoggingMode loggingMode = LoggingMode.NORMAL, params object[] args)
        {
            var msg = loggingMode == LoggingMode.DEBUG ? $"[DEBUG] {txt}" : txt;
            if (Preferences.loggingMode >= loggingMode)
                logger.Msg(txtcolor, msg, args);
        }

        public static void Error(object obj, LoggingMode loggingMode = LoggingMode.NORMAL)
        {
            var msg = loggingMode == LoggingMode.DEBUG ? $"[DEBUG] {obj}" : obj.ToString();
            if (Preferences.loggingMode >= loggingMode)
                logger.Error(msg);
        }

        public static void Error(string txt, LoggingMode loggingMode = LoggingMode.NORMAL)
        {
            var msg = loggingMode == LoggingMode.DEBUG ? $"[DEBUG] {txt}" : txt;
            if (Preferences.loggingMode >= loggingMode)
                logger.Error(msg);
        }

        public static void Error(string txt, LoggingMode loggingMode = LoggingMode.NORMAL, params object[] args)
        {
            var msg = loggingMode == LoggingMode.DEBUG ? $"[DEBUG] {txt}" : txt;
            if (Preferences.loggingMode >= loggingMode)
                logger.Error(msg, args);
        }

        public static void Warning(object obj, LoggingMode loggingMode = LoggingMode.NORMAL)
        {
            var msg = loggingMode == LoggingMode.DEBUG ? $"[DEBUG] {obj}" : obj.ToString();
            if (Preferences.loggingMode >= loggingMode)
                logger.Warning(msg);
        }

        public static void Warning(string txt, LoggingMode loggingMode = LoggingMode.NORMAL)
        {
            var msg = loggingMode == LoggingMode.DEBUG ? $"[DEBUG] {txt}" : txt;
            if (Preferences.loggingMode >= loggingMode)
                logger.Warning(msg);
        }

        public static void Warning(string txt, LoggingMode loggingMode = LoggingMode.NORMAL, params object[] args)
        {
            var msg = loggingMode == LoggingMode.DEBUG ? $"[DEBUG] {txt}" : txt;
            if (Preferences.loggingMode >= loggingMode)
                logger.Warning(msg, args);
        }
    }
}

internal enum LoggingMode
{
    NORMAL,
    DEBUG
}