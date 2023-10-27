using BoneLib;

namespace Paranoia.Events
{
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
        }
    }
}