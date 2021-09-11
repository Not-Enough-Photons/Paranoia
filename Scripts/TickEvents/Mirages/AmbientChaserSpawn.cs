using NotEnoughPhotons.Paranoia.Managers;

namespace NotEnoughPhotons.Paranoia.TickEvents.Mirages
{
    public class AmbientChaserSpawn : ParanoiaEvent
    {
        public override void Start()
        {
            ParanoiaGameManager.instance.hChaser.gameObject.SetActive(true);
        }
    }
}
