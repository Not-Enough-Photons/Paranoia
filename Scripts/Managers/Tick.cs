using NEP.Paranoia.Managers;
using NEP.Paranoia.TickEvents;
using UnityEngine;

using NEP.Paranoia.ParanoiaUtilities;

namespace NEP.Paranoia.Managers
{
    public class Tick
    {
        public int id { get; set; }
        public string name { get; set; }
        public float tick { get; set; }
        public float insanity { get; set; }

        public MapLevel runOnMaps { get; set; }

        public ParanoiaEvent pEvent { get; private set; }

        private float _tTick;

        public Tick(int id, string name, float tick, float insanity, MapLevel runOnMaps, ParanoiaEvent pEvent)
        {
            this.id = id;
            this.name = name;
            this.tick = tick;
            this.insanity = insanity;
            this.runOnMaps = runOnMaps;

            this.pEvent = pEvent;

            this._tTick = 0f;
        }

        public void Update()
        {
            _tTick += Time.deltaTime;

            if(_tTick >= tick)
            {
                _tTick = 0f;

                if (this.insanity > GameManager.insanity)
                {
                    pEvent?.Start();
                }
            }
        }
    }
}
