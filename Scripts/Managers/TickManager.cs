using NEP.Paranoia.ParanoiaUtilities;

namespace NEP.Paranoia.Managers
{
    public class TickManager
    {
        public TickManager() => Initialize();

        public Tick[] ticks;

        private void Initialize()
        {
            ticks = DataReader.ReadTicks();

            if (GameManager.randomizeTicks)
            {
                foreach(Tick tick in ticks)
                {
                    tick.insanity = UnityEngine.Random.Range(0.25f, 4f);
                }
            }
        }

        public void Update()
        {
            foreach (Tick tick in ticks)
            {
                try
                {
                    if (tick.runOnMaps.HasFlag(MapUtilities.currentLevel) ||
                     tick.runOnMaps == MapLevel.AllMaps)
                    {
                        tick.Update();
                    }
                }
                catch
                {

                }
            }
        }
    }
}
