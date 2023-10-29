using FieldInjector;
using Paranoia.Entities;
using Paranoia.Helpers;
using Paranoia.Managers;

namespace Paranoia
{
    public static class FieldInjection
    {
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
            SerialisationHandler.Inject<WeepingAngel>();
            // Helpers
            SerialisationHandler.Inject<SeasonalEntity>();
            SerialisationHandler.Inject<FreezePlayer>();
            // Managers
            SerialisationHandler.Inject<ParanoiaManager>();
        }
    }
}