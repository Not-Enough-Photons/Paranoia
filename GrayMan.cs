using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NotEnoughPhotons.paranoia
{
	public class GrayMan : MonoBehaviour
	{
		public GrayMan(System.IntPtr ptr) : base(ptr) { }

		public Transform target;

		public System.Action OnReachedTarget;

		private void OnEnable()
		{
			try
			{
				OnReachedTarget += new System.Action(() => gameObject.SetActive(false));

				Quaternion lookRotation = Quaternion.LookRotation(target.position - transform.forward);
				transform.rotation = Quaternion.Euler(transform.rotation.x, lookRotation.eulerAngles.y, transform.rotation.z);
			}
			catch
			{

			}
		}

		private void OnDisable()
		{
			OnReachedTarget -= new System.Action(() => gameObject.SetActive(false));
		}

		private void Update()
		{
			if (target == null) { return; }

			if (Vector3.Distance(target.position, transform.position) > 1f)
			{
				transform.position += transform.forward * 0.25f * Time.deltaTime;
			}
			else
			{
				if (gameObject.activeInHierarchy)
				{
					if (OnReachedTarget != null)
					{
						OnReachedTarget();
					}
				}
			}
		}
	}
}