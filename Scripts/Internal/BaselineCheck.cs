using HarmonyLib;
using Paranoia.Helpers;
using Paranoia.Managers;
using SLZ.Marrow.Warehouse;
using UnityEngine;

namespace Paranoia.Internal
{
    /// <summary>
    /// Checks if the player is in the Baseline level, then sets up what's needed.
    /// </summary>
    public static class BaselineCheck
    {
        public static bool enabled;
        
        [HarmonyPatch(typeof(Player_Health), "MakeVignette")]
        public static class VignettePatch
        {
            public static void Postfix(Player_Health __instance)
            {
                if (!enabled) return;
                SetupBaseline();
            }
        }
        
        private static void SetupBaseline()
        {
            if (Main.levelTitle == "Baseline")
            {
                ModConsole.Msg("Baseline detected.", LoggingMode.DEBUG);
            }
            else
            {
                return;
            }
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
    }
}