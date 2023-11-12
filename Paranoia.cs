using System;
using System.Threading;
using BoneLib;
using MelonLoader;
using Paranoia.Internal;
using UnityEngine;
using Random = UnityEngine.Random;
#if DEBUG
using System.Collections.Generic;
using BoneLib.BoneMenu;
using Paranoia.Helpers;
using Paranoia.Events;
using SLZ.Marrow.Warehouse;
#endif

namespace Paranoia
{
    /// <summary>
    /// <see cref="MelonMod"/>
    /// </summary>
    public class Paranoia : MelonMod
    {
        internal const string Name = "Paranoia";
        internal const string Description = "There's something hostile out there.";
        internal const string Author = "Not Enough Photons, adamdev, SoulWithMae";
        internal const string Company = "Not Enough Photons";
        internal const string Version = "0.0.1";
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
        /// Checks for if AssetWarehouse is ready, and if it is, calls <see cref="WarehouseReady"/>.
        /// </summary>
        public override void OnSceneWasInitialized(int buildIndex, string sceneName)
        {
            if (sceneName.ToUpper().Contains("BOOTSTRAP"))
            {
                AssetWarehouse.OnReady(new Action(WarehouseReady));
            }
        }
        
        /// <summary>
        /// Checks if you have the pallet installed, then warns if you don't.
        /// </summary>
        private static void WarehouseReady()
        {
            var deps = DependencyCheck.CheckForDependencies();
            if (!deps)
            {
                var missing = DependencyCheck.GetMissingDependency();
                ModConsole.Error($"You do not have {missing}!");
            }
            if (!AssetWarehouse.Instance.HasPallet(Pallet.Barcode))
            {
                ModConsole.Error("You do not have the required pallet for Paranoia.");
                ModStats.IncrementEntry("IdiotsWhoDidntInstallThePallet");
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
        
        /// <summary>
        /// Sends the stats request for launch/user.
        /// </summary>
        public override void OnLateInitializeMelon()
        {
            var initializationThread = new Thread(new ThreadStart(async () =>
            {
                await ModStats.IncrementLaunch();
                if (!PlayerPrefs.HasKey("ParanoiaLaunch"))
                    await ModStats.IncrementUser();
                PlayerPrefs.TrySetInt("ParanoiaLaunch", 1);
            }));
            initializationThread.Start();
        }
        
#if DEBUG
        /// <summary>
        /// Debug menu for testing various things.
        /// </summary>
        private static void SetupBoneMenu()
        {
            var maincat = MenuManager.CreateCategory("Not Enough Photons", Color.white);
            var cat = maincat.CreateCategory("Paranoia", Color.grey);
            #region Entities
            var cat1 = cat.CreateCategory("Entities", Color.cyan);
            cat1.CreateFunctionElement("AudioEvent", Color.white, Entities.AudioEvent);
            cat1.CreateFunctionElement("Ceilingman", Color.grey, Entities.Ceilingman);
            cat1.CreateFunctionElement("Chaser", Color.red, Entities.Chaser);
            cat1.CreateFunctionElement("Cisco", Color.red, Entities.Cisco);
            cat1.CreateFunctionElement("Crying", Color.white, Entities.Crying);
            cat1.CreateFunctionElement("Eyes", Color.white, Entities.Eyes);
            cat1.CreateFunctionElement("Footsteps", Color.white, Entities.Footsteps);
            cat1.CreateFunctionElement("Mirage", Color.grey, Entities.Mirage);
            cat1.CreateFunctionElement("Observer", Color.black, Entities.Observer);
            cat1.CreateFunctionElement("Paralyzer", Color.grey, Entities.Paralyzer);
            cat1.CreateFunctionElement("Radio", Color.grey, Entities.Radio);
            cat1.CreateFunctionElement("Stealer", Color.grey, Entities.Stealer);
            cat1.CreateFunctionElement("FastMirage", Color.black, Entities.FastMirage);
            cat1.CreateFunctionElement("Teeth", Color.white, Entities.Teeth);
            cat1.CreateFunctionElement("Whiteface", Color.white, Entities.Whiteface, "This guy will crash the game.");
            #endregion
            #region Events
            var cat2 = cat.CreateCategory("Events", Color.blue);
            cat2.CreateFunctionElement("Crabtroll", Color.red, Crabtroll.Activate);
            cat2.CreateFunctionElement("KillAI", Color.red, KillAI.Activate);
            cat2.CreateFunctionElement("MoveAIToRadio", Color.red, () =>
            {
                var player = Player.playerHead.transform;
                var go = new GameObject
                {
                    transform =
                    {
                        position = player.position + player.forward * 5f
                    }
                };
                var location = go.transform;
                MoveAIToRadio.Activate(location);
            });
            cat2.CreateFunctionElement("LaughAtPlayer", Color.red, LaughAtPlayer.Activate);
            cat2.CreateFunctionElement("MoveAIToPlayer", Color.green, MoveAIToPlayer.Activate);
            cat2.CreateFunctionElement("FakeFireGun", Color.blue, FakeFireGun.Activate);
            cat2.CreateFunctionElement("FireGunInHand", Color.cyan, FireGunInHand.Activate);
            cat2.CreateFunctionElement("FireGun", Color.cyan, FireGun.Activate);
            cat2.CreateFunctionElement("FlickerFlashlight", Color.yellow, FlickerFlashlights.Activate);
            cat2.CreateFunctionElement("FlingRandomObject", Color.magenta, FlingRandomObject.Activate);
            cat2.CreateFunctionElement("Crash Game", Color.red, Utilities.CrashGame, "This will crash the game!");
            #endregion
        }

        private static class Entities
        {
            public static void AudioEvent()
            {
                var player = Player.playerHead.transform;
                var location = player.position + player.forward * 10f;
                HelperMethods.SpawnCrate(Pallet.Entities.AudioEvent, location, Quaternion.identity, Vector3.one, false, null);
            }
            
            public static void Ceilingman()
            {
                var player = Player.playerHead.transform;
                var location = player.position + player.forward * 10f + Vector3.up * 10f;
                HelperMethods.SpawnCrate(Pallet.Entities.Ceilingman, location, Quaternion.identity, Vector3.one, false, null);
            }

            public static void Chaser()
            {
                var player = Player.playerHead.transform;
                var location = player.position + player.forward * 20f;
                HelperMethods.SpawnCrate(Pallet.Entities.Chaser, location, Quaternion.identity, Vector3.one, false, null);
            }

            public static void Cisco()
            {
                var player = Player.playerHead.transform;
                var location = player.position + player.forward * 10f + Vector3.up * 10f;
                HelperMethods.SpawnCrate(Pallet.Entities.Cisco, location, Quaternion.identity, Vector3.one, false, null);
            }

            public static void Crying()
            {
                var player = Player.playerHead.transform;
                var location = player.position + player.forward * 10f;
                var crying = new List<string>() { Pallet.Entities.Crying1, Pallet.Entities.Crying2, Pallet.Entities.Crying3 };
                HelperMethods.SpawnCrate(crying[Random.Range(0, crying.Count)], location, Quaternion.identity, Vector3.one, false, null);
            }

            public static void Eyes()
            {
                var player = Player.playerHead.transform;
                var location = player.position + player.forward * 20f;
                HelperMethods.SpawnCrate(Pallet.Entities.Eyes, location, Quaternion.identity, Vector3.one, false, null);
            }
            
            public static void Footsteps()
            {
                var player = Player.playerHead.transform;
                var location = player.position + player.forward * 20f;
                HelperMethods.SpawnCrate(Pallet.Entities.Footsteps, location, Quaternion.identity, Vector3.one, false, null);
            }
            
            public static void Mirage()
            {
                var player = Player.playerHead.transform;
                var location = player.position;
                HelperMethods.SpawnCrate(Pallet.Entities.Mirage, location, Quaternion.identity, Vector3.one, false, null);
            }
            
            public static void Observer()
            {
                var location = Vector3.zero;
                HelperMethods.SpawnCrate(Pallet.Entities.Observer, location, Quaternion.identity, Vector3.one, false, null);
            }
            
            public static void Paralyzer()
            {
                var location = Vector3.zero;
                HelperMethods.SpawnCrate(Pallet.Entities.Paralyzer, location, Quaternion.identity, Vector3.one, false, null);
            }
            
            public static void Radio()
            {
                var player = Player.playerHead.transform;
                var location = player.position + player.forward * 10f;
                HelperMethods.SpawnCrate(Pallet.Entities.Radio, location, Quaternion.identity, Vector3.one, false, null);
            }
            
            public static void Stealer()
            {
                var player = Player.playerHead.transform;
                var location = player.position + player.forward * 20f;
                HelperMethods.SpawnCrate(Pallet.Entities.Stealer, location, Quaternion.identity, Vector3.one, false, null);
            }
            
            public static void FastMirage()
            {
                var player = Player.playerHead.transform;
                var location = player.position;
                HelperMethods.SpawnCrate(Pallet.Entities.FastMirage, location, Quaternion.identity, Vector3.one, false, null);
            }
            
            public static void Teeth()
            {
                var player = Player.playerHead.transform;
                var location = player.position + player.forward * 20f;
                HelperMethods.SpawnCrate(Pallet.Entities.Teeth, location, Quaternion.identity, Vector3.one, false, null);
            }
            
            public static void Whiteface()
            {
                var player = Player.playerHead.transform;
                var location = player.position + player.forward * 30f;
                HelperMethods.SpawnCrate(Pallet.Entities.Whiteface, location, Quaternion.identity, Vector3.one, false, null);
            }
        }
#endif
    }
}