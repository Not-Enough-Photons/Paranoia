namespace NEP.Paranoia.Entities
{
    public class Observer : BaseHallucination
    {
        public Observer(System.IntPtr ptr) : base(ptr) { }

        protected override void Awake()
        {
            base.Awake();

            ReadValuesFromJSON(System.IO.File.ReadAllText(baseJsonPath + "Observer.json"));
        }
    }
}
