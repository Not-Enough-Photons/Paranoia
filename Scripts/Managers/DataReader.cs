using System;
using System.Collections.Generic;
using System.IO;

using UnityEngine;
using Newtonsoft.Json;

using MelonLoader;

namespace NEP.Paranoia.Managers
{
    public static class DataReader
    {
        private static List<Tick> ticks;

        public static string[] ReadFiles(string path)
        {
            List<string> files = new List<string>();

            foreach (string file in Directory.EnumerateFiles(path))
            {
                files.Add(file);
            }

            return files.ToArray();
        }

        public static Entities.BaseMirage.Stats ReadStats(string file)
        {
            string[] files = ReadFiles(MelonUtils.UserDataDirectory + "paranoia/Data/Mirages/");
            Entities.BaseMirage.Stats stats = new Entities.BaseMirage.Stats();

            foreach (string current in files)
            {
                if (current.EndsWith(file + ".json"))
                {
                    string json = File.ReadAllText(current);
                    return JsonConvert.DeserializeObject<Entities.BaseMirage.Stats>(json);
                }
            }

            return stats;
        }

        public static Tick[] ReadTicks()
        {
            string[] files = ReadFiles(MelonUtils.UserDataDirectory + "paranoia/Data/Ticks/");
            ticks = new List<Tick>();

            foreach (string file in files)
            {
                if (file.EndsWith(".meta"))
                {
                    continue;
                }

                string json = File.ReadAllText(file);

                if (string.IsNullOrEmpty(json))
                {
                    ticks.Add(new Tick(-1, "NULL", 0f, null));
                }

                TickTemplate template = JsonConvert.DeserializeObject<TickTemplate>(json);

                string func = template.pEvent;

                if (string.IsNullOrEmpty(func))
                {
                    BuildTick(ticks, template, null);
                }
                else
                {
                    Type eventType = Type.GetType(func, true);

                    if (eventType == null)
                    {
                        BuildTick(ticks, template, null);
                    }
                    else
                    {
                        TickEvents.ParanoiaEvent pEvent = Activator.CreateInstance(eventType) as TickEvents.ParanoiaEvent;

                        BuildTick(ticks, template, pEvent);
                    }
                }
            }

            return ticks.ToArray();
        }

        private static Tick BuildTick(List<Tick> list, TickTemplate template, TickEvents.ParanoiaEvent pEvent)
        {
            Tick tick = new Tick(template.id, template.name, template.tick, pEvent);

            list.Add(tick);

            return tick;
        }
    }

}
