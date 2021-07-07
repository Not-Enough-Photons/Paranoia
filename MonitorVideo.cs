using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

namespace NotEnoughPhotons.paranoia
{
    public class MonitorVideo : MonoBehaviour
    {
        public VideoClip[] clips;

        private VideoPlayer player;

        private void Awake()
        {
            player = GetComponent<VideoPlayer>();
        }

        private void OnEnable()
        {
            player.clip = clips[ParanoiaGameManager.instance.insanity];
            MelonLoader.MelonCoroutines.Start(CoHideMonitor());
        }

        private IEnumerator CoHideMonitor()
        {
            yield return new WaitForSeconds((float)player.clip.length + 0.5f);

            gameObject.SetActive(false);
        }
    }

}