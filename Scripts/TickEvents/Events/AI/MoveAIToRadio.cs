using UnityEngine;
using NEP.Paranoia.Managers;
using NEP.Paranoia.ParanoiaUtilities;
using StressLevelZero.AI;
using PuppetMasta;

namespace NEP.Paranoia.TickEvents.Events
{
    public class MoveAIToRadio : ParanoiaEvent
    {
        public override void Start()
        {
            /*BehaviourBaseNav[] navs;
            ParanoiaUtilities.Utilities.FindAIBrains(out navs);

            GameObject radio = GameManager.hRadio.gameObject;

            if(radio == null) { return; }

            foreach(BehaviourBaseNav nav in navs)
            {
                Paranoia.instance.gameManager.MoveAIToPoint(nav, radio.transform.position);
            }*/
        }
    }
}
