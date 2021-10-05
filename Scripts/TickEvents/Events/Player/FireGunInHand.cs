using StressLevelZero.Props.Weapons;

using NEP.Paranoia.Utilities;

namespace NEP.Paranoia.TickEvents.Events
{
    public class FireGunInHand : ParanoiaEvent
    {
        public override void Start()
        {
            if (Utilities.Utilities.GetGunInHand(StressLevelZero.Handedness.RIGHT) == null)
            {
                return;
            }

            Gun gun = Utilities.Utilities.GetGunInHand(StressLevelZero.Handedness.RIGHT);

            gun?.Fire();
        }
    }
}
