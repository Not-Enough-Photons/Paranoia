#if MELONLOADER
using System;
using MelonLoader;
#endif
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Paranoia.Entities
{
#if UNITY_EDITOR
    [AddComponentMenu("Paranoia/Entities/Audio Event")]
#endif
    public class AudioEvent : MonoBehaviour
    {
#if MELONLOADER
        public AudioClip[] audioClips;
        public AudioSource audioSource;
#else
        [Header("Audio Settings")]
        [Tooltip("The list of audio clips that might play.")]
        public AudioClip[] audioClips;
        [Tooltip("The audio source the clip will play from.")]
        public AudioSource audioSource;
#endif
#if MELONLOADER
        private void Start()
        {
            MelonLogger.Msg("Audio event spawned");
            audioSource.clip = audioClips[Random.Range(0, audioClips.Length)];
            audioSource.Play();
            MelonCoroutines.Start(DespawnSelf(audioSource.clip.length));
        }

        private IEnumerator DespawnSelf(float delay)
        {
            yield return new WaitForSeconds(delay);
            MelonLogger.Msg("Audio event despawned");
            Destroy(gameObject);
        }
#else
        private void Start()
        {

        }

        private IEnumerator DespawnSelf(float delay)
        {
            yield return new WaitForSeconds(delay);
        }
#endif
#if MELONLOADER
        public AudioEvent(IntPtr ptr) : base(ptr) { }
#endif
    }
}