using NEP.Paranoia.Managers;

namespace NEP.Paranoia.TickEvents.Events
{
    public class ChangeInsanity : ParanoiaEvent
    {
        public override void Start()
        {
            ParanoiaGameManager.instance.AddInsanity(0.025f);
        }
    }
}
