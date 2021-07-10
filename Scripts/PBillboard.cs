using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NotEnoughPhotons.paranoia
{
	public class PBillboard : MonoBehaviour
	{
		public PBillboard(System.IntPtr ptr) : base(ptr) { }

		public Transform target;

		private void Update()
		{
			transform.LookAt(target);
		}
	}

}