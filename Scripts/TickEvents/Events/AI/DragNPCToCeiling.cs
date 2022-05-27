using NEP.Paranoia.ParanoiaUtilities;
using StressLevelZero.AI;

using UnityEngine;

using Random = UnityEngine.Random;

namespace NEP.Paranoia.TickEvents.Events
{
    public class DragNPCToCeiling : ParanoiaEvent
    {
        public override void Start()
        {
            AIBrain[] brains = Utilities.FindAIBrains();

            if (brains == null || brains.Length == 0) { return; }

            AIBrain rand = brains[Random.Range(0, brains.Length)];

            if (rand == null) { return; }

            Transform physRoot = rand.transform.Find("Physics/Root_M");

            if (physRoot == null) { return; }

            Rigidbody targetRB = physRoot.Find("Spine_M/Chest_M/Head_M").GetComponent<Rigidbody>();

            rand.puppetMaster.muscleWeight = 0f;

            MelonLoader.MelonCoroutines.Start(CoGrabRoutine(rand, targetRB));
        }

        private System.Collections.IEnumerator CoGrabRoutine(AIBrain brain, Rigidbody part)
        {
            float timer = 0f;

            // Insert grab sound effect here.
            AudioClip[] grabClips = new AudioClip[]
            {
                Paranoia.instance.GetClipInDirectory("player_grab_01"),
                Paranoia.instance.GetClipInDirectory("player_grab_02"),
                Paranoia.instance.GetClipInDirectory("player_grab_03"),
            };

            AudioSource.PlayClipAtPoint(grabClips[Random.Range(0, grabClips.Length)], part.position);

            yield return new WaitForSeconds(2f);

            Vector3 dir = Vector3.up;
            float force = Random.Range(250f, 300f);

            while (timer < 7f)
            {
                timer += Time.deltaTime;

                if(part == null) { break; }

                part.AddForce(dir * force, ForceMode.Acceleration);
                yield return null;
            }

            brain.gameObject.SetActive(false);
        }
    }
}
