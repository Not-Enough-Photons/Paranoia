using System.Collections.Generic;
using SLZ.Rig;
using UnityEngine;

namespace Paranoia.Entities
{
    [AddComponentMenu("Paranoia/Crying")]
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(AudioSource))]
    public class Crying : ParanoiaEvent
    {
        public override string Comment => "Plays a sound and then stops when the player approaches. Deletes itself if there's already another crying entity spawned.";
        private readonly List<Crying> _cryings = new List<Crying>();
        [Header("Crying Settings")]
        [Tooltip("The audio clip to play.")]
        public AudioClip cryingClip;
        [Tooltip("The audio source to play the crying sound from.")]
        public AudioSource cryingSource;
        private void Start()
        {

        }

        private void OnTriggerEnter(Collider other)
        {
            
        }
    }
}