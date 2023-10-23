﻿using System.Collections;
using SLZ.AI;
using UnityEngine;
using Paranoia.Helpers;
using Paranoia.Managers;
using Random = UnityEngine.Random;

namespace Paranoia.Events
{
    public static class DragRandomNpc
    {
#if MELONLOADER
        public static void Activate(GameObject clipHolder)
        {
            var grabClips = clipHolder.GetComponent<ClipHolder>().clips;
            
            var brains = Utilities.FindAIBrains();

            if(brains == null || brains.Length == 0) { return; }

            var rand = brains[Random.Range(0, brains.Length)];

            if(rand == null) { return; }

            var physRoot = rand.transform.Find("Physics/Root_M");

            if(physRoot == null) { return; }

            var rbs = new Rigidbody[]
            {
                physRoot.Find("Hip_L/Knee_L").GetComponent<Rigidbody>(),
                physRoot.Find("Hip_R/Knee_R").GetComponent<Rigidbody>(),
                physRoot.Find("Spine_M/Chest_M/Shoulder_L/Elbow_L/Wrist_L").GetComponent<Rigidbody>(),
                physRoot.Find("Spine_M/Chest_M/Shoulder_R/Elbow_R/Wrist_R").GetComponent<Rigidbody>()
            };

            var targetRb = rbs[Random.Range(0, rbs.Length)];

            if(targetRb == null) { return; }

            MelonLoader.MelonCoroutines.Start(CoGrabRoutine(rand, targetRb, grabClips));
        }

        private static IEnumerator CoGrabRoutine(AIBrain brain, Rigidbody part, AudioClip[] grabClips)
        {
            var timer = 0f;
            
            AudioSource.PlayClipAtPoint(grabClips[Random.Range(0, grabClips.Length)], part.position);

            yield return new WaitForSeconds(2f);

            var dir = Vector3.up * 0.25f + (-Vector3.right * Random.Range(5f, 50f) + (Vector3.forward * Random.Range(5f, 10f)));
            const float force = 175f;

            while (timer < 7f)
            {
                timer += Time.deltaTime;

                part.AddForce(dir * force, ForceMode.Acceleration);
                yield return null;
            }

            brain.gameObject.SetActive(false);

            yield return null;
        }
#else 
        public static void Activate(GameObject clipHolder)
        {

        }

        private static IEnumerator CoGrabRoutine(AIBrain brain, Rigidbody part, AudioClip[] grabClips)
        {
            yield return null;
        }
#endif
    }
}