using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using ModThatIsNotMod.BoneMenu;

using TMPro;

using UnityEngine;

using UnhollowerBaseLib;
using UnhollowerRuntimeLib;

using MelonLoader;

using HName = NotEnoughPhotons.paranoia.Hallucination.HallucinationName;
using HType = NotEnoughPhotons.paranoia.Hallucination.HallucinationType;
using HClass = NotEnoughPhotons.paranoia.Hallucination.HallucinationClass;
using HFlags = NotEnoughPhotons.paranoia.Hallucination.HallucinationFlags;

namespace NotEnoughPhotons.paranoia
{
    public class ParanoiaGameManager : MonoBehaviour
    {
        public ParanoiaGameManager(System.IntPtr ptr) : base(ptr) { }

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

        public static ParanoiaGameManager instance;

        internal static bool debug = true;

        public List<Tick> ticks;
        public List<Tick> darkTicks;

        // Main prefabs
        public GameObject shadowMan;
        public GameObject staringMan;
        public GameObject ceilingWatcher;
        public GameObject observer;
        public GameObject radioObject;
        public GameObject monitorObject;

        public SpawnCircle playerCircle;

        public List<UnityEngine.Video.VideoClip> clipList;

        public Vector3[] staringManSpawns = new Vector3[3]
        {
            new Vector3(-53.9f, 1f, -55.1f),
            new Vector3(-53.7f, 1f, 32.1f),
            new Vector3(52.1f, 1f, 54.4f)
        };

        public Vector3[] ceilingManSpawns = new Vector3[3]
        {
            new Vector3(-53.9f, 75f, -55.1f),
            new Vector3(-53.7f, 75f, 32.1f),
            new Vector3(52.1f, 75f, 54.4f)
        };

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
        private Hallucination hObserver;

        private VLB.VolumetricLightBeam lightBeam;
        private Light blankBoxLight;
        private GameObject flashlightObject;

        private GameObject radioClone;
        private GameObject monitorClone;
        private AudioSource radioSource;

        // Audio ticks
        private Tick aAmbienceTick;
        private Tick aChaserTick;
        private Tick aDarkVoiceTick;

        // Visual ticks
        private Tick vShadowManTick;
        private Tick vStaringManTick;
        private Tick vCeilingManTick;
        private Tick vObserverTick;

        // Event ticks
        private Tick eTPoseTick;
        private Tick eRadioTick;
        private Tick eMonitorTick;
        private Tick eFirstRadioTick;
        private Tick eAIOriginTick;
        private Tick eKillAllTick;
        private Tick eItemDropTick;
        private Tick eLightFlickerTick;
        private Tick eMapGeoFlickerTick;

        private Tick rngGeneratorTick;

        private bool firstRadioSpawn = false;
        private bool isDark = false;

        private int rng = 1;

        internal int insanity;

        private SpawnCircle[] spawnCircles = new SpawnCircle[3];

        private Il2CppReferenceArray<LightmapData> lightmaps;
        private Il2CppStructArray<UnityEngine.Rendering.SphericalHarmonicsL2> bakedProbes;

        public static Hallucination CreateHallucination(Vector3 position, HName hName, HType hType, HFlags hFlags, HClass hClass, float distanceToDisappear, float chaseSpeed, float delayTime, bool usesDelay)
        {
            Hallucination hallucination = new GameObject($"{hType} {hClass} Hallucination").AddComponent<Hallucination>();
            PBillboard billboard = hallucination.gameObject.AddComponent<PBillboard>();

            billboard.target = ParanoiaUtilities.FindPlayer();
            hallucination.target = ParanoiaUtilities.FindPlayer();
            hallucination.cameraTarget = ParanoiaUtilities.FindHead();
            hallucination.audioManager = FindObjectOfType<AudioManager>();

            if (hType.HasFlag(Hallucination.HallucinationType.Auditory))
            {
                AudioSource source = hallucination.gameObject.AddComponent<AudioSource>();
                hallucination.source = source;
                source.dopplerLevel = 0;
            }

            hallucination.transform.position = position;
            hallucination.gameObject.SetActive(false);
            hallucination.Initialize(hName, hType, hFlags, hClass, distanceToDisappear, chaseSpeed, delayTime);
            return hallucination;
        }

