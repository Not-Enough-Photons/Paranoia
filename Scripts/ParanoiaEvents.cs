using UnityEngine;
using System.Reflection;

namespace NotEnoughPhotons.paranoia
{
    public class ParanoiaEvents
    {
        public static void RunEvent(string nameOf)
        {

        }

        private void KillAI()
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

                            ParanoiaUtilities.GetTick("eKillAllTick").maxTick = Random.Range(180f, 280f);
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
