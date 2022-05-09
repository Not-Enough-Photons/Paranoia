using NEP.Paranoia.ParanoiaUtilities;

namespace NEP.Paranoia.TickEvents.Mirages
{
    public class SpawnChaser : ParanoiaEvent
    {
        public override void Start()
        {
            Utilities.GetMirage("ent_a_Chaser").gameObject.SetActive(true);
        }

        public override void Stop()
        {

        }
    }
}
