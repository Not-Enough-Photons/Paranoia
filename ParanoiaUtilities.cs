﻿using UnityEngine;
using System;
using System.Globalization;

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

        public static DateTime CalculateTargetTimeDifference(DateTime targetTime)
        {
            TimeSpan difference = targetTime - DateTime.Now;

            return new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, difference.Hours, difference.Minutes, difference.Seconds);
        }

        internal static int GetSystemHour()
        {
            return int.Parse(DateTime.Now.ToString("HH", CultureInfo.InvariantCulture));
        }
    }
}
