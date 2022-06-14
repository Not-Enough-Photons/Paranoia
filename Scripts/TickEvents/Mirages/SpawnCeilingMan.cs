
using NEP.Paranoia.ParanoiaUtilities;

namespace NEP.Paranoia.TickEvents.Events
{
    public class SpawnCeilingMan : ParanoiaEvent
    {
        public override void Start()
        {
            Utilities.GetMirage("CeilingMan").gameObject.SetActive(true);
        }

        public override void Stop()
        {

        }
    }
}
