using UnityEngine;
using NEP.Paranoia.ParanoiaUtilities;
using NEP.Paranoia.Managers;
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
        private float m_delay = 4.75f;

        protected override void Awake()
        {
            base.Awake();

            ReadValuesFromJSON(System.IO.File.ReadAllText(baseJsonPath + "CursedDoor.json"));

            hingeTransform = transform.Find("GameObject/scaler/door_Boneworks/hinge");
            source = transform.Find("GameObject/scaler/source").GetComponent<AudioSource>();

            trigger = GetComponentInChildren<PlayerTrigger>();

            trigger.TriggerEnterEvent.AddListener(new System.Action(() =>
            {
                GameManager.endRoom.SetActive(true);
                Utilities.GetRigManager().GetComponent<RigManager>().Teleport(MapUtilities.endRoomPlayerSpawn.position, true);
                GameManager.hStaringMan.gameObject.SetActive(true);
                GameManager.hStaringMan.transform.position = MapUtilities.endRoomEyesSpawn.position;
                GameManager.hStaringMan.moveSpeed = 0.15f;
                GameManager.hStaringMan.disableDistance = 0.25f;

                MelonLoader.MelonCoroutines.Start(CoEndRoutine());
            }));

            MelonLoader.MelonCoroutines.Start(CoHideRoutine());
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

        private void FreezePlayer(PhysicsRig rig, bool freeze)
        {
            StressLevelZero.VRMK.PhysBody physBody = rig.physBody;

            physBody.rbFeet.isKinematic = freeze;
            physBody.rbPelvis.isKinematic = freeze;
            rig.leftHand.rb.isKinematic = freeze;
            rig.rightHand.rb.isKinematic = freeze;
        }

        protected System.Collections.IEnumerator CoHideRoutine()
        {
            yield return new WaitForSeconds(90f);
            gameObject.SetActive(false);
        }

        protected System.Collections.IEnumerator CoEndRoutine()
        {
            FreezePlayer(Object.FindObjectOfType<PhysicsRig>(), true);

            Transform staringManPos = GameManager.hStaringMan.transform;
            Transform playerPos = Utilities.FindPlayer().transform;
            AudioSource source = GameManager.endRoom.transform.Find("Audio Source").GetComponent<AudioSource>();

            yield return new WaitForSeconds(source.clip.length - 1.75f);

            UnityEngine.SceneManagement.SceneManager.LoadScene((int)MapLevel.MainMenu);

            yield return null;
        }
    }
}
