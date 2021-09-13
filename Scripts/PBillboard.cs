﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NotEnoughPhotons.Paranoia
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