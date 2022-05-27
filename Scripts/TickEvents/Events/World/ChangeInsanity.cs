using NEP.Paranoia.Managers;

namespace NEP.Paranoia.TickEvents.Events
{
    public class ChangeInsanity : ParanoiaEvent
    {
        public override void Start()
        {
            GameManager.insanity += GameManager.insanityRate;
        }
    }
}
