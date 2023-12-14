namespace NEP.Paranoia.Entities;

/// <summary>
/// Moves when the player isn't looking at it.
/// </summary>
public class WeepingAngel : MonoBehaviour
{
    public float lookThreshold = 0.5f;
    public float movementSpeed;
    public bool shootable;
    private Transform _player;
    private Transform This => transform;

    private void Start()
    {
        ModConsole.Msg("Weeping Angel spawned", LoggingMode.Debug);
        _player = Player.playerHead;
    }

    private void FixedUpdate()
    {
        This.LookAt(_player);
        var dotProduct = Vector3.Dot(_player.forward, This.forward);
        if (dotProduct >= lookThreshold)
        {
            This.localPosition += This.forward * (movementSpeed * Time.deltaTime);
        }
    }
        
    private void OnTriggerEnter(Collider other)
    {
        if (shootable) return;
        if (other.GetComponentInParent<RigManager>() != null)
        {
            ModConsole.Msg("Weeping Angel despawned", LoggingMode.Debug);
            Destroy(gameObject);
        }
    }

    public WeepingAngel(IntPtr ptr) : base(ptr) { }
}