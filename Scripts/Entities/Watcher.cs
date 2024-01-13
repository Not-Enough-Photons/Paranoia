namespace NEP.Paranoia.Scripts.Entities;

/// <summary>
/// Watches the player from wherever they spawned, never moving.
/// </summary>
public class Watcher : MonoBehaviour
{
    public float timeToDespawn;
    private Transform _player;
    private Transform This => transform;

    private void Start()
    {
        ModConsole.Msg("Watcher spawned", LoggingMode.Debug);
        _player = Player.playerHead;
        MelonCoroutines.Start(DespawnSelf(timeToDespawn));
    }
        
    private void FixedUpdate()
    {
        This.LookAt(_player);
    }
        
    private IEnumerator DespawnSelf(float delay)
    {
        yield return new WaitForSeconds(delay);
        ModConsole.Msg("Watcher despawned", LoggingMode.Debug);
        Destroy(gameObject);
    }

    public Watcher(IntPtr ptr) : base(ptr) { }
}