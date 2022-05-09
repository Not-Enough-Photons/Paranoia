using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NEP.Paranoia.Entities
{
    [MelonLoader.RegisterTypeInIl2Cpp]
    public class AudioMirage : BaseMirage
    {
        public AudioMirage(System.IntPtr ptr) : base(ptr) { }

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

        public List<AudioClip> clips;

        private AudioClip lastPlayedClip;

        private AudioSource source;

        private float clip_t;
        private float clipLength;

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

            AddClips();

            Play();

            AppendTimeStamp(new AudioTimeStamp(lastPlayedClip, Time.timeSinceLevelLoad));
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            clip_t = 0f;
            clipLength = 0f;
        }

        protected override void Update()
        {
            base.Update();

            if (!stats.audioTeleport && !stats.looping)
            {
                clip_t += Time.deltaTime;

                if (clip_t >= clipLength)
                {
                    clip_t = 0f;
                    gameObject.SetActive(false);
                }
            }
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

        private void AddClips()
        {
            string[] nameList = stats.clips;

            foreach(string clipName in nameList)
            {
                MelonLoader.MelonLogger.Msg(clipName);

                foreach(AudioClip clip in Paranoia.instance.mainClips)
                {
                    if(clips == null)
                    {
                        clips = new List<AudioClip>();
                    }

                    if(clip == null)
                    {
                        continue;
                    }

                    if(clipName == clip.name)
                    {
                        clips.Add(clip);
                    }
                }
            }
        }

        private void Play()
        {
            if (source == null)
            {
                return;
            }

            if (clips.Count <= 0)
            {
                return;
            }

            AudioClip clip = clips[Random.Range(0, clips.Count)];

            if (clip == null)
            {
                return;
            }

            if (lastPlayedClip != null)
            {
                if (clip == lastPlayedClip)
                {
                    clip = clips[Random.Range(0, clips.Count)];
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

            clipLength = source.clip.length;
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
