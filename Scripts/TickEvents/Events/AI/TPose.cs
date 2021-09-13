using NEP.Paranoia.Utilities;
using NEP.Paranoia.Managers;

using System.Linq;

using UnityEngine;

using UnhollowerBaseLib;
using StressLevelZero.AI;
using StressLevelZero.Combat;

namespace NEP.Paranoia.TickEvents.Events
{
    public class TPose : ParanoiaEvent
    {
        public override void Start()
        {
            ParanoiaGameManager manager = ParanoiaGameManager.instance;

            if (manager.insanity > 1)
            {
                Transform playerTrigger = manager.playerTrigger;
                int rng = manager.rng;
                bool isRareNumber = rng >= 20 && rng <= 29 || rng >= 25 && rng <= 30;

                if (isRareNumber)
                {
                    if (playerTrigger != null)
                    {
                        try
                        {
                            Il2CppArrayBase<AIBrain> brains = GameObject.FindObjectsOfType<AIBrain>();

                            foreach (AIBrain brain in brains)
                            {
                                Transform t = brain.transform;

                                if (t.gameObject != null)
                                {
                                    if (t.Find("Physics") && t.Find("AiRig"))
                                    {
                                        Transform physicsGrp = t.Find("Physics");
                                        Transform aiGrp = t.Find("AiRig");

                                        physicsGrp.gameObject.SetActive(false);
                                        aiGrp.gameObject.SetActive(false);

                                        t.GetComponent<VisualDamageController>().enabled = false;
                                        t.GetComponent<AIBrain>().enabled = false;
                                        t.GetComponent<Arena_EnemyReference>().enabled = false;

                                        if (t.Find("Physics/Root_M/Spine_M/Chest_M/Head_M/Jaw_M/Head_JawEndSHJnt") != null)
                                        {
                                            Material eyeMat = t.Find("brettEnemy@neutral/geoGrp/brett_face").GetComponent<SkinnedMeshRenderer>().materials.FirstOrDefault((mat) => mat.name.Contains("mat_Brett_eye"));
                                            eyeMat.color = new Color(0f, 0f, 0f, 0f);
                                            Transform jaw = t.Find("Physics/Root_M/Spine_M/Chest_M/Head_M/Jaw_M/Head_JawEndSHJnt");
                                            jaw.localPosition = new Vector3(jaw.localPosition.x, -0.35f, jaw.localPosition.z);
                                        }

                                        t.localPosition = new Vector3(t.localPosition.x, 0f, t.localPosition.z);

                                        Vector3 lookRotation = Quaternion.LookRotation(ParanoiaUtilities.FindPlayer().forward).eulerAngles;
                                        t.eulerAngles = new Vector3(t.eulerAngles.x, lookRotation.y, t.eulerAngles.z);
                                    }
                                }
                            }

                            MelonLoader.MelonCoroutines.Start(CoResetTPosedEnemies(5f));
                        }
                        catch
                        {

                        }
                    }
                }
                else
                {
                    return;
                }
            }
        }

        private System.Collections.IEnumerator CoResetTPosedEnemies(float seconds)
        {
            yield return new WaitForSeconds(seconds);

            Il2CppArrayBase<AIBrain> brains = GameObject.FindObjectsOfType<AIBrain>();

            for (int i = 0; i < brains.Count; i++)
            {
                Transform t = brains[i].transform;
                Transform physicsGrp = t.Find("Physics");
                Transform aiGrp = t.Find("AiRig");

                physicsGrp.gameObject.SetActive(true);
                aiGrp.gameObject.SetActive(true);

                t.GetComponent<VisualDamageController>().enabled = true;
                t.GetComponent<AIBrain>().enabled = true;
                t.GetComponent<Arena_EnemyReference>().enabled = true;

                if (t.Find("Physics/Root_M/Spine_M/Chest_M/Head_M/Jaw_M/Head_JawEndSHJnt") != null)
                {
                    Material eyeMat = t.Find("brettEnemy@neutral/geoGrp/brett_face").GetComponent<SkinnedMeshRenderer>().materials.FirstOrDefault((mat) => mat.name.Contains("mat_Brett_eye"));
                    Transform jaw = t.Find("Physics/Root_M/Spine_M/Chest_M/Head_M/Jaw_M/Head_JawEndSHJnt");
                    jaw.localPosition = new Vector3(jaw.localPosition.x, -0.044f, jaw.localPosition.z);
                }

                t.gameObject.SetActive(false);
            }
        }
    }
}
