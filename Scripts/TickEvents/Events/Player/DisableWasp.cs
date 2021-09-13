using UnityEngine;

namespace NEP.Paranoia.TickEvents.Events
{
    public class DisableWasp : ParanoiaEvent
    {
        public override void Start()
        {
            if (GameObject.FindObjectOfType<PuppetMasta.BehaviourHovercraft>())
            {
                PuppetMasta.BehaviourHovercraft wasp = GameObject.FindObjectOfType<PuppetMasta.BehaviourHovercraft>();
                wasp.KillStart();
            }
        }
    }
}
