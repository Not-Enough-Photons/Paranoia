using System;
using BoneLib;
using MelonLoader;
using System.Collections;
using UnityEngine;

namespace Paranoia.Entities
{
    /// <summary>
    /// Watches the player from wherever they spawned, never moving.
    /// </summary>
    public class Watcher : MonoBehaviour
    {
        public float timeToDespawn;
        private Transform _player;
        private Transform This => transform;

        private void Start()
        {
            ModConsole.Msg("Watcher spawned", LoggingMode.DEBUG);
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
            ModConsole.Msg("Watcher despawned", LoggingMode.DEBUG);
            Destroy(gameObject);
        }

        public Watcher(IntPtr ptr) : base(ptr) { }
    }
}