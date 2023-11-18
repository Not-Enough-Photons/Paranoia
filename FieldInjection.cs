﻿namespace Paranoia;

public static class FieldInjection
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
        SerialisationHandler.Inject<CryingMarker>();
        SerialisationHandler.Inject<Crying>();
        SerialisationHandler.Inject<WeepingAngel>();
        SerialisationHandler.Inject<Vanisher>();
        // Helpers
        SerialisationHandler.Inject<SeasonalEntity>();
        SerialisationHandler.Inject<FreezePlayer>();
        // Managers
        SerialisationHandler.Inject<ParanoiaManager>();
        SerialisationHandler.Inject<MuseumManager>();
        SerialisationHandler.Inject<BaselineManager>();
    }
}