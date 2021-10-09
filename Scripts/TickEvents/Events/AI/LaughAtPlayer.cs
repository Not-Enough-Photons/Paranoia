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
            AIBrain[] brains = Utilities.FindAIBrains();

            if(brains.Length == 0) { return; }

            foreach(AIBrain brain in brains)
            {
                BehaviourPowerLegs powerLegs = brain.behaviour.GetComponent<BehaviourPowerLegs>();

                if(powerLegs == null) { return; }

                MelonLoader.MelonCoroutines.Start(CoLaughRoutine(powerLegs));
            }
        }

        private IEnumerator CoLaughRoutine(BehaviourPowerLegs powerLegs)
        {
            for(int i = 0; i < Random.Range(1, 15); i++)
            {
                yield return new WaitForSeconds(1f);
                powerLegs.faceAnim.Attack1(Random.Range(0, 3), 1);
            }
        }
    }
}
