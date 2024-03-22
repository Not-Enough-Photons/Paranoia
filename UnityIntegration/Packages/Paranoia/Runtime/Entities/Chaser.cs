using UnityEngine;
using Random = UnityEngine.Random;
using NEP.Paranoia.Scripts.InternalBehaviours;
using UnityEngine.AI;

namespace NEP.Paranoia.Scripts.Entities
{
    [AddComponentMenu("Paranoia/Entities/Chaser")]
    [HelpURL("https://github.com/Not-Enough-Photons/Paranoia/wiki/Entities#chaser")]
    [RequireComponent(typeof(NavMeshAgent))]
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
        private NavMeshAgent _agent;
        
        private void Start()
        {
            _agent = GetComponent<NavMeshAgent>();
            _agent.speed = movementSpeed;
        }

        private void FixedUpdate()
        {
            Vector3 pos = transform.position + transform.forward * 1;
            _agent.SetDestination(pos);
        }

        private void OnTriggerEnter(Collider other)
        {
            return;
        }
    }
}