using UnityEngine;

using NEP.Paranoia.Managers;

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
            ParanoiaGameManager manager = Paranoia.instance.gameManager;

            int rng = manager.rng;

            bool isRareNumber = rng >= 20 && rng <= 45 || rng >= 50 && rng <= 75;

            useScreams = isRareNumber;

            clips = useScreams ? Paranoia.instance.screamAmbience.ToArray() : Paranoia.instance.genericAmbience.ToArray();

            base.OnEnable();
        }
    }
}
