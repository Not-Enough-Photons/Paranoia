using System.Collections;
using System.Collections.Generic;
using System.Linq;

using NEP.Paranoia.Entities;
using NEP.Paranoia.TickEvents;
using NEP.Paranoia.TickEvents.Mirages;
using NEP.Paranoia.Utilities;

using StressLevelZero.AI;
using PuppetMasta;

using UnityEngine;

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
        
        private AudioManager _audioManager;
        public AudioManager audioManager { get { return _audioManager; } }

        public static Ambience hAmbience;
        public static Chaser hChaser;
        public static DarkVoice hDarkVoice;
        public static TeleportingEntity hTeleportingEntity;
        public static Paralyzer hParalyzer;
        public static Radio hRadio;
        public static CeilingMan hCeilingMan;

        public static StaringMan hStaringMan;
        public static ShadowPerson hShadowPerson;
        public static ShadowPersonChaser hShadowPersonChaser;
        public static Observer hObserver;
        public static FordScaling hFordScaling;
        public static CursedDoorController hCursedDoor;
        public static InvisibleForce invisibleForce;

        public GameObject radioClone;

        public AudioSource deafenSource;
        
        private AudioSource _radioSource;
        public AudioSource radioSource { get { return _radioSource; } }

        private bool _isDark = true;
        public bool isDark { get {  return _isDark; } }

        private int _rng = 1;
        public int rng {  get { return _rng; } }

        public bool paralysisMode;

        public float insanity;

        private SpawnCircle[] spawnCircles = new SpawnCircle[3];

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

            instance.hideFlags = HideFlags.DontUnloadUnusedAsset;
        }

        private void Start()
        {
            ParanoiaMapUtilities.Initialize();

            InitializeEntities();

            InitializeTicks();

            _audioManager = FindObjectOfType<AudioManager>();

            _playerTrigger = ParanoiaUtilities.FindPlayer();

            playerCircle = new SpawnCircle(ParanoiaUtilities.FindPlayer());

            for (int i = 0; i < spawnCircles.Length; i++)
            {
                spawnCircles[i] = new SpawnCircle(new GameObject("Spawn Circle").transform);
                spawnCircles[i].radius = 25f;
                spawnCircles[i].originTransform.position = staringManSpawns[i];
            }
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

        private void InitializeTicks()
        {
            ticks = new List<Tick>();
            darkTicks = new List<Tick>();

            ReadTicksFromJSON(System.IO.File.ReadAllText("UserData/paranoia/json/Ticks/ticks.json"));
        }

        private void InitializeEntities()
		{
            SetupHallucinations();
        }

        private void SetupHallucinations()
        {
            hAmbience = SpawnPrefab("ent_soundentity").AddComponent<Ambience>();
            hChaser = SpawnPrefab("ent_soundentity").AddComponent<Chaser>();
            hDarkVoice = SpawnPrefab("ent_soundentity").AddComponent<DarkVoice>();
            hTeleportingEntity = SpawnPrefab("ent_grayman").AddComponent<TeleportingEntity>();
            hParalyzer = SpawnPrefab("ent_paralyzer").AddComponent<Paralyzer>();
            hRadio = SpawnPrefab("ent_radio").AddComponent<Radio>();
            
            hShadowPerson = SpawnPrefab("ent_shadowperson").AddComponent<ShadowPerson>();
            hShadowPersonChaser = SpawnPrefab("ent_shadowperson").AddComponent<ShadowPersonChaser>();
            hCeilingMan = SpawnPrefab("ent_ceilingman").AddComponent<CeilingMan>();
            hStaringMan = SpawnPrefab("ent_staringman").AddComponent<StaringMan>();
            hObserver = SpawnPrefab("ent_observer").AddComponent<Observer>();
            hFordScaling = SpawnPrefab("ent_fordscaling").AddComponent<FordScaling>();
            hCursedDoor = SpawnPrefab("ent_curseddoor").AddComponent<CursedDoorController>();
            invisibleForce = new GameObject("Invisible Force").AddComponent<InvisibleForce>();

            deafenSource = new GameObject("Deafen Source").AddComponent<AudioSource>();
            deafenSource.volume = 0f;
            deafenSource.loop = true;
        }

        private void ReadTicksFromJSON(string json)
        {
            List<Tick.JSONSettings> tickSettings = JsonConvert.DeserializeObject<List<Tick.JSONSettings>>(json);

            foreach(Tick.JSONSettings settings in tickSettings)
            {
                    if (settings.fireEvent.StartsWith("E_"))
                    {
                        string mainFunc = settings.fireEvent.Replace("E_", string.Empty);
                        string nameSpace = "NEP.Paranoia.TickEvents.Events.";
                        FinalizeTick(settings, nameSpace, mainFunc);
                    }
                    else if (settings.fireEvent.StartsWith("M_"))
                    {
                        string mainFunc = settings.fireEvent.Replace("M_", string.Empty);
                        string nameSpace = "NEP.Paranoia.TickEvents.Mirages.";
                        FinalizeTick(settings, nameSpace, mainFunc);
                    }
            }
        }

        private void FinalizeTick(JSONSettings settings, string nameSpace, string mainFunc)
        {
            try
            {
                TickType tickType = (TickType)System.Enum.Parse(typeof(TickType), settings.tickType);

                if (mainFunc.Contains('('))
                {
                    FinalizeTickMethod(settings, tickType, nameSpace, mainFunc);
                }
                else
                {
                    System.Type targetActionType = System.Type.GetType(nameSpace + mainFunc);

                    ParanoiaEvent ctorEvent = System.Activator.CreateInstance(targetActionType) as ParanoiaEvent;

                    CreateTick(settings.minRange != 0 || settings.maxRange != 0, settings, tickType, ctorEvent);
                }
            }
            catch(System.Exception e)
            {
                throw new System.Exception($"Exception at {settings.fireEvent} in {settings.tickName}: {e.ToString()}");
            }
            
        }

        private void FinalizeTickMethod(JSONSettings settings, TickType tickType, string nameSpace, string mainFunc)
        {
            string method = ParanoiaUtilities.GetMethodNameString(mainFunc);
            string parameter = ParanoiaUtilities.GetParameterString(mainFunc);

            System.Type type = System.Type.GetType(nameSpace + method);

            object instance = System.Activator.CreateInstance(type, new object[] { ParanoiaUtilities.GetHallucination(parameter) });

            CreateTick(settings.minRange != 0f || settings.maxRange != 0f, settings, tickType, instance as SpawnMirage);
        }

        private Tick CreateTick(bool isRandom, JSONSettings settings, TickType tickType, ParanoiaEvent Event)
        {
            Tick standard = new Tick(settings.tickName, settings.tick, settings.maxTick, settings.targetInsanity, tickType, Event);
            Tick random = new Tick(settings.tickName, settings.tick, settings.minRange, settings.maxRange, settings.targetInsanity, tickType, Event);

            if(tickType == TickType.Any || tickType == TickType.Light)
            {
                ticks?.Add(isRandom ? random : standard);
            }
            else if(tickType == TickType.Dark)
            {
                darkTicks?.Add(isRandom ? random : standard);
            }

            return isRandom ? random : standard;
        }

        private void UpdateTicks(List<Tick> ticks)
        {
            for (int i = 0; i < ticks.Count; i++)
            {
                if (ticks[i].GetEvent() == null) { continue; }

                ticks[i].Update();
            }
        }

        private GameObject SpawnPrefab(string entName)
        {
            return GameObject.Instantiate(Paranoia.instance.GetEntInDirectory(entName), Vector3.zero, Quaternion.identity);
        }

        public void MoveAIToPoint(Vector3 point, AIBrain brain)
        {
            MoveAIToPoint(point, brain.behaviour);
        }

        public void MoveAIToPoint(Vector3 point, BehaviourBaseNav behaviour)
        {
            behaviour.sensors.hearingSensitivity = 0f;
            behaviour.SwitchMentalState(BehaviourBaseNav.MentalState.Roam);
            behaviour.Investigate(point, true, 60f);
        }
    }
}