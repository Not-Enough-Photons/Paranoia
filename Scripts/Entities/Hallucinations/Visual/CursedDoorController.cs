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

        private float m_timer = 0f;
        private float m_delay = 1.5f;

        protected override void Awake()
        {
            base.Awake();

            ReadValuesFromJSON(System.IO.File.ReadAllText(baseJsonPath + "CursedDoor.json"));

            hingeTransform = transform.Find("GameObject/scaler/door_Boneworks/hinge");
            source = transform.Find("GameObject/scaler/source").GetComponent<AudioSource>();
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

                if (!playedOnce)
                {
                    m_timer += Time.deltaTime;

                    if(m_timer >= m_delay)
                    {
                        source.PlayOneShot(Paranoia.instance.doorOpenSounds[0]);
                        playedOnce = true;
                        m_timer = 0f;
                    }
                }
            }
        }
    }
}
