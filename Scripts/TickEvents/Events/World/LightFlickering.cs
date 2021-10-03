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
        }

        private void DisableGeneralLighting()
        {

        }
    }
}
