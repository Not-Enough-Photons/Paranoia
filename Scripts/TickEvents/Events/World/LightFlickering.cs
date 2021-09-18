using StressLevelZero.Rig;
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

            MelonLoader.MelonCoroutines.Start(CoLightFlicker());
        }

        public override void Stop()
        {
            base.Stop();
        }

        private System.Collections.IEnumerator CoLightFlicker()
        {
            GameObject[] grids;
            PropFlashlight[] flashlights;
            GameObject staticEnv;

            yield return null;
        }
    }
}
