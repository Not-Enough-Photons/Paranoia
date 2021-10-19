namespace NEP.Paranoia.Entities
{
    public class SjasFace : AudioHallucination
    {
        public SjasFace(System.IntPtr ptr) : base(ptr) { }

        protected override void Awake()
        {
            source = GetComponent<UnityEngine.AudioSource>();

            source.spatialBlend = 0.85f;
            source.dopplerLevel = 0f;

            base.Awake();

            ReadValuesFromJSON(System.IO.File.ReadAllText(audioJsonPath + "SjasFace.json"));
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            source.Play();
        }
    }
}
