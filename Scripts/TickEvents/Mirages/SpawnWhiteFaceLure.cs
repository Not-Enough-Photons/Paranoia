using NEP.Paranoia.ParanoiaUtilities;

namespace NEP.Paranoia.TickEvents.Events
{
    public class SpawnWhiteFaceLure : ParanoiaEvent
    {
        public override void Start()
        {
            Utilities.GetMirage("WhiteFaceLure").gameObject.SetActive(true);
        }
    }
}
