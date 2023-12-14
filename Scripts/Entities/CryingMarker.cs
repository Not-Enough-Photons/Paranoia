namespace NEP.Paranoia.Entities;

/// <summary>
/// Just exists to mark a crying entity after it's spawned, assuming there isn't another crying entity.
/// </summary>
public class CryingMarker : MonoBehaviour
{
    public CryingMarker(IntPtr ptr) : base(ptr) { }
}