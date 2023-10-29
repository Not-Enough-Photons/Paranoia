using System;
using BoneLib;
using MelonLoader;
using SLZ.Rig;
using UnityEngine;

namespace Paranoia.Entities
{
    /// <summary>
    /// Moves when the player isn't looking at it.
    /// </summary>
    public class WeepingAngel : MonoBehaviour
    {
        public float lookThreshold = 0.5f;
        public float movementSpeed;
        public bool shootable;
        private Transform _player;
        private Transform This => transform;

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

        public WeepingAngel(IntPtr ptr) : base(ptr) { }
    }
}