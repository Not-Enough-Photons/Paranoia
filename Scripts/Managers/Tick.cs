using NEP.Paranoia.Managers;
using NEP.Paranoia.TickEvents;
using UnityEngine;

namespace NEP.Paranoia.Managers
{
    public class Tick
    {
        public struct JSONSettings
        {
            public string tickName;
            public float tick;
            public float minRange;
            public float maxRange;
            public float maxTick;
            public int minRNG;
            public int maxRNG;
            public bool useInsanity;
            public float targetInsanity;
            public string fireEvent;
            public string tickType;
            public string triggerOnMap;
        }

        [System.Flags]
        public enum TickType
        {
            Light = 1,
            Dark = 2,
            Any = Light | Dark
        }

        public ParanoiaUtilities.MapLevel triggerOnMap { get; private set; }

        public string name { get; private set; }

        public float tick { get; private set; }

        public float minRange { get; private set; }

        public float maxRange { get; private set; }

        public float maxTick { get; private set; }

        public bool useRNG { get; private set; }

        private int _minRNG;
        private int _maxRNG;

        public int targetRNG
        {
            get
            {
                return Random.Range(_minRNG, _maxRNG);
            }
        }

        public bool useInsanity { get; private set; }

        public float targetInsanity { get; private set; }

        public ParanoiaEvent Event { get; private set; }

        public TickType tickType { get; private set; }

        public Tick
            (string tickName,
            float tick,
            float maxTick,
            int minRNG,
            int maxRNG,
            bool useInsanity,
            float targetInsanity,
            TickType tickType,
            ParanoiaUtilities.MapLevel triggerOnMap,
            ParanoiaEvent Event)
        {
            this.name = tickName;
            this.tick = tick;
            this.maxTick = maxTick;
            _minRNG = minRNG;
            _maxRNG = maxRNG;
            this.useInsanity = useInsanity;
            this.targetInsanity = targetInsanity;
            this.tickType = tickType;
            this.triggerOnMap = triggerOnMap;
            this.Event = Event;
        }

        public Tick
            (string tickName, 
            float tick, 
            float minRange, 
            float maxRange,
            int minRNG,
            int maxRNG,
            bool useInsanity, 
            float targetInsanity, 
            TickType tickType,
            ParanoiaUtilities.MapLevel triggerOnMap,
            ParanoiaEvent Event)
        {
            this.name = tickName;
            this.tick = tick;
            this.maxTick = Random.Range(minRange, maxRange);
            _minRNG = minRNG;
            _maxRNG = maxRNG;
            this.useInsanity = useInsanity;
            this.targetInsanity = targetInsanity;
            this.tickType = tickType;
            this.triggerOnMap = triggerOnMap;
            this.Event = Event;
        }

        public ParanoiaEvent GetEvent()
        {
            return Event;
        }

        public void Update()
        {
            tick += Time.deltaTime;

            if (tick >= maxTick)
            {
                if(triggerOnMap == ParanoiaUtilities.MapUtilities.currentLevel)
                {
                    bool reachedInsanity = Paranoia.instance.gameManager.insanity >= targetInsanity;

                    if (_minRNG != 0 && _maxRNG != 0)
                    {
                        int rng = Paranoia.instance.gameManager.rng;
                        bool reachedRNG = rng >= _minRNG && rng <= _maxRNG;

                        if (reachedRNG && useInsanity)
                        {
                            if (reachedInsanity)
                            {
                                Event?.Start();
                                tick = 0f;
                            }
                        }
                        else if (reachedRNG)
                        {
                            Event?.Start();
                            tick = 0f;
                        }
                    }
                    else if (useInsanity)
                    {
                        if (reachedInsanity)
                        {
                            Event?.Start();
                            tick = 0f;
                        }
                    }
                    else
                    {
                        Event?.Start();
                        tick = 0f;
                    }
                }

                tick = 0f;
            }
        }
    }
}
