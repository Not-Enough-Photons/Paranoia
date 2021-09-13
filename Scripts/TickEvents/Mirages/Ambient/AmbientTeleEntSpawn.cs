﻿using NotEnoughPhotons.Paranoia.Managers;

namespace NotEnoughPhotons.Paranoia.TickEvents.Mirages
{
    public class AmbientTeleEntSpawn : ParanoiaEvent
    {
        public override void Start()
        {
            if (ParanoiaGameManager.instance.paralysisMode) { return; }

            ParanoiaGameManager.instance.hTeleportingEntity.gameObject.SetActive(true);
        }
    }
}