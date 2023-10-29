using BoneLib.Nullables;
using MelonLoader;
using System;
using SLZ.Marrow.Data;
using SLZ.Marrow.Pool;
using SLZ.Marrow.Warehouse;
using UnityEngine;

namespace Paranoia.Helpers
{
    /// <summary>
    /// Contains helper methods for spawning crates within Marrow.
    /// </summary>
    public static class Warehouse
    {
        /// <summary>
        /// Spawns a crate at a given position and rotation.
        /// </summary>
        /// <param name="crateRef">The crate to spawn.</param>
        /// <param name="position">The position to spawn the crate at.</param>
        /// <param name="rotation">The rotation to spawn the crate at.</param>
        /// <param name="ignorePolicy">Whether or not to ignore the spawn policy.</param>
        /// <param name="callback">The code to run when the crate is spawned.</param>
        public static void Spawn(SpawnableCrateReference crateRef, Vector3 position, Quaternion rotation, bool ignorePolicy, Action<GameObject> callback)
        {
            ModConsole.Msg($"Spawning {crateRef.Crate._title}", LoggingMode.DEBUG);
            var spawnable = new Spawnable()
            {
                crateRef = crateRef
            };
            AssetSpawner.Register(spawnable);
            Action<GameObject> spawnAction = go =>
            {
                callback(go);
            };
            AssetSpawner.Spawn(spawnable, position, rotation, new BoxedNullable<Vector3>(null), ignorePolicy, new BoxedNullable<int>(null), spawnAction);
        }
    }
}