        public static Hallucination CreateHallucination(Vector3 position, GameObject prefab, HName hName, HType hType, HFlags hFlags, HClass hClass, float distanceToDisappear, float chaseSpeed, float delayTime, bool usesDelay)
        {
            Hallucination hallucination = new GameObject($"{hType} {hClass} Hallucination").AddComponent<Hallucination>();
            PBillboard billboard = hallucination.gameObject.AddComponent<PBillboard>();

            billboard.target = ParanoiaUtilities.FindPlayer();
            hallucination.target = ParanoiaUtilities.FindPlayer();
            hallucination.cameraTarget = ParanoiaUtilities.FindHead();
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
            hallucination.Initialize(hName, hType, hFlags, hClass, distanceToDisappear, chaseSpeed, delayTime);
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
        }

        private void Start()
        {
            InitializeEntities();

            InitializeTicks();

            InitializeSettings();

            manager = FindObjectOfType<AudioManager>();

            playerTrigger = ParanoiaUtilities.FindPlayer();

            playerCircle = new SpawnCircle(ParanoiaUtilities.FindPlayer());

            for (int i = 0; i < spawnCircles.Length; i++)
            {
                spawnCircles[i] = new SpawnCircle(new GameObject("Spawn Circle").transform);
                spawnCircles[i].radius = 25f;
                spawnCircles[i].originTransform.position = staringManSpawns[i];
            }

            lightmaps = LightmapSettings.lightmaps;
            bakedProbes = LightmapSettings.lightProbes.bakedProbes;
        }

