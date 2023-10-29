using System;
using BoneLib;
using MelonLoader;
using Paranoia.Helpers;
using SLZ.Rig;
using UnityEngine;

using Random = UnityEngine.Random;

namespace Paranoia.Entities
{
    /// <summary>
    /// Crashes the game if it gets too close.
    /// <br/>Assuming the person who set up the crasher is smart enough to read instructions, it disappears when shot, so you have a chance.
    /// </summary>
    public class Crasher : MonoBehaviour
    {
        public float movementSpeed;
        public AudioClip[] possibleSounds;
        public AudioSource audioSource;
        private Transform _player;
        private Transform This => transform;
        
        private void Start()
        {
            ModConsole.Msg("Crasher spawned", LoggingMode.DEBUG);
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
            ModConsole.Msg("Crasher despawned", LoggingMode.DEBUG);
            Destroy(gameObject);
        }
        
        public Crasher(IntPtr ptr) : base(ptr) { }
    }
}