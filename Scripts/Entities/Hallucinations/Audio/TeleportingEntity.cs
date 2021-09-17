using UnityEngine;

namespace NEP.Paranoia.Entities
{
    public class TeleportingEntity : AudioHallucination
    {
        public TeleportingEntity(System.IntPtr ptr) : base(ptr) { }

        protected override void Awake()
        {
            gameObject.AddComponent<AudioSource>();

            base.Awake();

            clips = Paranoia.instance.teleporterAmbience.ToArray();

            ReadValuesFromJSON(System.IO.File.ReadAllText(audioJsonPath + "TeleportingEntity.json"));
        }
    }
}
