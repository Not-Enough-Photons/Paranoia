using SLZ.Props;
using UnityEngine;

namespace NEP.Paranoia.TickEvents.Events
{
    public class DisableNimbus : ParanoiaEvent
    {
        public override void Start()
        {
            if (GameObject.FindObjectOfType<FlyingGun>())
            {
                FlyingGun nimbus = GameObject.FindObjectOfType<FlyingGun>();

                nimbus.triggerGrip.ForceDetach();
            }
        }
    }
}
