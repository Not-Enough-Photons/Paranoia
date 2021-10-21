using NEP.Paranoia.Managers;
using NEP.Paranoia.ParanoiaUtilities;

using UnityEngine;
using UnityEngine.Audio;

namespace NEP.Paranoia.TickEvents.Events
{
    public class DeafenPlayer : ParanoiaEvent
    {
        public override void Start()
        {
            Audio_Manager audioManager = Object.FindObjectOfType<Audio_Manager>();

            Paranoia.instance.gameManager.deafenSource.clip = Paranoia.instance.deafenSounds[0];

            Paranoia.instance.gameManager.deafenSource.Play();

            MelonLoader.MelonCoroutines.Start(CoVolumeRoutine(audioManager, Paranoia.instance.gameManager.deafenSource));
        }

        private System.Collections.IEnumerator CoVolumeRoutine(Audio_Manager manager, AudioSource deafSource)
        {
            if(deafSource == null) { yield break; }

            while(manager.audio_SFXVolume >= 1.25f)
            {
                if(deafSource == null) { break; }

                manager.audio_SFXVolume = Mathf.MoveTowards(manager.audio_SFXVolume, 1f, Time.deltaTime);
                manager.SETMIXERS();

                deafSource.volume = Mathf.MoveTowards(deafSource.volume, 1f, 0.1f * Time.deltaTime);

                yield return null;
            }

            manager.audio_SFXVolume = 0f;
            manager.SETMIXERS();

            yield return new WaitForSeconds(Random.Range(25f, 30f));

            while (manager.audio_SFXVolume <= 6f)
            {
                if (deafSource == null) { break; }

                manager.audio_SFXVolume = Mathf.MoveTowards(manager.audio_SFXVolume, 5f, Time.deltaTime);
                manager.SETMIXERS();

                deafSource.volume = Mathf.MoveTowards(deafSource.volume, 0f, 0.05f * Time.deltaTime);

                yield return null;
            }

            manager.audio_SFXVolume = 7f;
            manager.SETMIXERS();

            deafSource.Stop();

            yield return null;
        }
    }
}
