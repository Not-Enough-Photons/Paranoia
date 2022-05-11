using NEP.Paranoia.ParanoiaUtilities;

namespace NEP.Paranoia.TickEvents.Events
{
    public class SpawnShadowChaser : ParanoiaEvent
    {
        public override void Start()
        {
            Utilities.GetMirage("ShadowManChaser").gameObject.SetActive(true);
        }

        public override void Stop()
        {

        }
    }
}
