using NEP.Paranoia.ParanoiaUtilities;

namespace NEP.Paranoia.TickEvents.Mirages
{
    public class SpawnShadowChaser : ParanoiaEvent
    {
        public override void Start()
        {
            Utilities.GetMirage("ent_ShadowManChaser").gameObject.SetActive(true);
        }

        public override void Stop()
        {

        }
    }
}
