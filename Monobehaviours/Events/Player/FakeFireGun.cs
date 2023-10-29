using BoneLib;

namespace Paranoia.Events
{
    public static class FakeFireGun
    {
        public static void Activate()
        {
            var gun = Player.GetGunInHand(Player.rightHand);
            gun.gunSFX.GunShot();
        }
    }
}