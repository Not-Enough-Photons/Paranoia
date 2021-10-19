using UnityEngine;

namespace NEP.Paranoia.TickEvents.Events
{
    public class FlickerFlashlights : ParanoiaEvent
    {
        public override void Start()
        {
            PropFlashlight[] flashlights = Object.FindObjectsOfType<PropFlashlight>();

            if(flashlights.Length == -1) { return; }

            for(int i = 0; i < flashlights.Length; i++)
            {
                MelonLoader.MelonCoroutines.Start(CoLightFlicker(flashlights[i], Random.Range(15, 25)));
            }
        }

        private System.Collections.IEnumerator CoLightFlicker(PropFlashlight light, int iterations)
        {
            for (int i = 0; i < iterations; i++)
            {
                yield return new WaitForSeconds(Random.Range(0.05f, 0.10f));
                light.SwitchLight();
            }
        }
    }
}
