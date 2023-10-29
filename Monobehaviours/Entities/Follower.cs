using System;
using BoneLib;
using MelonLoader;
using SLZ.Rig;
using UnityEngine;

namespace Paranoia.Entities
{
    /// <summary>
    /// Follows the player.
    /// </summary>
    public class Follower : MonoBehaviour
    {
        public float movementSpeed;
        public bool shootable;
        private Transform _player;
        private Transform This => transform;
        
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
        
        public Follower(IntPtr ptr) : base(ptr) { }
    }
}