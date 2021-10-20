namespace NEP.Paranoia.Entities
{
    public class TestWeepingAngel : BaseHallucination
    {
        public TestWeepingAngel(System.IntPtr ptr) : base(ptr) { }

        protected override void Awake()
        {
            ReadValuesFromJSON(System.IO.File.ReadAllText(baseJsonPath + "TestWeepingAngel.json"));

            base.Awake();
        }
    }
}
