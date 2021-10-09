using UnityEngine;
using NEP.Paranoia.ParanoiaUtilities;
using StressLevelZero.Props.Weapons;
using StressLevelZero.Interaction;

namespace NEP.Paranoia.TickEvents.Events
{
    public class EjectMagazine : ParanoiaEvent
    {
        public override void Start()
        {
            Gun gun = ParanoiaUtilities.Utilities.GetGunInHand(StressLevelZero.Handedness.BOTH);

            if(gun == null) { return; }

            MagazineSocket socket = gun.magazineSocket;

            if(socket == null) { return; }

            socket.EjectMagazine();
        }
    }
}
