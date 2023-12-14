namespace NEP.Paranoia.Managers;

public class ParanoiaManager : MonoBehaviour
{
    public static ParanoiaManager Instance { get; private set; }
    private readonly List<Event> _events = new();
    
    #region Main Settings
    
    public ManagerType managerType;
    public float eventTimerMin = 30f;
    public float eventTimerMax = 60f;
    public Light[] lights;
    public Transform[] npcMoveLocations;
    public AudioClip[] grabSounds;
    public float entityTimerMin = 60f;
    public float entityTimerMax = 80f;
    public SpawnableCrateReference[] entities;
    public Transform[] airSpawns;
    public Transform[] groundSpawns;
    public Transform[] audioSpawns;
    public Transform mirageSpawn;
    public float doorTimerMin = 480f;
    public float doorTimerMax = 600f;
    public GameObject door;
    public Transform[] doorSpawnLocations;
    private bool _enabled;
    private bool _doorSpawned;
    
    #endregion
    
    #region Museum Settings
    
    public MeshRenderer signMesh;
    public Texture2D signTexture;
    public Texture2D signWarningTexture;
    public AudioSource warningSound;
    public ZoneMusic zoneMusic;
    public GameObject globalVolume;
    public Transform doorSpawnLocation;
    public float phase1Timer = 120f;
    public float phase2Timer = 15f;
    public float phase3Timer = 600f;
    
    #endregion
    
    #region Baseline Settings
    
    public GameObject thefog;
    private int _eventsCaused;
    private bool _musicDisabled;
    private int _entitiesSpawned;
    
    #endregion
    
    private void Awake()
    {
        Instance = this;
        
        #region AI
        
        _events.Add(new Crabtroll());
        _events.Add(new DragNpcToCeiling());
        _events.Add(new DragRandomNpc());
        _events.Add(new KillAI());
        _events.Add(new LaughAtPlayer());
        _events.Add(new MoveAIToPlayer());
        _events.Add(new MoveAIToRadio());
        _events.Add(new MoveAIToSpecificLocation());
        
        #endregion
        
        #region Player
        
        _events.Add(new FakeFireGun());
        _events.Add(new FireGunInHand());
        _events.Add(new GrabPlayer());
        
        #endregion
        
        #region World
        
        _events.Add(new FireGun());
        _events.Add(new FlickerFlashlights());
        _events.Add(new FlingRandomObject());
        _events.Add(new LightFlicker());
        
        #endregion world
        
    }

    public void Enable()
    {
        switch (managerType)
        {
            case ManagerType.Paranoia:
                if (_enabled) return;
                MelonCoroutines.Start(EventTick());
                MelonCoroutines.Start(EntityTick());
                MelonCoroutines.Start(DoorTick());
                _enabled = true;
                break;
            case ManagerType.Baseline:
                if (_enabled) return;
                MelonCoroutines.Start(EventTick());
                MelonCoroutines.Start(BaselineEntityTick());
                MelonCoroutines.Start(DoorTick());
                _enabled = true;
                break;
            case ManagerType.Museum:
                if (_enabled) return;
                MelonCoroutines.Start(EventTick());
                MelonCoroutines.Start(MuseumTick());
                _enabled = true;
                break;
            default:
                ModConsole.Error("Manager type not set!");
                break;
        }
    }
    
    public void Disable()
    {
        switch (managerType)
        {
            case ManagerType.Paranoia:
                if (!_enabled) return;
                MelonCoroutines.Stop(EventTick());
                MelonCoroutines.Stop(EntityTick());
                MelonCoroutines.Stop(DoorTick());
                _enabled = false;
                break;
            case ManagerType.Baseline:
                if (!_enabled) return;
                MelonCoroutines.Stop(EventTick());
                MelonCoroutines.Stop(BaselineEntityTick());
                MelonCoroutines.Stop(DoorTick());
                _enabled = false;
                break;
            case ManagerType.Museum:
                if (!_enabled) return;
                MelonCoroutines.Stop(EventTick());
                MelonCoroutines.Stop(MuseumTick());
                _enabled = false;
                break;
            default:
                ModConsole.Error("Manager type not set!");
                break;
        }
    }
    
