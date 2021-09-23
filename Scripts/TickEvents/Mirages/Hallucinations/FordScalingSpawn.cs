﻿using NEP.Paranoia.Managers;

namespace NEP.Paranoia.TickEvents.Mirages
{
    public class FordScalingSpawn : ParanoiaEvent
    {
        public override void Start()
        {
            ParanoiaGameManager.hFordScaling.gameObject.SetActive(true);
        }
    }
}
