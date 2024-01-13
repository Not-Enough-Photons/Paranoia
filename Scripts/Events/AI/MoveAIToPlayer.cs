namespace NEP.Paranoia.Scripts.Events;

/// <summary>
/// Moves all NPCs to the player's position.
/// </summary>
public class MoveAIToPlayer : Event
{
    public override void Invoke()
    {
        Utilities.FindAIBrains(out var navs);
        var player = Player.playerHead;

        if(player == null) { return; }
        if(navs == null) { return; }

        foreach (var nav in navs)
        {
            Utilities.MoveAIToPoint(nav, player.position);
        }
    }
    
    public override bool CanInvoke()
    {
        return true;
    }
}