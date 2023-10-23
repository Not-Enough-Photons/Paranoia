#if MELONLOADER
using System;
using BoneLib;
using MelonLoader;
#endif
using System.Collections;
using Paranoia.Helpers;
using UnityEngine;

namespace Paranoia.Entities
{
#if UNITY_EDITOR
    [AddComponentMenu("Paranoia/Entities/Paralyzer")]
#endif
    public class Paralyzer : MonoBehaviour
    {
#if MELONLOADER
        public AudioSource paralysisSound;
        private Transform _player;
        private Transform This => transform;
#else
        [Header("Paralyzer Settings")]
        [Tooltip("The audio source used.")]
        public AudioSource paralysisSound;
        private Transform _player;
        private Transform This => transform;
#endif
#if MELONLOADER
        private void Start()
        {
            MelonLogger.Msg("Paralyzer spawned");
            _player = Player.playerHead;
            This.position = _player.position + _player.forward * 25f;
            Utilities.FreezePlayer(true);
            paralysisSound.Play();
            This.LookAt(_player);
            MelonCoroutines.Start(MoveCloser());
        }

        private IEnumerator MoveCloser()
        {
            for (var i = 0; i < 3; i++)
            {
                This.position = Vector3.MoveTowards(This.position, _player.position, 5f);
                paralysisSound.Play();
                yield return new WaitForSeconds(5f);
                if (i == 2)
                {
                    This.position = Vector3.MoveTowards(This.position, _player.position, 5f);
                    paralysisSound.Play();
                    MelonCoroutines.Start(DespawnSelf());
                }
            }
        }
        
        private IEnumerator DespawnSelf()
        {
            yield return new WaitForSeconds(5f);
            MelonLogger.Msg("Paralyzer despawned");
            Utilities.FreezePlayer(false);
            Destroy(gameObject);
        }
#else
        private void Start()
        {
            
        }
        
        private IEnumerator MoveCloser()
        {
            yield return new WaitForSeconds(5f);
        }
        
        private IEnumerator DespawnSelf()
        {
            yield return new WaitForSeconds(5f);
        }
#endif
#if MELONLOADER
        public Paralyzer(IntPtr ptr) : base(ptr) { }
#endif
    }
}