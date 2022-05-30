using NEP.Paranoia.ParanoiaUtilities;

namespace NEP.Paranoia.TickEvents.Events
{
    public class SpawnLure : ParanoiaEvent
    {
        public override void Start()
        {
            Utilities.GetMirage("Lure").gameObject.SetActive(true);
        }
    }
}
