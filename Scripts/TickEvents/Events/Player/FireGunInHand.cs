using StressLevelZero.Props.Weapons;

using NEP.Paranoia.Utilities;

namespace NEP.Paranoia.TickEvents.Events
{
    public class FireGunInHand : ParanoiaEvent
    {
        public override void Start()
        {
            if (ParanoiaUtilities.GetGunInHand(StressLevelZero.Handedness.RIGHT) == null)
            {
                return;
            }

            Gun gun = ParanoiaUtilities.GetGunInHand(StressLevelZero.Handedness.RIGHT);

            gun?.Fire();
        }
    }
}
