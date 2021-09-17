using NEP.Paranoia.Managers;
using NEP.Paranoia.Entities;
using UnityEngine;

namespace NEP.Paranoia.TickEvents.Mirages
{
    public class AmbientAudioSpawn : ParanoiaEvent
    {
        public override void Start()
        {
            if (ParanoiaGameManager.instance.paralysisMode) { return; }

            ParanoiaGameManager manager = ParanoiaGameManager.instance;

            int rng = manager.rng;

            bool isRareNumber = rng >= 20 && rng <= 45 || rng >= 50 && rng <= 75;

            manager.hAmbience.useScreams = isRareNumber;
            manager.hAmbience.gameObject.SetActive(isRareNumber);
        }

        public override void Stop()
        {
            base.Stop();
        }
    }
}
