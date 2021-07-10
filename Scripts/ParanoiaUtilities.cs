using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

using Tick = NotEnoughPhotons.paranoia.ParanoiaGameManager.Tick;

namespace NotEnoughPhotons.paranoia
{
    public class ParanoiaUtilities
    {
        public static Transform FindHead()
        {
            return GameObject.Find("[RigManager (Default Brett)]/[PhysicsRig]/").transform;
        }

        public static Transform FindPlayer()
        {
            // Code lifted from the Boneworks Modding Toolkit.

            GameObject[] array = GameObject.FindGameObjectsWithTag("Player");

            for (int i = 0; i < array.Length; i++)
            {
                bool isTrigger = array[i].name == "PlayerTrigger";

                if (isTrigger)
                {
                    return array[i].transform;
                }
            }

            return null;
        }

        public static List<GameObject> FindGameObjectsWithLayer(string layerName)
        {
            GameObject[] objectsInScene = GameObject.FindObjectsOfType<GameObject>();
            List<GameObject> layerObjects = new List<GameObject>();

            for(int i = 0; i < objectsInScene.Length; i++)
            {
                if(objectsInScene[i].layer == LayerMask.NameToLayer("Static"))
                {
                    layerObjects?.Add(objectsInScene[i]);
                }
            }

            return layerObjects;
        }

        public static bool IsTargetHour(int hour)
        {
            // 24 hour time, for consistency!
            int currentHour = GetSystemHour();

            // Is this not the right hour?
            if (currentHour != hour)
            {
                return false;
            }

            return true;
        }

        public static Tick GetTick(string name)
        {
            return ParanoiaGameManager.instance.GetType().GetField(name, System.Reflection.BindingFlags.NonPublic).GetValue(nameof(name)) as Tick;
        }

        public static DateTime CalculateTargetTimeDifference(DateTime targetTime)
        {
            TimeSpan difference = DateTime.Now - targetTime;

            return new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, difference.Hours, difference.Minutes, difference.Seconds);
        }

        internal static int GetSystemHour()
        {
            return int.Parse(DateTime.Now.ToString("HH", CultureInfo.InvariantCulture));
        }
    }
}
