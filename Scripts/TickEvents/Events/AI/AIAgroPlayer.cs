using NEP.Paranoia.Managers;
using NEP.Paranoia.Utilities;

using StressLevelZero.AI;
using PuppetMasta;

using UnityEngine;

namespace NEP.Paranoia.TickEvents.Events
{
    public class AIAgroPlayer : ParanoiaEvent
    {
        public override void Start()
        {
            AIBrain[] brains = ParanoiaUtilities.FindAIBrains();
            TriggerRefProxy playerProxy = ParanoiaUtilities.GetPlayerProxy();

            if(brains == null) { return; }

            AIBrain brain = brains[Random.Range(0, brains.Length)];

            if(brain == null) { return; }

            BehaviourBaseNav behaviour = brain.behaviour;

            behaviour.AddThreat(playerProxy, 100f);
        }
    }
}
