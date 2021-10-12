﻿using NEP.Paranoia.Managers;
using NEP.Paranoia.ParanoiaUtilities;

using UnityEngine;
using UnityEngine.Audio;

namespace NEP.Paranoia.TickEvents.Events
{
    public class DeafenPlayer : ParanoiaEvent
    {
        public override void Start()
        {
            AudioMixerGroup sfxMixer = Utilities.GetAudioMixer("SFX");
            AudioMixerGroup gunshotMixer = Utilities.GetAudioMixer("GunShot");
            
            float sfxVolume = 0f;
            float gunshotVolume = 0f;

            sfxMixer.audioMixer.GetFloat("Volume", out sfxVolume);
            gunshotMixer.audioMixer.GetFloat("Volume", out gunshotVolume);

            Paranoia.instance.gameManager.deafenSource.clip = Paranoia.instance.deafenSounds[0];

            Paranoia.instance.gameManager.deafenSource.Play();

            MelonLoader.MelonCoroutines.Start(CoVolumeRoutine(Paranoia.instance.gameManager.deafenSource, sfxVolume, gunshotVolume));
        }

        private System.Collections.IEnumerator CoVolumeRoutine(AudioSource deafSource, float sfxVolume, float gunshotVolume)
        {
            if(deafSource == null) { yield break; }

            while(sfxVolume > 0f && gunshotVolume > 0f)
            {
                sfxVolume = Mathf.MoveTowards(sfxVolume, 0f, Time.deltaTime);
                gunshotVolume = Mathf.MoveTowards(gunshotVolume, 0f, Time.deltaTime);

                deafSource.volume = Mathf.MoveTowards(deafSource.volume, 1f, Time.deltaTime);

                yield return null;
            }

            yield return new WaitForSeconds(Random.Range(30f, 60f));

            while (sfxVolume <= 0f && gunshotVolume <= 0f)
            {
                sfxVolume = Mathf.MoveTowards(sfxVolume, 1f, Time.deltaTime);
                gunshotVolume = Mathf.MoveTowards(gunshotVolume, 1f, Time.deltaTime);

                deafSource.volume = Mathf.MoveTowards(deafSource.volume, 0f, Time.deltaTime);

                yield return null;
            }

            deafSource.Stop();

            yield return null;
        }
    }
}
