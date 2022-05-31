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
                float min = minMaxTime[0];
                float max = minMaxTime[1];

                if(min > max)
                {
                    float newMin = max;
                    float newMax = min;

                    min = newMin;
                    max = newMax;

                    minMaxTime[0] = min;
                    minMaxTime[1] = max;
                }

                // No tick is defined
                // Generate a new number
                if(this.tick <= 0f)
                {
                    this.tick = Random.Range(min, max);
                }
            }

            if (minMaxRNG != null)
            {
                int min = minMaxRNG[0];
                int max = minMaxRNG[1];

                if (min > max)
                {
                    int newMin = max;
                    int newMax = min;

                    min = newMin;
                    max = newMax;

                    minMaxRNG[0] = min;
                    minMaxRNG[1] = max;
                }
            }

            this.minMaxTime = minMaxTime;
            this.minMaxRNG = minMaxRNG;

            this.insanity = insanity;
            this.runOnMaps = runOnMaps;

            this.pEvent = pEvent;

            this._tTick = 0f;
        }

        public void Update()
        {
            try
            {
                _tTick += Time.deltaTime;

                if (_tTick >= tick)
                {
                    if (GameManager.insanity >= insanity)
                    {
                        if (minMaxTime != null)
                        {
                            if (minMaxRNG != null)
                            {
                                UpdateRNG(minMaxRNG[0], minMaxRNG[1]);
                            }
                            else
                            {
                                UpdateTime();
                            }
                        }
                        else
                        {
                            if (minMaxRNG != null)
                            {
                                UpdateRNG(minMaxRNG[0], minMaxRNG[1]);
                            }
                            else
                            {
                                pEvent?.Start();
                            }
                        }
                    }

                    _tTick = 0f;
                }
            }
            catch(System.Exception e)
            {
                throw new System.Exception($"Exception at {this.name}! {e.GetBaseException()}");
            }
        }

        private void UpdateRNG(int min, int max)
        {
            if (GameManager.rngValue >= min && GameManager.rngValue <= max)
            {
                UpdateTime();
            }
        }

        private void UpdateTime()
        {
            tick = Random.Range(minMaxTime[0], minMaxTime[1]);
            pEvent?.Start();
        }
    }
}
