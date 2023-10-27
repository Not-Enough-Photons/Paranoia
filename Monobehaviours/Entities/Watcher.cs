using System;
using BoneLib;
using MelonLoader;
using System.Collections;
using UnityEngine;

namespace Paranoia.Entities
{
    public class Watcher : MonoBehaviour
    {
        public float timeToDespawn;
        private Transform _player;
        private Transform This => transform;

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

        public Watcher(IntPtr ptr) : base(ptr) { }
    }
}