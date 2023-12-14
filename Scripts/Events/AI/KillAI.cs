using NEP.Paranoia.Helpers;

namespace NEP.Paranoia.Events;

/// <summary>
/// Kills all NPCs.
/// </summary>
public  class KillAI : Event
{
    public override void Invoke()
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