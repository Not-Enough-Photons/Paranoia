using System.Collections;
using System.Collections.Generic;
using System.Linq;

using TMPro;

using UnityEngine;

using UnhollowerBaseLib;
using UnhollowerRuntimeLib;

using MelonLoader;

namespace NotEnoughPhotons.paranoia
{
    public class GameManager : MonoBehaviour
    {
        public GameManager(System.IntPtr ptr) : base(ptr) { }

        public class Tick
        {
            [System.Flags]
            public enum TickType
            {
                TT_LIGHT = 1,
                TT_DARK = 2
            }

            public Tick(float maxTick, TickType tickType)
            {
                this.maxTick = maxTick;
                this.tickType = tickType;

                if (tickType.HasFlag(TickType.TT_LIGHT))
                {
                    instance.ticks.Add(this);
                }
                else if (tickType == TickType.TT_DARK)
                {
                    instance.darkTicks.Add(this);
                }
            }

            internal float tick = 0f;
            internal float maxTick = 5f;

            internal TickType tickType;

            internal System.Action OnTick;

            internal void Update()
            {
                tick += Time.deltaTime;

                if (tick >= maxTick)
                {
                    OnTick?.Invoke();
                    tick = 0f;
                }
            }
        }

        public static GameManager instance;

        public List<Tick> ticks;
        public List<Tick> darkTicks;

        // Main prefabs
        public GameObject shadowMan;
        public GameObject staringMan;
        public GameObject ceilingWatcher;
        public GameObject radioObject;

        public SpawnCircle playerCircle;

        private Transform playerTrigger;
        private Transform playerHead;

        private AudioManager manager;

        // Hallucinations
        private Hallucination hChaser;
        private Hallucination hDarkVoice;
        private Hallucination hCeilingMan;
        private Hallucination hStaringMan;
        private Hallucination hShadowPerson;
        private Hallucination hShadowPersonChaser;

        private VLB.VolumetricLightBeam lightBeam;
        private Light blankBoxLight;
        private GameObject flashlightObject;

        private GameObject radioClone;
        private AudioSource radioSource;

        // Audio ticks
        private Tick aAmbienceTick;
        private Tick aChaserTick;
        private Tick aDarkVoiceTick;

        // Visual ticks
        private Tick vShadowManTick;
        private Tick vStaringManTick;

        // Event ticks
        private Tick eTPoseTick;
        private Tick eRadioTick;
        private Tick eFirstRadioTick;
        private Tick eAIOriginTick;
        private Tick eKillAllTick;
        private Tick eItemDropTick;
        private Tick eLightFlickerTick;

        private Tick rngGeneratorTick;


        private bool firstRadioSpawn = false;
        private bool isDark = false;

        private int rng = 1;

        private Vector3[] staringManSpawns = new Vector3[3]
        {
            new Vector3(-53.9f, 1f, -55.1f),
            new Vector3(-53.7f, 1f, 32.1f),
            new Vector3(52.1f, 1f, 54.4f)
        };

        private SpawnCircle[] spawnCircles = new SpawnCircle[3];

        private Il2CppReferenceArray<LightmapData> lightmaps;
        private Il2CppStructArray<UnityEngine.Rendering.SphericalHarmonicsL2> bakedProbes;

        public static Hallucination CreateHallucination(Vector3 position, Hallucination.HallucinationType hType, Hallucination.HallucinationFlags hFlags, Hallucination.HallucinationClass hClass, float distanceToDisappear, float chaseSpeed, float delayTime, bool usesDelay)
        {
            Hallucination hallucination = new GameObject($"{hType} {hClass} Hallucination").AddComponent<Hallucination>();
            SpriteBillboard billboard = hallucination.gameObject.AddComponent<SpriteBillboard>();

            billboard.target = instance.FindPlayer();
            hallucination.target = instance.FindPlayer();
            hallucination.cameraTarget = instance.FindHead();
            hallucination.audioManager = FindObjectOfType<AudioManager>();

            if (hType.HasFlag(Hallucination.HallucinationType.Auditory))
            {
                AudioSource source = hallucination.gameObject.AddComponent<AudioSource>();
                hallucination.source = source;
                source.dopplerLevel = 0;
            }

            hallucination.transform.position = position;
            hallucination.gameObject.SetActive(false);
            hallucination.Initialize(hType, hFlags, hClass, distanceToDisappear, chaseSpeed, delayTime);
            return hallucination;
        }

