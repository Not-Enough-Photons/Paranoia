using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Log = MelonLoader.MelonLogger;

namespace NotEnoughPhotons.paranoia
{
    public class Hallucination : MonoBehaviour
    {
        public Hallucination(System.IntPtr ptr) : base(ptr) { }

        public enum HallucinationName
        {
            ShadowMan,
            StaringMan,
            CeilingMan,
            ChaserMirage,
            VoiceInTheDark
        }

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

        public HallucinationName hName;
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

        public void Initialize(HallucinationName hName, HallucinationType hType, HallucinationFlags hFlags, HallucinationClass hClass, float distanceToDisappear, float chaseSpeed, float delayTime)
        {
            this.hName = hName;
            this.hType = hType;
            this.hFlags = hFlags;
            this.hClass = hClass;
            this.hDistanceToDisappear = distanceToDisappear;
            this.hChaseSpeed = chaseSpeed;
            this.hDelayTime = delayTime;

            target = FindPlayer();
            cameraTarget = FindPlayer();
        }

        public void SetSpawnPoint(Vector3 spawnPoint)
        {
            transform.position = spawnPoint;
        }

        public Transform FindPlayer()
        {
            // Code lifted from the Boneworks Modding Toolkit.

            GameObject[] array = GameObject.FindGameObjectsWithTag("Player");

            for (int i = 0; i < array.Length; i++)
            {
                bool isTrigger = array[i].name == "PlayerTrigger";

                if (isTrigger)
                {
                    return array[i].transform;
                }
            }

            return null;
        }

        public AudioSource GetSource()
        {
            if (source == null) { return null; }

            return source;
        }

        private void Awake()
        {
            target = FindPlayer();
        }

        private void OnEnable()
        {
            MelonLoader.MelonCoroutines.Start(CoUpdateHallucination(hName, hType, hClass, hFlags));
        }

        private void OnDisable()
        {
            MelonLoader.MelonCoroutines.Stop(CoUpdateHallucination(hName, hType, hClass, hFlags));
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

        private IEnumerator CoUpdateHallucination(HallucinationName hName, HallucinationType hType, HallucinationClass hClass, HallucinationFlags hFlags)
        {
            if (hType.HasFlag(HallucinationType.Auditory) && GetComponent<AudioSource>() != null)
            {
                source = GetComponent<AudioSource>();
                source.dopplerLevel = 0f;
                source.spatialBlend = 0.95f;
                source.maxVolume = 15f;
                source.volume = 15f;

                if (hClass == HallucinationClass.DarkVoice)
                {
                    transform.position = target.forward * -0.05f;

                    if (Paranoia.instance.darkVoices.Count > 0)
					{
                        source.clip = Paranoia.instance.darkVoices[Random.Range(0, Paranoia.instance.darkVoices.Count)];
                    }
                    
                    source.loop = false;

                    source.Play();

                    yield return new WaitForSeconds(source.clip.length);

                    gameObject.SetActive(false);
                }
                else if (hClass == HallucinationClass.Chaser)
                {
                    if (Paranoia.instance.chaserAmbience.Count > 0)
					{
                        source.clip = Paranoia.instance.chaserAmbience[Random.Range(0, Paranoia.instance.chaserAmbience.Count)];
                        Log.Msg($"Set up {source.clip}");
                    }

                    transform.position = ParanoiaGameManager.instance.playerCircle.CalculatePlayerCircle(Random.Range(0, 360f), 300f);
                    
                    source.loop = true;

                    source.Play();
                }
                else if (hClass == HallucinationClass.Watcher)
                {
                    if (Paranoia.instance.watcherAmbience.Count > 0)
					{
                        source.clip = Paranoia.instance.watcherAmbience[Random.Range(0, Paranoia.instance.watcherAmbience.Count)];
                    }
                    
                    source.loop = true;

                    source.Play();
                }
            }

            if (hClass == HallucinationClass.Chaser)
            {
                if(hName == HallucinationName.StaringMan)
                {
                    transform.position = ParanoiaGameManager.instance.staringManSpawns[Random.Range(0, ParanoiaGameManager.instance.staringManSpawns.Length)];
                }

                if (hDelayTime > 0)
                {
                    yield return new WaitForSeconds(hDelayTime);
                }

                while (Vector3.Distance(target.position, transform.position) >= hDistanceToDisappear)
                {
                    transform.position += transform.forward * hChaseSpeed * Time.deltaTime;
                    yield return null;
                }

            }

            if (hClass == HallucinationClass.Watcher)
            {
                transform.position = ParanoiaGameManager.instance.playerCircle.CalculatePlayerCircle(Random.Range(0f, 360f));

                if (hName == HallucinationName.CeilingMan)
                {
                    transform.position = ParanoiaGameManager.instance.ceilingManSpawns[Random.Range(0, ParanoiaGameManager.instance.ceilingManSpawns.Length)];
                }
            }

            yield return null;
        }
    }

}