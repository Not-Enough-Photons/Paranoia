using NotEnoughPhotons.Paranoia.Managers;

namespace NotEnoughPhotons.Paranoia.TickEvents.Mirages
{
    public class CeilingManSpawn : ParanoiaEvent
    {
        public override void Start()
        {
            ParanoiaGameManager.instance.hCeilingMan.gameObject.SetActive(true);
        }
    }
}
