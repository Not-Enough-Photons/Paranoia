using NEP.Paranoia.Managers;

namespace NEP.Paranoia.TickEvents.Events
{
    public class ChangeInsanity : ParanoiaEvent
    {
        public override void Start()
        {
            ParanoiaGameManager.instance.insanity += 0.025f;
        }
    }
}
