using System;
using MelonLoader;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Paranoia.Entities
{
    public class Radio : MonoBehaviour
    {
        public AudioClip[] audioClips;
        public AudioSource audioSource;

        private void Start()
        {
            MelonLogger.Msg("Radio spawned");
            audioSource.clip = audioClips[Random.Range(0, audioClips.Length)];
            audioSource.Play();
            MelonCoroutines.Start(DespawnSelf(audioSource.clip.length));
        }

        private IEnumerator DespawnSelf(float delay)
        {
            yield return new WaitForSeconds(delay);
            MelonLogger.Msg("Radio despawned");
            Destroy(gameObject);
        }
        
        public Radio(IntPtr ptr) : base(ptr) { }
    }
}