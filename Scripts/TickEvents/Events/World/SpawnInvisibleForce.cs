using NEP.Paranoia.Managers;

namespace NEP.Paranoia.TickEvents.Events
{
    public class SpawnInvisibleForce : ParanoiaEvent
    {
        public override void Start()
        {
            ParanoiaGameManager.instance.invisibleForce.gameObject.SetActive(true);
        }
    }
}
