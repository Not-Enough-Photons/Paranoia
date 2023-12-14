using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace NEP.Paranoia.Entities
{
    [AddComponentMenu("Paranoia/Entities/Radio")]
	[HelpURL("https://github.com/Not-Enough-Photons/Paranoia/wiki/Entities#radio")]
    public class Radio : ParanoiaEvent
    {
        public override string Comment => "Plays a random audio clip from the list and is used as the radio for radio events.";
        public override string Warning => "Do NOT have multiple radios. It will cause issues with some events and may cause audio overlapping.";
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