        public static Hallucination CreateHallucination(Vector3 position, GameObject prefab, Hallucination.HallucinationType hType, Hallucination.HallucinationFlags hFlags, Hallucination.HallucinationClass hClass, float distanceToDisappear, float chaseSpeed, float delayTime, bool usesDelay)
        {
            Hallucination hallucination = new GameObject($"{hType} {hClass} Hallucination").AddComponent<Hallucination>();
            SpriteBillboard billboard = hallucination.gameObject.AddComponent<SpriteBillboard>();

            billboard.target = instance.FindPlayer();
            hallucination.target = instance.FindPlayer();
            hallucination.cameraTarget = instance.FindHead();
            hallucination.audioManager = FindObjectOfType<AudioManager>();

            if (prefab != null)
            {
                GameObject.Instantiate(prefab, hallucination.transform);
            }

            if (hType.HasFlag(Hallucination.HallucinationType.Auditory))
            {
                AudioSource source = hallucination.gameObject.AddComponent<AudioSource>();
                hallucination.source = source;
                source.dopplerLevel = 0;
            }

            hallucination.gameObject.transform.position = position;
            hallucination.gameObject.SetActive(false);
            hallucination.Initialize(hType, hFlags, hClass, distanceToDisappear, chaseSpeed, delayTime);
            return hallucination;
        }

        private void Awake()
		{
            if(instance == null)
			{
                instance = this;
			}
			else
			{
                Destroy(gameObject);
			}

            InitializeTicks();

            manager = FindObjectOfType<AudioManager>();

            InitializeEntities();

            playerTrigger = FindPlayer();

            

            playerCircle = new SpawnCircle(FindPlayer());

            for (int i = 0; i < spawnCircles.Length; i++)
            {
                spawnCircles[i] = new SpawnCircle(new GameObject("Spawn Circle").transform);
                spawnCircles[i].radius = 25f;
                spawnCircles[i].originTransform.position = staringManSpawns[i];
            }

            lightmaps = LightmapSettings.lightmaps;
            bakedProbes = LightmapSettings.lightProbes.bakedProbes;
        }

