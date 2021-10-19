using System.Collections;

using NEP.Paranoia.Managers;
using NEP.Paranoia.ParanoiaUtilities;
using UnityEngine;

namespace NEP.Paranoia.TickEvents.Events
{
    public class LightFlickering : ParanoiaEvent
    {
        public override void Start()
        {
            new DisableNimbus().Start();
            new DisableWasp().Start();

            new FlickerFlashlights().Start();

            GameObject mainLight = MapUtilities.mainLight;

            if(mainLight == null) { return; }

            MelonLoader.MelonCoroutines.Start(CoLightFlicker(mainLight, Random.Range(30, 45)));
        }

        private IEnumerator CoLightFlicker(GameObject mainLight, int iterations)
        {
            for(int i = 0; i < iterations; i++)
            {
                yield return new WaitForSeconds(Random.Range(0.05f, 0.15f));
                mainLight.SetActive(i % 2 == 0);
            }
        }
    }
}
