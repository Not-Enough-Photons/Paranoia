using System.Collections;
using UnityEngine;
using NEP.Paranoia.Scripts.InternalBehaviours;
using UnityEngine.AI;

namespace NEP.Paranoia.Scripts.Entities
{
    [AddComponentMenu("Paranoia/Entities/Mirage")]
	[HelpURL("https://github.com/Not-Enough-Photons/Paranoia/wiki/Entities#mirage")]
    [RequireComponent(typeof(NavMeshAgent))]
    public class Mirage : ParanoiaEvent
    {
        public override string Comment => "Randomly moves around the map. Should be initially spawned at the center of the map, Y value does not matter as long as X and Z are 0.";
        [Header("Mirage Settings")]
        [Tooltip("How fast the mirage moves")]
        public float movementSpeed;
        [Tooltip("How long until the mirage despawns")]
        public float despawnTime;
        [Tooltip("If this is enabled, the mirage will not despawn after the amounted time. You will have to implement shooting events yourself with a generic attack reciever.")]
        public bool shootable;
        private NavMeshAgent _agent;

        private void Start()
        {
            _agent = GetComponent<NavMeshAgent>();
            _agent.speed = movementSpeed;
            Move();
        }

        private void Move()
        {
            _agent.SetDestination(Utilities.GetRandomPointFromNavmesh());
        }

        private void FixedUpdate()
        {
            if (!_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance) Move();
        }

        private IEnumerator DespawnSelf(float delay)
        {
            yield return new WaitForSeconds(delay);
        }
    }
}