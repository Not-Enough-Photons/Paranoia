#if MELONLOADER
using System;
using BoneLib;
using MelonLoader;
#endif
using SLZ.Rig;
using UnityEngine;

namespace Paranoia.Entities
{
#if UNITY_EDITOR
    [AddComponentMenu("Paranoia/Entities/Weeping Angel")]
#endif
    public class WeepingAngel : MonoBehaviour
    {
#if MELONLOADER
        public float lookThreshold = 0.5f;
        public float movementSpeed;
        public bool shootable;
        private Transform _player;
        private Transform This => transform;
#else
        [Header("Weeping Angel Settings")]
        [Tooltip("The threshold for looking at it")]
        public float lookThreshold = 0.5f;
        [Tooltip("How fast the weeping angel moves towards the player")]
        public float movementSpeed;
        [Tooltip("If this is enabled, the weeping angel will not despawn when it gets near the player. You will have to implement shooting events yourself with a generic attack reciever.")]
        public bool shootable;
        private Transform _player;
        private Transform This => transform;
#endif
#if MELONLOADER
        private void Start()
        {
            MelonLogger.Msg("Weeping Angel spawned");
            _player = Player.playerHead;
        }

        private void FixedUpdate()
        {
            This.LookAt(_player);
            var dotProduct = Vector3.Dot(_player.forward, This.forward);
            if (dotProduct >= lookThreshold)
            {
                This.localPosition += This.forward * (movementSpeed * Time.deltaTime);
            }
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (shootable) return;
            if (other.GetComponentInParent<RigManager>() != null)
            {
                MelonLogger.Msg("Weeping Angel despawned");
                Destroy(gameObject);
            }
        }
#else
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
#endif
#if MELONLOADER
        public WeepingAngel(IntPtr ptr) : base(ptr) { }
#endif
    }
}