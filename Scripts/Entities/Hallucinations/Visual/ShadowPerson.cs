using NEP.Paranoia.Managers;

namespace NEP.Paranoia.Entities
{
    public class ShadowPerson : BaseHallucination
    {
        public ShadowPerson(System.IntPtr ptr) : base(ptr) { }

        protected override void OnEnable()
        {
            int rng = ParanoiaGameManager.instance.rng;

            string json = rng >= 25 || rng <= 30
                ? System.IO.File.ReadAllText(baseJsonPath + "ShadowManChaser.json")
                : System.IO.File.ReadAllText(baseJsonPath + "ShadowMan.json");

            ReadValuesFromJSON(json);

            base.OnEnable();
        }
    }
}
