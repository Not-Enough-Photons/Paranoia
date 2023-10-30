using HarmonyLib;
using Paranoia.Helpers;
using Paranoia.Managers;
using SLZ.Marrow.Warehouse;
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
                        break;
                    case "Museum Basement":
                        ModConsole.Msg("Museum Basement detected.", LoggingMode.DEBUG);
                        SetupMuseum();
                        break;
                    default:
                        ModConsole.Msg("Not in Baseline or museum basement.", LoggingMode.DEBUG);
                        break;
                }
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
            var lights = Object.FindObjectsOfType<Light>();
            // Spawn and setup
            ModConsole.Msg("Spawning BaselineParanoia.", LoggingMode.DEBUG);
            Warehouse.Spawn(managerCrate, Vector3.zero, Quaternion.identity, false, go =>
            {
                var manager = go.GetComponent<ParanoiaManager>();
                if (manager == null) return;
                ModConsole.Msg($"Got manager: {manager}", LoggingMode.DEBUG);
                foreach (var light in lights) manager._lights.Add(light);
                manager.AddLightsToArray();
                ModConsole.Msg("Added lights to array.", LoggingMode.DEBUG);
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
            // Get museum basement's sign
            var sign = GameObject.Find("//-----ENVIRONMENT/WORLDSHELL/Decals/holographic_sign_SandboxMuseum").GetComponent<MeshRenderer>();
            // Spawn and setup
            ModConsole.Msg("Spawning MuseumParanoia.", LoggingMode.DEBUG);
            var location = new Vector3(-20, 0, 20);
            Warehouse.Spawn(managerCrate, location, Quaternion.identity, false, go =>
            {
                var manager = go.GetComponent<MuseumManager>();
                if (manager == null) return;
                ModConsole.Msg($"Got manager: {manager}", LoggingMode.DEBUG);
                manager.signMesh = sign;
                ModConsole.Msg("Added sign to array.", LoggingMode.DEBUG);
                manager.Enable();
                ModConsole.Msg("Have fun :)");
            });
        }
    }
}