namespace NEP.Paranoia.Entities
{
    public class SjasFace : AudioHallucination
    {
        public SjasFace(System.IntPtr ptr) : base(ptr) { }

        protected override void Awake()
        {
            source = GetComponent<UnityEngine.AudioSource>();

            base.Awake();

            ReadValuesFromJSON(System.IO.File.ReadAllText(audioJsonPath + "SjasFace.json"));
        }
    }
}
