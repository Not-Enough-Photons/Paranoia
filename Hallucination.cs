using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        public bool hUsesDelay;

        private AudioSource _source;
        private AudioManager _manager;

        public void Initialize(HallucinationType hType, HallucinationFlags hFlags, HallucinationClass hClass, float distanceToDisappear, float chaseSpeed, float delayTime, bool usesDelay)
        {
            this.hType = hType;
            this.hFlags = hFlags;
            this.hClass = hClass;
            this.hDistanceToDisappear = distanceToDisappear;
            this.hChaseSpeed = chaseSpeed;
            this.hDelayTime = delayTime;
            this.hUsesDelay = usesDelay;
        }

        public void SetSpawnPoint(Vector3 spawnPoint)
        {
            transform.position = spawnPoint;
        }

        public AudioSource GetSource()
        {
            if (_source == null) { return null; }

            return _source;
        }

        private void OnEnable()
        {
            target = FindObjectOfType<Camera>().transform;

            cameraTarget = FindObjectOfType<Camera>().transform;

            SetSpawnPoint(target.GetComponent<PlayerUnitCircle>().CalculatePlayerCircle(Random.Range(0, 360)));

            _manager = FindObjectOfType<AudioManager>();

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
            bool isFar = Vector3.Distance(target.position, transform.position) > hDistanceToDisappear;

            if (hType.HasFlag(HallucinationType.Auditory) && GetComponent<AudioSource>() != null)
            {
                _source = GetComponent<AudioSource>();
                _source.spatialBlend = 1f;

                if (hClass == HallucinationClass.DarkVoice)
                {
                    transform.position = target.forward * -2f;

                    if(_manager.ambientDarkVoices.Count > 0)
					{
                        _source.clip = _manager.ambientDarkVoices[Random.Range(0, _manager.ambientDarkVoices.Count)];
                    }
                    
                    _source.loop = false;

                    _source.Play();

                    yield return new WaitForSeconds(_source.clip.length);
                    gameObject.SetActive(false);
                }
                else if (hClass == HallucinationClass.Chaser)
                {
                    if(_manager.ambientChaser.Count > 0)
					{
                        _source.clip = _manager.ambientChaser[Random.Range(0, _manager.ambientChaser.Count)];
                    }
                    
                    _source.loop = true;

                    _source.Play();
                }
                else if (hClass == HallucinationClass.Watcher)
                {
					if (_manager.ambientWatcher.Count > 0)
					{
                        _source.clip = _manager.ambientWatcher[Random.Range(0, _manager.ambientWatcher.Count)];
                    }
                    
                    _source.loop = true;

                    _source.Play();
                }
            }

            if (hClass == HallucinationClass.Chaser)
            {
                if (hDelayTime > 0)
                {
                    yield return new WaitForSeconds(hDelayTime);
                }

                while (isFar)
                {
                    transform.position += transform.forward * hChaseSpeed * Time.deltaTime;
                    yield return null;
                }
            }

            if (hClass == HallucinationClass.Watcher)
            {
                while (!isFar)
                {
                    gameObject.SetActive(false);
                    yield return null;
                }
            }

            yield return null;
        }
    }

}