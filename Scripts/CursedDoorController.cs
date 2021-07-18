using UnityEngine;

namespace NotEnoughPhotons.paranoia
{
    public class CursedDoorController : MonoBehaviour
    {
        public CursedDoorController(System.IntPtr ptr) : base(ptr) { }

        public AudioSource source;
        public Transform faceTransform;
        public Material faceMaterial;

        private Vector3 initialFaceSpawn;

        private void Awake()
        {
            MeshRenderer renderer = faceTransform.GetComponent<MeshRenderer>();
            faceMaterial = renderer.material;

            initialFaceSpawn = faceTransform.position;
        }

        private void OnEnable()
        {
            faceTransform.position = initialFaceSpawn;
            faceMaterial.color = Color.clear;
        }

        private void Update()
        {
            if(Vector3.Dot(faceTransform.forward, ParanoiaUtilities.FindPlayer().forward) > 1f)
            {
                faceMaterial.color = Color.Lerp(faceMaterial.color, Color.white, 0.25f * Time.deltaTime);
            }

            if(faceMaterial.color.a >= 255f)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
