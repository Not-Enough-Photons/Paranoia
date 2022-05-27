using NEP.Paranoia.ParanoiaUtilities;

namespace NEP.Paranoia.TickEvents.Events
{
    public class SpawnDarkVoice : ParanoiaEvent
    {
        public override void Start()
        {
            Utilities.GetMirage("DarkVoice").gameObject.SetActive(true);
        }

        public override void Stop()
        {

        }
    }
}
