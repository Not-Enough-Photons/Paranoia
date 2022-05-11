
using NEP.Paranoia.ParanoiaUtilities;

namespace NEP.Paranoia.TickEvents.Events
{
    public class SpawnAmbience : ParanoiaEvent
    {
        public override void Start()
        {
            Utilities.GetMirage("Ambience").gameObject.SetActive(true);
        }

        public override void Stop()
        {

        }
    }
}
