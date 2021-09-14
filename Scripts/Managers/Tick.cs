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
        private ParanoiaEvent _event;
        private TickType _tickType;

        public Tick(string tickName, float tick, float maxTick, TickType tickType, ParanoiaEvent Event)
        {
            _name = tickName;
            _tick = tick;
            _maxTick = maxTick;
            _tickType = tickType;
            _event = Event;
        }

        public Tick(string tickName, float tick, float minRange, float maxRange, TickType tickType, ParanoiaEvent Event)
        {
            _name = tickName;
            _tick = tick;
            _maxTick = Random.Range(minRange, maxRange);
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
                _event?.Start();
                _tick = 0f;
            }
        }
    }
}
