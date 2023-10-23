#if MELONLOADER
using BoneLib;
#endif
using Paranoia.Managers;
using UnityEngine;
using Random = UnityEngine.Random;
using System.Collections;

namespace Paranoia.Events
{
    public static class GrabPlayer
    {
#if MELONLOADER
        public static void Activate(GameObject clipHolder)
        {
            var grabClips = clipHolder.GetComponent<ClipHolder>().clips;
            
            var rig = Player.physicsRig;

            Rigidbody[] rbs = {
                rig.leftHand.rb,
                rig.rightHand.rb
            };

            MelonLoader.MelonCoroutines.Start(CoGrabRoutine(rbs[Random.Range(0, rbs.Length)], grabClips));
        }

        private static IEnumerator CoGrabRoutine(Rigidbody part, AudioClip[] grabClips)
        {
            yield return new WaitForSeconds(15f);

            var timer = 0f;
            
            AudioSource.PlayClipAtPoint(grabClips[Random.Range(0, grabClips.Length)], part.position);

            yield return new WaitForSeconds(2f);

            var dir = Vector3.up + (Random.onUnitSphere * 10f);
            var force = Random.Range(50f, 100f);

            while (timer < 7f)
            {
                timer += Time.deltaTime;

                part.AddForce(dir * force, ForceMode.Acceleration);
                yield return null;
            }
        }
#else
        public static void Activate(GameObject clipHolder)
        {

        }

        private static IEnumerator CoGrabRoutine(Rigidbody part, AudioClip[] grabClips)
        {
            yield return null;
        }
#endif
    }
}