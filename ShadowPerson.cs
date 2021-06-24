using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NotEnoughPhotons.paranoia
{
	public class ShadowPerson : MonoBehaviour
	{
		public ShadowPerson(System.IntPtr ptr) : base(ptr) { }

		public Transform target;

		private void Update()
		{
			if (Vector3.Distance(target.position, transform.position) < 50f) { gameObject.SetActive(false); }
		}
	}

}