using UnityEngine;

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
                    UnhollowerBaseLib.Il2CppArrayBase<StressLevelZero.AI.AIBrain> brains = GameObject.FindObjectsOfType<StressLevelZero.AI.AIBrain>();

                    foreach (StressLevelZero.AI.AIBrain brain in brains)
                    {
                        Transform ai = brain.transform;
                        if (ai.GetComponentInParent<StressLevelZero.AI.AIBrain>() != null)
                        {
                            StressLevelZero.AI.AIBrain parent = ai.GetComponentInParent<StressLevelZero.AI.AIBrain>();

                            PuppetMasta.PuppetMaster puppetMaster = parent.GetComponentInChildren<PuppetMasta.PuppetMaster>();
                            PuppetMasta.SubBehaviourHealth hp = parent.GetComponentInChildren<PuppetMasta.BehaviourBaseNav>().health;
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
