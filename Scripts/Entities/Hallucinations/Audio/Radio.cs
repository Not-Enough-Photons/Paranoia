﻿using UnityEngine;

using NEP.Paranoia.Managers;
using NEP.Paranoia.Utilities;

namespace NEP.Paranoia.Entities
{
    public class Radio : AudioHallucination
    {
        public Radio(System.IntPtr ptr) : base(ptr) { }

        protected override void Awake()
        {
            base.Awake();
            clips = Paranoia.instance.radioTunes.ToArray();
            ReadValuesFromJSON(System.IO.File.ReadAllText(audioJsonPath + "Radio.json"));
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            source.spatialBlend = 1f;

            MelonLoader.MelonCoroutines.Start(CoRadioHide(source.clip.length));
        }

        private System.Collections.IEnumerator CoRadioHide(float time)
        {
            yield return new WaitForSeconds(time);

            if (gameObject.activeInHierarchy)
            {
                gameObject.SetActive(false);
            }

            yield return null;
        }
    }
}
