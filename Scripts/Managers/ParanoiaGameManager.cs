using System.Collections;
using System.Collections.Generic;

using NEP.Paranoia.Entities;

using NEP.Paranoia.TickEvents;
using NEP.Paranoia.TickEvents.Events;
using NEP.Paranoia.TickEvents.Mirages;

using NEP.Paranoia.Utilities;

using TMPro;

using UnityEngine;

using UnhollowerBaseLib;

using Newtonsoft.Json;
using static NEP.Paranoia.Managers.Tick;

namespace NEP.Paranoia.Managers
{
    public class ParanoiaGameManager : MonoBehaviour
    {
        public ParanoiaGameManager(System.IntPtr ptr) : base(ptr) { }

        public static ParanoiaGameManager instance;

        internal static bool debug = false;
        public static bool insanityMode { get; private set;}

        public List<Tick> ticks;
        public List<Tick> darkTicks;

        // Main prefabs
        public GameObject shadowMan;
        public GameObject staringMan;
        public GameObject ceilingWatcher;
        public GameObject observer;
        public GameObject teleportingEntity;
        public GameObject paralyzerEntity;
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

        public AudioHallucination hChaser { get; private set; }
        public AudioHallucination hDarkVoice { get; private set; }
        public AudioHallucination hTeleportingEntity { get; private set; }
        public AudioHallucination hParalyzer { get; private set; }
        public BaseHallucination hCeilingMan { get; private set; }
        public BaseHallucination hStaringMan { get; private set; }
        public BaseHallucination hShadowPerson { get; private set; }
        public BaseHallucination hShadowPersonChaser { get; private set; }
        public BaseHallucination hObserver { get; private set; }
        public BaseHallucination hCursedDoor { get; private set; }
        public BaseHallucination invisibleForce { get; private set; }
        
        private VLB.VolumetricLightBeam _lightBeam;
        public VLB.VolumetricLightBeam lightBeam { get { return _lightBeam; } }
        
        private Light blankBoxLight;
        private GameObject flashlightObject;

        public GameObject radioClone;
        
        private GameObject monitorClone;
        private GameObject cursedDoorClone;
        
        private AudioSource _radioSource;
        public AudioSource radioSource { get { return _radioSource; } }

        private bool _firstRadioSpawn = false;
        public bool firstRadioSpawn { get { return _firstRadioSpawn; } }

        private bool _isDark = false;
        public bool isDark { get {  return _isDark; } }

        private int _rng = 1;
        public int rng {  get { return _rng; } }

        public bool paralysisMode;

        public float insanity { get; private set; }

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

        public void AddInsanity(float insanity)
        {
            this.insanity += insanity;
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

            if (insanityMode)
            {
                insanity = 0.1f;
            }

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

            ReadTicksFromJSON(System.IO.File.ReadAllText("UserData/paranoia/json/Ticks/ticks.json"));
        }

        private void ReadTicksFromJSON(string json)
        {
            List<Tick.JSONSettings> tickSettings = JsonConvert.DeserializeObject<List<Tick.JSONSettings>>(json);

            foreach(Tick.JSONSettings settings in tickSettings)
            {
                try
                {
                    if (settings.fireEvent.StartsWith("E_"))
                    {
                        string mainFunc = settings.fireEvent.Replace("E_", string.Empty);
                        string nameSpace = "NEP.Paranoia.TickEvents.Events.";
                        System.Type targetActionType = System.Type.GetType(nameSpace + mainFunc);

                        FinalizeTick(settings, targetActionType);
                    }
                    else if (settings.fireEvent.StartsWith("M_"))
                    {
                        string mainFunc = settings.fireEvent.Replace("M_", string.Empty);
                        string nameSpace = "NEP.Paranoia.TickEvents.Mirages.";
                        System.Type targetActionType = System.Type.GetType(nameSpace + mainFunc);

                        FinalizeTick(settings, targetActionType);
                    }
                }
                catch
                {
                    throw new System.Exception($"Exception in {settings.tickName}: The event {settings.fireEvent} could not be found. Check spelling, capitalization, or the documentation.");
                }
            }
        }

        private void FinalizeTick(Tick.JSONSettings settings, System.Type targetActionType)
        {
            if (targetActionType == null) { return; }

            Tick.TickType tickType = (Tick.TickType)System.Enum.Parse(typeof(Tick.TickType), settings.tickType);

            ParanoiaEvent ctorEvent = System.Activator.CreateInstance(targetActionType) as ParanoiaEvent;

            Tick generated = settings.minRange == 0f && settings.maxRange == 0f
                ? new Tick(settings.tickName, settings.tick, settings.maxTick, tickType, ctorEvent)
                : new Tick(settings.tickName, settings.tick, settings.minRange, settings.maxRange, tickType, ctorEvent);

            if(tickType == TickType.Any)
            {
                ticks?.Add(generated);
            }
            else if (tickType == TickType.Dark)
            {
                darkTicks?.Add(generated);
            }
        }

