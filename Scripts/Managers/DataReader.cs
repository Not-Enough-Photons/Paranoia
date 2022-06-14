using System;
using System.Collections.Generic;
using System.IO;

using UnityEngine;
using Newtonsoft.Json;

using MelonLoader;

using NEP.Paranoia.ParanoiaUtilities;

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

        public static string[] ReadEntityRegistry(string file)
        {
            string dir = MelonUtils.UserDataDirectory + "/paranoia/" + file;

            if(file == "entityreg.txt")
            {
                return File.ReadAllLines(dir);
            }

            return new string[0];
        }

        public static MapLevelFlags ParseTickMapLevel(string flags)
        {
            if (string.IsNullOrEmpty(flags))
            {
                return 0;
            }

            string[] split = flags.Split('|');
            MapLevelFlags mapLevel = 0;

            foreach (string flag in split)
            {
                object objParsed = Enum.Parse(typeof(MapLevel), flag);
                mapLevel ^= (MapLevelFlags)objParsed;
            }

            return mapLevel;
        }

        public static Entities.BaseMirage.Stats ReadStats(string file)
        {
            string[] files = ReadFiles(MelonUtils.UserDataDirectory + "/paranoia/json/Mirages/");
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
            string[] files = ReadFiles(MelonUtils.UserDataDirectory + "/paranoia/json/Ticks/");
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
                    ticks.Add(new Tick(-1, "NULL", 0f, null, null, 0f, MapLevelFlags.MainMenu, null));
                }

                TickTemplate template = JsonConvert.DeserializeObject<TickTemplate>(json);

                string func = "NEP.Paranoia.TickEvents.Events." + template.pEvent;

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
            Tick tick = new Tick(template.id, template.name, template.tick, template.minMaxTime, template.minMaxRNG, template.insanity, ParseTickMapLevel(template.runOnMaps), pEvent);

            list.Add(tick);

            return tick;
        }
    }

}
