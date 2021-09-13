using NEP.Paranoia.Managers;

namespace NEP.Paranoia.TickEvents.Mirages
{
    public class ObserverSpawn : ParanoiaEvent
    {
        public override void Start()
        {
            ParanoiaGameManager.instance.hObserver.gameObject.SetActive(true);
        }
    }
}
