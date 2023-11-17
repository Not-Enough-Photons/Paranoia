namespace Paranoia.Events;

/// <summary>
/// Moves all NPCs to a given location.
/// </summary>
public static class MoveAIToSpecificLocation
{
    public static void Activate(Transform location)
    {
        Utilities.FindAIBrains(out var navs);
        
        if(navs == null) { return; }

        foreach (var nav in navs)
        {
            Utilities.MoveAIToPoint(nav, location.position);
        }
    }
}