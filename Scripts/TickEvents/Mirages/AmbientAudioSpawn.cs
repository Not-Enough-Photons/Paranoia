using NotEnoughPhotons.Paranoia.Managers;
using UnityEngine;

namespace NotEnoughPhotons.Paranoia.TickEvents.Mirages
{
    public class AmbientAudioSpawn : ParanoiaEvent
    {
        public override void Start()
        {
            if (ParanoiaGameManager.instance.firstRadioSpawn) { return; }

            AudioManager audioManager = ParanoiaGameManager.instance.audioManager;

            int rng = ParanoiaGameManager.instance.rng;

            bool isRareNumber = rng >= 20 && rng <= 45 || rng >= 50 && rng <= 75;

            if (isRareNumber)
            {
                audioManager.PlayOneShot(audioManager.ambientScreaming[Random.Range(0, audioManager.ambientScreaming.Count)]);
            }
            else
            {
                audioManager.PlayOneShot(audioManager.ambientGeneric[Random.Range(0, audioManager.ambientGeneric.Count)]);
            }
        }

        public override void Stop()
        {
            base.Stop();
        }
    }
}
