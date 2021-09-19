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
        private float _tick;
        private float _minRange;
        private float _maxRange;
        private float _maxTick;
        private bool _useInsanity;
        private float _targetInsanity;
        private ParanoiaEvent _event;
        private TickType _tickType;

        public Tick(string tickName, float tick, float maxTick, bool useInsanity, float targetInsanity, TickType tickType, ParanoiaEvent Event)
        {
            _name = tickName;
            _tick = tick;
            _maxTick = maxTick;
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
            if (_tick >= _maxTick)
            {
                if (_useInsanity)
                {
                    if (_targetInsanity >= ParanoiaGameManager.instance.insanity)
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
