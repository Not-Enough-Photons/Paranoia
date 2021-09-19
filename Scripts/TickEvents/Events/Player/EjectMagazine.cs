using UnityEngine;
using NEP.Paranoia.Utilities;
using StressLevelZero.Props.Weapons;

namespace NEP.Paranoia.TickEvents.Events
{
    public class EjectMagazine : ParanoiaEvent
    {
        public override void Start()
        {
            Gun gun = ParanoiaUtilities.GetGunInHand(StressLevelZero.Handedness.RIGHT);

            if(gun == null) { return; }

            gun?.magazineSocket.EjectMagazine();
        }
    }
}
