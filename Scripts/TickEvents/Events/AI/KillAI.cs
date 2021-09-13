using UnityEngine;
using UnhollowerBaseLib;
using StressLevelZero.AI;
using PuppetMasta;

namespace NotEnoughPhotons.Paranoia.TickEvents.Events
{
    public class KillAI : ParanoiaEvent
    {
        public override void Start()
        {
            try
            {
                if (GameObject.FindObjectsOfType<StressLevelZero.AI.AIBrain>() != null)
                {
                    Il2CppArrayBase<AIBrain> brains = GameObject.FindObjectsOfType<AIBrain>();

                    foreach (AIBrain brain in brains)
                    {
                        Transform ai = brain.transform;
                        if (ai.GetComponentInParent<AIBrain>() != null)
                        {
                            AIBrain parent = ai.GetComponentInParent<AIBrain>();

                            PuppetMaster puppetMaster = parent.GetComponentInChildren<PuppetMaster>();
                            SubBehaviourHealth hp = parent.GetComponentInChildren<BehaviourBaseNav>().health;
                            hp.Kill();
                            puppetMaster.Kill();
                        }
                    }
                }
            }
            catch
            {

            }
        }
    }
}
