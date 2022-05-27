using NEP.Paranoia.ParanoiaUtilities;

namespace NEP.Paranoia.TickEvents.Events
{
    public class SpawnShadowMan : ParanoiaEvent
    {
        public override void Start()
        {
            Utilities.GetMirage("ShadowMan").gameObject.SetActive(true);
        }

        public override void Stop()
        {

        }
    }
}
