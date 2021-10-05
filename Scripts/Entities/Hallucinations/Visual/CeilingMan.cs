using NEP.Paranoia.Managers;

using UnityEngine;

namespace NEP.Paranoia.Entities
{
    public class CeilingMan : BaseHallucination
    {
        public CeilingMan(System.IntPtr ptr) : base(ptr) { }

        protected override void Awake()
        {
            m_spawnPoints = ParanoiaGameManager.instance.ceilingManSpawns;

            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.clip = Paranoia.instance.watcherAmbience[0];
            source.loop = true;
            source.spatialBlend = 0.75f;

            base.Awake();

            ReadValuesFromJSON(System.IO.File.ReadAllText(baseJsonPath + "CeilingMan.json"));
        }
    }
}
