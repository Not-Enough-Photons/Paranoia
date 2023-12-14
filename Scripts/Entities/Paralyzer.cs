using NEP.Paranoia.Helpers;

namespace NEP.Paranoia.Entities;

/// <summary>
/// Freezes the player in place and moves towards them.
/// </summary>
public class Paralyzer : MonoBehaviour
{
    public AudioSource paralysisSound;
    private Transform _playerHead;
    private Transform _player;
    private Transform This => transform;

    private void Start()
    {
        ModStats.IncrementEntry("PlayersParalyzed");
        ModConsole.Msg("Paralyzer spawned", LoggingMode.Debug);
        _player = Player.rigManager.artOutputRig.transform;
        _playerHead = Player.playerHead;
        This.position = _player.position + _player.forward * 25f + Vector3.up * 1.5f;
        Utilities.FreezePlayer(true);
        paralysisSound.Play();
        This.LookAt(_playerHead);
        MelonCoroutines.Start(MoveCloser());
    }

    private IEnumerator MoveCloser()
    {
        for (var i = 0; i < 3; i++)
        {
            This.position = Vector3.MoveTowards(This.position, _playerHead.position, 5f);
            paralysisSound.Play();
            yield return new WaitForSeconds(5f);
            if (i == 2)
            {
                This.position = Vector3.MoveTowards(This.position, _playerHead.position, 5f);
                paralysisSound.Play();
                MelonCoroutines.Start(DespawnSelf());
            }
        }
    }
        
    private IEnumerator DespawnSelf()
    {
        yield return new WaitForSeconds(5f);
        ModConsole.Msg("Paralyzer despawned", LoggingMode.Debug);
        Utilities.FreezePlayer(false);
        Destroy(gameObject);
    }

    public Paralyzer(IntPtr ptr) : base(ptr) { }
}