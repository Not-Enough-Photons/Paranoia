using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace NEP.Paranoia.Entities
{
    [AddComponentMenu("Paranoia/Entities/Stalker")]
	[HelpURL("https://github.com/Not-Enough-Photons/Paranoia/wiki/Entities#stalker")]
    public class Stalker : ParanoiaEvent
    {
        public override string Comment => "Spawns in a random position near the player then disappears after the set time.";
        [Header("Stalker Settings")]
        [Tooltip("How long the stalker will stay spawned for.")]
        public float timeToDespawn = 15f;
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