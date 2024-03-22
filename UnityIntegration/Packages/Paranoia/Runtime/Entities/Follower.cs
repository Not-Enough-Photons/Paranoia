using UnityEngine;
using Object = UnityEngine.Object;
using NEP.Paranoia.Scripts.InternalBehaviours;
using UnityEngine.AI;

namespace NEP.Paranoia.Scripts.Entities
{
    [AddComponentMenu("Paranoia/Entities/Follower")]
    [HelpURL("https://github.com/Not-Enough-Photons/Paranoia/wiki/Entities#follower")]
    [RequireComponent(typeof(NavMeshAgent))]
    public class Follower : ParanoiaEvent
    {
        public override string Comment => "Follows the player.";
        [Header("Follower Settings")]
        [Tooltip("How fast the follower moves towards the player")]
        public float movementSpeed = 1f;
        [Tooltip("If this is enabled, the follower will not despawn when it gets near the player. You will have to implement shooting events yourself with a generic attack reciever.")]
        public bool shootable;
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