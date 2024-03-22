using UnityEngine.AI;

namespace NEP.Paranoia.Scripts.Entities;

/// <summary>
/// Follows the player.
/// </summary>
public class Follower : MonoBehaviour
{
    public float movementSpeed;
    public bool shootable;
    private Transform _player;
    private NavMeshAgent _agent;
        
    private void Start()
    {
        ModConsole.Msg("Follower spawned", LoggingMode.Debug);
        _player = Player.playerHead;
        _agent = GetComponent<NavMeshAgent>();
        _agent.speed = movementSpeed;
    }
        
    private void FixedUpdate()
    {
        _agent.SetDestination(_player.position);
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