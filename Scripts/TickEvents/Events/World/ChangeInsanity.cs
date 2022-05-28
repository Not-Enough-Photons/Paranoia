using NEP.Paranoia.Managers;

namespace NEP.Paranoia.TickEvents.Events
{
    public class ChangeInsanity : ParanoiaEvent
    {
        public override void Start()
        {
            MelonLoader.MelonLogger.Msg("ChangeInsanity");
            GameManager.insanity += GameManager.insanityRate;
        }
    }
}
