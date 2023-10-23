#if MELONLOADER
using System;
using BoneLib;
using MelonLoader;
#endif
using System.Collections;
using UnityEngine;

namespace Paranoia.Entities
{
#if UNITY_EDITOR
    [AddComponentMenu("Paranoia/Entities/Watcher")]
#endif
    public class Watcher : MonoBehaviour
    {
#if MELONLOADER
        public float timeToDespawn;
        private Transform _player;
        private Transform This => transform;
#else
        [Header("Watcher Settings")]
        [Tooltip("How long the watcher will stay spawned for.")]
        public float timeToDespawn;
        private Transform _player;
        private Transform This => transform;
#endif
#if MELONLOADER
        private void Start()
        {
            MelonLogger.Msg("Watcher spawned");
            _player = Player.playerHead;
            MelonCoroutines.Start(DespawnSelf(timeToDespawn));
        }
        
        private void FixedUpdate()
        {
            This.LookAt(_player);
        }
        
        private IEnumerator DespawnSelf(float delay)
        {
            yield return new WaitForSeconds(delay);
            MelonLogger.Msg("Watcher despawned");
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
        public Watcher(IntPtr ptr) : base(ptr) { }
#endif
    }
}