namespace NEP.Paranoia.Scripts.Internal;

internal static class FieldInjection
{
    /// <summary>
    /// Handles the injection of fields.
    /// </summary>
    public static void Inject()
    {
        // Entities
        SerialisationHandler.Inject<AudioEvent>();
        SerialisationHandler.Inject<Crasher>();
        SerialisationHandler.Inject<Follower>();
        SerialisationHandler.Inject<Chaser>();
        SerialisationHandler.Inject<Watcher>();
        SerialisationHandler.Inject<Mirage>();
        SerialisationHandler.Inject<Paralyzer>();
        SerialisationHandler.Inject<Radio>();
        SerialisationHandler.Inject<Stalker>();
        SerialisationHandler.Inject<Crying>();
        SerialisationHandler.Inject<WeepingAngel>();
        // Helpers
        SerialisationHandler.Inject<SeasonalEntity>();
        SerialisationHandler.Inject<FreezePlayer>();
        // Internal
        SerialisationHandler.Inject<WorldBlocker>();
        // Managers
        SerialisationHandler.Inject<ParanoiaManager>();
    }
}