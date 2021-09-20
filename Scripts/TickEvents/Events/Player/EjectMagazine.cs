using UnityEngine;
using NEP.Paranoia.Utilities;
using StressLevelZero.Props.Weapons;
using StressLevelZero.Interaction;

namespace NEP.Paranoia.TickEvents.Events
{
    public class EjectMagazine : ParanoiaEvent
    {
        public override void Start()
        {
            Gun gun = ParanoiaUtilities.GetGunInHand(StressLevelZero.Handedness.BOTH);
            MagazineSocket socket = gun.magazineSocket; 

            if(gun == null) { return; }
            if(socket == null) { return; }

            socket.EjectMagazine();
        }
    }
}
