using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Paranoia.Events
{
    /// <summary>
    /// Flickers the given lights by toggling the gameobject over and over.
    /// <br/>Parent your lights to the gameobject given to this script.
    /// </summary>
    public static class LightFlicker
    {
        public static void Activate(GameObject lights)
        {
            if(lights == null) { return; }
            FlickerFlashlights.Activate();
            MelonLoader.MelonCoroutines.Start(CoLightFlicker(lights, Random.Range(30, 45)));
        }

        private static IEnumerator CoLightFlicker(GameObject lights, int iterations)
        {
            for(var i = 0; i < iterations; i++)
            {
                yield return new WaitForSeconds(Random.Range(0.05f, 0.15f));
                lights.SetActive(i % 2 == 0);
            }
        }
    }
}