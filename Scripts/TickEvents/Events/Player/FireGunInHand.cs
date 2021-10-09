using StressLevelZero.Props.Weapons;

using NEP.Paranoia.ParanoiaUtilities;

namespace NEP.Paranoia.TickEvents.Events
{
    public class FireGunInHand : ParanoiaEvent
    {
        public override void Start()
        {
            if (ParanoiaUtilities.Utilities.GetGunInHand(StressLevelZero.Handedness.RIGHT) == null)
            {
                return;
            }

            Gun gun = ParanoiaUtilities.Utilities.GetGunInHand(StressLevelZero.Handedness.RIGHT);

            gun?.Fire();
        }
    }
}
