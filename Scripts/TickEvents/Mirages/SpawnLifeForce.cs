
using NEP.Paranoia.ParanoiaUtilities;

namespace NEP.Paranoia.TickEvents.Events
{
    public class SpawnLifeForce : ParanoiaEvent
    {
        public override void Start()
        {
            Utilities.GetMirage("LifeForce").gameObject.SetActive(true);
        }

        public override void Stop()
        {

        }
    }
}
