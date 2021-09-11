using UnityEngine;

namespace NotEnoughPhotons.Paranoia.Entities
{
    public class CursedDoorController : BaseHallucination
    {
        public CursedDoorController(System.IntPtr ptr) : base(ptr) { }

        public AudioSource source;
        public Transform faceTransform;
        public Material faceMaterial;

        private Vector3 initialFaceSpawn;

        protected override void Awake()
        {
            base.Awake();

            faceTransform = transform.Find("scaler/Art/Face");
            initialFaceSpawn = faceTransform.position;
            faceMaterial = faceTransform.GetComponent<MeshRenderer>().material;
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            faceTransform.position = initialFaceSpawn;
        }

        protected override void Update()
        {
            base.Update();
        }
    }
}
