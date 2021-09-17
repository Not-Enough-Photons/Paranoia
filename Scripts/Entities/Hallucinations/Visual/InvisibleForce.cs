using UnityEngine;

namespace NEP.Paranoia.Entities
{
    public class InvisibleForce : BaseHallucination
    {
        public InvisibleForce(System.IntPtr ptr) : base(ptr) { }

        protected float impactRadius = 1.5f;

        protected override void Awake()
        {
            base.Awake();

            gameObject.AddComponent<SphereCollider>().radius = impactRadius;

            ReadValuesFromJSON(System.IO.File.ReadAllText(baseJsonPath + "InvisibleForce.json"));
        }
    }
}
