using UnityEngine.AI;

namespace NEP.Paranoia.Scripts.Entities;

/// <summary>
/// Chases the player. Very similar to Follower, but plays a random sound.
/// </summary>
public class Chaser : MonoBehaviour
{
    public float movementSpeed;
    public bool shootable;
    public AudioClip[] possibleSounds;
    public AudioSource audioSource;
    private Transform _player;
    private NavMeshAgent _agent;

    private void Start()
    {
        ModConsole.Msg("Chaser spawned", LoggingMode.Debug);
        audioSource.clip = possibleSounds[Random.Range(0, possibleSounds.Length)];
        audioSource.Play();
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
            ModConsole.Msg("Chaser despawned", LoggingMode.Debug);
            Destroy(gameObject);
        }
    }

    public Chaser(IntPtr ptr) : base(ptr) { }
}