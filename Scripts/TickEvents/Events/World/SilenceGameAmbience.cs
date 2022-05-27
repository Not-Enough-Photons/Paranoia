using NEP.Paranoia.TickEvents;
using ModThatIsNotMod;

using UnityEngine;

using StressLevelZero.Rig;
using StressLevelZero.SFX;

namespace NEP.Paranoia.TickEvents.Events
{
    public class SilenceGameAmbience : ParanoiaEvent
    {
        public override void Start()
        {
            GameObject rig = Player.GetRigManager();
            RigManager rigManager = rig.GetComponent<RigManager>();
            PhysicsRig physRig = rigManager.physicsRig;

            physRig.m_head.Find("AmbienceAndMusic").gameObject.SetActive(false);
        }
    }
}