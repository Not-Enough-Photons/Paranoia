using UnityEngine.AI;

namespace NEP.Paranoia.Scripts.Entities;

/// <summary>
/// Rapidly darts around the map.
/// </summary>
public class Mirage : MonoBehaviour
{
    public float movementSpeed;
    public float despawnTime;
    public bool shootable;
    private NavMeshAgent _agent;

    private void Start()
    {
        ModConsole.Msg("Mirage spawned", LoggingMode.Debug);
        _agent = GetComponent<NavMeshAgent>();
        _agent.speed = movementSpeed;
        Move();
        if (shootable) return;
        MelonCoroutines.Start(DespawnSelf(despawnTime));
    }

    private void Move()
    {
        _agent.SetDestination(Utilities.GetRandomPointFromNavmesh());
    }

    private void FixedUpdate()
    {
        if (!_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance) Move();
    }
    
    private IEnumerator DespawnSelf(float delay)
    {
        yield return new WaitForSeconds(delay);
        ModConsole.Msg("Mirage despawned", LoggingMode.Debug);
        Destroy(gameObject);
    }

    public Mirage(IntPtr ptr) : base(ptr) { }
}