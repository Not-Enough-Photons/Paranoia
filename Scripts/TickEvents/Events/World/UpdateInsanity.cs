using NEP.Paranoia.Managers;

namespace NEP.Paranoia.TickEvents.Events.World
{
    public class UpdateInsanity : ParanoiaEvent
    {
        public override void Start()
        {
            if (ParanoiaGameManager.insanityMode)
            {
                ParanoiaGameManager.instance.AddInsanity(0.025f);
            }
        }
    }
}
