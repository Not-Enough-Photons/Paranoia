namespace NEP.Paranoia.Managers
{
    public class TickTemplate
    {
        public int id { get; set; }
        public string name { get; set; }
        public float tick { get; set; }
        public float[] minMaxTimes { get; set; }
        public int[] minMaxRNG { get; set; }
        public float insanity { get; set; }
        public string runOnMaps { get; set; }
        public string pEvent { get; set; }
    }
}
