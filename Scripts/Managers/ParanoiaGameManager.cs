using System.Collections;
using System.Collections.Generic;
using System.Linq;

using NEP.Paranoia.Entities;
using NEP.Paranoia.TickEvents;
using NEP.Paranoia.TickEvents.Mirages;
using NEP.Paranoia.ParanoiaUtilities;

using StressLevelZero.AI;
using PuppetMasta;

using UnityEngine;

using Newtonsoft.Json;
using static NEP.Paranoia.Managers.Tick;
using StressLevelZero.Rig;

namespace NEP.Paranoia.Managers
{
    public class ParanoiaGameManager : MonoBehaviour
    {
        public ParanoiaGameManager(System.IntPtr ptr) : base(ptr) { }

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

        public GameObject playerHead;

        public GameWorldSkeletonRig clonedRig;

        public static Ambience hAmbience;
        public static Chaser hChaser;
        public static DarkVoice hDarkVoice;
        public static SjasFace hSjasFace;
        public static TeleportingEntity hTeleportingEntity;
        public static Paralyzer hParalyzer;
        public static Radio hRadio;
        public static CeilingMan hCeilingMan;
        public static CryingEntity hCryingEntity;

        public static StaringMan hStaringMan;
        public static FastStaringMan hFastStaringMan;
        public static ShadowPerson hShadowPerson;
        public static ShadowPersonChaser hShadowPersonChaser;
        public static Observer hObserver;
        public static FordScaling hFordScaling;
        public static CursedDoorController hCursedDoor;
        public static InvisibleForce invisibleForce;

        public static GameObject endRoom;

        public GameObject radioClone;

        public AudioSource deafenSource;
        
        private AudioSource _radioSource;
        public AudioSource radioSource { get { return _radioSource; } }

        private int _rng = 1;
        public int rng {  get { return _rng; } }

        public bool paralysisMode;

        public float insanity;

        private SpawnCircle[] spawnCircles = new SpawnCircle[3];

        public Tick GetTick(string name, TickType type)
        {
            Tick selectedTick = type == TickType.Any 
                ? ticks.FirstOrDefault((tick) => tick.name == name) 
                : darkTicks.FirstOrDefault((tick) => tick.name == name);

            return selectedTick;
        }

        public void SetRNG(int rng)
        {
            this._rng = rng;
        }

        /// <summary>
        /// Unloads absolutely everything and destroys itself.
        /// </summary>
        public void Cleanup()
        {
            BaseHallucination[] hallucinations = Object.FindObjectsOfType<BaseHallucination>();

            foreach(BaseHallucination hallucination in hallucinations)
            {
                if(hallucination is AudioHallucination)
                {
                    hallucination.gameObject.SetActive(false);
                }

                Destroy(hallucination.gameObject);  
            }

            ticks.Clear();
            darkTicks.Clear();

            ticks = null;
            darkTicks = null;

            hAmbience = null;
            hCeilingMan = null;
            hChaser = null;
            hCryingEntity = null;
            hCursedDoor = null;
            hDarkVoice = null;
            hFastStaringMan = null;
            hStaringMan = null;
            hDarkVoice = null;
            hSjasFace = null;
            hFordScaling = null;
            hObserver = null;
            hParalyzer = null;
            hShadowPerson = null;
            hShadowPersonChaser = null;
            hTeleportingEntity = null;
            hRadio = null;
            invisibleForce = null;

            clonedRig = null;

            MapUtilities.fog.startDistance = MapUtilities.baseFog.startDistance;
            MapUtilities.fog.endDistance = MapUtilities.baseFog.endDistance;
            MapUtilities.fog.heightFogFalloff = MapUtilities.baseFog.heightFogFalloff;
            MapUtilities.fog.heightFogThickness = MapUtilities.baseFog.heightFogThickness;
            MapUtilities.fog.heightFogColor = MapUtilities.baseFog.heightFogColor;
        }

        private void Start()
        {
            endRoom = GameObject.Instantiate(Paranoia.instance.GetEntInDirectory("ent_room"));
            endRoom.transform.position = Vector3.up * 500f;
            endRoom.SetActive(false);

            MapUtilities.Initialize();

            _playerTrigger = Utilities.FindPlayer();

            playerCircle = new SpawnCircle(Utilities.FindPlayer());

            playerHead = Utilities.FindHead().gameObject;

            for (int i = 0; i < spawnCircles.Length; i++)
            {
                spawnCircles[i] = new SpawnCircle(new GameObject("Spawn Circle").transform);
                spawnCircles[i].radius = 25f;
                spawnCircles[i].originTransform.position = staringManSpawns[i];
            }

            GameObject.Find("AirParticles")?.SetActive(false);

            MapUtilities.SwitchFog(MapUtilities.baseFog, MapUtilities.darkFog, 0.60f, 3600f);

            InitializeEntities();

            InitializeTicks();
        }

