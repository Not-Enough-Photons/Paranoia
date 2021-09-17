using UnityEngine;

namespace NEP.Paranoia.Entities
{
    public class Ambience : AudioHallucination
    {
        public Ambience(System.IntPtr ptr) : base(ptr) { }

        public bool useScreams = false;

        protected override void Awake()
        {
            ReadValuesFromJSON(System.IO.File.ReadAllText(audioJsonPath + "Ambience.json"));

            base.Awake();
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            clips = useScreams ? Paranoia.instance.screamAmbience.ToArray() : Paranoia.instance.genericAmbience.ToArray();
        }
    }
}
