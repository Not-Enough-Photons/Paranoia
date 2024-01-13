namespace NEP.Paranoia.Scripts.Entities;

/// <summary>
/// Prevents duplicates of this entity from spawning.
/// </summary>
public class Crying : MonoBehaviour
{
    private void Start()
    {
        var othercryers = FindObjectsOfType<CryingMarker>();
        if (othercryers.Length > 0)
        {
            Destroy(gameObject);
        }
        else
        {
            gameObject.AddComponent<CryingMarker>();
        }
    }
    public Crying(IntPtr ptr) : base(ptr) { }
}