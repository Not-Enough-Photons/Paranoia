using BoneLib.Nullables;
using MelonLoader;
using System;
using SLZ.Marrow.Data;
using SLZ.Marrow.Pool;
using SLZ.Marrow.Warehouse;
using UnityEngine;

namespace Paranoia.Helpers
{
    public static class Warehouse
    {
        public static void Spawn(SpawnableCrateReference crateRef, Vector3 position, Quaternion rotation, bool ignorePolicy, Action<GameObject> callback)
        {
            MelonLogger.Msg($"Spawning {crateRef.Crate._title}");
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