using NEP.Paranoia.ParanoiaUtilities;

namespace NEP.Paranoia.TickEvents.Events
{
    public class SpawnUboa : ParanoiaEvent
    {
        public override void Start()
        {
            Utilities.GetMirage("Uboa").gameObject.SetActive(true);
        }

        public override void Stop()
        {

        }
    }
}
