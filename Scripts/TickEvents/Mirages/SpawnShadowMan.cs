using NEP.Paranoia.ParanoiaUtilities;

namespace NEP.Paranoia.TickEvents.Mirages
{
    public class SpawnShadowMan : ParanoiaEvent
    {
        public override void Start()
        {
            Utilities.GetMirage("ent_ShadowMan").gameObject.SetActive(true);
        }

        public override void Stop()
        {

        }
    }
}
