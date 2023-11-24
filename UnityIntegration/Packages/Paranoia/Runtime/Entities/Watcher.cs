using System.Collections;
using UnityEngine;

namespace Paranoia.Entities
{
    [AddComponentMenu("Paranoia/Entities/Watcher")]
	[HelpURL("https://github.com/Not-Enough-Photons/Paranoia/wiki/Entities#watcher")]
    public class Watcher : ParanoiaEvent
    {
        public override string Comment => "Looks at the player then disappears after the set time.";
        [Header("Watcher Settings")]
        [Tooltip("How long the watcher will stay spawned for.")]
        public float timeToDespawn;
        private Transform _player;
        private Transform This => transform;

        private void Start()
        {
            
        }
        
        private void FixedUpdate()
        {
            
        }
        
        private IEnumerator DespawnSelf(float delay)
        {
            yield return new WaitForSeconds(delay);
        }
    }
}