using NEP.Paranoia.Managers;

namespace NEP.Paranoia.Entities
{
    public class FastStaringMan : BaseHallucination
    {
        public FastStaringMan(System.IntPtr ptr) : base(ptr) { }

        protected override void Awake()
        {
            base.Awake();

            ReadValuesFromJSON(System.IO.File.ReadAllText(baseJsonPath + "StaringManFast.json"));
        }
    }
}
