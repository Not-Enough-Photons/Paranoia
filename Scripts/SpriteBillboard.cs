using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NotEnoughPhotons.paranoia
{
	public class SpriteBillboard : MonoBehaviour
	{
		public SpriteBillboard(System.IntPtr ptr) : base(ptr) { }

		public Transform target;

		private void Update()
		{
			transform.LookAt(target);
		}
	}

}