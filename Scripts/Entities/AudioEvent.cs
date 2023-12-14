namespace NEP.Paranoia.Entities;

/// <summary>
/// Plays a random sound.
/// </summary>
public class AudioEvent : MonoBehaviour
{
    public AudioClip[] audioClips;
    public AudioSource audioSource;
        
    private void Start()
    {
        ModConsole.Msg("Audio event spawned", LoggingMode.Debug);
        audioSource.clip = audioClips[Random.Range(0, audioClips.Length)];
        audioSource.Play();
        MelonCoroutines.Start(DespawnSelf(audioSource.clip.length));
    }

    private IEnumerator DespawnSelf(float delay)
    {
        yield return new WaitForSeconds(delay);
        ModConsole.Msg("Audio event despawned", LoggingMode.Debug);
        Destroy(gameObject);
    }
        
    public AudioEvent(IntPtr ptr) : base(ptr) { }
}