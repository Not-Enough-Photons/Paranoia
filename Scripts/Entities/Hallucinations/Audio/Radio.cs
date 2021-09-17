using UnityEngine;

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

            ReadValuesFromJSON(System.IO.File.ReadAllText(audioJsonPath + "Radio.json"));
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            source.spatialBlend = 1f;
            clips = Paranoia.instance.radioTunes.ToArray();
            MelonLoader.MelonCoroutines.Start(CoRadioHide(source.clip.length));
        }

        private System.Collections.IEnumerator CoRadioHide(float time)
        {
            yield return new WaitForSeconds(time);

            if (ParanoiaGameManager.instance.radioClone.activeInHierarchy)
            {
                ParanoiaGameManager.instance.radioClone.SetActive(false);
            }

            yield return null;
        }
    }
}
