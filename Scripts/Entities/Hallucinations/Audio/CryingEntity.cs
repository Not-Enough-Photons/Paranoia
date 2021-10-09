using NEP.Paranoia.Entities;

namespace NEP.Paranoia.Entities
{
    public class CryingEntity : AudioHallucination
    {
        public CryingEntity(System.IntPtr ptr) : base(ptr) { }

        protected override void Awake()
        {
            ReadValuesFromJSON(System.IO.File.ReadAllText(audioJsonPath + "CryingEntity.json"));

            base.Awake();
        }

        protected override void OnEnable()
        {
            clips = Paranoia.instance.cryingAmbience.ToArray();

            base.OnEnable();
        }
    }
}
