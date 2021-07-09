using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NotEnoughPhotons.paranoia
{
	public class ShadowPersonChaser : MonoBehaviour
	{
		public ShadowPersonChaser(System.IntPtr ptr) : base(ptr) { }

		public Transform target;
		private AudioManager manager;
		private float chaseSpeed = 10f;
		private float time;

		private void Awake()
		{
			manager = FindObjectOfType<AudioManager>();
		}

		private void OnDisable()
		{
			time = 0f;
		}

		private void Update()
		{
			time += Time.deltaTime;

			if (time >= 5f)
			{
				if (Vector3.Distance(target.position, transform.position) > 1f)
				{
					transform.position += transform.forward * 75f * Time.deltaTime;
				}
				else
				{
					gameObject.SetActive(false);
				}
			}
		}
	}

}