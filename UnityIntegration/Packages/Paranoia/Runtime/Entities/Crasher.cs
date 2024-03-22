using UnityEngine;
using UltEvents;
using NEP.Paranoia.Scripts.InternalBehaviours;
using UnityEngine.AI;

using Random = UnityEngine.Random;

namespace NEP.Paranoia.Scripts.Entities
{
    [RequireComponent(typeof(UltEventHolder))]
    [AddComponentMenu("Paranoia/Entities/Crasher")]
    [HelpURL("https://github.com/Not-Enough-Photons/Paranoia/wiki/Entities#crasher")]
    [RequireComponent(typeof(NavMeshAgent))]
    public class Crasher : ParanoiaEvent
    {
        public override string Comment => "A chaser that crashes the game when it gets close.";
        public override string Warning => "SET UP THE ATTACK RECIEVER TO CALL THIS SCRIPT'S DESPAWN FUNCTION IN AN ULTEVENT (NOT A UNITY EVENT!). OTHERWISE CRASHING IS INEVITABLE. GIVE THE PLAYER A CHANCE.";
        [Header("Crasher Settings")]
        [Tooltip("How fast the chaser moves towards the player")]
        public float movementSpeed;
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
        
        public void Despawn()
        {
            
        }
    }
}