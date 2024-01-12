namespace NEP.Paranoia.Internal;

/// <summary>
/// Checks if the player is in the Baseline or Museum Basement level, then sets up what's needed.
/// </summary>
public static class MapCheck
{
    public static bool enabled;
        
    /// <summary>
    /// Ensures that the RigManager is fully initialised before doing anything.
    /// </summary>
    [HarmonyPatch(typeof(Player_Health), "MakeVignette")]
    public static class VignettePatch
    {
        public static void Postfix(Player_Health __instance)
        {
            if (!AssetWarehouse.Instance.HasPallet(Pallet.Barcode))
            {
                var notif = new Notification()
                {
                    Title = "Missing Pallet!",
                    Message = "You are missing the Paranoia pallet! Paranoia will not be functional!",
                    Type = NotificationType.Error,
                    PopupLength = 5f,
                    ShowTitleOnPopup = true
                };
                Notifier.Send(notif);
            }
            if (!enabled) return;
            switch (Paranoia.levelBarcode)
            {
                case CommonBarcodes.Maps.Baseline:
                    ModConsole.Msg("Baseline detected.", LoggingMode.Debug);
                    SetupBaseline();
                    SpecCamSetup();
                    break;
                case CommonBarcodes.Maps.MuseumBasement:
                    ModConsole.Msg("Museum Basement detected.", LoggingMode.Debug);
                    SetupMuseum();
                    SpecCamSetup();
                    break;
                case Pallet.Maps.Paranoia:
                    ModConsole.Msg("Paranoia detected.", LoggingMode.Debug);
                    SpecCamSetup();
                    break;
                case Pallet.Maps.OldParanoia:
                    ModConsole.Msg("Paranoia detected.", LoggingMode.Debug);
                    SpecCamSetup();
                    break;
                case "Atlas.96.BlankBox.Level.BlankBox":
                    ModConsole.Msg("Blank Box detected.", LoggingMode.Debug);
                    SpecCamSetup();
                    SetupBlankbox();
                    break;
                default:
                    ModConsole.Msg("Not in Baseline or Museum Basement.", LoggingMode.Debug);
                    ModConsole.Msg("Checking for ParanoiaManager...", LoggingMode.Debug);
                    var manager = ParanoiaManager.Instance;
                    if (manager != null)
                    {
                        ModConsole.Msg($"ParanoiaManager found: {manager}", LoggingMode.Debug);
                        SpecCamSetup();
                    }
                    else
                    {
                        ModConsole.Msg("Manager not found.", LoggingMode.Debug);
                    }
                    break;
            }
        }
    }
        
    /// <summary>
    /// Allows for entities that the player can see but people watching can't.
    /// </summary>
    private static void SpecCamSetup()
    {
        var cameras = Resources.FindObjectsOfTypeAll<SmoothFollower>();
        ModConsole.Msg($"Got cameras: {cameras}", LoggingMode.Debug);
        foreach (var obj in cameras)
        {
            var camera = obj.gameObject.GetComponent<Camera>();
            if (camera != null)
            {
                ModConsole.Msg($"Got camera: {camera}", LoggingMode.Debug);
                CameraHelper.LayerCullingHide(camera, "Water");
            }
        }
    }
        
    /// <summary>
    /// Sets up the BaselineManager and sets the lights field.
    /// </summary>
    private static void SetupBaseline()
    {
        // Get baseline's lights
        var zoneMusic = Object.FindObjectOfType<ZoneMusic>();
        // Spawn and setup
        ModConsole.Msg("Spawning BaselineParanoia.", LoggingMode.Debug);
        HelperMethods.SpawnCrate(Preferences.BaselineManager.Value, Vector3.zero, Quaternion.identity, Vector3.one, false, go =>
        {
            var manager = ParanoiaManager.Instance;
            if (manager == null) return;
            ModConsole.Msg($"Got manager: {manager}", LoggingMode.Debug);
            manager.extraSettings.zoneMusic = zoneMusic;
            ModConsole.Msg("Set zone music field", LoggingMode.Debug);
            manager.Enable();
            ModConsole.Msg("Have fun :)");
        });
    }
        
    /// <summary>
    /// Sets up the MuseumManager and sets the lights field.
    /// </summary>
    private static void SetupMuseum()
    {
        // Get museum basement's sign
        var sign = GameObject.Find("holographic_sign_SandboxMuseum").GetComponent<MeshRenderer>();
        ModConsole.Msg($"Got sign: {sign}", LoggingMode.Debug);
        // Get zone music
        var zoneMusic = Object.FindObjectOfType<ZoneMusic>();
        ModConsole.Msg($"Got zone music: {zoneMusic}", LoggingMode.Debug);
        // Spawn and setup
        ModConsole.Msg("Spawning MuseumParanoia.", LoggingMode.Debug);
        var location = new Vector3(-20, 0, 20);
        HelperMethods.SpawnCrate(Preferences.MuseumManager.Value, location, Quaternion.identity, Vector3.one, false, go =>
        {
            var manager = ParanoiaManager.Instance;
            if (manager == null) return;
            ModConsole.Msg($"Got manager: {manager}", LoggingMode.Debug);
            manager.extraSettings.signMesh = sign;
            ModConsole.Msg("Added sign to field.", LoggingMode.Debug);
            manager.extraSettings.zoneMusic = zoneMusic;
            ModConsole.Msg("Added zone music to field.", LoggingMode.Debug);
            manager.Enable();
            ModConsole.Msg("Have fun :)");
        });
    }
    
    /// <summary>
    /// Setup the BlankboxManager and sets the lights field.
    /// </summary>
    private static void SetupBlankbox()
    {
        // Get baseline's lights
        var lights = GameObject.Find("Lighting/REALTIMELIGHT").GetComponents<Light>();
        // Spawn and setup
        ModConsole.Msg("Spawning BlankboxParanoia.", LoggingMode.Debug);
        HelperMethods.SpawnCrate(Preferences.BlankboxManager.Value, Vector3.zero, Quaternion.identity, Vector3.one, false, go =>
        {
            var manager = ParanoiaManager.Instance;
            if (manager == null) return;
            ModConsole.Msg($"Got manager: {manager}", LoggingMode.Debug);
            manager.eventSettings.lights = lights;
            manager.Enable();
            ModConsole.Msg("Have fun :)");
        });
    }
}