        private void Update()
        {
			if (Paranoia.instance.isTargetLevel)
			{
                playerCircle.CalculatePlayerCircle(0f);

                if (debug || Time.timeScale == 0) { return; }

                try
                {
                    UpdateTicks(ticks);
                    UpdateTicks(darkTicks);
                }
                catch(System.Exception e)
                {
                    MelonLoader.MelonLogger.Error(e);
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
            hAmbience = SpawnPrefab<Ambience>("ent_soundentity");
            hChaser = SpawnPrefab<Chaser>("ent_soundentity");
            hDarkVoice = SpawnPrefab<DarkVoice>("ent_soundentity");
            hCryingEntity = SpawnPrefab<CryingEntity>("ent_soundentity");
            hTeleportingEntity = SpawnPrefab<TeleportingEntity>("ent_grayman");
            hSjasFace = SpawnPrefab<SjasFace>("ent_sjasface");
            hParalyzer = SpawnPrefab<Paralyzer>("ent_paralyzer");
            hRadio = SpawnPrefab<Radio>("ent_radio");

            hShadowPerson = SpawnPrefab<ShadowPerson>("ent_shadowperson");
            hShadowPersonChaser = SpawnPrefab<ShadowPersonChaser>("ent_shadowperson");
            hCeilingMan = SpawnPrefab<CeilingMan>("ent_ceilingman");
            hStaringMan = SpawnPrefab<StaringMan>("ent_staringman");
            hFastStaringMan = SpawnPrefab<FastStaringMan>("ent_staringman");
            hObserver = SpawnPrefab<Observer>("ent_observer");
            hFordScaling = SpawnPrefab<FordScaling>("ent_fordscaling");
            hCursedDoor = SpawnPrefab<CursedDoorController>("ent_curseddoor");
            invisibleForce = new GameObject("Invisible Force").AddComponent<InvisibleForce>();

            deafenSource = new GameObject("Deafen Source").AddComponent<AudioSource>();
            deafenSource.clip = Paranoia.instance.deafenSounds[0];
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
            string method = Utilities.GetMethodNameString(mainFunc);
            string parameter = Utilities.GetParameterString(mainFunc);

            System.Type type = System.Type.GetType(nameSpace + method);

            object instance = System.Activator.CreateInstance(type, new object[] { Utilities.GetHallucination(parameter) });

            CreateTick(settings.minRange != 0f || settings.maxRange != 0f, settings, tickType, instance as SpawnMirage);
        }

        private Tick CreateTick(bool isRandom, JSONSettings settings, TickType tickType, ParanoiaEvent Event)
        {
            Tick standard = new Tick(settings.tickName, settings.tick, settings.maxTick, settings.minRNG, settings.maxRNG, settings.useInsanity, settings.targetInsanity, tickType, MapLevel.Arena, Event);
            Tick random = new Tick(settings.tickName, settings.tick, settings.minRange, settings.maxRange, settings.minRNG, settings.maxRNG, settings.useInsanity, settings.targetInsanity, tickType, MapLevel.Arena, Event);

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

        private IEnumerator DebugModeTick()
        {
            yield return new WaitForSeconds(Random.Range(10, 30f));

            int rng = Random.Range(1, 10);

            GameObject g = new GameObject();
            AudioSource a = g.AddComponent<AudioSource>();
            a.clip = Paranoia.instance.GetClipInDirectory(Paranoia.instance.chaserAmbience, "amb_chaser_17");
            a.Play();

            yield return new WaitForSeconds(10f);

            hSjasFace.gameObject.SetActive(true);
            hSjasFace.moveSpeed = 50f;

            while (hSjasFace.gameObject.activeInHierarchy) { yield return null; }

            MelonLoader.MelonLogger.LogError($"Exception thrown: 'System.AccessViolationException' in NEP.Paranoia.Managers.ParanoiaGameManager\n"
                    + "An unhandled exception of type 'System.AccessViolationException' occurred in NEP.Paranoia.Managers.ParanoiaGameManager\n"
                    + "Attempted to read or write in unverified and illegal memory." +
                    "\nWE CAN'T HELP YOU\nWE CAN'T HELP YOU\nWE CAN'T HELP YOU\nWE CAN'T HELP YOU\nWE CAN'T HELP YOU\nWE CAN'T HELP YOU\nWE CAN'T HELP YOU\nWE CAN'T HELP YOU\nWE CAN'T HELP YOU\nWE CAN'T HELP YOU\nWE CAN'T HELP YOU\nWE CAN'T HELP YOU\n");

            if(rng == 7)
            {
                Application.OpenURL("https://www.youtube.com/watch?v=i8ju_10NkGY");
            }

            Application.ForceCrash(0);
        }

        private T SpawnPrefab<T>(string entName) where T : BaseHallucination
        {
            GameObject obj = GameObject.Instantiate(Paranoia.instance.GetEntInDirectory(entName), Vector3.zero, Quaternion.identity);
            T type = obj.AddComponent<T>();
            return type;
        }

        public void MoveAIToPoint(Vector3 point, AIBrain brain)
        {
            MoveAIToPoint(brain.behaviour, point);
        }

        public void AILookAtTarget(BehaviourBaseNav behaviour, Vector3 point)
        {
            behaviour.sensors.hearingSensitivity = 0f;
            behaviour.Investigate(point, true, 60f);
        }

        public void MoveAIToPoint(BehaviourBaseNav behaviour, Vector3 point)
        {
            behaviour.sensors.hearingSensitivity = 0f;
            behaviour.SetHomePosition(point, true, true);
            behaviour.Investigate(point, true, 120f);
        }
    }
}