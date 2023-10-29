using System;
using BoneLib;
using MelonLoader;
using Paranoia.Helpers;
using SLZ.Rig;
using UnityEngine;

using Random = UnityEngine.Random;

namespace Paranoia.Entities
{
    public class Crasher : MonoBehaviour
    {
        public float movementSpeed;
        public AudioClip[] possibleSounds;
        public AudioSource audioSource;
        private Transform _player;
        private Transform This => transform;
        
        private void Start()
        {
            MelonLogger.Msg("Crasher spawned");
            audioSource.clip = possibleSounds[Random.Range(0, possibleSounds.Length)];
            audioSource.Play();
            _player = Player.playerHead;
        }
        
        private void FixedUpdate()
        {
            This.localPosition += This.forward * (movementSpeed * Time.deltaTime);
            This.LookAt(_player);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponentInParent<RigManager>() != null)
            {
                Utilities.CrashGame();
            }
        }

        public void Despawn()
        {
            MelonLogger.Msg("Crasher despawned");
            Destroy(gameObject);
        }
        
        public Crasher(IntPtr ptr) : base(ptr) { }
    }
}