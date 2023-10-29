using System;
using MelonLoader;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Paranoia.Entities
{
    /// <summary>
    /// Plays a random sound.
    /// </summary>
    public class AudioEvent : MonoBehaviour
    {
        public AudioClip[] audioClips;
        public AudioSource audioSource;
        
        private void Start()
        {
            ModConsole.Msg("Audio event spawned", LoggingMode.DEBUG);
            audioSource.clip = audioClips[Random.Range(0, audioClips.Length)];
            audioSource.Play();
            MelonCoroutines.Start(DespawnSelf(audioSource.clip.length));
        }

        private IEnumerator DespawnSelf(float delay)
        {
            yield return new WaitForSeconds(delay);
            ModConsole.Msg("Audio event despawned", LoggingMode.DEBUG);
            Destroy(gameObject);
        }
        
        public AudioEvent(IntPtr ptr) : base(ptr) { }
    }
}