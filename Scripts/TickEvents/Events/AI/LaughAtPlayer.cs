using System.Collections;
using NEP.Paranoia.ParanoiaUtilities;
using PuppetMasta;
using StressLevelZero.AI;
using UnityEngine;

namespace NEP.Paranoia.TickEvents.Events
{
    public class LaughAtPlayer : ParanoiaEvent
    {
        public override void Start()
        {
            UnhollowerBaseLib.Il2CppArrayBase<AIBrain> brains = Utilities.FindAIBrains();

            foreach(AIBrain brain in brains)
            {
                if(brain == null)
                {
                    continue;
                }

                BehaviourPowerLegs powerLegs = brain?.behaviour.TryCast<BehaviourPowerLegs>();

                if(!powerLegs)
                {
                    return;
                }

                powerLegs?.faceAnim?.Attack1(Random.Range(1, 3));
            }
        }
    }
}
