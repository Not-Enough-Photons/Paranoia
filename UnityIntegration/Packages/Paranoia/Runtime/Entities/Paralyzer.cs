using System.Collections;
using Paranoia.Helpers;
using UnityEngine;

namespace Paranoia.Entities
{
    [AddComponentMenu("Paranoia/Entities/Paralyzer")]
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