using Paranoia.Helpers;

namespace Paranoia.Events
{
    public static class KillAI
    {
        public static void Activate()
        {
            var brains = Utilities.FindAIBrains();

            if(brains == null) { return; }

            foreach(var brain in brains)
            {
                var puppetMaster = brain.puppetMaster;
                var health = brain.behaviour.health;

                health.Kill();
                puppetMaster.Kill();
            }
        }
    }
}