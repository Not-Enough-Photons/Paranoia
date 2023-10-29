using System;
using BoneLib;
using UnhollowerBaseLib;
using System.Collections.Generic;
using System.Linq;
using PuppetMasta;
using SLZ.AI;
using UnityEngine;
using UnityEngine.Diagnostics;
using Object = UnityEngine.Object;

namespace Paranoia.Helpers
{
    /// <summary>
    /// A collection of helper methods for Paranoia.
    /// </summary>
    public static class Utilities
    {
        /// <summary>
        /// Finds all NPCs within the scene.
        /// </summary>
        /// <returns>An array of all NPCs within the scene.</returns>
        public static Il2CppArrayBase<AIBrain> FindAIBrains()
        {
            return Object.FindObjectsOfType<AIBrain>();
        }
        /// <summary>
        /// Similar to Il2CppArayBase FindAIBrains, but also returns the BehaviourBaseNav array.
        /// </summary>
        /// <returns>An array of all NPCs within the scene and their BehaviourBaseNavs.</returns>
        public static AIBrain[] FindAIBrains(out BehaviourBaseNav[] navs)
        {
            AIBrain[] result = Object.FindObjectsOfType<AIBrain>();
            navs = FindBaseNavs(result);

            return result;
        }

        private static BehaviourBaseNav[] FindBaseNavs(AIBrain[] brains)
        {
            var baseNavs = new List<BehaviourBaseNav>();

            brains.ToList().ForEach((brain) =>
            {
                baseNavs.Add(brain != null ? brain.behaviour : null);
            });

            return baseNavs.ToArray();
        }
        /// <summary>
        /// Moves all NPCs to a given point.
        /// </summary>
        /// <param name="behaviour">The AIBrain to be moved</param>
        /// <param name="point">The position to move the AIBrain to</param>
        public static void MoveAIToPoint(BehaviourBaseNav behaviour, Vector3 point)
        {
            behaviour.sensors.hearingSensitivity = 0f;
            behaviour.SetHomePosition(point, true, true);
            behaviour.Investigate(point, true, 120f);
        }
        /// <summary>
        /// Freezes the player.
        /// <br/>Used in Paralyzer.<see cref="Entities.Paralyzer.Start"/>
        /// </summary>
        /// <param name="freeze">Whether to freeze or unfreeze the player.</param>
        public static void FreezePlayer(bool freeze)
        {
            var physRig = Player.physicsRig;
            physRig.rbFeet.isKinematic = freeze;
            physRig.m_pelvis.GetComponent<Rigidbody>().isKinematic = freeze;
            physRig.leftHand.GetComponent<Rigidbody>().isKinematic = freeze;
            physRig.rightHand.GetComponent<Rigidbody>().isKinematic = freeze;
        }
        /// <summary>
        /// Checks the date to see if it is a given month and day, then returns as true if it is that day.
        /// </summary>
        /// <param name="month">The number of the month to use</param>
        /// <param name="day">The number of the day to use</param>
        /// <returns>True if it is the given month and day, false if not.</returns>
        public static bool CheckDate(int month, int day)
        {
            var currentDate = DateTime.Now;
            return currentDate.Month == month && currentDate.Day == day;
        }
        /// <summary>
        /// Crashes the game in a truely Unity fashion: Access Violation
        /// <br/>Used in Crasher.<see cref="Entities.Crasher.OnTriggerEnter"/>
        /// </summary>
        public static void CrashGame()
        {
            Utils.ForceCrash(ForcedCrashCategory.AccessViolation);
        }
    }
}