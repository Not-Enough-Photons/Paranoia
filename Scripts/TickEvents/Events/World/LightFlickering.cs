using System.Collections;

using NEP.Paranoia.Managers;
using NEP.Paranoia.Utilities;
using UnityEngine;

namespace NEP.Paranoia.TickEvents.Events
{
    public class LightFlickering : ParanoiaEvent
    {
        public override void Start()
        {
            new DisableNimbus().Start();
            new DisableWasp().Start();
        }

        private IEnumerator CoLightFlicker(int iterations)
        {
            VLB.VolumetricLightBeam vlb = 

            for(int i = 0; i < iterations; i++)
            {
                yield return new WaitForSeconds(Random.Range(1f, 10f));
            }
        }
    }
}
