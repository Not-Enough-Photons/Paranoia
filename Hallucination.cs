using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Log = MelonLoader.MelonLogger;

namespace NotEnoughPhotons.paranoia
{
    public class Hallucination : MonoBehaviour
    {
        public Hallucination(System.IntPtr ptr) : base(ptr) { }

        [System.Flags]
        public enum HallucinationType
        {
            Auditory = 1,
            Visual = 2
        }

        [System.Flags]
        public enum HallucinationFlags
        {
            None,
            HideWhenSeen = 1
        }

        public enum HallucinationClass
        {
            Chaser,
            Watcher,
            DarkVoice
        }

        public HallucinationType hType;
        public HallucinationFlags hFlags;
        public HallucinationClass hClass;

        public System.Action OnReachedTarget;

        public Transform target;

        public Transform cameraTarget;

        public Vector3 spawnPoint;

        public float hDistanceToDisappear;
        public float hChaseSpeed;
        public float hDelayTime;

        public AudioSource source;
        public AudioManager audioManager;

        public void Initialize(HallucinationType hType, HallucinationFlags hFlags, HallucinationClass hClass, float distanceToDisappear, float chaseSpeed, float delayTime)
        {
            this.hType = hType;
            this.hFlags = hFlags;
            this.hClass = hClass;
            this.hDistanceToDisappear = distanceToDisappear;
            this.hChaseSpeed = chaseSpeed;
            this.hDelayTime = delayTime;

            target = Paranoia.instance.FindPlayer();
            cameraTarget = Paranoia.instance.FindPlayer();
        }

        public void SetSpawnPoint(Vector3 spawnPoint)
        {
            transform.position = spawnPoint;
        }

        public AudioSource GetSource()
        {
            if (source == null) { return null; }

            return source;
        }

        private void OnEnable()
        {
            MelonLoader.MelonCoroutines.Start(CoUpdateHallucination(hType, hClass, hFlags));
        }

        private void OnDisable()
        {
            MelonLoader.MelonCoroutines.Stop(CoUpdateHallucination(hType, hClass, hFlags));
        }

        private void Update()
        {
            if (Vector3.Distance(target.position, transform.position) < hDistanceToDisappear)
            {
                if (OnReachedTarget != null)
                {
                    OnReachedTarget();
                }

                gameObject.SetActive(false);
            }

            if (hFlags.HasFlag(HallucinationFlags.HideWhenSeen))
            {
                if (cameraTarget != null)
                {
                    if (Vector3.Dot(cameraTarget.forward, transform.forward) <= -0.95f)
                    {
                        gameObject.SetActive(false);
                    }
                }
            }
        }

        private IEnumerator CoUpdateHallucination(HallucinationType hType, HallucinationClass hClass, HallucinationFlags hFlags)
        {
            if (hType.HasFlag(HallucinationType.Auditory) && GetComponent<AudioSource>() != null)
            {
                Log.Msg($"Hallucination type is {hType}");

                source = GetComponent<AudioSource>();
                Log.Msg($"Got {source.name} source");
                source.spatialBlend = 1f;
                Log.Msg($"Set spacial blend");

                if (hClass == HallucinationClass.DarkVoice)
                {
                    Log.Msg($"Hallucination class is {hClass}");
                    transform.position = target.forward * -2f;
                    Log.Msg($"Got {target.name}");

                    if (Paranoia.instance.darkVoices.Count > 0)
					{
                        source.clip = Paranoia.instance.darkVoices[Random.Range(0, Paranoia.instance.darkVoices.Count)];
                        Log.Msg($"Set up {source.clip}");
                    }
                    
                    source.loop = false;

                    source.Play();
                    Log.Msg($"Playing...");

                    Log.Msg($"Waiting for {source.clip.length} seconds before stopping");
                    yield return new WaitForSeconds(source.clip.length);
                    gameObject.SetActive(false);
                }
                else if (hClass == HallucinationClass.Chaser)
                {
                    Log.Msg($"Hallucination class is {hClass}");

                    if (Paranoia.instance.chaserAmbience.Count > 0)
					{
                        source.clip = Paranoia.instance.chaserAmbience[Random.Range(0, Paranoia.instance.chaserAmbience.Count)];
                        Log.Msg($"Set up {source.clip}");
                    }
                    
                    source.loop = true;

                    source.Play();
                    Log.Msg($"Playing...");
                }
                else if (hClass == HallucinationClass.Watcher)
                {
                    Log.Msg($"Hallucination class is {hClass}");
                    if (Paranoia.instance.watcherAmbience.Count > 0)
					{
                        source.clip = Paranoia.instance.watcherAmbience[Random.Range(0, Paranoia.instance.watcherAmbience.Count)];
                        Log.Msg($"Set up {source.clip}");
                    }
                    
                    source.loop = true;

                    source.Play();
                    Log.Msg($"Playing...");
                }
            }

            if (hClass == HallucinationClass.Chaser)
            {
                Log.Msg($"Hallucination class is {hClass}");

                if (hDelayTime > 0)
                {
                    Log.Msg($"Waiting for {hDelayTime} seconds...");
                    yield return new WaitForSeconds(hDelayTime);
                }

                while (Vector3.Distance(target.position, transform.position) > hDistanceToDisappear)
                {
                    transform.position += transform.forward * hChaseSpeed * Time.deltaTime;
                    yield return null;
                }
            }

            if (hClass == HallucinationClass.Watcher)
            {
                Log.Msg($"Hallucination class is {hClass}");

                while (Vector3.Distance(target.position, transform.position) < hDistanceToDisappear)
                {
                    gameObject.SetActive(false);
                    yield return null;
                }
            }

            yield return null;
        }
    }

}