        private void InitializeSettings()
        {
            MenuCategory mainCategory = MenuManager.CreateCategory("Paranoia", Color.gray);
            MenuCategory optionsMenu = mainCategory.CreateSubCategory("Options", Color.cyan);
            MenuCategory debugMenu = mainCategory.CreateSubCategory("Debug", Color.red);

            optionsMenu.CreateBoolElement("Debug Mode", Color.blue, debug, null);

            debugMenu.CreateFunctionElement("Start Ambience", Color.white, new System.Action(() => AudioRoutine()));
            debugMenu.CreateFunctionElement("Start Chaser", Color.white, new System.Action(() => hChaser.gameObject.SetActive(true)));
            debugMenu.CreateFunctionElement("Start Dark Voice", Color.white, new System.Action(() => hDarkVoice.gameObject.SetActive(true)));

            debugMenu.CreateFunctionElement("Create Shadow Person", Color.white, new System.Action(() => hShadowPerson.gameObject.SetActive(true)));
            debugMenu.CreateFunctionElement("Create Shadow Person Chaser", Color.white, new System.Action(() => hShadowPersonChaser.gameObject.SetActive(true)));
            debugMenu.CreateFunctionElement("Create Staring Man", Color.white, new System.Action(() => hStaringMan.gameObject.SetActive(true)));
            debugMenu.CreateFunctionElement("Create Ceiling Man", Color.white, new System.Action(() => hCeilingMan.gameObject.SetActive(true)));
            debugMenu.CreateFunctionElement("Create Observer", Color.white, new System.Action(() => hObserver.gameObject.SetActive(true)));

            debugMenu.CreateFunctionElement("Start T Pose Event", Color.white, new System.Action(() => TPoseEvent()));
            debugMenu.CreateFunctionElement("Start Radio Event", Color.white, new System.Action(() => SpawnRadio()));
            debugMenu.CreateFunctionElement("Start Monitor Event", Color.white, new System.Action(() => SpawnMonitor()));
            debugMenu.CreateFunctionElement("Start First Radio", Color.white, new System.Action(() => SpawnFirstRadio()));
            debugMenu.CreateFunctionElement("Start AI Origin", Color.white, new System.Action(() => MoveAIToPoint(Vector3.zero, true)));
            debugMenu.CreateFunctionElement("Start Kill All AI", Color.white, new System.Action(() => KillAIRandomly()));
            debugMenu.CreateFunctionElement("Start Item Drop", Color.white, new System.Action(() => DropHeadItem()));
            debugMenu.CreateFunctionElement("Start Light Flicker", Color.white, new System.Action(() => MelonCoroutines.Start(CoLightFlickerRoutine(5))));
            debugMenu.CreateFunctionElement("Start Geo Flicker", Color.white, new System.Action(() => MelonCoroutines.Start(CoFlickerMapGeometry(5))));
            debugMenu.CreateFunctionElement("Start RNG", Color.white, new System.Action(() => rng = Random.Range(1, 300)));
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
            vCeilingManTick     = new Tick(260f, Tick.TickType.TT_LIGHT | Tick.TickType.TT_DARK);
            vObserverTick       = new Tick(360f, Tick.TickType.TT_LIGHT | Tick.TickType.TT_DARK);

            // Event tick initialization
            eTPoseTick          = new Tick(120f, Tick.TickType.TT_LIGHT | Tick.TickType.TT_DARK);
            eRadioTick          = new Tick(190f, Tick.TickType.TT_LIGHT | Tick.TickType.TT_DARK);
            eMonitorTick        = new Tick(90f, Tick.TickType.TT_LIGHT | Tick.TickType.TT_DARK);
            eFirstRadioTick     = new Tick(30f, Tick.TickType.TT_LIGHT | Tick.TickType.TT_DARK);
            eAIOriginTick       = new Tick(260f, Tick.TickType.TT_LIGHT | Tick.TickType.TT_DARK);
            eKillAllTick        = new Tick(240f, Tick.TickType.TT_LIGHT | Tick.TickType.TT_DARK);
            eItemDropTick       = new Tick(15f, Tick.TickType.TT_LIGHT | Tick.TickType.TT_DARK);
            eLightFlickerTick   = new Tick(90f, Tick.TickType.TT_LIGHT | Tick.TickType.TT_DARK);
            eMapGeoFlickerTick  = new Tick(30f, Tick.TickType.TT_LIGHT | Tick.TickType.TT_DARK);

            // Global tick initialization
            rngGeneratorTick    = new Tick(1f, Tick.TickType.TT_LIGHT | Tick.TickType.TT_DARK);

            // Audio tick subscription
            aAmbienceTick.OnTick += AudioRoutine;
            aChaserTick.OnTick += new System.Action(() => hChaser.gameObject.SetActive(true));
            aDarkVoiceTick.OnTick += new System.Action(() => hDarkVoice.gameObject.SetActive(true));

            // Visual tick subscription
            vShadowManTick.OnTick += new System.Action(() => 
            {
                if(insanity > 2)
                {
                    if(rng >= 25 || rng <= 30)
                    {
                        hShadowPersonChaser.gameObject.SetActive(true);
                    }
                    else
                    {
                        hShadowPerson.gameObject.SetActive(true);
                    }
                }
            });

            vStaringManTick.OnTick += new System.Action(() => 
            {
                if(insanity > 1)
                {
                    hStaringMan.gameObject.SetActive(true);
                }
            });

            vCeilingManTick.OnTick += new System.Action(() => 
            {
                if(insanity > 2)
                {
                    hCeilingMan.gameObject.SetActive(true);
                }
            });

            vObserverTick.OnTick += new System.Action(() => 
            { 
                if(insanity > 3)
                {
                    hObserver.gameObject.SetActive(true);
                }
            });

            // Event tick subscription
            eTPoseTick.OnTick += TPoseEvent;
            eRadioTick.OnTick += new System.Action(() => 
            { 
                if(insanity > 0)
                {
                    SpawnRadio();
                    MoveAIToPoint(radioClone.transform.position, false);
                }
            });

            eMonitorTick.OnTick += new System.Action(() =>
            {
                if(insanity != 5)
                {
                    SpawnMonitor();
                }
            });

            eFirstRadioTick.OnTick += SpawnFirstRadio;
            eAIOriginTick.OnTick += new System.Action(() => MoveAIToPoint(Vector3.zero, true));
            eKillAllTick.OnTick += KillAIRandomly;
            eItemDropTick.OnTick += DropHeadItem;
            eLightFlickerTick.OnTick += new System.Action(() =>
            {
                KillNimbus();
                KillWasp();
                eLightFlickerTick.maxTick = Random.Range(120f, 180f);
                MelonCoroutines.Start(CoLightFlickerRoutine(Random.Range(4, 8)));
            });
            eMapGeoFlickerTick.OnTick += new System.Action(() => MelonCoroutines.Start(CoFlickerMapGeometry(Random.Range(4, 8))));

            // Global tick subscription
            rngGeneratorTick.OnTick += new System.Action(() => rng = Random.Range(1, 300));
        }

