using UnityEngine;

namespace NEP.Paranoia.TickEvents.Events
{
    public class DisableNimbus : ParanoiaEvent
    {
        public override void Start()
        {
            if (GameObject.FindObjectOfType<StressLevelZero.Props.Weapons.FlyingGun>())
            {
                StressLevelZero.Props.Weapons.FlyingGun nimbus = GameObject.FindObjectOfType<StressLevelZero.Props.Weapons.FlyingGun>();

                nimbus.triggerGrip.ForceDetach();
            }
        }
    }
}
