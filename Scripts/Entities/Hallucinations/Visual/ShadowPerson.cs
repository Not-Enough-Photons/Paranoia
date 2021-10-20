using NEP.Paranoia.Managers;

namespace NEP.Paranoia.Entities
{
    public class ShadowPerson : BaseHallucination
    {
        public ShadowPerson(System.IntPtr ptr) : base(ptr) { }

        protected override void Awake()
        {
            ReadValuesFromJSON(System.IO.File.ReadAllText(baseJsonPath + "ShadowMan.json"));

            base.Awake();
        }
    }
}
