using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        public GameObject shadowMan;
        public GameObject staringMan;
        public GameObject ceilingWatcher;

        private Hallucination hChaser;
        private Hallucination hDarkVoice;
        private Hallucination hCeilingMan;
        private Hallucination hStaringMan;
        private Hallucination hShadowPerson;
        private Hallucination hShadowPersonChaser;

        public static Hallucination CreateHallucination(Hallucination.HallucinationType hType, Hallucination.HallucinationFlags hFlags, Hallucination.HallucinationClass hClass, float distanceToDisappear, float chaseSpeed, float delayTime, bool usesDelay)
        {
            Hallucination hallucination = new GameObject($"{hType} {hClass} Hallucination").AddComponent<Hallucination>();
            hallucination.gameObject.AddComponent<SpriteBillboard>();

            if (hType.HasFlag(Hallucination.HallucinationType.Auditory))
            {
                AudioSource source = hallucination.gameObject.AddComponent<AudioSource>();
                source.dopplerLevel = 0;
            }

            hallucination.gameObject.SetActive(false);
            hallucination.Initialize(hType, hFlags, hClass, distanceToDisappear, chaseSpeed, delayTime, usesDelay);
            return hallucination;
        }

        public static Hallucination CreateHallucination(GameObject prefab, Hallucination.HallucinationType hType, Hallucination.HallucinationFlags hFlags, Hallucination.HallucinationClass hClass, float distanceToDisappear, float chaseSpeed, float delayTime, bool usesDelay)
        {
            Hallucination hallucination = new GameObject($"{hType} {hClass} Hallucination").AddComponent<Hallucination>();
            hallucination.gameObject.AddComponent<SpriteBillboard>();

            if (prefab != null)
            {
                GameObject.Instantiate(prefab, hallucination.transform);
            }

            if (hType.HasFlag(Hallucination.HallucinationType.Auditory))
            {
                AudioSource source = hallucination.gameObject.AddComponent<AudioSource>();
                source.dopplerLevel = 0;
            }

            hallucination.gameObject.SetActive(false);
            hallucination.Initialize(hType, hFlags, hClass, distanceToDisappear, chaseSpeed, delayTime, usesDelay);
            return hallucination;
        }

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            hChaser = CreateHallucination(Hallucination.HallucinationType.Auditory, Hallucination.HallucinationFlags.None, Hallucination.HallucinationClass.Chaser, 1f, 30f, 0f, false);
            hDarkVoice = CreateHallucination(Hallucination.HallucinationType.Auditory, Hallucination.HallucinationFlags.None, Hallucination.HallucinationClass.DarkVoice, 0f, 0f, 0f, false);
            hCeilingMan = CreateHallucination(ceilingWatcher, Hallucination.HallucinationType.Auditory | Hallucination.HallucinationType.Visual, Hallucination.HallucinationFlags.HideWhenSeen, Hallucination.HallucinationClass.Watcher, 0f, 0f, 0f, false);
            hStaringMan = CreateHallucination(staringMan, Hallucination.HallucinationType.Visual, Hallucination.HallucinationFlags.None, Hallucination.HallucinationClass.Chaser, 30f, 1f, 0f, false);
            hShadowPerson = CreateHallucination(shadowMan, Hallucination.HallucinationType.Visual, Hallucination.HallucinationFlags.None, Hallucination.HallucinationClass.Watcher, 0f, 0f, 0f, false);
            hShadowPersonChaser = CreateHallucination(shadowMan, Hallucination.HallucinationType.Visual, Hallucination.HallucinationFlags.None, Hallucination.HallucinationClass.Chaser, 1f, 50f, 5f, true);

            ticks = new List<Tick>();
            darkTicks = new List<Tick>();
        }

        private void Start()
        {
            Tick test = new Tick(5f, Tick.TickType.TT_DARK | Tick.TickType.TT_LIGHT);
        }

        private void Update()
        {
            //for(int i = 0; i < ticks.Count; i++) { ticks[i].Update(); }
        }
    }

}