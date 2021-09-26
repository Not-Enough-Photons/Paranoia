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
            public float minRNG;
            public float maxRNG;
            public bool useInsanity;
            public float targetInsanity;
            public string fireEvent;
            public string tickType;
        }

        [System.Flags]
        public enum TickType
        {
            Light = 1,
            Dark = 2,
            Any = Light | Dark
        }

        private string _name;
        public string name { get { return _name; } }

        private float _tick;
        public float tick { get { return _tick; } }

        private float _minRange;
        public float minRange { get { return _minRange; } }

        private float _maxRange;
        public float maxRange { get { return _maxRange; } }

        private float _minRNG;
        public float minRNG { get { return _minRNG; } }

        private float _maxRNG;
        public float maxRNG { get { return _maxRNG; } }

        private float _maxTick;
        public float maxTick { get { return _maxTick; } }


        private bool _useInsanity;
        public bool useInsanity { get { return _useInsanity; } }

        private float _targetInsanity;
        public float targetInsanity { get { return _targetInsanity; } }

        private ParanoiaEvent _event;
        public ParanoiaEvent Event { get { return _event; } }

        private TickType _tickType;
        public TickType tickType { get { return _tickType; } }

        public Tick(string tickName, float tick, float maxTick, bool useInsanity, float targetInsanity, TickType tickType, ParanoiaEvent Event)
        {
            _name = tickName;
            _tick = tick;
            _maxTick = maxTick;
            _useInsanity = useInsanity;
            _targetInsanity = targetInsanity;
            _tickType = tickType;
            _event = Event;
        }

        public Tick(string tickName, float tick, float minRange, float maxRange, bool useInsanity, float targetInsanity, TickType tickType, ParanoiaEvent Event)
        {
            _name = tickName;
            _tick = tick;
            _maxTick = Random.Range(minRange, maxRange);
            _useInsanity = useInsanity;
            _targetInsanity = targetInsanity;
            _tickType = tickType;
            _event = Event;
        }

        public ParanoiaEvent GetEvent()
        {
            return _event;
        }

        public void Update()
        {
            _tick += Time.deltaTime;

            if (_tick >= _maxTick)
            {
                if (_useInsanity)
                {
                    if (ParanoiaGameManager.instance.insanity >= _targetInsanity)
                    {
                        _event?.Start();
                    }
                }
                else
                {
                    _event?.Start();
                }

                _tick = 0f;
            }
        }
    }
}
