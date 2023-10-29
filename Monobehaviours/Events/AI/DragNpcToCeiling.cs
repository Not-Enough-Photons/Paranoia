using MelonLoader;
using Paranoia.Helpers;
using Paranoia.Managers;
using SLZ.AI;
using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

namespace Paranoia.Events
{
    /// <summary>
    /// Drags a random NPC to the ceiling.
    /// </summary>
    public static class DragNpcToCeiling
    {
        public static void Activate(GameObject paranoiaManager)
        {
            MelonLogger.Msg("DragNpcToCeiling activated");
            var clips = paranoiaManager.GetComponent<ParanoiaManager>().grabSounds;
            var brains = Utilities.FindAIBrains();
            if (brains == null || brains.Length == 0) { return; }
            MelonLogger.Msg($"Got AIBrains: {brains.Length}");
            var rand = brains[Random.Range(0, brains.Length)];
            if (rand == null) { return; }
            MelonLogger.Msg($"Got random AIBrain: {rand.name}");
            var physRoot = rand.transform.Find("Physics/Root_M");
            if (physRoot == null) { return; }
            MelonLogger.Msg($"Got physRoot: {physRoot.name}");
            var targetRB = physRoot.Find("Spine_M/Chest_M/Head_M").GetComponent<Rigidbody>();
            if (targetRB == null) { return; }
            MelonLogger.Msg($"Got targetRB: {targetRB.name}");
            rand.puppetMaster.muscleWeight = 0f;
            MelonCoroutines.Start(CoGrabRoutine(rand, targetRB, clips));
        }

        private static IEnumerator CoGrabRoutine(AIBrain brain, Rigidbody part, AudioClip[] grabClips)
        {
            var timer = 0f;
            
            AudioSource.PlayClipAtPoint(grabClips[Random.Range(0, grabClips.Length)], part.position);

            yield return new WaitForSeconds(2f);

            var dir = Vector3.up;
            var force = Random.Range(250f, 300f);

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