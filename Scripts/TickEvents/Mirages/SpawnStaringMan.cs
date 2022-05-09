using NEP.Paranoia.ParanoiaUtilities;

namespace NEP.Paranoia.TickEvents.Mirages
{
    public class SpawnStaringMan : ParanoiaEvent
    {
        public override void Start()
        {
            Utilities.GetMirage("ent_StaringMan").gameObject.SetActive(true);
        }

        public override void Stop()
        {

        }
    }
}