        private void InitializeEntities()
		{
            radioClone = GameObject.Instantiate(radioObject);
            monitorClone = GameObject.Instantiate(monitorObject);

            SetupHallucinations();

            _radioSource = radioClone.GetComponentInChildren<AudioSource>();
            MonitorVideo monitorVideo = monitorClone.AddComponent<MonitorVideo>();
            monitorVideo.clips = clipList;

            CursedDoorController cursedDoorCtrlr = cursedDoorClone.AddComponent<CursedDoorController>();

            radioClone.SetActive(false);
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
                       observerClone, teleportingEntityClone, paralyzerEntityClone,
                       invisibleForceObject;

            chaserAudio = new GameObject("ChaserMirage");
            darkVoiceAudio = new GameObject("DarkVoice");
            shadowManClone = SpawnPrefab(shadowMan);
            shadowManChaserClone = SpawnPrefab(shadowMan);
            ceilingManClone = SpawnPrefab(ceilingWatcher);
            staringManClone = SpawnPrefab(staringMan);
            observerClone = SpawnPrefab(observer);
            teleportingEntityClone = SpawnPrefab(teleportingEntity);
            paralyzerEntityClone = SpawnPrefab(paralyzerEntity);
            invisibleForceObject = new GameObject("Invisible Force");

            cursedDoorClone = SpawnPrefab(cursedDoorObject);

            chaserAudio.AddComponent<AudioSource>();
            darkVoiceAudio.AddComponent<AudioSource>();
            paralyzerEntityClone.AddComponent<AudioSource>();
            hChaser = chaserAudio.AddComponent<AudioHallucination>();
            hDarkVoice = darkVoiceAudio.AddComponent<AudioHallucination>();
            hTeleportingEntity = teleportingEntityClone.AddComponent<AudioHallucination>();
            hParalyzer = paralyzerEntityClone.AddComponent<AudioHallucination>();
            
            hShadowPerson = shadowManClone.AddComponent<BaseHallucination>();
            hShadowPersonChaser = shadowManChaserClone.AddComponent<BaseHallucination>();
            hCeilingMan = ceilingManClone.AddComponent<BaseHallucination>();
            hStaringMan = staringManClone.AddComponent<BaseHallucination>();
            hObserver = observerClone.AddComponent<BaseHallucination>();
            hCursedDoor = cursedDoorClone.AddComponent<CursedDoorController>();
            invisibleForce = invisibleForceObject.AddComponent<BaseHallucination>();

            invisibleForce.gameObject.AddComponent<SphereCollider>().radius = 2f;
            
            hChaser.clips = Paranoia.instance.chaserAmbience.ToArray();
            hDarkVoice.clips = Paranoia.instance.darkVoices.ToArray();
            hTeleportingEntity.clips = Paranoia.instance.teleporterAmbience.ToArray();
            hParalyzer.clips = Paranoia.instance.paralyzerAmbience.ToArray();
            
            hStaringMan.spawnPoints = staringManSpawns;
            hCeilingMan.spawnPoints = ceilingManSpawns;

            ApplyHallucinationSettings();
        }

        private void ApplyHallucinationSettings()
        {
            string baseJsonDir = "UserData/paranoia/json/BaseHallucination";
            string audioJsonDir = "UserData/paranoia/json/AudioHallucination";

            hShadowPerson.ReadValuesFromJSON(System.IO.File.ReadAllText(baseJsonDir + "/ShadowMan.json"));
            hShadowPersonChaser.ReadValuesFromJSON(System.IO.File.ReadAllText(baseJsonDir + "/ShadowManChaser.json"));
            hCeilingMan.ReadValuesFromJSON(System.IO.File.ReadAllText(baseJsonDir + "/CeilingMan.json"));
            hStaringMan.ReadValuesFromJSON(System.IO.File.ReadAllText(baseJsonDir + "/StaringMan.json"));
            hObserver.ReadValuesFromJSON(System.IO.File.ReadAllText(baseJsonDir + "/Observer.json"));
            hCursedDoor.ReadValuesFromJSON(System.IO.File.ReadAllText(baseJsonDir + "/CursedDoor.json"));
            invisibleForce.ReadValuesFromJSON(System.IO.File.ReadAllText(baseJsonDir + "/InvisibleForce.json"));

            hChaser.ReadValuesFromJSON(System.IO.File.ReadAllText(audioJsonDir + "/Chaser.json"));
            hDarkVoice.ReadValuesFromJSON(System.IO.File.ReadAllText(audioJsonDir + "/DarkVoice.json"));
            hTeleportingEntity.ReadValuesFromJSON(System.IO.File.ReadAllText(audioJsonDir + "/TeleportingEntity.json"));
            hParalyzer.ReadValuesFromJSON(System.IO.File.ReadAllText(audioJsonDir + "/Paralyzer.json"));
        }

        private void Update()
        {
			if (Paranoia.instance.isBlankBox)
			{
                playerCircle.CalculatePlayerCircle(0f);

                if (debug || Time.timeScale == 0) { return; }

                UpdateTicks(ticks);

                if (isDark)
                {
                    UpdateTicks(darkTicks);
                }
            }
        }

        private void UpdateTicks(List<Tick> ticks)
        {
            if (!insanityMode)
            {
                insanity = 0.1f;
            }

            for (int i = 0; i < ticks.Count; i++)
            {
                if (ticks[i].GetEvent() == null) { continue; }

                ticks[i].Update();
            }
        }

        internal void ChangeClipboardText()
        {
            TextMeshPro tmp = GameObject.Find("prop_clipboard_MuseumBasement/TMP").GetComponent<TextMeshPro>();

            tmp.text = "TURN AROUND. NOT IN GAME.";
            tmp.old_text = "TURN AROUND. NOT IN GAME.";
            tmp.m_text = tmp.text;
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