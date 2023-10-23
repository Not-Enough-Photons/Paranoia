#if MELONLOADER
using System;
using BoneLib;
using MelonLoader;
#endif
using SLZ.Rig;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Paranoia.Entities
{
#if UNITY_EDITOR
    [AddComponentMenu("Paranoia/Entities/Chaser")]
#endif
    public class Chaser : MonoBehaviour
    {
#if MELONLOADER
        public float movementSpeed;
        public bool shootable;
        public AudioClip[] possibleSounds;
        public AudioSource audioSource;
        private Transform _player;
        private Transform This => transform;
#else
        [Header("Chaser Settings")]
        [Tooltip("How fast the chaser moves towards the player")]
        public float movementSpeed;
        [Tooltip("If this is enabled, the chaser will not despawn when it gets near the player. You will have to implement shooting events yourself with a generic attack reciever.")]
        public bool shootable;
        [Header("Audio Settings")]
        [Tooltip("The list of audio clips the chaser may play.")]
        public AudioClip[] possibleSounds;
        [Tooltip("The audio source the chaser will play sounds from.")]
        public AudioSource audioSource;
        private Transform _player;
        private Transform This => transform;
#endif
        
#if MELONLOADER
        private void Start()
        {
            MelonLogger.Msg("Chaser spawned");
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
            if (shootable) return;
            if (other.GetComponentInParent<RigManager>() != null)
            {
                MelonLogger.Msg("Chaser despawned");
                Destroy(gameObject);
            }
        }
#else
        private void Start()
        {
        
        }

        private void FixedUpdate()
        {
        
        }

        private void OnTriggerEnter(Collider other)
        {
            return;
        }
#endif
#if MELONLOADER
        public Chaser(IntPtr ptr) : base(ptr) { }
#endif
    }
}