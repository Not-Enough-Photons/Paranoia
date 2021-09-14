using UnityEngine;
using NEP.Paranoia.Utilities;

namespace NEP.Paranoia.Entities
{
    public class CursedDoorController : BaseHallucination
    {
        public CursedDoorController(System.IntPtr ptr) : base(ptr) { }

        public AudioSource source;
        public Transform hingeTransform;
        public Material faceMaterial;
        private bool playedOnce = false;

        protected override void Awake()
        {
            base.Awake();

            hingeTransform = transform.Find("GameObject/scaler/door_Boneworks/hinge");
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            playedOnce = false;
            hingeTransform.localEulerAngles = Vector3.zero;
        }

        protected override void Update()
        {
            base.Update();

            if(Vector3.Distance(hingeTransform.position, ParanoiaUtilities.FindPlayer().transform.position) < 5f)
            {
                Vector3 target = -Vector3.up * 125f;
                hingeTransform.localRotation = Quaternion.Lerp(hingeTransform.localRotation, Quaternion.Euler(target), 0.15f * Time.deltaTime);
            }
        }
    }
}
