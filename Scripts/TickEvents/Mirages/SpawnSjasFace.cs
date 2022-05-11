using NEP.Paranoia.ParanoiaUtilities;

namespace NEP.Paranoia.TickEvents.Events
{
    public class SpawnSjasFace : ParanoiaEvent
    {
        public override void Start()
        {
            Utilities.GetMirage("SjasFace").gameObject.SetActive(true);
        }

        public override void Stop()
        {

        }
    }
}
