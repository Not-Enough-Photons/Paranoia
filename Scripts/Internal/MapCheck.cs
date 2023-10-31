using BoneLib;
using HarmonyLib;
using Paranoia.Helpers;
using Paranoia.Managers;
using SLZ.Bonelab;
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
                    case "Blank Box":
                        ModConsole.Msg("Blank Box detected.", LoggingMode.DEBUG);
                        SpecCamSetup();
                        SetupBlankbox();
                        break;
                    default:
                        ModConsole.Msg("Not in Baseline or Museum Basement.", LoggingMode.DEBUG);
                        ModConsole.Msg("Checking for ParanoiaManager...", LoggingMode.DEBUG);
                        var manager = Object.FindObjectOfType<ParanoiaManager>();
                        if (manager != null)
                        {
                            ModConsole.Msg($"ParanoiaManager found: {manager}", LoggingMode.DEBUG);
                            SpecCamSetup();
                        }
                        else
                        {
                            ModConsole.Msg("Manager not found.");
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
            var cameras = Object.FindObjectsOfType<SmoothFollower>();
            foreach (var obj in cameras)
            {
                obj.gameObject.GetComponent<Camera>().cullingMask &= ~waterLayer;
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
            HelperMethods.SpawnCrate(managerCrate, Vector3.zero, Quaternion.identity, Vector3.one, false, go =>
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
            HelperMethods.SpawnCrate(managerCrate, location, Quaternion.identity, Vector3.one, false, go =>
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
        
        private static void SetupBlankbox()
        {
            // Get a crateref for the manager
            var managerCrate = new SpawnableCrateReference("NotEnoughPhotons.Paranoia.Spawnable.BlankboxParanoia");
            // Get baseline's lights
            var lights = GameObject.Find("Lighting/REALTIMELIGHT").GetComponents<Light>();
            // Spawn and setup
            ModConsole.Msg("Spawning BlankboxParanoia.", LoggingMode.DEBUG);
            HelperMethods.SpawnCrate(managerCrate, Vector3.zero, Quaternion.identity, Vector3.one, false, go =>
            {
                var manager = go.GetComponent<ParanoiaManager>();
                if (manager == null) return;
                ModConsole.Msg($"Got manager: {manager}", LoggingMode.DEBUG);
                manager.AddLights(lights);
                manager.Enable();
                ModConsole.Msg("Have fun :)");
            });
        }
    }
}