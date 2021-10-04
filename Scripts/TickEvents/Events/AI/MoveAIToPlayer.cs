﻿using NEP.Paranoia.Utilities;
using NEP.Paranoia.Managers;

using UnityEngine;
using PuppetMasta;

namespace NEP.Paranoia.TickEvents.Events
{
    public class MoveAIToPlayer : ParanoiaEvent
    {
        public override void Start()
        {
            BehaviourBaseNav[] navs;
            ParanoiaUtilities.FindAIBrains(out navs);
            Transform player = ParanoiaUtilities.FindPlayer();

            if(player == null) { return; }
            if(navs == null) { return; }

            foreach (BehaviourBaseNav nav in navs)
            {
                ParanoiaGameManager.instance.MoveAIToPoint(nav, player.position);
            }
        }
    }
}
