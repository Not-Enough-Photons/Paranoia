﻿using StressLevelZero.Rig;

using NEP.Paranoia.Utilities;
using UnityEngine;

namespace NEP.Paranoia.TickEvents.Events
{
    public class ClonePlayer : ParanoiaEvent
    {
        public override void Start()
        {
            int count = Object.FindObjectsOfType<RigManager>().Count;

            if(count >= 1) { return; }

            GameObject rig = null;
            Utilities.Utilities.ClonePlayerBody(out rig, Vector3.zero * 2f, Quaternion.identity);
        }
    }
}