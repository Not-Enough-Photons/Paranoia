using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NotEnoughPhotons.paranoia
{
    public class AudioHallucination : BaseHallucination
    {
        public AudioHallucination(System.IntPtr ptr) : base(ptr) { }

        public enum AuditoryType
        {
            None,
            Chaser,
            Ambient,
            Darkness
        }

        public AuditoryType auditoryType;

        public AudioClip[] clips;

        private AudioSource source;

        private float audioTimer = 0f;

        protected override void Awake()
        {
            base.Awake();

            if (GetComponent<AudioSource>() == null) { return; }

            source = GetComponent<AudioSource>();
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            if (source == null || clips == null || clips.Length == -1) { return; }
            if (auditoryType == AuditoryType.None) { return; }

            audioTimer = 0f;

            source.clip = clips[Random.Range(0, clips.Length)];
            source.dopplerLevel = 0f;
            source.spatialBlend = 0.80f;

            if (auditoryType == AuditoryType.Ambient)
            {
                source.dopplerLevel = 0f;
                source.spatialBlend = 0f;
            }

            if (auditoryType == AuditoryType.Chaser)
            {
                source.loop = true;
            }

            source.Play();
        }

        protected override void Update()
        {
            base.Update();

            if (!source.loop)
            {
                if (source.clip == null) { return; }

                audioTimer += Time.deltaTime;

                if (audioTimer >= source.clip.length)
                {
                    gameObject.SetActive(false);
                }
            }
        }
    }

}