using NEP.Paranoia.Managers;
using NEP.Paranoia.TickEvents;
using UnityEngine;

namespace NEP.Paranoia.Managers
{
    public class Tick
    {
        public int id { get; set; }
        public string name { get; set; }
        public float tick { get; set; }

        public ParanoiaEvent pEvent { get; private set; }

        private float _tTick;

        public Tick(int id, string name, float tick, ParanoiaEvent pEvent)
        {
            this.id = id;
            this.name = name;
            this.tick = tick;
            this.pEvent = pEvent;

            this._tTick = 0f;
        }

        public void Update()
        {
            _tTick += UnityEngine.Time.deltaTime;

            if (_tTick >= tick)
            {
                pEvent?.Start();
                _tTick = 0f;
            }
        }
    }
}
