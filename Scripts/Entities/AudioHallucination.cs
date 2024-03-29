﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;

namespace NEP.Paranoia.Entities
{
    public class AudioHallucination : BaseHallucination
    {
        public AudioHallucination(System.IntPtr ptr) : base(ptr) { }

        public struct AudioSettings
        {
            public AuditoryType auditoryType { get; set; }
        }

        public enum AuditoryType
        {
            None,
            Chaser,
            Ambient,
            Darkness,
            Teleporting,
            Crying,
            Paralyzer
        }

        public AuditoryType auditoryType;

        public AudioClip[] clips;

        public AudioClip lastPlayedClip;

        public bool timerUsesAudioLength;

        protected readonly string audioJsonPath = "UserData/paranoia/json/AudioHallucination/";

        protected AudioSource source;

        private float audioTimer = 0f;

        public override void ReadValuesFromJSON(string json)
        {
            JObject obj = JObject.Parse(json);

            if(obj["auditoryType"] != null)
            {
                auditoryType = (AuditoryType)System.Enum.Parse(typeof(AuditoryType), obj["auditoryType"].ToString());
            }
            
            if(obj["timerUsesAudioLength"] != null)
            {
                timerUsesAudioLength = bool.Parse(obj["timerUsesAudioLength"].ToString());
            }
            
            base.ReadValuesFromJSON(json);
        }

        protected override void Awake()
        {
            if (GetComponent<AudioSource>() == null) { return; }

            source = GetComponent<AudioSource>();

            base.Awake();
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            if (source == null || clips == null || clips.Length == -1) { return; }
            if (auditoryType == AuditoryType.None) { return; }

            audioTimer = 0f;

            AudioClip nextClip = clips[Random.Range(0, clips.Length)];
            source.clip = nextClip;

            source.dopplerLevel = 0f;
            source.spatialBlend = 0.95f;

            if (auditoryType == AuditoryType.Ambient)
            {
                source.dopplerLevel = 0f;
                source.spatialBlend = 0f;

                MelonLoader.MelonCoroutines.Start(CoHideSelf(source.clip.length));
            }

            if (auditoryType == AuditoryType.Crying)
            {
                source.spatialBlend = 0.85f;
                source.loop = true;
            }

            if (auditoryType == AuditoryType.Darkness)
            {
                source.dopplerLevel = 1f;
                source.spatialBlend = 1f;

                MelonLoader.MelonCoroutines.Start(CoHideSelf(source.clip.length));
            }

            if (auditoryType == AuditoryType.Chaser)
            {
                source.loop = true;
            }

            if (auditoryType == AuditoryType.Teleporting)
            {
                if (timerUsesAudioLength)
                {
                    maxTeleportDelay = source.clip.length;
                }
            }

            lastPlayedClip = nextClip;

            if(nextClip == lastPlayedClip)
            {
                nextClip = clips[Random.Range(0, clips.Length)];
            }

            source.Play();
        }

        protected override void Update()
        {
            base.Update();

            if (GetReachedTeleportDelay())
            {
                source.Play();
            }

            if (!source.loop && auditoryType == AuditoryType.Chaser)
            {
                if (source.clip == null) { return; }

                audioTimer += Time.deltaTime;

                if (audioTimer >= source.clip.length)
                {
                    gameObject.SetActive(false);
                }
            }
        }

        protected virtual IEnumerator CoHideSelf(float duration)
        {
            if(duration <= 0f)
            {
                duration = 0f;
            }

            yield return new WaitForSeconds(duration);
            gameObject.SetActive(false);
        }
    }

}