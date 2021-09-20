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

        public Ambience hAmbience { get; private set; }
        public Chaser hChaser { get; private set; }
        public DarkVoice hDarkVoice { get; private set; }
        public TeleportingEntity hTeleportingEntity { get; private set; }
        public Paralyzer hParalyzer { get; private set; }
        public Radio hRadio { get; private set; }
        public CeilingMan hCeilingMan { get; private set; }
        public StaringMan hStaringMan { get; private set; }
        public ShadowPerson hShadowPerson { get; private set; }
        public ShadowPersonChaser hShadowPersonChaser { get; private set; }
        public Observer hObserver { get; private set; }
        public FordScaling hFordScaling { get; private set; }
        public CursedDoorController hCursedDoor { get; private set; }
        public InvisibleForce invisibleForce { get; private set; }

        public GameObject radioClone;
        
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
                        FinalizeTickDirty(settings, nameSpace, mainFunc);
                    }
                    else if (settings.fireEvent.StartsWith("M_"))
                    {
                        string mainFunc = settings.fireEvent.Replace("M_", string.Empty);
                        string nameSpace = "NEP.Paranoia.TickEvents.Mirages.";

                        FinalizeTickDirty(settings, nameSpace, mainFunc);
                    }
                }
                catch
                {
                    throw new System.Exception($"Exception in {settings.tickName}: The event {settings.fireEvent} could not be found. Check spelling, capitalization, or the documentation.");
                }
            }
        }

        private void FinalizeTickDirty(JSONSettings settings, string nameSpace, string mainFunc)
        {
            SpawnMirage spawnFunc = null;

            if (mainFunc.EndsWith("("))
            {
                spawnFunc = BuildInlineFunction(mainFunc) as SpawnMirage;
                mainFunc.Remove(mainFunc.IndexOf('('));
            }

            System.Type targetActionType = System.Type.GetType(nameSpace + mainFunc);

            /*if (targetActionType == null)
            {
                targetActionType = System.Activator.CreateInstance(typeof(ParanoiaEvent)) as System.Type;
                MelonLoader.MelonLogger.Warning($"WARNING at {settings.tickName}: " +
                    $"You are making a tick without an event. This tick will run, but no event will be called.");
            }*/

            TickType tickType = (TickType)System.Enum.Parse(typeof(TickType), settings.tickType);

            ParanoiaEvent ctorEvent = System.Activator.CreateInstance(targetActionType) as ParanoiaEvent;

            Tick generated = null;

            if(spawnFunc != null)
            {
                generated = settings.minRange == 0f && settings.maxRange == 0f
                ? new Tick(settings.tickName, settings.tick, settings.maxTick, settings.useInsanity, settings.targetInsanity, tickType, spawnFunc)
                : new Tick(settings.tickName, settings.tick, settings.minRange, settings.maxRange, settings.useInsanity, settings.targetInsanity, tickType, spawnFunc);
            }
            else
            {
                generated = settings.minRange == 0f && settings.maxRange == 0f
                ? new Tick(settings.tickName, settings.tick, settings.maxTick, settings.useInsanity, settings.targetInsanity, tickType, ctorEvent)
                : new Tick(settings.tickName, settings.tick, settings.minRange, settings.maxRange, settings.useInsanity, settings.targetInsanity, tickType, ctorEvent);
            }

            if(generated == null) { return; }

            if(tickType == TickType.Any)
            {
                ticks?.Add(generated);
            }
            else if (tickType == TickType.Dark)
            {
                darkTicks?.Add(generated);
            }
        }

        private object BuildInlineFunction(string functionname)
        {
            // The function gets split into different sections.
            // The name: M_SpawnMirage(hChaser)
            // To: [M_SpawnMirage], [(hChaser)]
            string[] splitStr = functionname.Split('(');

            // M_SpawnMirage
            string mainFuncSplit = splitStr[0];

            // (hChaser)
            string mainFuncParams = splitStr[1];

            // Now that the parameter name is isolated, remove the parenthesis.
            // e.x. from (hChaser) to hChaser

            // hChaser
            string param = mainFuncParams.Replace("(", string.Empty).Replace(")", string.Empty);

            // Build the constructor
            object instance = System.Activator.CreateInstance(typeof(SpawnMirage), new object[] { ParanoiaUtilities.GetHallucination(param) });

            return instance;
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