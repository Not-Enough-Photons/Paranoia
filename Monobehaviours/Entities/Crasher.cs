#if MELONLOADER
using System;
using BoneLib;
using MelonLoader;
#else
using SLZ.Combat;
#endif
using SLZ.Rig;
using UnityEngine;

using Random = UnityEngine.Random;

namespace Paranoia.Entities
{
#if UNITY_EDITOR
    [RequireComponent(typeof(GenericAttackReceiver))]
    [RequireComponent(typeof(CollisionEnterSensor))]
    [AddComponentMenu("Paranoia/Entities/Crasher")]
#endif
    public class Crasher : MonoBehaviour
    {
#if MELONLOADER
        public float movementSpeed;
        public AudioClip[] possibleSounds;
        public AudioSource audioSource;
        private Transform _player;
        private Transform This => transform;
#else
        [Header("Crasher Settings")]
        [Tooltip("How fast the chaser moves towards the player")]
        public float movementSpeed;
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
                Application.Quit();
            }
        }

        public void Despawn()
        {
            MelonLogger.Msg("Crasher despawned");
            Destroy(gameObject);
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
        
        public void Despawn()
        {
            
        }
#endif
#if MELONLOADER
        public Crasher(IntPtr ptr) : base(ptr) { }
#endif
    }
}