namespace NEP.Paranoia.Events;

/// <summary>
/// Fires a random gun in the scene.
/// </summary>
public class FireGun : Event
{
    public override void Invoke()
    {
        var guns = Object.FindObjectsOfType<Gun>();
        guns?[Random.Range(0, guns.Length)].Fire();
        ModStats.IncrementEntry("GunsForceFired");
    }
}