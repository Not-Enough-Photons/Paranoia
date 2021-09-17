using NEP.Paranoia.Managers;

namespace NEP.Paranoia.Entities
{
    public class StaringMan : BaseHallucination
    {
        public StaringMan(System.IntPtr ptr) : base(ptr) { }

        protected override void Awake()
        {
            m_spawnPoints = ParanoiaGameManager.instance.staringManSpawns;

            base.Awake();

            ReadValuesFromJSON(System.IO.File.ReadAllText(baseJsonPath + "StaringMan.json"));
        }
    }
}