        private void InitializeEntities()
		{
            radioClone = GameObject.Instantiate(radioObject);
            monitorClone = GameObject.Instantiate(monitorObject);

            hChaser = CreateHallucination(Vector3.zero, HName.ChaserMirage, HType.Auditory, HFlags.HideWhenClose, HClass.Chaser, 0.5f, 50f, 0f, false);
            hDarkVoice = CreateHallucination(Vector3.zero, HName.VoiceInTheDark, HType.Auditory, HFlags.None, HClass.DarkVoice, 0f, 0f, 0f, false);
            hCeilingMan = CreateHallucination(Vector3.zero, ceilingWatcher, HName.CeilingMan, HType.Auditory | HType.Visual, HFlags.HideWhenSeen, HClass.Watcher, 0f, 0f, 0f, false);
            hStaringMan = CreateHallucination(Vector3.zero, staringMan, HName.StaringMan, HType.Visual, HFlags.HideWhenSeen, HClass.Chaser, 30f, 1f, 0f, false);
            hShadowPerson = CreateHallucination(Vector3.zero, shadowMan, HName.ShadowMan, HType.Visual, HFlags.HideWhenClose, HClass.Watcher, 30f, 0f, 0f, false);
            hShadowPersonChaser = CreateHallucination(Vector3.zero, shadowMan, HName.ShadowMan, HType.Visual, HFlags.HideWhenClose, HClass.Chaser, 1f, 50f, 5f, true);
            hObserver = CreateHallucination(Vector3.zero, observer, HName.Observer, HType.Visual, HFlags.HideWhenSeen, HClass.Watcher, 0f, 0f, 0f, false);

            radioSource = radioClone.GetComponentInChildren<AudioSource>();
            MonitorVideo monitorVideo = monitorClone.AddComponent<MonitorVideo>();
            monitorVideo.clips = clipList;

            radioClone.SetActive(false);
            monitorClone.SetActive(false);
        }

