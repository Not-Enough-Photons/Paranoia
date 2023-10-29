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
        [HarmonyPatch(typeof(Player_Health), "MakeVignette")]
        public static class VignettePatch
        {
            public static void Postfix(Player_Health instance)
            {
                if (Preferences.baselineSchizophrenia == false)
                {
                    ModConsole.Msg("Baseline activation is disabled.", LoggingMode.DEBUG);
                    return;
                }
                if (Main.levelTitle == "Baseline")
                {
                    ModConsole.Msg("Baseline detected.", LoggingMode.DEBUG);
                    SetupBaseline();
                }
            }
        }
        
        private static void SetupBaseline()
        {
            // Get a crateref for the manager
            var managerCrate = new SpawnableCrateReference("NotEnoughPhotons.Paranoia.Spawnable.BaselineParanoia");
            // Get baseline's lights
            var spotLightWhite = GameObject.Find("//-----LIGHTING/spotLight_white").GetComponent<Light>();
            ModConsole.Msg($"Spotlight white: {spotLightWhite}", LoggingMode.DEBUG);
            var spotLightRed = GameObject.Find("//-----LIGHTING/spotLight_red").GetComponent<Light>();
            ModConsole.Msg($"Spotlight red: {spotLightRed}", LoggingMode.DEBUG);
            var spotLightBlue = GameObject.Find("//-----LIGHTING/spotLight_blue").GetComponent<Light>();
            ModConsole.Msg($"Spotlight blue: {spotLightBlue}", LoggingMode.DEBUG);
            var spotLightBigSoft = GameObject.Find("//-----LIGHTING/spotLight_bigSoft").GetComponent<Light>();
            ModConsole.Msg($"Spotlight big soft: {spotLightBigSoft}", LoggingMode.DEBUG);
            // Spawn and setup
            ModConsole.Msg("Spawning BaselineParanoia.", LoggingMode.DEBUG);
            Warehouse.Spawn(managerCrate, Vector3.zero, Quaternion.identity, false, go =>
            {
                var manager = go.GetComponent<ParanoiaManager>();
                if (manager == null) return;
                ModConsole.Msg($"Got manager: {manager}", LoggingMode.DEBUG);
                manager._lights.Add(spotLightWhite);
                ModConsole.Msg($"Added spotlight white: {spotLightWhite}", LoggingMode.DEBUG);
                manager._lights.Add(spotLightRed);
                ModConsole.Msg($"Added spotlight red: {spotLightRed}", LoggingMode.DEBUG);
                manager._lights.Add(spotLightBlue);
                ModConsole.Msg($"Added spotlight blue: {spotLightBlue}", LoggingMode.DEBUG);
                manager._lights.Add(spotLightBigSoft);
                ModConsole.Msg($"Added spotlight big soft: {spotLightBigSoft}", LoggingMode.DEBUG);
                manager.AddLightsToArray();
                ModConsole.Msg("Added lights to array.", LoggingMode.DEBUG);
            });
        }
    }
}