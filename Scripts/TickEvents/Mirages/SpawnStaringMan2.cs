using NEP.Paranoia.ParanoiaUtilities;

namespace NEP.Paranoia.TickEvents.Events
{
    public class SpawnStaringMan2 : ParanoiaEvent
    {
        public override void Start()
        {
            Utilities.GetMirage("StaringMan2").gameObject.SetActive(true);
        }

        public override void Stop()
        {

        }
    }
}
