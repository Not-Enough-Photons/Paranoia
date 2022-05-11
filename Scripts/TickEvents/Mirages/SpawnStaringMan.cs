using NEP.Paranoia.ParanoiaUtilities;

namespace NEP.Paranoia.TickEvents.Events
{
    public class SpawnStaringMan : ParanoiaEvent
    {
        public override void Start()
        {
            Utilities.GetMirage("StaringMan").gameObject.SetActive(true);
        }

        public override void Stop()
        {

        }
    }
}
