using System;
using System.Collections.Generic;
using SLZ.Rig;
using UnityEngine;

namespace Paranoia.Entities
{
    /// <summary>
    /// Prevents duplicates of this entity from spawning.
    /// </summary>
    public class Crying : MonoBehaviour
    {
        // TODO: This is broken. Add logging, find why
        private readonly List<Crying> _cryings = new List<Crying>();
        public AudioClip cryingClip;
        public AudioSource cryingSource;
        private void Start()
        {
            var cryings = Resources.FindObjectsOfTypeAll<Crying>();
            foreach (var crying in cryings)
            {
                _cryings.Add(crying);
            }
            _cryings.Remove(this);
            if (_cryings != null)
            {
                Destroy(gameObject);
            }
            cryingSource.loop = true;
            if (cryingSource.clip == null)
            {
                cryingSource.clip = cryingClip;
            }
            if (cryingSource.isPlaying) return;
            cryingSource.Play();
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponentInParent<RigManager>() != null)
            {
                Destroy(gameObject);
            }
        }
        public Crying(IntPtr ptr) : base(ptr) { }
    }
}