
using NEP.Paranoia.ParanoiaUtilities;

namespace NEP.Paranoia.TickEvents.Mirages
{
    public class SpawnAmbience : ParanoiaEvent
    {
        public override void Start()
        {
            Utilities.GetMirage("ent_a_Ambience").gameObject.SetActive(true);
        }

        public override void Stop()
        {

        }
    }
}
