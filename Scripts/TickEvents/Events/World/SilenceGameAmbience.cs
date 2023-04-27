using NEP.Paranoia.TickEvents;

using UnityEngine;

using SLZ.Rig;
using SLZ.SFX;

namespace NEP.Paranoia.TickEvents.Events
{
    public class SilenceGameAmbience : ParanoiaEvent
    {
        public override void Start()
        {
            GameObject rig = BoneLib.Player.rigManager.gameObject;
            RigManager rigManager = rig.GetComponent<RigManager>();
            PhysicsRig physRig = rigManager.physicsRig;

            physRig.m_head.Find("AmbienceAndMusic").gameObject.SetActive(false);
        }
    }
}