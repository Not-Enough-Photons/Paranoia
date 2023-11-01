using BoneLib;
using Paranoia.Helpers;
using PuppetMasta;
using SLZ.Marrow.Warehouse;
using UnityEngine;

namespace Paranoia.Events
{
    /// <summary>
    /// Spawns a Crablet behind the player. Lol
    /// </summary>
    public static class Crabtroll
    {
        public static void Activate()
        {
            var playerPos = Player.playerHead;
            var spawnPos = playerPos.position - playerPos.forward * 10f;
            var crabCrate = new SpawnableCrateReference(CommonBarcodes.NPCs.Crablet);
            HelperMethods.SpawnCrate(crabCrate, spawnPos, Quaternion.identity, Vector3.one, false, go =>
            {
                var crab = go.GetComponentInChildren<BehaviourCrablet>();
                if (crab == null) return;
                crab.Activate();
            });
        }
    }
}