        private void InitializeTicks()
        {
            ticks = new List<Tick>();
            darkTicks = new List<Tick>();

            // Audio tick initialization
            aAmbienceTick       = new Tick(60f, Tick.TickType.TT_LIGHT | Tick.TickType.TT_DARK);
            aChaserTick         = new Tick(90f, Tick.TickType.TT_LIGHT | Tick.TickType.TT_DARK);
            aDarkVoiceTick      = new Tick(30f, Tick.TickType.TT_DARK);

            // Visual tick initialization
            vShadowManTick      = new Tick(90f, Tick.TickType.TT_LIGHT | Tick.TickType.TT_DARK);
            vStaringManTick     = new Tick(180f, Tick.TickType.TT_LIGHT | Tick.TickType.TT_DARK);

            // Event tick initialization
            eTPoseTick          = new Tick(120f, Tick.TickType.TT_LIGHT | Tick.TickType.TT_DARK);
            eRadioTick          = new Tick(190f, Tick.TickType.TT_LIGHT | Tick.TickType.TT_DARK);
            eFirstRadioTick     = new Tick(30f, Tick.TickType.TT_LIGHT | Tick.TickType.TT_DARK);
            eAIOriginTick       = new Tick(260f, Tick.TickType.TT_LIGHT | Tick.TickType.TT_DARK);
            eKillAllTick        = new Tick(240f, Tick.TickType.TT_LIGHT | Tick.TickType.TT_DARK);
            eItemDropTick       = new Tick(15f, Tick.TickType.TT_LIGHT | Tick.TickType.TT_DARK);
            eLightFlickerTick   = new Tick(90f, Tick.TickType.TT_LIGHT | Tick.TickType.TT_DARK);


            // Global tick initialization
            rngGeneratorTick    = new Tick(5f, Tick.TickType.TT_LIGHT | Tick.TickType.TT_DARK);

            // Audio tick subscription
            aAmbienceTick.OnTick += AudioRoutine;
            aChaserTick.OnTick += new System.Action(() => hChaser.gameObject.SetActive(true));
            aDarkVoiceTick.OnTick += new System.Action(() => hDarkVoice.gameObject.SetActive(true));

            // Visual tick subscription
            vShadowManTick.OnTick += new System.Action(() => hShadowPerson.gameObject.SetActive(true));
            vStaringManTick.OnTick += new System.Action(() => hStaringMan.gameObject.SetActive(true));

            // Event tick subscription
            eTPoseTick.OnTick += TPoseEvent;
            eRadioTick.OnTick += new System.Action(() => { SpawnRadio(); MoveAIToPoint(radioClone.transform.position, false); });
            eFirstRadioTick.OnTick += SpawnFirstRadio;
            eAIOriginTick.OnTick += new System.Action(() => MoveAIToPoint(Vector3.zero, true));
            eKillAllTick.OnTick += KillAIRandomly;
            eItemDropTick.OnTick += DropHeadItem;
            eLightFlickerTick.OnTick += new System.Action(() =>
            {
                KillNimbus();
                KillWasp();
                eLightFlickerTick.maxTick = Random.Range(120f, 180f);
                MelonCoroutines.Start(CoLightFlickerRoutine(Random.Range(15, 25)));
            });

            // Global tick subscription
            rngGeneratorTick.OnTick += new System.Action(() => rng = Random.Range(23, 150));
        }

        private void InitializeEntities()
		{
            radioClone = GameObject.Instantiate(radioObject);

            hChaser = CreateHallucination(Vector3.zero, Hallucination.HallucinationType.Auditory, Hallucination.HallucinationFlags.None, Hallucination.HallucinationClass.Chaser, 1f, 30f, 0f, false);
            hDarkVoice = CreateHallucination(Vector3.zero, Hallucination.HallucinationType.Auditory, Hallucination.HallucinationFlags.None, Hallucination.HallucinationClass.DarkVoice, 0f, 0f, 0f, false);
            hCeilingMan = CreateHallucination(Vector3.zero, ceilingWatcher, Hallucination.HallucinationType.Auditory | Hallucination.HallucinationType.Visual, Hallucination.HallucinationFlags.HideWhenSeen, Hallucination.HallucinationClass.Watcher, 0f, 0f, 0f, false);
            hStaringMan = CreateHallucination(Vector3.zero, staringMan, Hallucination.HallucinationType.Visual, Hallucination.HallucinationFlags.None, Hallucination.HallucinationClass.Chaser, 30f, 1f, 0f, false);
            hShadowPerson = CreateHallucination(Vector3.zero, shadowMan, Hallucination.HallucinationType.Visual, Hallucination.HallucinationFlags.None, Hallucination.HallucinationClass.Watcher, 0f, 0f, 0f, false);
            hShadowPersonChaser = CreateHallucination(Vector3.zero, shadowMan, Hallucination.HallucinationType.Visual, Hallucination.HallucinationFlags.None, Hallucination.HallucinationClass.Chaser, 1f, 50f, 5f, true);

            radioSource = radioClone.GetComponentInChildren<AudioSource>();

            radioClone.SetActive(false);
        }

        private bool Is3AM()
        {
            // 24 hour time, for consistency!
            int currentHour = int.Parse(System.DateTime.Now.ToString("HH", System.Globalization.CultureInfo.InvariantCulture));

            // 3 AM
            if (currentHour == 03)
            {
                return true;
            }

            return false;
        }

        private Transform FindPlayer()
        {
            // Code lifted from the Boneworks Modding Toolkit.

            GameObject[] array = GameObject.FindGameObjectsWithTag("Player");

            for (int i = 0; i < array.Length; i++)
            {
                bool isTrigger = array[i].name == "PlayerTrigger";

                if (isTrigger)
                {
                    return array[i].transform;
                }
            }

            return null;
        }

