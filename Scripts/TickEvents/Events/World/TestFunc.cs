namespace NEP.Paranoia.TickEvents.Events
{
    public class TestFunc : ParanoiaEvent
    {
        public override void Start()
        {
            MelonLoader.MelonLogger.Msg("Test");
        }
    }
}
