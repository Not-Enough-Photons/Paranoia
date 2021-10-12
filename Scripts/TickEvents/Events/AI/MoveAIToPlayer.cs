using NEP.Paranoia.ParanoiaUtilities;
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
            ParanoiaUtilities.Utilities.FindAIBrains(out navs);
            Transform player = Utilities.FindPlayer();

            if(player == null) { return; }
            if(navs == null) { return; }

            foreach (BehaviourBaseNav nav in navs)
            {
                Paranoia.instance.gameManager.MoveAIToPoint(nav, player.position);
            }
        }
    }
}