        private Transform FindHead()
		{
            return GameObject.Find("[RigManager (Default Brett)]/[PhysicsRig]/").transform;
		}

        private void Update()
        {
			if (Paranoia.instance.isBlankBox)
			{
                for (int i = 0; i < ticks.Count; i++) { ticks[i].Update(); }

                if (isDark)
                {
                    for (int i = 0; i < darkTicks.Count; i++) { darkTicks[i].Update(); }
                }
            }
        }

        internal void AudioRoutine()
        {
            if (firstRadioSpawn) { return; }

            aAmbienceTick.maxTick = Random.Range(rng, 150);

            bool isRareNumber = rng >= 20 && rng <= 45 || rng >= 50 && rng <= 75;

            if (isRareNumber)
            {
                manager.PlayOneShot(manager.ambientScreaming[Random.Range(0, manager.ambientScreaming.Count)]);
            }
            else
            {
                manager.PlayOneShot(manager.ambientGeneric[Random.Range(0, manager.ambientGeneric.Count)]);
            }
        }

        internal void TPoseEvent()
        {
            eTPoseTick.maxTick = Random.Range(rng, 150);
            bool isRareNumber = rng >= 20 && rng <= 29 || rng >= 25 && rng <= 30;

            if (isRareNumber)
            {
                if (playerTrigger != null)
                {
                    try
                    {
                        UnhollowerBaseLib.Il2CppArrayBase<StressLevelZero.AI.AIBrain> brains = GameObject.FindObjectsOfType<StressLevelZero.AI.AIBrain>();

                        foreach (StressLevelZero.AI.AIBrain brain in brains)
                        {
                            Transform t = brain.transform;

                            if (t.gameObject != null)
                            {
                                if (t.Find("Physics") && t.Find("AiRig"))
                                {
                                    Transform physicsGrp = t.Find("Physics");
                                    Transform aiGrp = t.Find("AiRig");

                                    physicsGrp.gameObject.SetActive(false);
                                    aiGrp.gameObject.SetActive(false);

                                    t.GetComponent<StressLevelZero.Combat.VisualDamageController>().enabled = false;
                                    t.GetComponent<StressLevelZero.AI.AIBrain>().enabled = false;
                                    t.GetComponent<Arena_EnemyReference>().enabled = false;

                                    if (t.Find("Physics/Root_M/Spine_M/Chest_M/Head_M/Jaw_M/Head_JawEndSHJnt") != null)
                                    {
                                        Material eyeMat = t.Find("brettEnemy@neutral/geoGrp/brett_face").GetComponent<SkinnedMeshRenderer>().materials.FirstOrDefault((mat) => mat.name.Contains("mat_Brett_eye"));
                                        eyeMat.color = new Color(0f, 0f, 0f, 0f);
                                        Transform jaw = t.Find("Physics/Root_M/Spine_M/Chest_M/Head_M/Jaw_M/Head_JawEndSHJnt");
                                        jaw.localPosition = new Vector3(jaw.localPosition.x, -0.35f, jaw.localPosition.z);
                                    }

                                    t.localPosition = new Vector3(t.localPosition.x, 0f, t.localPosition.z);

                                    Vector3 lookRotation = Quaternion.LookRotation(FindPlayer().forward).eulerAngles;
                                    t.eulerAngles = new Vector3(t.eulerAngles.x, lookRotation.y, t.eulerAngles.z);
                                }
                            }
                        }

                        MelonCoroutines.Start(CoResetTPosedEnemies(5f));
                    }
                    catch
                    {

                    }
                }
            }
            else
            {
                return;
            }
        }

        internal void SpawnFirstRadio()
        {
            radioClone.SetActive(false);

            radioClone.transform.position = playerCircle.CalculatePlayerCircle(Random.Range(0, 360), 10f);

            radioClone.transform.LookAt(FindPlayer());

            radioSource.clip = manager.startingTune;
            radioSource.spatialBlend = 0.75f;

            firstRadioSpawn = true;

            radioClone.SetActive(true);

            MelonLoader.MelonCoroutines.Start(CoRadioHide(radioSource.clip.length));

            eFirstRadioTick.OnTick -= SpawnFirstRadio;
        }

