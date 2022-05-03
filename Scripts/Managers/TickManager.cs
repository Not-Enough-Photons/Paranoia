using UnityEngine;
using System.Collections.Generic;

using Newtonsoft.Json;

using NEP.Paranoia.Entities;
using NEP.Paranoia.TickEvents;
using NEP.Paranoia.TickEvents.Mirages;
using NEP.Paranoia.ParanoiaUtilities;
using static NEP.Paranoia.Managers.Tick;

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
                tick.Update();
            }
        }
    }
}
