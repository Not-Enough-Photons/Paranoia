#if MELONLOADER
using System;
using BoneLib;
using MelonLoader;
#endif
using SLZ.Marrow.Pool;
using SLZ.Rig;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Paranoia.Entities
{
#if UNITY_EDITOR
    [AddComponentMenu("Paranoia/Entities/Follower")]
#endif
    public class Follower : MonoBehaviour
    {
#if MELONLOADER
        public float movementSpeed;
        public bool shootable;
        private Transform _player;
        private Transform This => transform;
#else
        [Header("Follower Settings")]
        [Tooltip("How fast the follower moves towards the player")]
        public float movementSpeed;
        [Tooltip("If this is enabled, the follower will not despawn when it gets near the player. You will have to implement shooting events yourself with a generic attack reciever.")]
        public bool shootable;
        private Transform _player;
        private Transform This => transform;
#endif
        
#if MELONLOADER
        private void Start()
        {
            MelonLogger.Msg("Follower spawned");
            _player = Player.playerHead;
        }
        
        private void FixedUpdate()
        {
            This.localPosition += This.forward * (movementSpeed * Time.deltaTime);
            This.LookAt(_player);
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (shootable) return;
            if (other.GetComponentInParent<RigManager>() != null)
            {
                MelonLogger.Msg("Follower despawned");
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
        public Follower(IntPtr ptr) : base(ptr) { }
#endif
    }
}