        internal void SpawnRadio()
        {
            if (firstRadioSpawn) { return; }

            radioClone.SetActive(false);

            radioClone.transform.position = playerCircle.CalculatePlayerCircle(Random.Range(0, 360), 10f);

            radioClone.transform.LookAt(FindPlayer());

            radioSource.clip = manager.radioTunes[Random.Range(0, manager.radioTunes.Count)];
            radioSource.spatialBlend = 1f;

            radioClone.SetActive(true);

            MelonCoroutines.Start(CoRadioHide(radioSource.clip.length));
        }

        internal void MoveAIToPoint(Vector3 point, bool fallThroughWorld)
        {
            try
            {
                UnhollowerBaseLib.Il2CppArrayBase<StressLevelZero.AI.AIBrain> brains = GameObject.FindObjectsOfType<StressLevelZero.AI.AIBrain>();

                foreach (StressLevelZero.AI.AIBrain brain in brains)
                {
                    Transform ai = brain.transform;
                    MelonCoroutines.Start(CoMoveAIToPoint(ai, point, fallThroughWorld));
                }
            }
            catch
            {

            }
        }

        internal void MoveAIToPlayer()
        {
            try
            {
                if (GameObject.FindObjectsOfType<StressLevelZero.AI.AIBrain>() != null)
                {
                    UnhollowerBaseLib.Il2CppArrayBase<StressLevelZero.AI.AIBrain> brains = GameObject.FindObjectsOfType<StressLevelZero.AI.AIBrain>();

                    foreach (StressLevelZero.AI.AIBrain brain in brains)
                    {
                        Transform t = brain.transform;
                        MelonCoroutines.Start(CoMoveAIToPlayer(t));
                    }
                }
            }
            catch
            {

            }
        }

        internal void KillAIRandomly()
        {
            try
            {
                if (GameObject.FindObjectsOfType<StressLevelZero.AI.AIBrain>() != null)
                {
                    UnhollowerBaseLib.Il2CppArrayBase<StressLevelZero.AI.AIBrain> brains = GameObject.FindObjectsOfType<StressLevelZero.AI.AIBrain>();

                    foreach (StressLevelZero.AI.AIBrain brain in brains)
                    {
                        Transform ai = brain.transform;
                        if (ai.GetComponentInParent<StressLevelZero.AI.AIBrain>() != null)
                        {
                            StressLevelZero.AI.AIBrain parent = ai.GetComponentInParent<StressLevelZero.AI.AIBrain>();

                            PuppetMasta.PuppetMaster puppetMaster = parent.GetComponentInChildren<PuppetMasta.PuppetMaster>();
                            PuppetMasta.SubBehaviourHealth hp = parent.GetComponentInChildren<PuppetMasta.BehaviourBaseNav>().health;
                            hp.Kill();
                            puppetMaster.Kill();

                            eKillAllTick.maxTick = Random.Range(180f, 280f);
                        }
                    }
                }
            }
            catch
            {

            }
        }

        internal void KillNimbus()
        {
            if (GameObject.FindObjectOfType<StressLevelZero.Props.Weapons.FlyingGun>())
            {
                StressLevelZero.Props.Weapons.FlyingGun nimbus = GameObject.FindObjectOfType<StressLevelZero.Props.Weapons.FlyingGun>();

                nimbus.triggerGrip.ForceDetach();
            }
        }

        internal void KillWasp()
        {
            if (GameObject.FindObjectOfType<PuppetMasta.BehaviourHovercraft>())
            {
                PuppetMasta.BehaviourHovercraft wasp = GameObject.FindObjectOfType<PuppetMasta.BehaviourHovercraft>();
                wasp.KillStart();
            }
        }

