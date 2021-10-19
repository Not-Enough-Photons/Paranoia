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
            AudioMixer mainMixer = Utilities.GetAudioMixer();

            Paranoia.instance.gameManager.deafenSource.clip = Paranoia.instance.deafenSounds[0];

            Paranoia.instance.gameManager.deafenSource.Play();

            MelonLoader.MelonCoroutines.Start(CoVolumeRoutine(mainMixer, Paranoia.instance.gameManager.deafenSource));
        }

        private System.Collections.IEnumerator CoVolumeRoutine(AudioMixer mixer, AudioSource deafSource)
        {
            if(deafSource == null) { yield break; }

            float mainVolume = 0f;
            mixer.GetFloat("channel_SFX", out mainVolume);

            while(mainVolume > 0f)
            {
                mainVolume = Mathf.MoveTowards(mainVolume, 0f, Time.deltaTime);
                mixer.SetFloat("channel_SFX", mainVolume);

                deafSource.volume = Mathf.MoveTowards(deafSource.volume, 1f, Time.deltaTime);

                yield return null;
            }

            yield return new WaitForSeconds(Random.Range(30f, 60f));

            while (mainVolume <= 0f)
            {
                mainVolume = Mathf.MoveTowards(mainVolume, 1f, Time.deltaTime);
                mixer.SetFloat("channel_SFX", mainVolume);

                deafSource.volume = Mathf.MoveTowards(deafSource.volume, 0f, Time.deltaTime);

                yield return null;
            }

            deafSource.Stop();

            yield return null;
        }
    }
}
