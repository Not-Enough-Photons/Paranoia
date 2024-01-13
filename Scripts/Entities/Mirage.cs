namespace NEP.Paranoia.Scripts.Entities;

/// <summary>
/// Rapidly darts around the map.
/// </summary>
public class Mirage : MonoBehaviour
{
    public float movementSpeed;
    public float despawnTime;
    public bool shootable;
    public float minX = -195f;
    public float minZ = -195f;
    public float maxX = 195f;
    public float maxZ = 195f;
    private Transform _player;
    private Transform This => transform;
    private Vector3 _targetPosition;

    private void Start()
    {
        ModConsole.Msg("Mirage spawned", LoggingMode.Debug);
        _player = Player.playerHead;
        SetTargetPosition();
        if (shootable) return;
        MelonCoroutines.Start(DespawnSelf(despawnTime));
    }

    private void Update()
    {
        This.position = Vector3.Lerp(transform.position, _targetPosition, movementSpeed * Time.deltaTime);
        This.LookAt(_player);
        if (Vector3.Distance(transform.position, _targetPosition) < 0.1f)
        {
            SetTargetPosition();
        }
    }

    private void SetTargetPosition()
    {
        var randX = UnityEngine.Random.Range(minX, maxX);
        var randZ = UnityEngine.Random.Range(minZ, maxZ);
        _targetPosition = new Vector3(randX, transform.position.y, randZ);
    }
        
    private IEnumerator DespawnSelf(float delay)
    {
        yield return new WaitForSeconds(delay);
        ModConsole.Msg("Mirage despawned", LoggingMode.Debug);
        Destroy(gameObject);
    }

    public Mirage(IntPtr ptr) : base(ptr) { }
}