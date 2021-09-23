using NEP.Paranoia.Managers;

namespace NEP.Paranoia.TickEvents.Events
{
    public class CursedDoorSpawn : ParanoiaEvent
    {
        public override void Start()
        {
            ParanoiaGameManager.hCursedDoor.gameObject.SetActive(true);
        }
    }
}
