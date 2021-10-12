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

            MelonLoader.MelonCoroutines.Start(CoLightFlicker(Random.Range(7, 10)));
        }

        private IEnumerator CoLightFlicker(int iterations)
        {
            GameObject mainLight = MapUtilities.mainLight;

            if(mainLight == null) { yield break; }

            for(int i = 0; i < iterations; i++)
            {
                yield return new WaitForSeconds(Random.Range(0.25f, 0.75f));
                mainLight.SetActive(i % iterations == 0);
            }
        }
    }
}
