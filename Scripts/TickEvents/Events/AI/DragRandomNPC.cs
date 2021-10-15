using NEP.Paranoia.ParanoiaUtilities;
using StressLevelZero.AI;

using UnityEngine;

using Random = UnityEngine.Random;

namespace NEP.Paranoia.TickEvents.Events
{
    public class DragRandomNPC : ParanoiaEvent
    {
        public override void Start()
        {
            AIBrain[] brains = Utilities.FindAIBrains();

            if(brains == null || brains.Length == 0) { return; }

            AIBrain rand = brains[Random.Range(0, brains.Length)];

            if(rand == null) { return; }

            Transform physRoot = rand.transform.Find("Physics/Root_M");

            if(physRoot == null) { return; }

            Rigidbody[] rbs = new Rigidbody[]
            {
                physRoot.Find("Hip_L/Knee_L").GetComponent<Rigidbody>(),
                physRoot.Find("Hip_R/Knee_R").GetComponent<Rigidbody>(),
                physRoot.Find("Spine_M/Chest_M/Shoulder_L/Elbow_L/Wrist_L").GetComponent<Rigidbody>(),
                physRoot.Find("Spine_M/Chest_M/Shoulder_R/Elbow_R/Wrist_R").GetComponent<Rigidbody>()
            };

            Rigidbody targetRB = rbs[Random.Range(0, rbs.Length)];

            if(targetRB == null) { return; }

            MelonLoader.MelonCoroutines.Start(CoGrabRoutine(rand, targetRB));
        }

        private System.Collections.IEnumerator CoGrabRoutine(AIBrain brain, Rigidbody part)
        {
            float timer = 0f;

            // Insert grab sound effect here.
            AudioSource.PlayClipAtPoint(Paranoia.instance.grabSounds[Random.Range(0, Paranoia.instance.grabSounds.Count)], part.position);

            yield return new WaitForSeconds(2f);

            Vector3 dir = Vector3.up * 0.25f + (-Vector3.right * Random.Range(5f, 50f) + (Vector3.forward * Random.Range(5f, 10f)));
            float force = 195f;

            while (timer < 7f)
            {
                timer += Time.deltaTime;

                part.AddForce(dir * force, ForceMode.Acceleration);
                yield return null;
            }

            brain.gameObject.SetActive(false);
        }
    }
}
