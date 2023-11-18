using UnityEngine;
using UltEvents;

namespace Paranoia.Helpers
{
    [AddComponentMenu("Paranoia/Helpers/Seasonal Entity")]
    public class SeasonalEntity : ParanoiaEvent
    {
        public override string Comment => "Changes the material of the object depending on the date.";
        [Header("Seasonal Settings")]
        [Tooltip("The mesh renderer of the object.")]
        public MeshRenderer renderer;
        [Tooltip("The material to use on Christmas.")]
        public Material christmasMaterial;
        [Tooltip("The material to use on April Fools.")]
        public Material aprilfoolsMaterial;
        [Tooltip("The event to invoke on Christmas.")]
        public UltEvent onChristmas;
        [Tooltip("The event to invoke on April Fools.")]
        public UltEvent onAprilFools;

        private void Start()
        {
            
        }
    }
}