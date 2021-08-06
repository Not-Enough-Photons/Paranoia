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
            VoiceInTheDark,
            Observer
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
            HideWhenSeen = 1,
            HideWhenClose = 2
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

        public float hViewingDeadzone = 0.85f;

        public AudioSource source;
        public AudioManager audioManager;

        private IEnumerator m_iter;

        public void Initialize(HallucinationName hName, HallucinationType hType, HallucinationFlags hFlags, HallucinationClass hClass, float distanceToDisappear, float chaseSpeed, float delayTime)
        {
            this.hName = hName;
            this.hType = hType;
            this.hFlags = hFlags;
            this.hClass = hClass;
            this.hDistanceToDisappear = distanceToDisappear;
            this.hChaseSpeed = chaseSpeed;
            this.hDelayTime = delayTime;
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

        private void Awake()
        {
            if(GetComponent<AudioSource>() != null)
            {
                source = GetComponent<AudioSource>();
            }

            if(FindObjectOfType<AudioManager>() != null)
            {
                audioManager = FindObjectOfType<AudioManager>();
            }

            target = ParanoiaUtilities.FindPlayer();
            cameraTarget = ParanoiaUtilities.FindPlayer();
        }

        private void OnEnable()
        {
            m_iter = MelonLoader.MelonCoroutines.Start(CoUpdateHallucination(hName, hType, hClass, hFlags)) as IEnumerator;
        }

        private void OnDisable()
        {
            MelonLoader.MelonCoroutines.Stop(m_iter);
        }

        private void Update()
        {
            if (hFlags.HasFlag(HallucinationFlags.HideWhenClose))
            {
                if (Vector3.Distance(target.position, transform.position) < hDistanceToDisappear)
                {
                    if (OnReachedTarget != null)
                    {
                        OnReachedTarget();
                    }

                    gameObject.SetActive(false);
                }
            }

            if (hFlags.HasFlag(HallucinationFlags.HideWhenSeen))
            {
                if (cameraTarget != null)
                {
                    if (Vector3.Dot(cameraTarget.forward, transform.forward) <= -hViewingDeadzone)
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

                // Dark Voice
                if (hClass == HallucinationClass.DarkVoice)
                {
                    transform.position = ParanoiaGameManager.instance.playerCircle.CalculatePlayerCircle(Random.Range(0, 360f), 0.75f);

                    if (Paranoia.instance.darkVoices.Count > 0)
					{
                        source.clip = Paranoia.instance.darkVoices[Random.Range(0, Paranoia.instance.darkVoices.Count)];
                    }
                    
                    source.loop = false;

                    source.Play();

                    yield return new WaitForSeconds(source.clip.length);

                    gameObject.SetActive(false);
                }
                // Chaser audio
                else if (hClass == HallucinationClass.Chaser)
                {
                    if (Paranoia.instance.chaserAmbience.Count > 0)
					{
                        source.clip = Paranoia.instance.chaserAmbience[Random.Range(0, Paranoia.instance.chaserAmbience.Count)];
                    }

                    transform.position = ParanoiaGameManager.instance.playerCircle.CalculatePlayerCircle(Random.Range(0, 360f), 250f);
                    
                    source.loop = true;

                    source.Play();
                }
                // Watcher ambient audio
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

            // Chaser
            if (hClass == HallucinationClass.Chaser)
            {
                transform.position = ParanoiaGameManager.instance.playerCircle.CalculatePlayerCircle(Random.Range(0f, 360f), 100f);

                if (hName == HallucinationName.StaringMan)
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

                if (hName == HallucinationName.ShadowMan)
                {
                    transform.position = ParanoiaGameManager.instance.playerCircle.CalculatePlayerCircle(Random.Range(0f, 360f));
                }
                else if (hName == HallucinationName.CeilingMan)
                {
                    transform.position = ParanoiaGameManager.instance.ceilingManSpawns[Random.Range(0, ParanoiaGameManager.instance.ceilingManSpawns.Length)];
                }
                else if (hName == HallucinationName.Observer)
                {
                    Transform player = ParanoiaUtilities.FindPlayer();
                    Vector3 behind = new Vector3(player.position.x, 0f, player.position.z - 10f);
                    transform.position = behind;
                }
            }

            yield return null;
        }
    }
}