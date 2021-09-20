using NEP.Paranoia.Managers;
using NEP.Paranoia.Utilities;

using UnityEngine;
using UnityEngine.Audio;

namespace NEP.Paranoia.TickEvents.Events
{
    public class DeafenPlayer : ParanoiaEvent
    {
        public override void Start()
        {
            AudioMixerGroup sfxMixer = ParanoiaUtilities.GetAudioMixer("SFX");
            AudioMixerGroup gunshotMixer = ParanoiaUtilities.GetAudioMixer("GunShot");
            
            float sfxVolume = 0f;
            float gunshotVolume = 0f;

            sfxMixer.audioMixer.GetFloat("Volume", out sfxVolume);
            gunshotMixer.audioMixer.GetFloat("Volume", out gunshotVolume);

            MelonLoader.MelonCoroutines.Start(CoVolumeRoutine(sfxVolume, gunshotVolume));
        }

        private System.Collections.IEnumerator CoVolumeRoutine(float sfxVolume, float gunshotVolume)
        {
            while(sfxVolume > 0f && gunshotVolume > 0f)
            {
                sfxVolume = Mathf.MoveTowards(sfxVolume, 0f, Time.deltaTime);
                gunshotVolume = Mathf.MoveTowards(gunshotVolume, 0f, Time.deltaTime);

                yield return null;
            }

            yield return new WaitForSeconds(Random.Range(1f, 10f));

            yield return new WaitForSeconds(Random.Range(30f, 60f));

            while (sfxVolume <= 0f && gunshotVolume <= 0f)
            {
                sfxVolume = Mathf.MoveTowards(sfxVolume, 1f, Time.deltaTime);
                gunshotVolume = Mathf.MoveTowards(gunshotVolume, 1f, Time.deltaTime);

                yield return null;
            }

            yield return null;
        }
    }
}
