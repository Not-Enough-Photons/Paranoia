using SLZ.Marrow.SceneStreaming;
using UnityEngine.AI;

namespace NEP.Paranoia.Scripts.Entities;

/// <summary>
/// Crashes the game if it gets too close.
/// <br/>Assuming the person who set up the crasher is smart enough to read instructions, it disappears when shot, so you have a chance.
/// </summary>
public class Crasher : MonoBehaviour
{
    public float movementSpeed;
    public AudioClip[] possibleSounds;
    public AudioSource audioSource;
    private Transform _player;
    private NavMeshAgent _agent;
        
    private void Start()
    {
        ModConsole.Msg("Crasher spawned", LoggingMode.Debug);
        audioSource.clip = possibleSounds[Random.Range(0, possibleSounds.Length)];
        audioSource.Play();
        _player = Player.playerHead;
        _agent.speed = movementSpeed;
        _agent.speed = movementSpeed;
    }
        
    private void FixedUpdate()
    {
        _agent.SetDestination(_player.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<RigManager>() != null)
        {
            ModStats.IncrementEntry("PlayersTrolled");
            if (Preferences.AllowCrashing.Value) Utilities.CrashGame();
            else SceneStreamer.Load(CommonBarcodes.Maps.VoidG114);
        }
    }

    public void Despawn()
    {
        ModConsole.Msg("Crasher despawned", LoggingMode.Debug);
        Destroy(gameObject);
    }
        
    public Crasher(IntPtr ptr) : base(ptr) { }
}