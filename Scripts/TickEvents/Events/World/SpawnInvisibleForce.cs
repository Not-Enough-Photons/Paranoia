using NEP.Paranoia.Managers;
using NEP.Paranoia.ParanoiaUtilities;

namespace NEP.Paranoia.TickEvents.Events
{
    public class SpawnInvisibleForce : ParanoiaEvent
    {
        public override void Start()
        {
            Utilities.GetMirage("InvisibleForce").gameObject.SetActive(true);
        }
    }
}
