using NEP.Paranoia.ParanoiaUtilities;
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
            AIBrain[] brains = Utilities.FindAIBrains();

            if(brains == null) { return; }

            foreach(AIBrain brain in brains)
            {
                Transform transform = brain.transform;
                Transform physicsGroup = transform.Find("Physics");

                if(transform == null || physicsGroup == null) { return; }

                physicsGroup.gameObject.SetActive(false);

                Transform head = physicsGroup.transform.Find("Root_M/Spine_M/Chest_M/Head_M");

                physicsGroup.localPosition = new Vector3(physicsGroup.localPosition.x, 0f, physicsGroup.localPosition.z);
                Quaternion lookRotation = Quaternion.LookRotation(ModThatIsNotMod.Player.GetPlayerHead().transform.position - physicsGroup.forward);
                physicsGroup.rotation = lookRotation;

                if(head != null)
                {
                    MelonLoader.MelonCoroutines.Start(CoHeadLookat(head));
                }
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

        private System.Collections.IEnumerator CoHeadLookat(Transform head)
        {
            while (head.gameObject.active)
            {
                head.LookAt(Utilities.GetPhysicsRig().transform);
                yield return null;
            }
        }
    }
}
