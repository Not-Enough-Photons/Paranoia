using UnityEngine;
using NEP.Paranoia.ParanoiaUtilities;
using StressLevelZero.AI;
using PuppetMasta;

namespace NEP.Paranoia.TickEvents.Events
{
    public class KillAI : ParanoiaEvent
    {
        public override void Start()
        {
            AIBrain[] brains = Utilities.FindAIBrains();

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
