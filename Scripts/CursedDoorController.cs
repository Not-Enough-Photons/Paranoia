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
            faceTransform = transform.Find("scaler/Art/Face");
            initialFaceSpawn = faceTransform.position;
            faceMaterial = faceTransform.GetComponent<MeshRenderer>().material;
        }

        private void OnEnable()
        {
            faceTransform.position = initialFaceSpawn;
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
