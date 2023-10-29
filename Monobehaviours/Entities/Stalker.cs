using System;
using BoneLib;
using MelonLoader;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Paranoia.Entities
{
    public class Stalker : MonoBehaviour
    {
        public float timeToDespawn;
        private Transform _player;
        private Transform _playerHead;
        private Transform This => transform;
        
        private void Start()
        {
            MelonLogger.Msg("Stalker spawned");
            _player = Player.physicsRig.m_chest;
            _playerHead = Player.playerHead;
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
            This.LookAt(_playerHead);
        }
        
        private IEnumerator DespawnSelf(float delay)
        {
            yield return new WaitForSeconds(delay);
            MelonLogger.Msg("Stalker despawned");
            Destroy(gameObject);
        }

        public Stalker(IntPtr ptr) : base(ptr) { }
    }
}