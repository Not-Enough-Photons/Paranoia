using System;
using UnityEngine;

namespace Paranoia.Helpers
{
    public class SeasonalEntity : MonoBehaviour
    {
        public MeshRenderer renderer;
        public Material christmasMaterial;
        public Material aprilfoolsMaterial;
        public Material defaultMaterial;
        
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

        public SeasonalEntity(IntPtr ptr) : base(ptr) { }
    }
}