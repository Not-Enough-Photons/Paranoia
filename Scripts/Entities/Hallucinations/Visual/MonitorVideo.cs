using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Video;

namespace NEP.Paranoia.Entities
{
    public class MonitorVideo : MonoBehaviour
    {
        public MonitorVideo(System.IntPtr ptr) : base(ptr) {}

        public List<VideoClip> clips;

        private VideoPlayer player;

        private void Awake()
        {
            if(GetComponent<VideoPlayer>() != null)
            {
                player = GetComponent<VideoPlayer>();
            }
        }

        private void OnEnable()
        {
            if(clips == null) { return; }
            if(player == null) { return; }

            //player.clip = clips[ParanoiaGameManager.instance.insanity];
            MelonLoader.MelonCoroutines.Start(CoHideMonitor());
        }

        private IEnumerator CoHideMonitor()
        {
            yield return new WaitForSeconds((float)player.clip.length + 0.5f);

            gameObject.SetActive(false);
        }
    }

}