        private void Update()
        {
			if (Paranoia.instance.isBlankBox)
			{
                playerCircle.CalculatePlayerCircle(0f);

                if (debug) { return; }

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
            if(insanity > 1)
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

                                        Vector3 lookRotation = Quaternion.LookRotation(ParanoiaUtilities.FindPlayer().forward).eulerAngles;
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
        }

        internal void SpawnFirstRadio()
        {
            MelonLogger.Msg("Setting inactive");
            radioClone.SetActive(false);

            MelonLogger.Msg("Setting position");
            radioClone.transform.position = playerCircle.CalculatePlayerCircle(Random.Range(0, 360), 10f);

            MelonLogger.Msg("Looking at player");
            radioClone.transform.LookAt(ParanoiaUtilities.FindPlayer());

            MelonLogger.Msg("Setting clip");
            radioSource.clip = manager.startingTune;
            MelonLogger.Msg("Setting spatial blend");
            radioSource.spatialBlend = 0.75f;

            MelonLogger.Msg("Setting first radio spawn to true");
            firstRadioSpawn = true;

            MelonLogger.Msg("Setting active");
            radioClone.SetActive(true);

            MelonLogger.Msg("Starting coroutine");
            MelonLoader.MelonCoroutines.Start(CoRadioHide(radioSource.clip.length));

            MelonLogger.Msg("Unsubscribing");
            eFirstRadioTick.OnTick -= SpawnFirstRadio;
        }

        internal void SpawnRadio()
        {
            if (firstRadioSpawn) { return; }

            radioClone.SetActive(false);

            radioClone.transform.position = playerCircle.CalculatePlayerCircle(Random.Range(0, 360), 10f);

            radioClone.transform.LookAt(ParanoiaUtilities.FindPlayer());

            radioSource.clip = manager.radioTunes[Random.Range(0, manager.radioTunes.Count)];
            radioSource.spatialBlend = 1f;

            radioClone.SetActive(true);

            MelonCoroutines.Start(CoRadioHide(radioSource.clip.length));
        }

        internal void SpawnMonitor()
        {
            //if (firstRadioSpawn) { return; }

            monitorClone.transform.position = playerCircle.CalculatePlayerCircle(Random.Range(0, 360), 10f);

            monitorClone.transform.LookAt(ParanoiaUtilities.FindPlayer());

            monitorClone.SetActive(true);

            insanity++;
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

            if (GameObject.FindObjectsOfType<VLB.VolumetricLightBeam>() != null)
            {
                Renderer[] renderers = GameObject.FindObjectsOfType<Renderer>();
                VLB.VolumetricLightBeam[] lightbeams = GameObject.FindObjectsOfType<VLB.VolumetricLightBeam>();

                Light blankBoxLight = GameObject.FindObjectOfType<CustomLightMachine>().light;

                foreach (VLB.VolumetricLightBeam lightbeam in lightbeams)
                {
                    for (i = 0; i < iterations; i++)
                    {
                        yield return new WaitForSeconds(0.05f);

                        random = Random.Range(1, iterations);

                        if (blankBoxLight != null || lightBeam != null)
                        {
                            if ((i * random / 2) * rng % 2 == 0)
                            {
                                isOn = false;
                                blankBoxLight.gameObject.SetActive(false);
                                LightmapSettings.lightmaps = new LightmapData[0];
                                LightmapSettings.lightProbes.bakedProbes = null;
                            }
                            else
                            {
                                isOn = true;
                                blankBoxLight.gameObject.SetActive(true);
                                LightmapSettings.lightmaps = lightmaps;
                                LightmapSettings.lightProbes.bakedProbes = new UnhollowerBaseLib.Il2CppStructArray<UnityEngine.Rendering.SphericalHarmonicsL2>(bakedProbes.Count);
                                LightmapSettings.lightProbes.bakedProbes = bakedProbes;
                            }

                            DynamicGI.UpdateEnvironment();
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

        internal IEnumerator CoFlickerMapGeometry(int iterations)
        {
            List<GameObject> staticObjects = ParanoiaUtilities.FindGameObjectsWithLayer("Static");

            for (int j = 0; j < iterations; j++)
            {
                int random = iterations * Random.RandomRange(1, 300);

                if ((j * random) * iterations % 2 == 0)
                {
                    staticObjects[Random.Range(0, staticObjects.Count)].SetActive(false);
                }
                else
                {
                    staticObjects[Random.Range(0, staticObjects.Count)].SetActive(true);
                }
            }

            yield return null;
        }

        internal IEnumerator CoRadioHide(float time)
        {
            MelonLogger.Msg("Waiting for " + time + " seconds...");
            yield return new WaitForSeconds(time);

            MelonLogger.Msg("First radio spawn is done for");
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

                    baseNav.SetHomePosition(ParanoiaUtilities.FindPlayer().position, true);

                    while (Vector3.Distance(baseNav.transform.position, ParanoiaUtilities.FindPlayer().position) > 0.25f) { yield return null; }

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