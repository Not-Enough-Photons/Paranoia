#if MELONLOADER
using System;
using BoneLib;
using MelonLoader;
#endif
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Paranoia.Entities
{
#if UNITY_EDITOR
    [AddComponentMenu("Paranoia/Entities/Stalker")]
#endif
    public class Stalker : MonoBehaviour
    {
#if MELONLOADER
        public float timeToDespawn;
        private Transform _player;
        private Transform This => transform;
#else
        [Header("Stalker Settings")]
        [Tooltip("How long the stalker will stay spawned for.")]
        public float timeToDespawn;
        private Transform _player;
        private Transform This => transform;
#endif
#if MELONLOADER
        private void Start()
        {
            MelonLogger.Msg("Stalker spawned");
            _player = Player.playerHead;
            var randint = Random.Range(0, 1);
            switch (randint)
            {
                case 0:
                    This.position = _player.position + _player.forward * 10f;
                    break;
                case 1:
                    This.position = _player.position + _player.right * 10f;
                    break;
                default:
                    MelonLogger.Error("Somehow, the random was not 0 or 1. Defaulting to forward.");
                    This.position = _player.position + _player.forward * 10f;
                    break;
            }
            MelonCoroutines.Start(DespawnSelf(timeToDespawn));
        }
        
        private void FixedUpdate()
        {
            This.LookAt(_player);
        }
        
        private IEnumerator DespawnSelf(float delay)
        {
            yield return new WaitForSeconds(delay);
            MelonLogger.Msg("Stalker despawned");
            Destroy(gameObject);
        }
#else
        private void Start()
        {
            
        }

        private void FixedUpdate()
        {
            
        }

        private IEnumerator DespawnSelf(float delay)
        {
            yield return new WaitForSeconds(delay);
        }
#endif
#if MELONLOADER
        public Stalker(IntPtr ptr) : base(ptr) { }
#endif
    }
}