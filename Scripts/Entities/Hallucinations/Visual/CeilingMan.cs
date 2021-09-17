using NEP.Paranoia.Managers;

namespace NEP.Paranoia.Entities
{
    public class CeilingMan : BaseHallucination
    {
        public CeilingMan(System.IntPtr ptr) : base(ptr) { }

        protected override void Awake()
        {
            m_spawnPoints = ParanoiaGameManager.instance.ceilingManSpawns;

            base.Awake();

            ReadValuesFromJSON(System.IO.File.ReadAllText(baseJsonPath + "CeilingMan.json"));
        }
    }
}
