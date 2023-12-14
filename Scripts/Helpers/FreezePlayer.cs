namespace NEP.Paranoia.Helpers;

/// <summary>
/// A way to freeze the player through UltEvents.
/// </summary>
public class FreezePlayer : MonoBehaviour
{
    public void Freeze()
    {
        Utilities.FreezePlayer(true);
    }

    public void Unfreeze()
    {
        Utilities.FreezePlayer(false);
    }

    public FreezePlayer(IntPtr ptr) : base(ptr) { }
}