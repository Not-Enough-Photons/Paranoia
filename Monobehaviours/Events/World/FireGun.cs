using SLZ.Props.Weapons;
using UnityEngine;

namespace Paranoia.Events
{
    public static class FireGun
    {
#if MELONLOADER
        public static void Activate()
        {
            var guns = Object.FindObjectsOfType<Gun>();
            guns?[Random.Range(0, guns.Length)].Fire();
        }
#else
        public static void Activate()
        {
            
        }
#endif
    }
}