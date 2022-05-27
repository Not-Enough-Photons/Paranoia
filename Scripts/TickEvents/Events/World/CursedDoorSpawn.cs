using NEP.Paranoia.Managers;
using NEP.Paranoia.ParanoiaUtilities;

namespace NEP.Paranoia.TickEvents.Events
{
    public class CursedDoorSpawn : ParanoiaEvent
    {
        public override void Start()
        {
            Utilities.GetMirage("CursedDoor").gameObject.SetActive(true);
        }
    }
}
