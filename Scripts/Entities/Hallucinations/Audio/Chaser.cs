using UnityEngine;

namespace NEP.Paranoia.Entities
{
    public class Chaser : AudioHallucination
    {
        public Chaser(System.IntPtr ptr) : base(ptr) { }

        protected override void Awake()
        {
            gameObject.AddComponent<AudioSource>();

            base.Awake();

            clips = Paranoia.instance.chaserAmbience.ToArray();

            ReadValuesFromJSON(System.IO.File.ReadAllText(audioJsonPath + "Chaser.json"));
        }
    }
}
