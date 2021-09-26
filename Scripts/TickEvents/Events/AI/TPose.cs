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
            AIBrain[] brains = ParanoiaUtilities.FindAIBrains();

            if(brains == null) { return; }

            foreach(AIBrain brain in brains)
            {
                Transform transform = brain.transform;
                Transform physicsGroup = transform.Find("Physics");
                Transform aiGroup = transform.Find("AiRig");

                if(physicsGroup == null) { return; }

                physicsGroup.gameObject.SetActive(false);

                if(aiGroup == null) { return; }

                aiGroup.gameObject.SetActive(false);

                transform.localPosition = new Vector3(transform.localPosition.x, 0f, transform.localPosition.z);
                Vector3 lookRotation = Quaternion.LookRotation(ParanoiaUtilities.FindHead().forward).eulerAngles;
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, lookRotation.y, transform.eulerAngles.z);
            }

            MelonLoader.MelonCoroutines.Start(CoResetTPosedEnemies(brains, 10f));
        }

        private System.Collections.IEnumerator CoResetTPosedEnemies(AIBrain[] brains, float seconds)
        {
            yield return new WaitForSeconds(seconds);

            for (int i = 0; i < brains.Length; i++)
            {
                Transform t = brains[i].transform;
                Transform physicsGrp = t.Find("Physics");
                Transform aiGrp = t.Find("AiRig");

                physicsGrp.gameObject.SetActive(true);
                aiGrp.gameObject.SetActive(true);

                t.GetComponent<VisualDamageController>().enabled = true;
                t.GetComponent<AIBrain>().enabled = true;
                t.GetComponent<Arena_EnemyReference>().enabled = true;

                t.gameObject.SetActive(false);
            }
        }
    }
}