    private IEnumerator EntityTick()
    {
        ModConsole.Msg("Entity tick started", LoggingMode.Debug);
        while (_enabled)
        {
            ModConsole.Msg("Entity tick begin", LoggingMode.Debug);
            var time = Random.Range(entityTimerMin, entityTimerMax);
            yield return new WaitForSeconds(time);
            ModConsole.Msg("Entity tick spawn phase", LoggingMode.Debug);
            var entity = Utilities.GetRandomEntity(entities);
            ModConsole.Msg($"Chosen entity: {entity.Crate.name}", LoggingMode.Debug);
            var crateTag = entity.Crate.Tags;
            switch (crateTag.Contains("Air") ? "Air" : crateTag.Contains("Ground") ? "Ground" : crateTag.Contains("Special") ? "Special" : crateTag.Contains("Audio") ? "Audio" : "None")
            {
                case "Air":
                {
                    ModConsole.Msg("Entity had Air tag", LoggingMode.Debug);
                    var location = airSpawns[Random.Range(0, airSpawns.Length)];
                    HelperMethods.SpawnCrate(entity, location.position, Quaternion.identity, Vector3.one, false, _ => { });
                    break;
                }
                case "Ground":
                {
                    ModConsole.Msg("Entity had Ground tag", LoggingMode.Debug);
                    var location = groundSpawns[Random.Range(0, groundSpawns.Length)];
                    HelperMethods.SpawnCrate(entity, location.position, Quaternion.identity, Vector3.one, false, _ => { });
                    break;
                }
                case "Special":
                {
                    ModConsole.Msg("Entity had Special tag", LoggingMode.Debug);
                    HelperMethods.SpawnCrate(entity, mirageSpawn.position, Quaternion.identity, Vector3.one,  false, _ => { });
                    break;
                }
                case "Audio":
                {
                    ModConsole.Msg("Entity had Audio tag", LoggingMode.Debug);
                    var location = audioSpawns[Random.Range(0, audioSpawns.Length)];
                    HelperMethods.SpawnCrate(entity, location.position, Quaternion.identity, Vector3.one, false, _ => { });
                    break;
                }
                case "None":
                {
                    ModConsole.Error($"You forgot to tag Entity {entity.Crate.name}! Spawning at a ground location. Your fault if it's wrong.");
                    var location = groundSpawns[Random.Range(0, groundSpawns.Length)];
                    HelperMethods.SpawnCrate(entity, location.position, Quaternion.identity, Vector3.one, false, _ => { });
                    break;
                }
                default:
                {
                    MelonLogger.Error("Something broke. Couldn't read the entity type.");
                    break;
                } 
            }
        }
    }
    
    private IEnumerator EventTick()
    {
        ModConsole.Msg("Event tick started", LoggingMode.Debug);
        while (_enabled)
        {
            ModConsole.Msg("Event tick begin", LoggingMode.Debug);
            var time = Random.Range(eventTimerMin, eventTimerMax);
            yield return new WaitForSeconds(time);
            ModConsole.Msg("Event tick event phase", LoggingMode.Debug);
            var rand = Random.Range(0, _events.Count);
            var chosenEvent = _events[rand];
            chosenEvent.Invoke();
        }
    }
    
    private IEnumerator DoorTick()
    {
        ModConsole.Msg("Door tick started", LoggingMode.Debug);
        var time = Random.Range(doorTimerMin, doorTimerMax);
        yield return new WaitForSeconds(time);
        ModConsole.Msg("Door tick door phase", LoggingMode.Debug);
        var location = doorSpawnLocations[Random.Range(0, doorSpawnLocations.Length)];
        Instantiate(door, location.position, location.rotation);
        _doorSpawned = true;
    }
    
