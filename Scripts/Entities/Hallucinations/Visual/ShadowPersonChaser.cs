namespace NEP.Paranoia.Entities
{
    public class ShadowPersonChaser : BaseHallucination
    {
        public ShadowPersonChaser(System.IntPtr ptr) : base(ptr) { }

        protected override void Awake()
        {
            base.Awake();

            ReadValuesFromJSON(System.IO.File.ReadAllText(baseJsonPath + "ShadowManChaser.json"));
        }
    }
}
