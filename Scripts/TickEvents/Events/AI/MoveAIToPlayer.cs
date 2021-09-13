using MelonLoader;
using NotEnoughPhotons.Paranoia.Utilities;
using NotEnoughPhotons.Paranoia.Managers;
using UnityEngine;

namespace NotEnoughPhotons.Paranoia.TickEvents.Events
{
    public class MoveAIToPlayer : ParanoiaEvent
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
                        Transform t = brain.transform;
                        MelonCoroutines.Start(CoMoveAIToPlayer(t));
                    }
                }
            }
            catch
            {

            }
        }

        private System.Collections.IEnumerator CoMoveAIToPlayer(Transform ai)
        {
            /*if (ParanoiaGameManager.instance.playerTrigger != null)
            {
                if (ai.GetComponentInParent<StressLevelZero.AI.AIBrain>() != null)
                {
                    Transform parent = ai.GetComponentInParent<StressLevelZero.AI.AIBrain>().transform;
                    PuppetMasta.BehaviourBaseNav baseNav = parent.GetComponentInChildren<PuppetMasta.BehaviourBaseNav>();

                    baseNav.sensors.hearingSensitivity = 0f;
                    baseNav.sensors.visionFov = 0f;
                    baseNav.sensors._visionSphere.enabled = false;
                    baseNav.breakAgroHomeDistance = 0f;

                    if (baseNav.mentalState != PuppetMasta.BehaviourBaseNav.MentalState.Rest)
                    {
                        baseNav.SwitchMentalState(PuppetMasta.BehaviourBaseNav.MentalState.Rest);
                    }

                    baseNav.SetHomePosition(ParanoiaUtilities.FindPlayer().position, true);

                    while (Vector3.Distance(baseNav.transform.position, ParanoiaUtilities.FindPlayer().position) > 0.25f) { yield return null; }

                    yield return null;
                }
                else
                {
                    yield return null;
                }
            }*/

            yield return null;
        }
    }
}
