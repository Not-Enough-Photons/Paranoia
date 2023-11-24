using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Paranoia.Entities
{
    [AddComponentMenu("Paranoia/Entities/Audio Event")]
    [HelpURL("https://github.com/Not-Enough-Photons/Paranoia/wiki/Entities#audioevent")]
    public class AudioEvent : ParanoiaEvent
    {
        public override string Comment => "Plays a random audio clip from a list of audio clips.";
        public override string Warning => "Do not have multiple audio events. Just dump all audio clips into one audio event.";
        [Header("Audio Settings")]
        [Tooltip("The list of audio clips that might play.")]
        public AudioClip[] audioClips;
        [Tooltip("The audio source the clip will play from.")]
        public AudioSource audioSource;
        private void Start()
        {

        }

        private IEnumerator DespawnSelf(float delay)
        {
            yield return new WaitForSeconds(delay);
        }
    }
}