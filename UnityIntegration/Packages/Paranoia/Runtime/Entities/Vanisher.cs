using UnityEngine;
using UltEvents;
using System.Collections;

namespace Paranoia.Entities
{
    [AddComponentMenu("Paranoia/Entities/Vanisher")]
    [RequireComponent(typeof(Collider))]
    public class Vanisher : ParanoiaEvent
    {
        public override string Comment => "Moves to the player until the player looks at it.";
        [Header("Vanisher Settings")]
        [Tooltip("The threshold for looking at it")]
        public float lookThreshold = 0.5f;
        [Tooltip("How fast the vanisher moves towards the player")]
        public float movementSpeed;
        [Tooltip("The object to vanish")]
        public GameObject vanishObject;
        [Tooltip("The particles to play when vanishing")]
        public ParticleSystem vanishParticles;
        // Obama Vanish!
        [Tooltip("The sound to play when vanishing")]
        public AudioSource vanishSound;
        private Transform _player;
        private Transform This => transform;
        private bool _isVanishing;

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

        private IEnumerator Vanish()
        {
            yield return null;
        }
    }
}