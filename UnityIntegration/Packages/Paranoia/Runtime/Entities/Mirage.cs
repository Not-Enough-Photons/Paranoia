using System.Collections;
using UnityEngine;
using NEP.Paranoia.Scripts.InternalBehaviours;

namespace NEP.Paranoia.Scripts.Entities
{
    [AddComponentMenu("Paranoia/Entities/Mirage")]
	[HelpURL("https://github.com/Not-Enough-Photons/Paranoia/wiki/Entities#mirage")]
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
        [Header("Bounds Settings")]
        [Tooltip("The minimum X value the mirage can move to")]
        public float minX = -195f;
        [Tooltip("The minimum Z value the mirage can move to")]
        public float minZ = -195f;
        [Tooltip("The maximum X value the mirage can move to")]
        public float maxX = 195f;
        [Tooltip("The maximum Z value the mirage can move to")]
        public float maxZ = 195f;
        private Transform _player;
        private Transform This => transform;
        private Vector3 _targetPosition;

        private void Start()
        {

        }

        private void Update()
        {

        }

        private void SetTargetPosition()
        {

        }

        private IEnumerator DespawnSelf(float delay)
        {
            yield return new WaitForSeconds(delay);
        }
    }
}