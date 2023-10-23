#if MELONLOADER
using BoneLib;
#endif
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Paranoia.Events
{
    public static class FlingRandomObject
    {
#if MELONLOADER
        public static void Activate()
        {
            Rigidbody[] rbs = Object.FindObjectsOfType<Rigidbody>();
            var player = Player.playerHead;

            var randomRb = rbs[Random.Range(0, rbs.Length)];

            randomRb.AddForce((player.position - randomRb.transform.position) * Random.Range(100f, 200f), ForceMode.Impulse);
        }
#else
        public static void Activate()
        {

        }
#endif
    }
}