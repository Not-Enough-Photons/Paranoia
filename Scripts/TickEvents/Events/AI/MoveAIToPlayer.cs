using MelonLoader;
using NotEnoughPhotons.Paranoia.Utilities;
using NotEnoughPhotons.Paranoia.Managers;
using UnityEngine;

using StressLevelZero.AI;
using PuppetMasta;

using UnhollowerBaseLib;

namespace NotEnoughPhotons.Paranoia.TickEvents.Events
{
    public class MoveAIToPlayer : ParanoiaEvent
    {
        public override void Start()
        {
            try
            {
                if (GameObject.FindObjectsOfType<AIBrain>() != null)
                {
                    Il2CppArrayBase<AIBrain> brains = GameObject.FindObjectsOfType<AIBrain>();

                    foreach (AIBrain brain in brains)
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
            ParanoiaGameManager manager = ParanoiaGameManager.instance;

            if (manager.playerTrigger != null)
            {
                if (ai.GetComponentInParent<AIBrain>() != null)
                {
                    Transform parent = ai.GetComponentInParent<AIBrain>().transform;
                    BehaviourBaseNav baseNav = parent.GetComponentInChildren<BehaviourBaseNav>();

                    baseNav.sensors.hearingSensitivity = 0f;
                    baseNav.sensors.visionFov = 0f;
                    baseNav.sensors._visionSphere.enabled = false;
                    baseNav.breakAgroHomeDistance = 0f;

                    if (baseNav.mentalState != BehaviourBaseNav.MentalState.Rest)
                    {
                        baseNav.SwitchMentalState(BehaviourBaseNav.MentalState.Rest);
                    }

                    baseNav.SetHomePosition(ParanoiaUtilities.FindPlayer().position, true);

                    while (Vector3.Distance(baseNav.transform.position, ParanoiaUtilities.FindPlayer().position) > 0.25f) { yield return null; }

                    yield return null;
                }
                else
                {
                    yield return null;
                }
            }

            yield return null;
        }
    }
}
