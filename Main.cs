﻿namespace NEP.Paranoia;

/// <summary>
/// <see cref="MelonMod"/>
/// </summary>
public class Main : MelonMod
{
    internal const string Name = "Paranoia";
    internal const string Description = "There's something hostile out there.";
    internal const string Author = "Not Enough Photons, adamdev, SoulWithMae";
    internal const string Company = "Not Enough Photons";
    internal const string Version = "0.1.0";
    internal const string DownloadLink = "null";
    
    internal static Assembly CurrAsm => Assembly.GetExecutingAssembly();
        
    /// <summary>
    /// Sets up the logger, preferences, fieldinjection, and hooks.
    /// <br/>If you're using a debug build, also sets up BoneMenu.
    /// </summary>
    public override void OnInitializeMelon()
    {
        ModConsole.Setup(LoggerInstance);
        Preferences.Setup();
        SetupBoneMenu();
        Event.Initialize();
#if DEBUG
        var recording = Utilities.CheckIfRecording();
        ModConsole.Msg($"Recording software running = {recording}", LoggingMode.Debug);
#endif
        MapCheck.enabled = Preferences.EnabledInBaseGameMaps.Value;
        ModConsole.Msg("THIS PERSON IS USING PARANOIA. THERE IS AN EVENT THAT CRASHES THE GAME. THIS LOG MAY BE VOID, CHECK LATER IN THE LOG FOR A SIMILAR WARNING TO CONFIRM");
        FieldInjection.Inject();
        Hooking.OnLevelInitialized += OnLevelLoaded;
#if DEBUG
        ModConsole.Msg("THE DEBUG BUILD OF PARANOIA IS BEING USED. THIS IS NOT RECOMMENDED FOR NORMAL USE.");
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
        EntityLoader.CheckEntities();
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
        ModConsole.Msg($"Level loaded: {levelInfo.title}", LoggingMode.Debug);
        levelBarcode = levelInfo.barcode;
    }
        
    /// <summary>
    /// Sends the stats request for launch/user.
    /// </summary>
    public override void OnLateInitializeMelon()
    {
        if (!PlayerPrefs.HasKey("ParanoiaLaunch"))
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            ModStats.IncrementLaunch();
            PlayerPrefs.TrySetInt("ParanoiaLaunch", 1);
        }
        ModStats.IncrementUser();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
    }
    
    /// <summary>
    /// Bonemenu options, some are debug only, some will be in the release build.
    /// </summary>
    private static void SetupBoneMenu()
    {
        var maincat = MenuManager.CreateCategory("Not Enough Photons", Color.white);
        var cat = maincat.CreateCategory("Paranoia", Color.grey);
        cat.CreateBoolElement("Base Game Map Activation", Color.white, Preferences.EnabledInBaseGameMaps.Value, OnBoolUpdate);
#if DEBUG
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
#endif
    }

    private static void OnBoolUpdate(bool value)
    {
        MapCheck.enabled = value;
        Preferences.EnabledInBaseGameMaps.Value = value;
        Preferences.Category.SaveToFile(false);
    }
#if DEBUG
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