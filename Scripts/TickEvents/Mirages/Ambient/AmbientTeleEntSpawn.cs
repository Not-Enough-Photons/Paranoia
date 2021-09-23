using NEP.Paranoia.Managers;

namespace NEP.Paranoia.TickEvents.Mirages
{
    public class AmbientTeleEntSpawn : ParanoiaEvent
    {
        public override void Start()
        {
            if (ParanoiaGameManager.instance.paralysisMode) { return; }

            ParanoiaGameManager.hTeleportingEntity.gameObject.SetActive(true);
        }
    }
}
