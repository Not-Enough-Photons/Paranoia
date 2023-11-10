using BoneLib;

namespace Paranoia.Events
{
    /// <summary>
    /// Actually fires the gun in the player's hand.
    /// </summary>
    public static class FireGunInHand
    {
        public static void Activate()
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
}