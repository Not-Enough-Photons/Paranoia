namespace NEP.Paranoia.Events;

/// <summary>
/// Spawns a Crablet behind the player. Lol
/// </summary>
public class Crabtroll : Event
{
    public override void Invoke()
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
    
    public override bool CanInvoke()
    {
        // JUST HOW DEEP DO YOU BELIEVE?
        return true;
    }
}