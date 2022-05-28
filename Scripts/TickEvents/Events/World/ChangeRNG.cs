using NEP.Paranoia.Managers;

namespace NEP.Paranoia.TickEvents.Events
{
    public class ChangeRNG : ParanoiaEvent
    {
        public override void Start()
        {
            GameManager.rngValue = UnityEngine.Random.Range(1, 100);
        }
    }
}
