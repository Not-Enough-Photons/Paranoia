using SLZ.Props.Weapons;
using UnityEngine;

namespace Paranoia.Events
{
    /// <summary>
    /// Fires a random gun in the scene.
    /// </summary>
    public static class FireGun
    {
        public static void Activate()
        {
            var guns = Object.FindObjectsOfType<Gun>();
            guns?[Random.Range(0, guns.Length)].Fire();
            ModStats.IncrementEntry("GunsForceFired");
        }
    }
}