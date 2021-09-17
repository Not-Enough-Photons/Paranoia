using UnityEngine;

namespace NEP.Paranoia.Entities
{
    public class DarkVoice : AudioHallucination
    {
        public DarkVoice(System.IntPtr ptr) : base (ptr) { }

        protected override void Awake()
        {
            base.Awake();

            clips = Paranoia.instance.darkVoices.ToArray();

            ReadValuesFromJSON(System.IO.File.ReadAllText(audioJsonPath + "DarkVoice.json"));
        }
    }
}
