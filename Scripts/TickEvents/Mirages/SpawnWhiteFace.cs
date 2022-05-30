using NEP.Paranoia.ParanoiaUtilities;

namespace NEP.Paranoia.TickEvents.Events
{
    public class SpawnWhiteFace : ParanoiaEvent
    {
        public override void Start()
        {
            Utilities.GetMirage("WhiteFace").gameObject.SetActive(true);
        }

        public override void Stop()
        {

        }
    }
}
