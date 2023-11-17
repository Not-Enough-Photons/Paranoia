namespace Paranoia.Managers;

public class MuseumManager : MonoBehaviour
{
    public MeshRenderer signMesh;
    public Texture2D signTexture;
    public Texture2D signWarningTexture;
    public AudioSource warningSound;
    public ZoneMusic zoneMusic;
    public GameObject globalVolume;
    public GameObject door;
    public Transform doorSpawnLocation;
    public float phase1Timer = 120f;
    public float phase2Timer = 15f;
    public float phase3Timer = 600f;
    public Transform[] radioSpawns;
    public float eventTimerMin = 30f;
    public float eventTimerMax = 60f;
    public Transform[] npcMoveLocations;
    public AudioClip[] grabSounds;
    private bool _enabled;
        
    /// <summary>
    /// Enables all tick coroutines.
    /// </summary>
    public void Enable()
    {
        if (_enabled) return;
        _enabled = true;
        MelonCoroutines.Start(EventTick());
        MelonCoroutines.Start(MuseumTick());
    }
    /// <summary>
    /// Disables all tick coroutines.
    /// <br/>This is generally not needed, as the manager is gone when the level is unloaded, but it's just there for further control.
    /// </summary>
    public void Disable()
    {
        if (!_enabled) return;
        _enabled = false;
        MelonCoroutines.Stop(EventTick());
        MelonCoroutines.Stop(MuseumTick());
    }
    /// <summary>
    /// Handles the sign, fog, and door.
    /// </summary>
    private IEnumerator MuseumTick()
    {
        yield return new WaitForSeconds(phase1Timer);
        zoneMusic.StopMusic(3f);
        Events.MuseumEvents.ChangeSign(signMesh, signTexture);
        globalVolume.SetActive(true);
        yield return new WaitForSeconds(phase2Timer);
        zoneMusic.PlayMusic(3f);
        Events.MuseumEvents.HideSign(signMesh);
        globalVolume.SetActive(false);
        yield return new WaitForSeconds(phase3Timer);
        zoneMusic.StopMusic(3f);
        globalVolume.SetActive(true);
        warningSound.Play();
        Events.MuseumEvents.UnhideSign(signMesh);
        Events.MuseumEvents.ChangeSign(signMesh, signWarningTexture);
        Instantiate(door, doorSpawnLocation.position, doorSpawnLocation.rotation);
    }
        
    /// <summary>
    ///  Event Tick runs every X seconds, where X is generated from a random range between serialized fields eventTimerMin and eventTimerMax.
    ///  <br/>Once that time is up, a random event is chosen from the switch statement below.
    /// </summary>
    private IEnumerator EventTick()
    {
        ModConsole.Msg("Event tick started", LoggingMode.Debug);
        while (_enabled)
        {
            ModConsole.Msg("Event tick begin", LoggingMode.Debug);
            var time = Random.Range(eventTimerMin, eventTimerMax);
            yield return new WaitForSeconds(time);
            ModConsole.Msg("Event tick event phase", LoggingMode.Debug);
            // When adding new events, make sure to add them to the switch statement below. Increment the random range by 1, and add a new case.
            var rand = Random.Range(1, 14);
            switch (rand)
            {
                case 1:
                    ModConsole.Msg("Chosen event: DragRandomNpc", LoggingMode.Debug);
                    Events.DragRandomNpc.Activate(grabSounds);
                    break;
                case 2:
                    ModConsole.Msg("Chosen event: DragNpcToCeiling", LoggingMode.Debug);
                    Events.DragNpcToCeiling.Activate(grabSounds);
                    break;
                case 3:
                    ModConsole.Msg("Chosen event: KillAI", LoggingMode.Debug);
                    Events.KillAI.Activate();
                    break;
                case 4:
                    ModConsole.Msg("Chosen event: LaughAtPlayer", LoggingMode.Debug);
                    Events.LaughAtPlayer.Activate();
                    break;
                case 5:
                    ModConsole.Msg("Chosen event: MoveAIToPlayer", LoggingMode.Debug);
                    Events.MoveAIToPlayer.Activate();
                    break;
                case 6:
                    ModConsole.Msg("Chosen event: MoveAIToRadio", LoggingMode.Debug);
                    var location = radioSpawns[Random.Range(0, radioSpawns.Length)];
                    Events.MoveAIToRadio.Activate(location);
                    break;
                case 7:
                    ModConsole.Msg("Chosen event: FireGunInHand", LoggingMode.Debug);
                    Events.FireGunInHand.Activate();
                    break;
                case 8:
                    ModConsole.Msg("Chosen event: FireGun", LoggingMode.Debug);
                    Events.FireGun.Activate();
                    break;
                case 9:
                    ModConsole.Msg("Chosen Event: FlickerFlashlights", LoggingMode.Debug);
                    Events.FlickerFlashlights.Activate();
                    break;
                case 10:
                    ModConsole.Msg("Chosen event: FlingRandomObject", LoggingMode.Debug);
                    Events.FlingRandomObject.Activate();
                    break;
                case 11:
                    ModConsole.Msg("Chosen event: GrabPlayer", LoggingMode.Debug);
                    Events.GrabPlayer.Activate(grabSounds);
                    break;
                case 12:
                    ModConsole.Msg("Chosen event: Crabtroll", LoggingMode.Debug);
                    Events.Crabtroll.Activate();
                    break;
                case 13:
                    ModConsole.Msg("Chosen event: MoveAIToSpecificLocation", LoggingMode.Debug);
                    var location3 = npcMoveLocations[Random.Range(0, npcMoveLocations.Length)];
                    Events.MoveAIToSpecificLocation.Activate(location3);
                    break;
                case 14:
                    ModConsole.Msg("Chosen event: FakeFireGun", LoggingMode.Debug);
                    Events.FakeFireGun.Activate();
                    break;
                default:
                    ModConsole.Error("Something broke. Random number couldn't be read. Falling back to DragRandomNpc.");
                    Events.DragRandomNpc.Activate(grabSounds);
                    break;
            }
        }
    }

    public MuseumManager(IntPtr ptr) : base(ptr) { }
}