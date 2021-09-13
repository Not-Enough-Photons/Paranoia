using NotEnoughPhotons.Paranoia.Utilities;
using NotEnoughPhotons.Paranoia.Managers;
using UnityEngine;

namespace NotEnoughPhotons.Paranoia.TickEvents.Events
{
    public class SpawnFirstRadio : ParanoiaEvent
    {
        public override void Start()
        {
           /* GameObject radioClone = ParanoiaGameManager.instance.radioClone;
            AudioSource radioSource = ParanoiaGameManager.instance.radioSource;
            AudioManager audioManager = ParanoiaGameManager.instance.audioManager;

            radioClone.SetActive(false);

            radioClone.transform.position = ParanoiaGameManager.instance.playerCircle.CalculatePlayerCircle(Random.Range(0, 360), 10f);

            radioClone.transform.LookAt(ParanoiaUtilities.FindPlayer());

            radioSource.clip = audioManager.startingTune;
            radioSource.spatialBlend = 0.75f;

            ParanoiaGameManager.instance.SetFirstRadioSpawn(true);

            radioClone.SetActive(true);

            MelonLoader.MelonCoroutines.Start(CoRadioHide(radioSource.clip.length));*/
        }

        private System.Collections.IEnumerator CoRadioHide(float time)
        {
            yield return new WaitForSeconds(time);
            /*
            ParanoiaGameManager.instance.SetFirstRadioSpawn(false);

            if (ParanoiaGameManager.instance.radioClone.activeInHierarchy)
            {
                ParanoiaGameManager.instance.radioClone.SetActive(false);
            }*/
        }
    }
}
