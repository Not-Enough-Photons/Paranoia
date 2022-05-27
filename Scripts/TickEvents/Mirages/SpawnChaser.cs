using NEP.Paranoia.ParanoiaUtilities;

namespace NEP.Paranoia.TickEvents.Events
{
    public class SpawnChaser : ParanoiaEvent
    {
        public override void Start()
        {
            Utilities.GetMirage("Chaser").gameObject.SetActive(true);
        }

        public override void Stop()
        {

        }
    }
}
