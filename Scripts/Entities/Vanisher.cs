namespace Paranoia.Entities;

/// <summary>
/// Moves to the player until the player looks at it.
/// </summary>
public class Vanisher : MonoBehaviour
{
    public float lookThreshold = 0.5f;
    public float movementSpeed;
    public GameObject vanishObject;
    public ParticleSystem vanishParticles;
    // Obama Vanish!
    public AudioSource vanishSound;
    private Transform _player;
    private Transform This => transform;
    private bool _isVanishing;

    private void Start()
    {
        ModConsole.Msg("Vanisher spawned", LoggingMode.Debug);
        _player = Player.playerHead;
    }

    private void FixedUpdate()
    {
        This.LookAt(_player);
        This.localPosition += This.forward * (movementSpeed * Time.deltaTime);
        var dotProduct = Vector3.Dot(_player.forward, This.forward);
        if (dotProduct >= lookThreshold)
        {
            if (_isVanishing) return;
            MelonCoroutines.Start(Vanish());
        }
    }
        
    private void OnTriggerEnter(Collider other)
    {
        if (_isVanishing) return;
        if (other.GetComponentInParent<RigManager>() != null)
        {
            ModConsole.Msg("Vanisher despawned", LoggingMode.Debug);
            Destroy(gameObject);
        }
    }

    private IEnumerator Vanish()
    {
        _isVanishing = true;
        vanishParticles.Play();
        vanishObject.SetActive(false);
        vanishSound.Play();
        var delay = vanishParticles.duration;
        yield return new WaitForSeconds(delay);
        ModConsole.Msg("Vanisher despawned.", LoggingMode.Debug);
        Destroy(gameObject);
    }

    public Vanisher(IntPtr ptr) : base(ptr) { }
}