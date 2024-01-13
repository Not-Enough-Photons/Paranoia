using SLZ.Combat;
using SLZ.Rig;
using UnityEngine;
using UltEvents;

using Random = UnityEngine.Random;

namespace NEP.Paranoia.Scripts.Entities
{
    [RequireComponent(typeof(GenericAttackReceiver))]
    [RequireComponent(typeof(CollisionEnterSensor))]
    [RequireComponent(typeof(UltEventHolder))]
    [AddComponentMenu("Paranoia/Entities/Crasher")]
    [HelpURL("https://github.com/Not-Enough-Photons/Paranoia/wiki/Entities#crasher")]
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
        
        public void Despawn()
        {
            
        }
    }
}