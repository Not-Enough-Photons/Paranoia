using System;
using BoneLib;
using MelonLoader;
using SLZ.Rig;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Paranoia.Entities
{
    /// <summary>
    /// Chases the player. Very similar to Follower, but plays a random sound.
    /// </summary>
    public class Chaser : MonoBehaviour
    {
        public float movementSpeed;
        public bool shootable;
        public AudioClip[] possibleSounds;
        public AudioSource audioSource;
        private Transform _player;
        private Transform This => transform;

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

        public Chaser(IntPtr ptr) : base(ptr) { }
    }
}