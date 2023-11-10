using SLZ.Props;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using System.Collections;

namespace Paranoia.Events
{
    /// <summary>
    /// Flicker all flashlights in the scene.
    /// </summary>
    public static class FlickerFlashlights
    {
        public static void Activate()
        {
            PropFlashlight[] flashlights = Object.FindObjectsOfType<PropFlashlight>();

            foreach (var t in flashlights)
            {
                ModStats.IncrementEntry("LightsFlickered");
                MelonLoader.MelonCoroutines.Start(CoLightFlicker(t, Random.Range(15, 25)));
            }
        }

        private static IEnumerator CoLightFlicker(PropFlashlight light, int iterations)
        {
            for (var i = 0; i < iterations; i++)
            {
                yield return new WaitForSeconds(Random.Range(0.05f, 0.10f));
                light.SwitchLight();
            }
        }
    }
}