﻿using UnityEngine;
using NEP.Paranoia.Managers;
using NEP.Paranoia.Utilities;
using StressLevelZero.AI;
using PuppetMasta;

namespace NEP.Paranoia.TickEvents.Events
{
    public class MoveAIToRadio : ParanoiaEvent
    {
        public override void Start()
        {
            BehaviourBaseNav[] navs;
            Utilities.Utilities.FindAIBrains(out navs);

            GameObject radio = ParanoiaGameManager.hRadio.gameObject;

            if(radio == null) { return; }

            foreach(BehaviourBaseNav nav in navs)
            {
                ParanoiaGameManager.instance.MoveAIToPoint(nav, radio.transform.position);
            }
        }
    }
}
