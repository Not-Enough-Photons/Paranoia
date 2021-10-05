using UnityEngine;
using NEP.Paranoia.Utilities;
using StressLevelZero.AI;
using PuppetMasta;

namespace NEP.Paranoia.TickEvents.Events
{
    public class KillAI : ParanoiaEvent
    {
        public override void Start()
        {
            AIBrain[] brains = Utilities.Utilities.FindAIBrains();

            if(brains == null) { return; }

            foreach(AIBrain brain in brains)
            {
                PuppetMaster puppetMaster = brain.puppetMaster;
                SubBehaviourHealth health = brain.behaviour.health;

                health.Kill();
                puppetMaster.Kill();
            }
        }
    }
}
