using UnityEngine;
using NEP.Paranoia.Utilities;
using StressLevelZero.Props.Weapons;

namespace NEP.Paranoia.TickEvents.Events
{
    public class LockGun : ParanoiaEvent
    {
        public override void Start()
        {
            if(ParanoiaUtilities.GetGunInHand(StressLevelZero.Handedness.RIGHT) == null)
            {
                return;
            }

            Gun gun = ParanoiaUtilities.GetGunInHand(StressLevelZero.Handedness.RIGHT);

            gun?.magazineSocket.ClearMagazine();
        }
    }
}
