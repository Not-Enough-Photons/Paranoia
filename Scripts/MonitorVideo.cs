using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

namespace NotEnoughPhotons.paranoia
{
    public class MonitorVideo : MonoBehaviour
    {
        public MonitorVideo(System.IntPtr ptr) : base(ptr) {}

        public List<VideoClip> clips;

        private VideoPlayer player;

        private void Awake()
        {
            player = GetComponent<VideoPlayer>();
        }

        private void OnEnable()
        {
            if (clips != null || player != null)
            {
                player.clip = clips[ParanoiaGameManager.instance.insanity];
                MelonLoader.MelonCoroutines.Start(CoHideMonitor());
            }
        }

        private IEnumerator CoHideMonitor()
        {
            yield return new WaitForSeconds((float)player.clip.length + 0.5f);

            gameObject.SetActive(false);
        }
    }

}