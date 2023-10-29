using BoneLib;

namespace Paranoia.Events
{
    /// <summary>
    /// "Fires" the gun in the player's hand, it'll make the noise but it didn't actually fire.
    /// </summary>
    public static class FakeFireGun
    {
        public static void Activate()
        {
            var gun = Player.GetGunInHand(Player.rightHand);
            gun.gunSFX.GunShot();
        }
    }
}