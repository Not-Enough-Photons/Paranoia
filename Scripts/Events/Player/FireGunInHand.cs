namespace NEP.Paranoia.Events;

/// <summary>
/// Actually fires the gun in the player's hand.
/// </summary>
public class FireGunInHand : Event
{
    public override void Invoke()
    {
        if (Player.GetGunInHand(Player.rightHand) == null)
        {
            return;
        }

        var gun = Player.GetGunInHand(Player.rightHand);

        gun.Fire();
        ModStats.IncrementEntry("GunsForceFired");
    }
}