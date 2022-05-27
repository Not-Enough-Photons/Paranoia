using NEP.Paranoia.Entities;
using NEP.Paranoia.ParanoiaUtilities;

namespace NEP.Paranoia.TickEvents.Events
{
    public class SpawnParalyzer : ParanoiaEvent
    {
        public override void Start()
        {
            Utilities.GetMirage("Paralyzer").gameObject.SetActive(true);
        }
    }
}
