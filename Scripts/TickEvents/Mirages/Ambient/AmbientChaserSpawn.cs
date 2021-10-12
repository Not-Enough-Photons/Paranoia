using NEP.Paranoia.Managers;

namespace NEP.Paranoia.TickEvents.Mirages
{
    public class AmbientChaserSpawn : ParanoiaEvent
    {
        public override void Start()
        {
            if (Paranoia.instance.gameManager.paralysisMode) { return; }

            ParanoiaGameManager.hChaser.gameObject.SetActive(true);
        }
    }
}