        internal void DropHeadItem()
        {
            StressLevelZero.Props.Weapons.HandWeaponSlotReciever slot = GameObject.Find("[RigManager (Default Brett)]/[SkeletonRig (GameWorld Brett)]/Body/skull/Head/HeadSlotContainer/WeaponReciever").GetComponent<StressLevelZero.Props.Weapons.HandWeaponSlotReciever>();

            if (slot.m_WeaponHost != null)
            {
                slot.GetHost().DisableColliders();
                slot.DropWeapon();
            }
        }

        internal void ChangeClipboardText()
        {
            TextMeshPro tmp = GameObject.Find("prop_clipboard_MuseumBasement/TMP").GetComponent<TextMeshPro>();

            tmp.text = "TURN AROUND. NOT IN GAME.";
            tmp.old_text = "TURN AROUND. NOT IN GAME.";
            tmp.m_text = tmp.text;
        }

        internal IEnumerator CoLightFlickerRoutine(int iterations)
        {
            int i = 0;
            bool isOn = false;
            float random = 0f;

            PropFlashlight[] flashlights;
            VLB.VolumetricLightBeam[] lightbeams;

            Light blankBoxLight;
            VLB.VolumetricLightBeam centerLightBeam;

            if (GameObject.FindObjectsOfType<PropFlashlight>() != null && GameObject.FindObjectsOfType<VLB.VolumetricLightBeam>() != null)
            {
                flashlights = GameObject.FindObjectsOfType<PropFlashlight>();
                lightbeams = GameObject.FindObjectsOfType<VLB.VolumetricLightBeam>();

                blankBoxLight = GameObject.FindObjectOfType<CustomLightMachine>().light;
                centerLightBeam = GameObject.FindObjectOfType<CustomLightMachine>().lightBeam;

                foreach (PropFlashlight flashlight in flashlights)
                {
                    foreach (VLB.VolumetricLightBeam lightbeam in lightbeams)
                    {
                        for (i = 0; i < iterations; i++)
                        {
                            yield return new WaitForSeconds(0.10f);

                            random = Random.Range(1, iterations);

                            if (blankBoxLight != null || lightBeam != null)
                            {
                                if ((i * random / 2) % 2 == 0)
                                {
                                    isOn = false;
                                    blankBoxLight.gameObject.SetActive(false);
                                    lightBeam.gameObject.SetActive(false);
                                    LightmapSettings.lightmaps = new LightmapData[0];
                                    LightmapSettings.lightProbes.bakedProbes = null;
                                }
                                else
                                {
                                    isOn = true;
                                    blankBoxLight.gameObject.SetActive(true);
                                    lightBeam.gameObject.SetActive(true);
                                    LightmapSettings.lightmaps = lightmaps;
                                    LightmapSettings.lightProbes.bakedProbes = new UnhollowerBaseLib.Il2CppStructArray<UnityEngine.Rendering.SphericalHarmonicsL2>(bakedProbes.Count);
                                    LightmapSettings.lightProbes.bakedProbes = bakedProbes;
                                }

                                DynamicGI.UpdateEnvironment();
                            }

                            GameObject lightStuff = flashlight.GetComponent<PropFlashlight>().LightStuff;
                            GameObject beam = lightBeam.gameObject;

                            lightStuff.active = (i * random / 2) % 2 == 0;
                            beam.active = (i * random / 2) % 2 == 0;
                        }
                    }
                }
            }

            if (!isOn)
            {
                isDark = true;
            }

            yield return null;
        }

        internal IEnumerator CoRadioHide(float time)
        {
            yield return new WaitForSeconds(time);

            firstRadioSpawn = false;

            if (radioClone.activeInHierarchy)
            {
                radioClone.SetActive(false);
            }
        }

