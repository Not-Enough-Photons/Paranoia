using System.Collections;
using NEP.Paranoia.Helpers;
using UnityEngine;

namespace NEP.Paranoia.Entities
{
    [AddComponentMenu("Paranoia/Entities/Paralyzer")]
	[HelpURL("https://github.com/Not-Enough-Photons/Paranoia/wiki/Entities#paralyzer")]
    public class Paralyzer : ParanoiaEvent
    {
        public override string Comment => "Freezes the player in place for 15 seconds and slowly approaches.";
        [Header("Paralyzer Settings")]
        [Tooltip("The audio source used.")]
        public AudioSource paralysisSound;
        private Transform _player;
        private Transform This => transform;

        private void Start()
        {
            
        }
        
        private IEnumerator MoveCloser()
        {
            yield return new WaitForSeconds(5f);
        }
        
        private IEnumerator DespawnSelf()
        {
            yield return new WaitForSeconds(5f);
        }
    }
}