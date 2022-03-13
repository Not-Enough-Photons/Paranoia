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
    [MelonLoader.RegisterTypeInIl2Cpp]
    public class TickManager : MonoBehaviour
    {
        public TickManager(System.IntPtr ptr) : base(ptr) { }

        public List<Tick> ticks = new List<Tick>();
        public List<Tick> darkTicks = new List<Tick>();

        public bool debug = false;

        private void Awake() => InitializeTicks();

        private void Update()
        {
            if (Paranoia.instance.isTargetLevel)
            {
                GameManager.playerCircle.CalculatePlayerCircle(0f);

                if (debug || Time.timeScale == 0) { return; }

                try
                {
                    UpdateTicks(ticks);
                    UpdateTicks(darkTicks);
                }
                catch (System.Exception e)
                {
                    MelonLoader.MelonLogger.Error(e);
                }
            }
        }

        private void UpdateTicks(List<Tick> ticks)
        {
            for (int i = 0; i < ticks.Count; i++)
            {
                if (ticks[i].GetEvent() == null) { continue; }

                ticks[i].Update();
            }
        }

        private void ReadTicksFromJSON(string json)
        {
            List<Tick.JSONSettings> tickSettings = JsonConvert.DeserializeObject<List<Tick.JSONSettings>>(json);

            foreach (Tick.JSONSettings settings in tickSettings)
            {
                if (settings.fireEvent.StartsWith("E_"))
                {
                    string mainFunc = settings.fireEvent.Replace("E_", string.Empty);
                    string nameSpace = "NEP.Paranoia.TickEvents.Events.";
                    FinalizeTick(settings, nameSpace, mainFunc);
                }
                else if (settings.fireEvent.StartsWith("M_"))
                {
                    string mainFunc = settings.fireEvent.Replace("M_", string.Empty);
                    string nameSpace = "NEP.Paranoia.TickEvents.Mirages.";
                    FinalizeTick(settings, nameSpace, mainFunc);
                }
            }
        }

        private void FinalizeTick(JSONSettings settings, string nameSpace, string mainFunc)
        {
            try
            {
                TickType tickType = (TickType)System.Enum.Parse(typeof(TickType), settings.tickType);

                if (mainFunc.Contains("("))
                {
                    FinalizeTickMethod(settings, tickType, nameSpace, mainFunc);
                }
                else
                {
                    System.Type targetActionType = System.Type.GetType(nameSpace + mainFunc);

                    ParanoiaEvent ctorEvent = System.Activator.CreateInstance(targetActionType) as ParanoiaEvent;

                    CreateTick(settings.minRange != 0 || settings.maxRange != 0, settings, tickType, ctorEvent);
                }
            }
            catch (System.Exception e)
            {
                throw new System.Exception($"Exception at {settings.fireEvent} in {settings.tickName}: {e.ToString()}");
            }
        }

        private void FinalizeTickMethod(JSONSettings settings, TickType tickType, string nameSpace, string mainFunc)
        {
            string method = Utilities.GetMethodNameString(mainFunc);
            string parameter = Utilities.GetParameterString(mainFunc);

            System.Type type = System.Type.GetType(nameSpace + method);

            object instance = System.Activator.CreateInstance(type, new object[] { Utilities.GetHallucination(parameter) });

            CreateTick(settings.minRange != 0f || settings.maxRange != 0f, settings, tickType, instance as SpawnMirage);
        }

        private Tick CreateTick(bool isRandom, JSONSettings settings, TickType tickType, ParanoiaEvent Event)
        {
            Tick standard = new Tick(settings.tickName, settings.tick, settings.maxTick, settings.minRNG, settings.maxRNG, settings.useInsanity, settings.targetInsanity, tickType, MapLevel.Arena, Event);
            Tick random = new Tick(settings.tickName, settings.tick, settings.minRange, settings.maxRange, settings.minRNG, settings.maxRNG, settings.useInsanity, settings.targetInsanity, tickType, MapLevel.Arena, Event);

            if (tickType == TickType.Any || tickType == TickType.Light)
            {
                ticks?.Add(isRandom ? random : standard);
            }
            else if (tickType == TickType.Dark)
            {
                darkTicks?.Add(isRandom ? random : standard);
            }

            return isRandom ? random : standard;
        }


        private void InitializeTicks()
        {
            ticks = new List<Tick>();
            darkTicks = new List<Tick>();

            ReadTicksFromJSON(System.IO.File.ReadAllText("UserData/paranoia/json/Ticks/ticks.json"));
        }
    }
}
