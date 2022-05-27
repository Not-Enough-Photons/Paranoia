﻿using NEP.Paranoia.ParanoiaUtilities;

namespace NEP.Paranoia.Managers
{
    public class TickManager
    {
        public TickManager()
        {
            ticks = DataReader.ReadTicks();

            instance = this;
        }

        public static TickManager instance;

        public Tick[] ticks;

        public void Update()
        {
            foreach (Tick tick in ticks)
            {
                if(tick.runOnMaps.HasFlag(MapUtilities.currentLevel) ||
                    tick.runOnMaps == MapLevel.AllMaps)
                {
                    tick.Update();
                }
            }
        }
    }
}
