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
        public float[] minMaxTime { get; set; }
        public int[] minMaxRNG { get; set; }
        public float insanity { get; set; }

        public MapLevel runOnMaps { get; set; }

        public ParanoiaEvent pEvent { get; set; }

        private float _tTick;

        public Tick() { }

        public Tick(int id, string name, float tick, float[] minMaxTime, int[] minMaxRNG, float insanity, MapLevel runOnMaps, ParanoiaEvent pEvent)
        {
            this.id = id;
            this.name = name;
            this.tick = tick;

            if(minMaxTime != null)
            {
                if(minMaxTime[0] > minMaxTime[1]) // Edge case: [25.0, 1.0]
                {
                    float lowest = minMaxTime[1];
                    float highest = minMaxTime[0];

                    minMaxTime[1] = highest;
                    minMaxTime[0] = lowest;
                }

                this.minMaxTime = minMaxTime;

                // No defined base tick?
                if(tick <= 0f)
                {
                    // Set one.
                    SetRangedTime();
                }
            }

            if(minMaxRNG != null)
            {
                if (minMaxRNG[0] > minMaxRNG[1]) // Edge case: [25, 1]
                {
                    int lowest = minMaxRNG[1];
                    int highest = minMaxRNG[0];

                    minMaxRNG[1] = highest;
                    minMaxRNG[0] = lowest;
                }

                this.minMaxTime = minMaxTime;
            }

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
                OnTick();
            }
        }

        private void OnTick()
        {
            if(GameManager.insanity > this.insanity)
            {
                if (minMaxRNG != null)
                {
                    SetRangedRNG();
                }
                else
                {
                    pEvent?.Start();
                }
            }
            else if(this.insanity <= 0f)
            {
                // No defined insanity
                pEvent?.Start();
            }

            if(minMaxTime != null)
            {
                SetRangedTime();
                return;
            }
            else
            {
                // No defined min/max time
                _tTick = 0f;
            }

            _tTick = 0f;
        }

        private void SetRangedRNG()
        {
            int min = minMaxRNG[0];
            int max = minMaxRNG[1];

            if (GameManager.rngValue >= max || GameManager.rngValue <= min)
            {
                pEvent?.Start();
            }
        }

        private void SetRangedTime()
        {
            float min = minMaxTime[0];
            float max = minMaxTime[1];

            _tTick = Random.Range(min, max);
        }
    }
}
