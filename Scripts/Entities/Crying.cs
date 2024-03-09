namespace NEP.Paranoia.Scripts.Entities;

/// <summary>
/// Prevents duplicates of this entity from spawning.
/// </summary>
public class Crying : MonoBehaviour
{
    private static readonly List<Crying> Cryers = new();
    
    private void Start()
    {
        if (Cryers.Count > 0)
        {
            Destroy(gameObject);
        }
        else
        {
            Cryers.Add(this);
        }
    }
    
    private void OnDestroy()
    {
        Cryers.Remove(this);
    }
    
    public Crying(IntPtr ptr) : base(ptr) { }
}