using System;
using BoneLib;
using UnhollowerBaseLib;
using System.Collections.Generic;
using System.Linq;
using PuppetMasta;
using SLZ.AI;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Paranoia.Helpers
{
    public static class Utilities
    {
        public static Il2CppArrayBase<AIBrain> FindAIBrains()
        {
            return Object.FindObjectsOfType<AIBrain>();
        }
        
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
        
        public static void MoveAIToPoint(BehaviourBaseNav behaviour, Vector3 point)
        {
            behaviour.sensors.hearingSensitivity = 0f;
            behaviour.SetHomePosition(point, true, true);
            behaviour.Investigate(point, true, 120f);
        }

        public static void FreezePlayer(bool freeze)
        {
            var physRig = Player.physicsRig;
            physRig.rbFeet.isKinematic = freeze;
            physRig.m_pelvis.GetComponent<Rigidbody>().isKinematic = freeze;
            physRig.leftHand.GetComponent<Rigidbody>().isKinematic = freeze;
            physRig.rightHand.GetComponent<Rigidbody>().isKinematic = freeze;
        }

        public static bool CheckDate(int month, int day)
        {
            var currentDate = DateTime.Now;
            return currentDate.Month == month && currentDate.Day == day;
        }
    }
}