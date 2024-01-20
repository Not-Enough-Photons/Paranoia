namespace NEP.Paranoia.Scripts.Events;

/// <summary>
/// Moves all NPCs to a given location.
/// </summary>
public class MoveAIToSpecificLocation : Event
{
    public override void Invoke()
    {
        var manager = ParanoiaManager.Instance;
        var location = manager.npcMoveLocations[Random.Range(0, manager.npcMoveLocations.Length)];
        Utilities.FindAIBrains(out var navs);
        
        if(navs == null) { return; }

        foreach (var nav in navs)
        {
            Utilities.MoveAIToPoint(nav, location.position);
        }
    }
    
    public override bool CanInvoke()
    {
        return true;
    }
}