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

                if(transform == null || physicsGroup == null) { return; }

                physicsGroup.gameObject.SetActive(false);

                physicsGroup.localPosition = new Vector3(physicsGroup.localPosition.x, 0f, physicsGroup.localPosition.z);
                Quaternion lookRotation = Quaternion.LookRotation(ParanoiaUtilities.FindPlayer().forward - physicsGroup.position);
                physicsGroup.rotation = lookRotation;
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

                if(t == null || physicsGrp == null || aiGrp == null) { yield break; }

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
