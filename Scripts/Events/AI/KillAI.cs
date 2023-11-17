namespace Paranoia.Events;

/// <summary>
/// Kills all NPCs.
/// </summary>
public static class KillAI
{
    public static void Activate()
    {
        var brains = Utilities.FindAIBrains();

        if(brains == null) { return; }

        foreach(var brain in brains)
        {
            ModStats.IncrementEntry("FordsKilled");
            var puppetMaster = brain.puppetMaster;
            var health = brain.behaviour.health;

            health.Kill();
            puppetMaster.Kill();
        }
    }
}