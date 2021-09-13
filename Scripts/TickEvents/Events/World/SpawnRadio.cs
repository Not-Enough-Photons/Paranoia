using MelonLoader;
using ModThatIsNotMod.RandomShit;
using NotEnoughPhotons.Paranoia.Utilities;
using NotEnoughPhotons.Paranoia.Managers;
using UnityEngine;

namespace NotEnoughPhotons.Paranoia.TickEvents.Events
{
    public class SpawnRadio : ParanoiaEvent
    {
        public override void Start()
        {
            new MoveAIToRadio().Start();

            GameObject radioClone = ParanoiaGameManager.instance.radioClone;
            AudioSource radioSource = ParanoiaGameManager.instance.radioSource;
            AudioManager audioManager = ParanoiaGameManager.instance.audioManager;

            radioClone.SetActive(false);

            radioClone.transform.position = ParanoiaGameManager.instance.playerCircle.CalculatePlayerCircle(Random.Range(0, 360), 10f);
            radioClone.transform.LookAt(ParanoiaUtilities.FindPlayer());
            radioSource.clip = audioManager.radioTunes[Random.Range(0, audioManager.radioTunes.Count)];
            radioSource.spatialBlend = 1f;

            radioClone.SetActive(true);

            MelonCoroutines.Start(CoRadioHide(radioSource.clip.length));
        }

        private System.Collections.IEnumerator CoRadioHide(float time)
        {
            yield return new WaitForSeconds(time);

            ParanoiaGameManager.instance.SetFirstRadioSpawn(false);

            if (ParanoiaGameManager.instance.radioClone.activeInHierarchy)
            {
                ParanoiaGameManager.instance.radioClone.SetActive(false);
            }

            yield return null;
        }
    }
}
