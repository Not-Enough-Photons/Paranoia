namespace NEP.Paranoia.Entities;

/// <summary>
/// Follows the player.
/// </summary>
public class Follower : MonoBehaviour
{
    public float movementSpeed;
    public bool shootable;
    private Transform _player;
    private Transform This => transform;
        
    private void Start()
    {
        ModConsole.Msg("Follower spawned", LoggingMode.Debug);
        _player = Player.playerHead;
    }
        
    private void FixedUpdate()
    {
        This.localPosition += This.forward * (movementSpeed * Time.deltaTime);
        This.LookAt(_player);
    }
        
    private void OnTriggerEnter(Collider other)
    {
        if (shootable) return;
        if (other.GetComponentInParent<RigManager>() != null)
        {
            ModConsole.Msg("Follower despawned", LoggingMode.Debug);
            Destroy(gameObject);
        }
    }
        
    public Follower(IntPtr ptr) : base(ptr) { }
}