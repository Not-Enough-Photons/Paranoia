using System;
using BoneLib;
using MelonLoader;
using Paranoia.Internal;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
#if DEBUG
using BoneLib.BoneMenu;
using Paranoia.Helpers;
using Paranoia.Events;
using Paranoia.Managers;
#endif

namespace Paranoia
{
    /// <summary>
    /// <see cref="MelonMod"/>
    /// </summary>
    public class Paranoia : MelonMod
    {
        internal const string Name = "Paranoia"; // Required
        internal const string Description = "There's something hostile out there."; // Required
        internal const string Author = "Not Enough Photons, adamdev, SoulWithMae"; // Required
        internal const string Company = "Not Enough Photons";
        internal const string Version = "1.0.0";
        internal const string DownloadLink = "null";
        
        /// <summary>
        /// Sets up the logger, preferences, fieldinjection, and hooks.
        /// <br/>If you're using a debug build, also sets up BoneMenu.
        /// </summary>
        public override void OnInitializeMelon()
        {
            ModConsole.Setup(LoggerInstance);
            Preferences.Setup();
#if DEBUG
            var recording = Utilities.CheckIfRecording();
            ModConsole.Msg($"Recording software running = {recording}", LoggingMode.DEBUG);
#endif
            MapCheck.enabled = Preferences.enabledInBaseGameMaps.entry.Value;
            ModConsole.Msg("THIS PERSON IS USING PARANOIA. THERE IS AN EVENT THAT CRASHES THE GAME. THIS LOG MAY BE VOID, CHECK LATER IN THE LOG FOR A SIMILAR WARNING TO CONFIRM");
            FieldInjection.Inject();
            Hooking.OnLevelInitialized += OnLevelLoaded;
#if DEBUG
            ModConsole.Msg("THE DEBUG BUILD OF PARANOIA IS BEING USED. THIS IS NOT RECOMMENDED FOR NORMAL USE.");
            SetupBoneMenu();
#endif
        }
        
        /// <summary>
        /// Checks for Monodirector later than normal in case it's loaded after Paranoia.
        /// </summary>
        public override void OnLateInitializeMelon()
        {
            CheckForMonodirector();
        }
        
        /// <summary>
        /// Whether Monodirector is installed or not
        /// </summary>
        public static bool hasMonodirector;
        /// <summary>
        /// Checks loaded assemblies for Monodirector, and sets <see cref="hasMonodirector"/> to true if it's loaded.
        /// </summary>
        private static void CheckForMonodirector()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var asm in assemblies)
            {
                if (asm.GetName().Name.ToLower().Equals("monodirector"))
                {
                    MelonLogger.Msg("Monodirector found!", LoggingMode.DEBUG);
                    hasMonodirector = true;
                }
            }
        }
        
        /// <summary>
        /// Set to be whatever level you're in. Used in <see cref="MapCheck"/>.
        /// </summary>
        public static string levelBarcode;
        /// <summary>
        /// Detects whatever level is loaded and sets <see cref="levelBarcode"/> to it.
        /// </summary>
        private static void OnLevelLoaded(LevelInfo levelInfo)
        {
            ModConsole.Msg($"Level loaded: {levelInfo.title}", LoggingMode.DEBUG);
            levelBarcode = levelInfo.barcode;
        }
        
#if DEBUG
        /// <summary>
        /// Debug menu for testing various things.
        /// </summary>
        private static void SetupBoneMenu()
        {
            var maincat = MenuManager.CreateCategory("Not Enough Photons", Color.white);
            var cat = maincat.CreateCategory("Paranoia", Color.grey);
            cat.CreateFunctionElement("Force Door Spawn", Color.white, delegate
            {
                var manager = GameObject.Find("ParanoiaManager").GetComponent<ParanoiaManager>();
                try
                {
                    var baselineManager = GameObject.Find("BaselineManager").GetComponent<BaselineManager>();
                    if (baselineManager != null)
                    {
                        var door = baselineManager.door;
                        var doorSpawnLocations = baselineManager.doorSpawnLocations;
                        var location = doorSpawnLocations[Random.Range(0, doorSpawnLocations.Length)];
                        Object.Instantiate(door, location.position, location.rotation);
                    }
                }
                catch (Exception e)
                {
                    ModConsole.Error(e.ToString());
                }
                if (manager != null)
                {
                    var door = manager.door;
                    var doorSpawnLocations = manager.doorSpawnLocations;
                    var location = doorSpawnLocations[Random.Range(0, doorSpawnLocations.Length)];
                    Object.Instantiate(door, location.position, location.rotation);
                }
            });
            var cat2 = cat.CreateCategory("Events", Color.white);
            cat2.CreateFunctionElement("Crabtroll", Color.red, Crabtroll.Activate);
            cat2.CreateFunctionElement("KillAI", Color.red, KillAI.Activate);
            cat2.CreateFunctionElement("LaughAtPlayer", Color.red, LaughAtPlayer.Activate);
            cat2.CreateFunctionElement("MoveAIToPlayer", Color.green, MoveAIToPlayer.Activate);
            cat2.CreateFunctionElement("FakeFireGun", Color.blue, FakeFireGun.Activate);
            cat2.CreateFunctionElement("FireGunInHand", Color.cyan, FireGunInHand.Activate);
            cat2.CreateFunctionElement("FireGun", Color.cyan, FireGun.Activate);
            cat2.CreateFunctionElement("FlickerFlashlight", Color.yellow, FlickerFlashlights.Activate);
            cat2.CreateFunctionElement("FlingRandomObject", Color.magenta, FlingRandomObject.Activate);
            cat2.CreateFunctionElement("Crash Game", Color.red, Utilities.CrashGame, "This will crash the game!");
        }
#endif
    }
}