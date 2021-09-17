using UnityEngine;

namespace NEP.Paranoia.Entities
{
    public class Paralyzer : AudioHallucination
    {
        public Paralyzer(System.IntPtr ptr) : base(ptr) { }

        protected override void Awake()
        {
            gameObject.AddComponent<AudioSource>();

            base.Awake();

            clips = Paranoia.instance.paralyzerAmbience.ToArray();

            ReadValuesFromJSON(System.IO.File.ReadAllText(audioJsonPath + "Paralyzer.json"));
        }
    }
}
