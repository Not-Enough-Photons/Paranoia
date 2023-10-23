#if MELONLOADER
using System;
#endif
using UnityEngine;

namespace Paranoia.Helpers
{
#if UNITY_EDITOR
    [AddComponentMenu("Paranoia/Helpers/Seasonal Entity")]
#endif
    public class SeasonalEntity : MonoBehaviour
    {
#if MELONLOADER
        public MeshRenderer renderer;
        public Material christmasMaterial;
        public Material aprilfoolsMaterial;
        public Material defaultMaterial;
#else
        [Header("Seasonal Settings")]
        [Tooltip("The mesh renderer of the object.")]
        public MeshRenderer renderer;
        [Tooltip("The material to use on Christmas.")]
        public Material christmasMaterial;
        [Tooltip("The material to use on April Fools.")]
        public Material aprilfoolsMaterial;
        [Tooltip("The material to use on any other day.")]
        public Material defaultMaterial;
#endif
#if MELONLOADER
        private void Start()
        {
            var isChristmas = Utilities.CheckDate(12, 25);
            var isAprilFools = Utilities.CheckDate(4, 1);
            if (isChristmas)
            {
                renderer.material = christmasMaterial;
            }
            else if (isAprilFools)
            {
                renderer.material = aprilfoolsMaterial;
            }
            else
            {
                renderer.material = defaultMaterial;
            }
        }
#else
        private void Start()
        {
            
        }
#endif
#if MELONLOADER
        public SeasonalEntity(IntPtr ptr) : base(ptr) { }
#endif
    }
}