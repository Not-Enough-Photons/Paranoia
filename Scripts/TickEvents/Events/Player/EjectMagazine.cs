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
            Gun gun = Utilities.Utilities.GetGunInHand(StressLevelZero.Handedness.BOTH);

            if(gun == null) { return; }

            MagazineSocket socket = gun.magazineSocket;

            if(socket == null) { return; }

            socket.EjectMagazine();
        }
    }
}
