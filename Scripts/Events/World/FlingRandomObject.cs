namespace NEP.Paranoia.Events;

/// <summary>
/// Flings a random object.
/// </summary>
public class FlingRandomObject : Event
{
    public override void Invoke()
    {
        Rigidbody[] rbs = Object.FindObjectsOfType<Rigidbody>();
        var player = Player.playerHead;

        var randomRb = rbs[Random.Range(0, rbs.Length)];

        randomRb.AddForce((player.position - randomRb.transform.position) * Random.Range(100f, 200f), ForceMode.Impulse);
        ModStats.IncrementEntry("ObjectsFlung");
    }
}