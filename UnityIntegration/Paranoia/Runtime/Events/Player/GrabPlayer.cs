using Paranoia.Managers;
using UnityEngine;
using Random = UnityEngine.Random;
using System.Collections;

namespace Paranoia.Events
{
    public static class GrabPlayer
    {
        public static void Activate(GameObject clipHolder)
        {

        }

        private static IEnumerator CoGrabRoutine(Rigidbody part, AudioClip[] grabClips)
        {
            yield return null;
        }
    }
}