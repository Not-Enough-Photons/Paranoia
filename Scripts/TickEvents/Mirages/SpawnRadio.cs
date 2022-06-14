
using NEP.Paranoia.ParanoiaUtilities;

namespace NEP.Paranoia.TickEvents.Events
{
    public class SpawnRadio : ParanoiaEvent
    {
        public override void Start()
        {
            Utilities.GetMirage("Radio").gameObject.SetActive(true);
        }

        public override void Stop()
        {

        }
    }
}
