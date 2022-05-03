using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NEP.Paranoia.Entities
{
    public class AudioMirage : BaseMirage
    {
        struct AudioTimeStamp
        {
            public AudioTimeStamp(AudioClip clip, float time)
            {
                this.clip = clip;
                this.time = time;
            }

            public AudioClip clip;
            public float time;
        }

        public AudioClip[] clips;

        private AudioClip lastPlayedClip;

        private AudioSource source;

        private List<AudioTimeStamp> timeStamps;

        protected override void Awake()
        {
            base.Awake();

            source = GetComponent<AudioSource>();

            if (source == null)
            {
                return;
            }
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            if (source == null)
            {
                return;
            }

            if (clips.Length <= 0)
            {
                return;
            }

            AudioClip clip = clips[Random.Range(0, clips.Length)];

            if (clip == null)
            {
                return;
            }

            if (lastPlayedClip != null)
            {
                if (clip == lastPlayedClip)
                {
                    clip = clips[Random.Range(0, clips.Length)];
                }
            }

            if (stats.audioTeleport)
            {
                stats.timeTeleport = clip.length;
            }

            source.clip = clip;

            source.loop = stats.looping;
            source.dopplerLevel = stats.doppler;
            source.spatialBlend = stats.spatialBlend;

            source.Play();

            lastPlayedClip = source.clip;

            AppendTimeStamp(new AudioTimeStamp(lastPlayedClip, Time.timeSinceLevelLoad));
        }

        protected override void Update()
        {
            base.Update();
        }

        protected override void OnTeleport()
        {
            base.OnTeleport();

            if (source.loop || source.clip == null)
            {
                return;
            }

            source.Play();
        }

        private void AppendTimeStamp(AudioTimeStamp stamp)
        {
            if (timeStamps == null)
            {
                timeStamps = new List<AudioTimeStamp>();
            }

            if (timeStamps.Count >= 10)
            {
                timeStamps.Clear();
            }

            timeStamps.Add(stamp);
        }
    }

}
