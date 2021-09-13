using NEP.Paranoia.Managers;

namespace NEP.Paranoia.TickEvents.Mirages
{
    public class StaringManSpawn : ParanoiaEvent
    {
        public override void Start()
        {
            ParanoiaGameManager.instance.hStaringMan.gameObject.SetActive(true);
        }
    }
}
