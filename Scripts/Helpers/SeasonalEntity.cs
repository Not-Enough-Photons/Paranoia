namespace Paranoia.Helpers;

/// <summary>
/// Changes the entity's material based on the holiday.
/// <br/>Currently changes for Christmas and April Fools
/// </summary>
public class SeasonalEntity : MonoBehaviour
{
    public MeshRenderer renderer;
    public Material christmasMaterial;
    public Material aprilfoolsMaterial;
    public UltEvent onChristmas;
    public UltEvent onAprilFools;
        
    private void Start()
    {
        var isChristmas = Utilities.CheckDate(12, 25);
        var isAprilFools = Utilities.CheckDate(4, 1);
        if (isChristmas)
        {
            renderer.material = christmasMaterial;
            onChristmas.Invoke();
        }
        else if (isAprilFools)
        {
            renderer.material = aprilfoolsMaterial;
            onAprilFools.Invoke();
        }
    }

    public SeasonalEntity(IntPtr ptr) : base(ptr) { }
}