using UnityEngine;

namespace NotEnoughPhotons.paranoia
{
	public class ChaserMirage : MonoBehaviour
	{
		public ChaserMirage(System.IntPtr ptr) : base(ptr) { }

		public AudioManager manager;
		public AudioSource src;
		public Transform target;

		private void Awake()
		{
			manager = FindObjectOfType<AudioManager>();

			GameObject[] arr = GameObject.FindGameObjectsWithTag("Player");
			for (int i = 0; i < arr.Length; i++)
			{
				bool isTrigger = arr[i].name == "PlayerTrigger";
				if (isTrigger)
				{
					target = arr[i].transform;
				}
			}

			src = gameObject.AddComponent<AudioSource>();
		}

		private void OnEnable()
		{
			src.dopplerLevel = 0f;
			src.spatialBlend = 0.95f;
			src.maxVolume = 15f;
			src.volume = 15f;
			src.clip = manager.ambientChaser[Random.Range(0, manager.ambientChaser.Count)];
			src.loop = true;
			src.Play();
		}

		private void Update()
		{
			transform.LookAt(target);

			if (Vector3.Distance(target.position, transform.position) > 0.25f)
			{
				transform.position += transform.forward * 50f * Time.deltaTime;
			}
			else
			{
				this.gameObject.SetActive(false);
			}
		}
	}
}