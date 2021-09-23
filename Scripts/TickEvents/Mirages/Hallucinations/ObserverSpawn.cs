using NEP.Paranoia.Managers;

namespace NEP.Paranoia.TickEvents.Mirages
{
    public class ObserverSpawn : ParanoiaEvent
    {
        public override void Start()
        {
            ParanoiaGameManager.hObserver.gameObject.SetActive(true);
        }
    }
}
