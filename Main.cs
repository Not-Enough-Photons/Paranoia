using BoneLib;
using HarmonyLib;
using MelonLoader;
using Paranoia.Helpers;

namespace Paranoia
{
    public class Main : MelonMod
    {
        internal const string Name = "Paranoia"; // Required
        internal const string Description = "Keep your clones close."; // Required
        internal const string Author = "Not Enough Photons, adamdev, SoulWithMae"; // Required
        internal const string Company = "Not Enough Photons";
        internal const string Version = "1.0.0";
        internal const string DownloadLink = "null";
        
        public override void OnInitializeMelon()
        {
            ModConsole.Setup(LoggerInstance);
            Preferences.Setup();
            FieldInjection.Inject();
            Hooking.OnLevelInitialized += OnLevelLoad;
        }
        
        public static string levelTitle;
        private static void OnLevelLoad(LevelInfo levelInfo)
        {
            levelTitle = levelInfo.title;
        }
    }
}