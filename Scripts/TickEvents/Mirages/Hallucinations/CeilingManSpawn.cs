using NEP.Paranoia.Managers;

namespace NEP.Paranoia.TickEvents.Mirages
{
    public class CeilingManSpawn : ParanoiaEvent
    {
        public override void Start()
        {
            ParanoiaGameManager.hCeilingMan.gameObject.SetActive(true);
        }
    }
}
