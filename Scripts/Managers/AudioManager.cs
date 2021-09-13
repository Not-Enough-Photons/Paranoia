using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace NEP.Paranoia.Managers
{
	public class AudioManager : MonoBehaviour
	{
		public AudioManager(System.IntPtr ptr) : base(ptr) { }

		public ObjectPool pool;

		public List<AudioClip> ambientGeneric;
		public List<AudioClip> ambientScreaming;
		public List<AudioClip> ambientChaser;
		public List<AudioClip> ambientWatcher;
		public List<AudioClip> ambientDarkVoices;
		public List<AudioClip> radioTunes;

		public AudioClip startingTune;

		public System.Action<AudioSource, AudioClip> OnAudioPlay;

		private void Awake()
		{
			pool = GetComponent<ObjectPool>();
		}

		private void OnEnable()
		{
			OnAudioPlay += HideSource;
		}

		private void OnDisable()
		{
			OnAudioPlay -= HideSource;
		}

		public void PlayOneShot(AudioClip clip)
		{
			if (clip == null) { return; }

			if (pool.GetFirstObjectActive(false) != null)
			{
				AudioSource pooledSrc = pool.GetFirstObjectActive(false).GetComponent<AudioSource>();

				pooledSrc.clip = clip;

				if(pooledSrc.spatialBlend != 0)
				{
					pooledSrc.spatialBlend = 0;
				}

				pooledSrc.gameObject.SetActive(true);
				OnAudioPlay(pooledSrc, clip);
			}
			else
			{
				pool.GrowPool(1);
				PlayOneShot(clip);
			}
		}

		[UnhollowerBaseLib.Attributes.HideFromIl2Cpp]
		public void PlayOneShotAtPoint(Vector3 position, AudioClip clip)
		{
			if (clip == null) { return; }

			if (pool.GetFirstObjectActive(false) != null)
			{
				AudioSource pooledSrc = pool.GetFirstObjectActive(false).GetComponent<AudioSource>();

				pooledSrc.clip = clip;
				pooledSrc.transform.position = position;

				pooledSrc.gameObject.SetActive(true);
				OnAudioPlay(pooledSrc, clip);
			}
			else
			{
				pool.GrowPool(1);
				PlayOneShot(clip);
			}
		}

		[UnhollowerBaseLib.Attributes.HideFromIl2Cpp]
		public void PlayOneShotAtPoint(Vector3 position, AudioClip clip, bool use3DSound, bool looping)
		{
			if (clip == null) { return; }

			if (pool.GetFirstObjectActive(false) != null)
			{
				AudioSource pooledSrc = pool.GetFirstObjectActive(false).GetComponent<AudioSource>();

				pooledSrc.clip = clip;

				pooledSrc.maxVolume = 10f;
				pooledSrc.volume = 5f;

				if(pooledSrc.spatialBlend != 0)
				{
					pooledSrc.spatialBlend = 0;
				}

				if (position == null)
				{
					pooledSrc.transform.position = Vector3.zero;
				}
				else
				{
					pooledSrc.transform.position = position;
				}

				if (use3DSound == true)
				{
					pooledSrc.spatialBlend = 1f;
				}
				else
				{
					pooledSrc.spatialBlend = 0f;
				}

				if (looping == true)
				{
					pooledSrc.loop = true;
				}
				else
				{
					OnAudioPlay(pooledSrc, clip);
				}

				pooledSrc.gameObject.SetActive(true);
			}
			else
			{
				pool.GrowPool(1);
				PlayOneShotAtPoint(position, clip, use3DSound, looping);
			}
		}

		[UnhollowerBaseLib.Attributes.HideFromIl2Cpp]
		public void PlayOneShotAtPoint(Transform parent, AudioClip clip, bool use3DSound, bool looping)
		{
			if (clip == null) { return; }

			if (pool.GetFirstObjectActive(false) != null)
			{
				AudioSource pooledSrc = pool.GetFirstObjectActive(false).GetComponent<AudioSource>();

				pooledSrc.clip = clip;

				pooledSrc.maxVolume = 10f;
				pooledSrc.volume = 5f;

				if (parent == null)
				{
					pooledSrc.transform.position = Vector3.zero;
				}
				else
				{
					pooledSrc.transform.SetParent(null);
					pooledSrc.transform.SetParent(parent);
					pooledSrc.transform.position = Vector3.zero;
				}

				if (use3DSound == true)
				{
					pooledSrc.spatialBlend = 1f;
				}
				else
				{
					pooledSrc.spatialBlend = 0f;
				}

				if (looping == true)
				{
					pooledSrc.loop = true;
				}
				else
				{
					OnAudioPlay(pooledSrc, clip);
				}

				pooledSrc.gameObject.SetActive(true);
			}
			else
			{
				pool.GrowPool(1);
				PlayOneShotAtPoint(parent, clip, use3DSound, looping);
			}
		}

		public void HideSource(AudioSource src, AudioClip srcClip)
		{
			MelonLoader.MelonCoroutines.Start(CoHideSource(src, srcClip));
		}

		[UnhollowerBaseLib.Attributes.HideFromIl2Cpp]
		private System.Collections.IEnumerator CoHideSource(AudioSource src, AudioClip clip)
		{
			float timer = 0f;

			while (timer < clip.length)
			{
				timer += Time.deltaTime;
				yield return null;
			}

			if (timer >= clip.length && src.isActiveAndEnabled)
			{
				src.gameObject.SetActive(false);
			}

			yield return null;
		}
	}
}