using BoneLib;
using HarmonyLib;
using Paranoia.Helpers;
using Paranoia.Managers;
using SLZ.Marrow.Warehouse;
using SLZ.SFX;
using UnityEngine;

namespace Paranoia.Internal
{
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
                if (!enabled) return;
                switch (Paranoia.levelTitle)
                {
                    case "Baseline":
                        ModConsole.Msg("Baseline detected.");
                        SetupBaseline();
                        SpecCamSetup();
                        break;
                    case "Museum Basement":
                        ModConsole.Msg("Museum Basement detected.", LoggingMode.DEBUG);
                        SetupMuseum();
                        SpecCamSetup();
                        break;
                    case "Paranoia":
                        ModConsole.Msg("Paranoia detected.", LoggingMode.DEBUG);
                        SpecCamSetup();
                        break;
                    default:
                        ModConsole.Msg("Not in Baseline or Museum Basement.", LoggingMode.DEBUG);
                        ModConsole.Msg("Checking for ParanoiaManager...", LoggingMode.DEBUG);
                        if (Object.FindObjectsOfType<ParanoiaManager>() != null)
                        {
                            ModConsole.Msg("ParanoiaManager found, spec cam setup", LoggingMode.DEBUG);
                            SpecCamSetup();
                        }
                        else
                        {
                            ModConsole.Msg("ParanoiaManager not found, avoiding spectator cam setup", LoggingMode.DEBUG);
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
            LayerMask waterLayer = LayerMask.NameToLayer("Water");
            Player.playerHead.GetComponent<Camera>().cullingMask &= ~waterLayer;
            if (Paranoia.hasMonodirector)
            {
                ModConsole.Msg("Monodirector exists.");
                var camera = GameObject.Find("Spectator Camera").GetComponent<Camera>();
                camera.cullingMask &= ~waterLayer;
            }
            else if (Paranoia.levelTitle == "Museum Basement" || Paranoia.levelTitle == "Baseline")
            {
                var camera = GameObject.Find("[RigManager (Blank)]/Spectator Camera").GetComponent<Camera>();
                camera.cullingMask &= ~waterLayer;
            }
            else
            {
                var camera = GameObject.Find("Default Player Rig [0]/[RigManager (Blank)]/Spectator Camera").GetComponent<Camera>();
                camera.cullingMask &= ~waterLayer;
            }
        }
        
        /// <summary>
        /// Sets up the BaselineManager and sets the lights field.
        /// </summary>
        private static void SetupBaseline()
        {
            // Get a crateref for the manager
            var managerCrate = new SpawnableCrateReference("NotEnoughPhotons.Paranoia.Spawnable.BaselineParanoia");
            // Get baseline's lights
            var zoneMusic = Object.FindObjectOfType<ZoneMusic>();
            // Spawn and setup
            ModConsole.Msg("Spawning BaselineParanoia.", LoggingMode.DEBUG);
            Warehouse.Spawn(managerCrate, Vector3.zero, Quaternion.identity, false, go =>
            {
                var manager = go.GetComponent<BaselineManager>();
                if (manager == null) return;
                ModConsole.Msg($"Got manager: {manager}", LoggingMode.DEBUG);
                manager.zoneMusic = zoneMusic;
                ModConsole.Msg("Set zone music field", LoggingMode.DEBUG);
                manager.Enable();
                ModConsole.Msg("Have fun :)");
            });
        }
        
        /// <summary>
        /// Sets up the MuseumManager and sets the lights field.
        /// </summary>
        private static void SetupMuseum()
        {
            // Get a crateref for the manager
            var managerCrate = new SpawnableCrateReference("NotEnoughPhotons.Paranoia.Spawnable.MuseumParanoia");
            ModConsole.Msg($"Got crate: {managerCrate}", LoggingMode.DEBUG);
            // Get museum basement's sign
            var sign = GameObject.Find("holographic_sign_SandboxMuseum").GetComponent<MeshRenderer>();
            ModConsole.Msg($"Got sign: {sign}", LoggingMode.DEBUG);
            // Spawn and setup
            ModConsole.Msg("Spawning MuseumParanoia.", LoggingMode.DEBUG);
            var location = new Vector3(-20, 0, 20);
            Warehouse.Spawn(managerCrate, location, Quaternion.identity, false, go =>
            {
                var manager = go.GetComponent<MuseumManager>();
                if (manager == null) return;
                ModConsole.Msg($"Got manager: {manager}", LoggingMode.DEBUG);
                manager.signMesh = sign;
                ModConsole.Msg("Added sign to field.", LoggingMode.DEBUG);
                manager.Enable();
                ModConsole.Msg("Have fun :)");
            });
        }
    }
}