#if MELONLOADER
using BoneLib;
#endif
using Paranoia.Helpers;
using PuppetMasta;
using SLZ.Marrow.Warehouse;
using UnityEngine;

namespace Paranoia.Events
{
    public static class Crabtroll
    {
#if MELONLOADER
        public static void Activate()
        {
            var playerPos = Player.playerHead;
            var spawnPos = playerPos.position - playerPos.forward * 10f;
            var crabCrate = new SpawnableCrateReference("c1534c5a-4583-48b5-ac3f-eb9543726162");
            Warehouse.Spawn(crabCrate, spawnPos, Quaternion.identity, false, go =>
            {
                var crab = go.GetComponentInChildren<BehaviourCrablet>();
                if (crab == null) return;
                crab.Activate();
            });
        }
#else
        public static void Activate()
        {

        }
#endif
    }
}