using NEP.Paranoia.Helpers;

namespace NEP.Paranoia.Events;

/// <summary>
/// Moves all NPCs to a given location.
/// </summary>
public class MoveAIToSpecificLocation : Event
{
    public override void Invoke()
    {
        var location = ParanoiaManager.Instance.npcMoveLocations[Random.Range(0, ParanoiaManager.Instance.npcMoveLocations.Length)];
        Utilities.FindAIBrains(out var navs);
        
        if(navs == null) { return; }

        foreach (var nav in navs)
        {
            Utilities.MoveAIToPoint(nav, location.position);
        }
    }
}