    private void OnDestroy()
    {
        Instance = null;
    }
    
    #region Other Managers
    
    private IEnumerator MuseumTick()
    {
        yield return new WaitForSeconds(phase1Timer);
        zoneMusic.StopMusic(3f);
        MuseumEvents.ChangeSign(signMesh, signTexture);
        globalVolume.SetActive(true);
        yield return new WaitForSeconds(phase2Timer);
        zoneMusic.PlayMusic(3f);
        MuseumEvents.HideSign(signMesh);
        globalVolume.SetActive(false);
        yield return new WaitForSeconds(phase3Timer);
        zoneMusic.StopMusic(3f);
        globalVolume.SetActive(true);
        warningSound.Play();
        MuseumEvents.UnhideSign(signMesh);
        MuseumEvents.ChangeSign(signMesh, signWarningTexture);
        Instantiate(door, doorSpawnLocation.position, doorSpawnLocation.rotation);
    }
    
    private IEnumerator BaselineEntityTick()
    {
        ModConsole.Msg("Entity tick started", LoggingMode.Debug);
        while (_enabled)
        {
            ModConsole.Msg("Entity tick begin", LoggingMode.Debug);
            var time = Random.Range(entityTimerMin, entityTimerMax);
            yield return new WaitForSeconds(time);
            ModConsole.Msg("Entity tick spawn phase", LoggingMode.Debug);
            if (_entitiesSpawned >= 3) _entitiesSpawned++;
            if (_entitiesSpawned == 3)
            {
                thefog.gameObject.SetActive(true);
                zoneMusic.StopMusic(1f);
            }
            var entity = Utilities.GetRandomEntity(entities);
            ModConsole.Msg($"Chosen entity: {entity.Crate.name}", LoggingMode.Debug);
            var crateTag = entity.Crate.Tags;
            switch (crateTag.Contains("Air") ? "Air" : crateTag.Contains("Ground") ? "Ground" : crateTag.Contains("Special") ? "Special" : crateTag.Contains("Audio") ? "Audio" : "None")
            {
                case "Air":
                {
                    ModConsole.Msg("Entity had Air tag", LoggingMode.Debug);
                    var location = airSpawns[Random.Range(0, airSpawns.Length)];
                    HelperMethods.SpawnCrate(entity, location.position, Quaternion.identity, Vector3.one, false, _ => { });
                    break;
                }
                case "Ground":
                {
                    ModConsole.Msg("Entity had Ground tag", LoggingMode.Debug);
                    var location = groundSpawns[Random.Range(0, groundSpawns.Length)];
                    HelperMethods.SpawnCrate(entity, location.position, Quaternion.identity, Vector3.one, false, _ => { });
                    break;
                }
                case "Special":
                {
                    ModConsole.Msg("Entity had Special tag", LoggingMode.Debug);
                    HelperMethods.SpawnCrate(entity, mirageSpawn.position, Quaternion.identity, Vector3.one,  false, _ => { });
                    break;
                }
                case "Audio":
                {
                    ModConsole.Msg("Entity had Audio tag", LoggingMode.Debug);
                    var location = audioSpawns[Random.Range(0, audioSpawns.Length)];
                    HelperMethods.SpawnCrate(entity, location.position, Quaternion.identity, Vector3.one, false, _ => { });
                    break;
                }
                case "None":
                {
                    ModConsole.Error($"You forgot to tag Entity {entity.Crate.name}! Spawning at a ground location. Your fault if it's wrong.");
                    var location = groundSpawns[Random.Range(0, groundSpawns.Length)];
                    HelperMethods.SpawnCrate(entity, location.position, Quaternion.identity, Vector3.one, false, _ => { });
                    break;
                }
                default:
                {
                    MelonLogger.Error("Something broke. Couldn't read the entity type.");
                    break;
                } 
            }
        }
    }
    
    #endregion
}