using SLZ.Rig;
using UnityEngine;

namespace Paranoia.Entities
{
    [AddComponentMenu("Paranoia/Entities/Weeping Angel")]
    public class WeepingAngel : ParanoiaEvent
    {
        public override string Comment => "If the player is not looking, it will move towards the player. If the player is looking, it will not move.";
        [Header("Weeping Angel Settings")]
        [Tooltip("The threshold for looking at it")]
        public float lookThreshold = 0.5f;
        [Tooltip("How fast the weeping angel moves towards the player")]
        public float movementSpeed;
        [Tooltip("If this is enabled, the weeping angel will not despawn when it gets near the player. You will have to implement shooting events yourself with a generic attack reciever.")]
        public bool shootable;
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