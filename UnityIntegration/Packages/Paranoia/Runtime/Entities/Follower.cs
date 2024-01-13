using SLZ.Marrow.Pool;
using SLZ.Rig;
using UnityEngine;
using Object = UnityEngine.Object;
using NEP.Paranoia.Scripts.InternalBehaviours;

namespace NEP.Paranoia.Scripts.Entities
{
    [AddComponentMenu("Paranoia/Entities/Follower")]
    [HelpURL("https://github.com/Not-Enough-Photons/Paranoia/wiki/Entities#follower")]
    public class Follower : ParanoiaEvent
    {
        public override string Comment => "Follows the player.";
        [Header("Follower Settings")]
        [Tooltip("How fast the follower moves towards the player")]
        public float movementSpeed = 1f;
        [Tooltip("If this is enabled, the follower will not despawn when it gets near the player. You will have to implement shooting events yourself with a generic attack reciever.")]
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