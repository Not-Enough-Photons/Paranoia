using UnityEngine;
using NEP.Paranoia.ParanoiaUtilities;
using StressLevelZero.Rig;

namespace NEP.Paranoia.Entities
{
    public class CursedDoorController : BaseHallucination
    {
        public CursedDoorController(System.IntPtr ptr) : base(ptr) { }

        public AudioSource source;
        public Transform hingeTransform;
        public Material faceMaterial;

        private PlayerTrigger trigger;

        private bool playedOnce = false;

        private float m_timer = 0f;
        private float m_delay = 3f;

        protected override void Awake()
        {
            base.Awake();

            ReadValuesFromJSON(System.IO.File.ReadAllText(baseJsonPath + "CursedDoor.json"));

            hingeTransform = transform.Find("GameObject/scaler/door_Boneworks/hinge");
            source = transform.Find("GameObject/scaler/source").GetComponent<AudioSource>();

            trigger = GetComponentInChildren<PlayerTrigger>();

            trigger.TriggerEnterEvent.AddListener(new System.Action(() =>
            {
                GameObject.Find("HALFPIPE").SetActive(true);
                Utilities.GetRigManager().GetComponent<RigManager>().Teleport(new Vector3(-134.10f, 44.56f, -85.98f), true);
            }));
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

            if(Vector3.Distance(hingeTransform.position, ParanoiaUtilities.Utilities.FindPlayer().transform.position) < 5f)
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