        internal IEnumerator CoResetTPosedEnemies(float seconds)
        {
            yield return new WaitForSeconds(seconds);

            UnhollowerBaseLib.Il2CppArrayBase<StressLevelZero.AI.AIBrain> brains = GameObject.FindObjectsOfType<StressLevelZero.AI.AIBrain>();

            for (int i = 0; i < brains.Count; i++)
            {
                Transform t = brains[i].transform;
                Transform physicsGrp = t.Find("Physics");
                Transform aiGrp = t.Find("AiRig");

                physicsGrp.gameObject.SetActive(true);
                aiGrp.gameObject.SetActive(true);

                t.GetComponent<StressLevelZero.Combat.VisualDamageController>().enabled = true;
                t.GetComponent<StressLevelZero.AI.AIBrain>().enabled = true;
                t.GetComponent<Arena_EnemyReference>().enabled = true;

                if (t.Find("Physics/Root_M/Spine_M/Chest_M/Head_M/Jaw_M/Head_JawEndSHJnt") != null)
                {
                    Material eyeMat = t.Find("brettEnemy@neutral/geoGrp/brett_face").GetComponent<SkinnedMeshRenderer>().materials.FirstOrDefault((mat) => mat.name.Contains("mat_Brett_eye"));
                    Transform jaw = t.Find("Physics/Root_M/Spine_M/Chest_M/Head_M/Jaw_M/Head_JawEndSHJnt");
                    jaw.localPosition = new Vector3(jaw.localPosition.x, -0.044f, jaw.localPosition.z);
                }

                t.gameObject.SetActive(false);
            }
        }

        internal IEnumerator CoMoveAIToPoint(Transform ai, Vector3 point, bool fallThroughWorld)
        {
            if (ai != null)
            {
                if (ai.GetComponentInParent<StressLevelZero.AI.AIBrain>() != null)
                {
                    Transform parent = ai.GetComponentInParent<StressLevelZero.AI.AIBrain>().transform;

                    if (parent.GetComponentInChildren<PuppetMasta.BehaviourBaseNav>())
                    {
                        PuppetMasta.BehaviourBaseNav baseNav = parent.GetComponentInChildren<PuppetMasta.BehaviourBaseNav>();

                        baseNav.sensors.hearingSensitivity = 0f;
                        baseNav.sensors.visionFov = 0f;
                        baseNav.sensors._visionSphere.enabled = false;
                        baseNav.breakAgroHomeDistance = 0f;

                        if (baseNav.mentalState != PuppetMasta.BehaviourBaseNav.MentalState.Rest)
                        {
                            baseNav.SwitchMentalState(PuppetMasta.BehaviourBaseNav.MentalState.Rest);
                        }

                        baseNav.SetHomePosition(point, true);

                        while (Vector3.Distance(baseNav.transform.position, point) > 2f) { yield return null; }

                        if (fallThroughWorld)
                        {
                            foreach (Collider c in ai.GetComponentsInChildren<Collider>())
                            {
                                c.enabled = false;
                            }

                            yield return new WaitForSeconds(1f);

                            foreach (Collider c in ai.GetComponentsInChildren<Collider>())
                            {
                                c.enabled = true;
                            }

                            ai.gameObject.SetActive(false);
                        }

                        yield return null;
                    }
                }
                else
                {
                    yield return null;
                }
            }
        }

        internal IEnumerator CoMoveAIToPlayer(Transform ai)
        {
            if (playerTrigger != null)
            {
                if (ai.GetComponentInParent<StressLevelZero.AI.AIBrain>() != null)
                {
                    Transform parent = ai.GetComponentInParent<StressLevelZero.AI.AIBrain>().transform;
                    PuppetMasta.BehaviourBaseNav baseNav = parent.GetComponentInChildren<PuppetMasta.BehaviourBaseNav>();

                    baseNav.sensors.hearingSensitivity = 0f;
                    baseNav.sensors.visionFov = 0f;
                    baseNav.sensors._visionSphere.enabled = false;
                    baseNav.breakAgroHomeDistance = 0f;

                    if (baseNav.mentalState != PuppetMasta.BehaviourBaseNav.MentalState.Rest)
                    {
                        baseNav.SwitchMentalState(PuppetMasta.BehaviourBaseNav.MentalState.Rest);
                    }

                    baseNav.SetHomePosition(FindPlayer().position, true);

                    while (Vector3.Distance(baseNav.transform.position, FindPlayer().position) > 0.25f) { yield return null; }

                    yield return null;
                }
                else
                {
                    yield return null;
                }
            }
        }
    }
}