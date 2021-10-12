using NEP.Paranoia.Managers;
using NEP.Paranoia.Entities;
using UnityEngine;

namespace NEP.Paranoia.TickEvents.Mirages
{
    public class AmbientAudioSpawn : ParanoiaEvent
    {
        public override void Start()
        {
            if (Paranoia.instance.gameManager.paralysisMode) { return; }
        }

        public override void Stop()
        {
            base.Stop();
        }
    }
}
