#if MELONLOADER
using BoneLib;
#endif

namespace Paranoia.Events
{
    public static class FireGunInHand
    {
#if MELONLOADER
        public static void Activate()
        {
            if (Player.GetGunInHand(Player.rightHand) == null)
            {
                return;
            }

            var gun = Player.GetGunInHand(Player.rightHand);

            gun.Fire();
        }
#else
        public static void Activate()
        {

        }
#endif
    }
}