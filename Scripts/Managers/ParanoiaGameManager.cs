using System.Collections;
using System.Collections.Generic;
using System.Linq;

using ModThatIsNotMod.BoneMenu;

using NotEnoughPhotons.Paranoia.Entities;

using NotEnoughPhotons.Paranoia.TickEvents;
using NotEnoughPhotons.Paranoia.TickEvents.Events;
using NotEnoughPhotons.Paranoia.TickEvents.Mirages;

using NotEnoughPhotons.Paranoia.Utilities;

using TMPro;

using UnityEngine;

using UnhollowerBaseLib;

using MelonLoader;

namespace NotEnoughPhotons.Paranoia.Managers
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

            public Tick(float maxTick, TickType tickType, ParanoiaEvent Event)
            {
                this.maxTick = maxTick;
                this.tickType = tickType;
                this.Event = Event;

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

            internal ParanoiaEvent Event;

            internal void Update()
            {
                tick += Time.deltaTime;

                if (tick >= maxTick)
                {
                    Event?.Start();
                    tick = 0f;
                }
            }
        }

        public static ParanoiaGameManager instance;

        internal static bool debug = false;

        public List<Tick> ticks;
        public List<Tick> darkTicks;

        // Main prefabs
        public GameObject shadowMan;
        public GameObject staringMan;
        public GameObject ceilingWatcher;
        public GameObject observer;
        public GameObject teleportingEntity;
        public GameObject radioObject;
        public GameObject monitorObject;
        public GameObject cursedDoorObject;

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

        private Transform _playerTrigger;
        public Transform playerTrigger { get { return _playerTrigger; } }

        private Transform playerHead;
        
        private AudioManager _audioManager;
        public AudioManager audioManager { get { return _audioManager; } }
        
        // Hallucinations
        private List<BaseHallucination> baseHallucinations;
        private List<AudioHallucination> audioHallucinations;
        
        private AudioHallucination _hChaser;
        public AudioHallucination hChaser { get => _hChaser; }
        
        private AudioHallucination _hDarkVoice;
        public AudioHallucination hDarkVoice { get => _hDarkVoice; }
        
        private AudioHallucination _hTeleportingEntity;
        public AudioHallucination hTeleportingEntity { get => _hTeleportingEntity; }
        
        private BaseHallucination _hCeilingMan;
        public BaseHallucination hCeilingMan { get => _hCeilingMan; }
        
        private BaseHallucination _hStaringMan;
        public BaseHallucination hStaringMan { get => _hStaringMan; }
        
        private BaseHallucination _hShadowPerson;
        public BaseHallucination hShadowPerson { get => _hShadowPerson; }
        
        private BaseHallucination _hShadowPersonChaser;
        public BaseHallucination hShadowPersonChaser { get => _hShadowPersonChaser; }
        
        private BaseHallucination _hObserver;
        public BaseHallucination hObserver { get => _hObserver; }
        
        private BaseHallucination _hCursedDoor;
        public BaseHallucination hCursedDoor { get => _hCursedDoor; }
        
        private VLB.VolumetricLightBeam _lightBeam;
        public VLB.VolumetricLightBeam lightBeam { get { return _lightBeam; } }
        
        private Light blankBoxLight;
        private GameObject flashlightObject;
        
        private GameObject _radioClone;
        public GameObject radioClone { get { return _radioClone; } }
        
        private GameObject monitorClone;
        private GameObject cursedDoorClone;
        
        private AudioSource _radioSource;
        public AudioSource radioSource { get { return _radioSource; } }
        
        // Audio ticks
        private Tick aAmbienceTick;
        private Tick aChaserTick;
        private Tick aDarkVoiceTick;
        private Tick aTeleportingEntTick;

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

        private bool _firstRadioSpawn = false;
        public bool firstRadioSpawn { get { return _firstRadioSpawn; } }

        private bool _isDark = false;
        public bool isDark { get {  return _isDark; } }

        private int _rng = 1;
        public int rng {  get { return _rng; } }

        internal int insanity;

        private SpawnCircle[] spawnCircles = new SpawnCircle[3];

        private Il2CppReferenceArray<LightmapData> _lightmaps;
        public Il2CppReferenceArray<LightmapData> lightmaps { get { return _lightmaps; } }

        private Il2CppStructArray<UnityEngine.Rendering.SphericalHarmonicsL2> _bakedProbes;
        public Il2CppStructArray<UnityEngine.Rendering.SphericalHarmonicsL2> bakedProbes { get { return _bakedProbes; } }


        public void SetFirstRadioSpawn(bool condition)
        {
            _firstRadioSpawn = condition;
        }

        public void SetIsDark(bool condition)
        {
            _isDark = condition;
        }

        public void SetRNG(int rng)
        {
            this._rng = rng;
        }

        private void Awake()
		{
            if(instance == null)
			{
                instance = this;
			}
			else
			{
                Destroy(instance.gameObject);
			}
        }

        private void Start()
        {
            baseHallucinations = new List<BaseHallucination>();
            audioHallucinations = new List<AudioHallucination>();

            InitializeTicks();

            InitializeEntities();

            _audioManager = FindObjectOfType<AudioManager>();

            _playerTrigger = ParanoiaUtilities.FindPlayer();

            playerCircle = new SpawnCircle(ParanoiaUtilities.FindPlayer());

            for (int i = 0; i < spawnCircles.Length; i++)
            {
                spawnCircles[i] = new SpawnCircle(new GameObject("Spawn Circle").transform);
                spawnCircles[i].radius = 25f;
                spawnCircles[i].originTransform.position = staringManSpawns[i];
            }

            _lightmaps = LightmapSettings.lightmaps;
            _bakedProbes = LightmapSettings.lightProbes.bakedProbes;
        }

        private void InitializeTicks()
        {
            ticks = new List<Tick>();
            darkTicks = new List<Tick>();

            // Audio tick initialization
            aAmbienceTick       = new Tick(Random.Range(60f, 95f), Tick.TickType.TT_LIGHT | Tick.TickType.TT_DARK, new AmbientAudioSpawn());
            aChaserTick         = new Tick(Random.Range(90f, 125f), Tick.TickType.TT_LIGHT | Tick.TickType.TT_DARK, new AmbientChaserSpawn());
            aDarkVoiceTick      = new Tick(Random.Range(15f, 20f), Tick.TickType.TT_DARK, new AmbientDarkVoiceSpawn());
            aTeleportingEntTick = new Tick(Random.Range(120f, 165f), Tick.TickType.TT_DARK | Tick.TickType.TT_LIGHT, new AmbientTeleEntSpawn());

            // Visual tick initialization
            vShadowManTick      = new Tick(Random.Range(90f, 125f), Tick.TickType.TT_LIGHT | Tick.TickType.TT_DARK, new ShadowSpawn());
            vStaringManTick     = new Tick(Random.Range(180f, 210f), Tick.TickType.TT_LIGHT | Tick.TickType.TT_DARK, new StaringManSpawn());
            vCeilingManTick     = new Tick(260f, Tick.TickType.TT_LIGHT | Tick.TickType.TT_DARK, new CeilingManSpawn());
            vObserverTick       = new Tick(360f, Tick.TickType.TT_LIGHT | Tick.TickType.TT_DARK, new ObserverSpawn());
            
            // Event tick initialization
            eTPoseTick          = new Tick(120f, Tick.TickType.TT_LIGHT | Tick.TickType.TT_DARK, new TPose());
            eRadioTick          = new Tick(190f, Tick.TickType.TT_LIGHT | Tick.TickType.TT_DARK, new SpawnRadio());
            eMonitorTick        = new Tick(90f, Tick.TickType.TT_LIGHT | Tick.TickType.TT_DARK, new SpawnMonitor());
            eAIOriginTick       = new Tick(260f, Tick.TickType.TT_LIGHT | Tick.TickType.TT_DARK, new MoveAIToPlayer());
            eKillAllTick        = new Tick(240f, Tick.TickType.TT_LIGHT | Tick.TickType.TT_DARK, new KillAI());
            eItemDropTick       = new Tick(15f, Tick.TickType.TT_LIGHT | Tick.TickType.TT_DARK, new DropHeadItem());
            eLightFlickerTick   = new Tick(90f, Tick.TickType.TT_LIGHT | Tick.TickType.TT_DARK, new LightFlickering());
            eMapGeoFlickerTick  = new Tick(30f, Tick.TickType.TT_LIGHT | Tick.TickType.TT_DARK);
            
            //// Global tick initialization
            rngGeneratorTick    = new Tick(1f, Tick.TickType.TT_LIGHT | Tick.TickType.TT_DARK, new ChangeRNG());
        }

        private void InitializeEntities()
		{
            _radioClone = GameObject.Instantiate(radioObject);
            monitorClone = GameObject.Instantiate(monitorObject);

            SetupHallucinations();

            _radioSource = _radioClone.GetComponentInChildren<AudioSource>();
            MonitorVideo monitorVideo = monitorClone.AddComponent<MonitorVideo>();
            monitorVideo.clips = clipList;

            CursedDoorController cursedDoorCtrlr = cursedDoorClone.AddComponent<CursedDoorController>();

            _radioClone.SetActive(false);
            monitorClone.SetActive(false);
        }

        private GameObject SpawnPrefab(GameObject obj)
        {
            return GameObject.Instantiate(obj, Vector3.zero, Quaternion.identity);
        }

        private void SetupHallucinations()
        {
            GameObject chaserAudio, darkVoiceAudio, shadowManClone,
                       shadowManChaserClone, ceilingManClone, staringManClone,
                       observerClone, teleportingEntityClone;

            chaserAudio = new GameObject("ChaserMirage");
            darkVoiceAudio = new GameObject("DarkVoice");
            shadowManClone = SpawnPrefab(shadowMan);
            shadowManChaserClone = SpawnPrefab(shadowMan);
            ceilingManClone = SpawnPrefab(ceilingWatcher);
            staringManClone = SpawnPrefab(staringMan);
            observerClone = SpawnPrefab(observer);
            teleportingEntityClone = SpawnPrefab(teleportingEntity);

            cursedDoorClone = SpawnPrefab(cursedDoorObject);

            chaserAudio.AddComponent<AudioSource>();
            darkVoiceAudio.AddComponent<AudioSource>();
            _hChaser = chaserAudio.AddComponent<AudioHallucination>();
            _hDarkVoice = darkVoiceAudio.AddComponent<AudioHallucination>();
            _hTeleportingEntity = teleportingEntityClone.AddComponent<AudioHallucination>();
            
            _hShadowPerson = shadowManClone.AddComponent<BaseHallucination>();
            _hShadowPersonChaser = shadowManChaserClone.AddComponent<BaseHallucination>();
            _hCeilingMan = ceilingManClone.AddComponent<BaseHallucination>();
            _hStaringMan = staringManClone.AddComponent<BaseHallucination>();
            _hObserver = observerClone.AddComponent<BaseHallucination>();
            _hCursedDoor = cursedDoorClone.AddComponent<CursedDoorController>();
            
            _hChaser.clips = Paranoia.instance.chaserAmbience.ToArray();
            _hDarkVoice.clips = Paranoia.instance.darkVoices.ToArray();
            _hTeleportingEntity.clips = Paranoia.instance.teleporterAmbience.ToArray();
            
            _hStaringMan.spawnPoints = staringManSpawns;
            _hCeilingMan.spawnPoints = ceilingManSpawns;

            ApplyHallucinationSettings();
        }

        private void ApplyHallucinationSettings()
        {
            string baseJsonDir = "UserData/paranoia/json/BaseHallucination";
            string audioJsonDir = "UserData/paranoia/json/AudioHallucination";

            _hShadowPerson.ReadValuesFromJSON(System.IO.File.ReadAllText(baseJsonDir + "/ShadowMan.json"));
            _hShadowPersonChaser.ReadValuesFromJSON(System.IO.File.ReadAllText(baseJsonDir + "/ShadowManChaser.json"));
            _hCeilingMan.ReadValuesFromJSON(System.IO.File.ReadAllText(baseJsonDir + "/CeilingMan.json"));
            _hStaringMan.ReadValuesFromJSON(System.IO.File.ReadAllText(baseJsonDir + "/StaringMan.json"));
            _hObserver.ReadValuesFromJSON(System.IO.File.ReadAllText(baseJsonDir + "/Observer.json"));

            _hChaser.ReadValuesFromJSON(System.IO.File.ReadAllText(audioJsonDir + "/Chaser.json"));
            _hDarkVoice.ReadValuesFromJSON(System.IO.File.ReadAllText(audioJsonDir + "/DarkVoice.json"));
            _hTeleportingEntity.ReadValuesFromJSON(System.IO.File.ReadAllText(audioJsonDir + "/TeleportingEntity.json"));
        }

        private void Update()
        {
			if (Paranoia.instance.isBlankBox)
			{
                playerCircle.CalculatePlayerCircle(0f);

                if (debug || Time.timeScale == 0) { return; }

                for (int i = 0; i < ticks.Count; i++) { ticks[i].Update(); }

                if (isDark)
                {
                    for (int i = 0; i < darkTicks.Count; i++) { darkTicks[i].Update(); }
                }
            }
        }

        internal void ChangeClipboardText()
        {
            TextMeshPro tmp = GameObject.Find("prop_clipboard_MuseumBasement/TMP").GetComponent<TextMeshPro>();

            tmp.text = "TURN AROUND. NOT IN GAME.";
            tmp.old_text = "TURN AROUND. NOT IN GAME.";
            tmp.m_text = tmp.text;
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
    }
}