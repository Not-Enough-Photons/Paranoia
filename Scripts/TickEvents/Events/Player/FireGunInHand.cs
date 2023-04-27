using SLZ.Props.Weapons;

using NEP.Paranoia.ParanoiaUtilities;

namespace NEP.Paranoia.TickEvents.Events
{
    public class FireGunInHand : ParanoiaEvent
    {
        public override void Start()
        {
            if (Utilities.GetGunInHand(SLZ.Handedness.RIGHT) == null)
            {
                return;
            }

            Gun gun = Utilities.GetGunInHand(SLZ.Handedness.RIGHT);

            gun?.Fire();
        }
    }
}
