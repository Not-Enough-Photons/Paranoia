using SLZ.Rig;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Paranoia.Entities
{
    [AddComponentMenu("Paranoia/Entities/Chaser")]
    public class Chaser : ParanoiaEvent
    {
        public override string Comment => "Chases the player with a random sound playing.";
        [Header("Chaser Settings")]
        [Tooltip("How fast the chaser moves towards the player")]
        public float movementSpeed = 10f;
        [Tooltip("If this is enabled, the chaser will not despawn when it gets near the player. You will have to implement shooting events yourself with a generic attack reciever.")]
        public bool shootable;
        [Header("Audio Settings")]
        [Tooltip("The list of audio clips the chaser may play.")]
        public AudioClip[] possibleSounds;
        [Tooltip("The audio source the chaser will play sounds from.")]
        public AudioSource audioSource;
        private Transform _player;
        private Transform This => transform;
        
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
    }
}