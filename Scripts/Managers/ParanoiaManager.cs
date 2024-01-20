namespace NEP.Paranoia.Scripts.Managers;

public class ParanoiaManager : MonoBehaviour
{
    public static ParanoiaManager Instance;
    
    public ManagerType managerType;
    
    #region Event Settings
    
    public float eventTimerMin = 30f;
    public float eventTimerMax = 60f;
    public Light[] lights;
    public Transform[] npcMoveLocations;
    public AudioClip[] grabSounds;
    
    #endregion
    
    #region Entity Settings
    
    public float entityTimerMin = 60f;
    public float entityTimerMax = 80f;
    public bool allowExtensionEntities = true;
    public List<SpawnableCrateReference> entities;
    public Transform[] airSpawns;
    public Transform[] groundSpawns;
    public Transform[] audioSpawns;
    public Transform mirageSpawn;
    
    #endregion
    
    #region Door Settings
    
    public float doorTimerMin = 480f;
    public float doorTimerMax = 600f;
    public GameObject door;
    public Transform[] doorSpawnLocations;
    
    #endregion
    
    #region Extra Settings
    
    public MeshRenderer signMesh;
    public Texture2D signTexture;
    public Texture2D signWarningTexture;
    public AudioSource warningSound;
    public ZoneMusic zoneMusic;
    public GameObject thefog;
    public Transform doorSpawnLocation;
    public float phase1Timer = 120f;
    public float phase2Timer = 15f;
    public float phase3Timer = 600f;
    
    #endregion
    
    private bool _enabled;
    private bool _doorSpawned;
    
    private int _eventsCaused;
    private bool _musicDisabled;
    private int _entitiesSpawned;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        if (!allowExtensionEntities || EntityLoader.EntityCrates.Count == 0) return;
        foreach (var entity in EntityLoader.EntityCrates)
        {
            entities.Add(entity);
            ModConsole.Msg($"Added extension entity {entity.Crate.name}", LoggingMode.Debug);
        }
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
            // Set a random time between the event timer settings
            var time = Random.Range(eventTimerMin, eventTimerMax);
            // Wait for that time
            yield return new WaitForSeconds(time);
            ModConsole.Msg("Event tick event phase", LoggingMode.Debug);
            // Choose a random event
            var rand = Random.Range(0, Event.Events.Count);
            var chosenEvent = Event.Events[rand];
            if (chosenEvent.CanInvoke()) chosenEvent.Invoke();
            else
            {
                // if it can't invoke, try to get one that can.
                while (!chosenEvent.CanInvoke())
                {
                    var randAgain = Random.Range(0, Event.Events.Count);
                    // ensure that it's not just picking the same one again
                    while (randAgain == rand) randAgain = Random.Range(0, Event.Events.Count);
                    chosenEvent = Event.Events[randAgain];
                    chosenEvent.Invoke();
                }
            }
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
    
    #region Other Managers
    
    private IEnumerator MuseumTick()
    {
        yield return new WaitForSeconds(phase1Timer);
        zoneMusic.StopMusic(3f);
        MuseumEvents.ChangeSign(signMesh, signTexture);
        thefog.SetActive(true);
        yield return new WaitForSeconds(phase2Timer);
        zoneMusic.PlayMusic(3f);
        MuseumEvents.HideSign(signMesh);
        thefog.SetActive(false);
        yield return new WaitForSeconds(phase3Timer);
        zoneMusic.StopMusic(3f);
        thefog.SetActive(true);
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
            // once 3 entities have been spawned, start the fog and stop the music
            if (_entitiesSpawned >= 3) _entitiesSpawned++;
            if (_entitiesSpawned == 3)
            {
                thefog.gameObject.SetActive(true);
                zoneMusic.StopMusic(1f);
            }
            // see Utilities.cs's GetRandomEntity method for more info
            var entity = Utilities.GetRandomEntity(entities);
            ModConsole.Msg($"Chosen entity: {entity.Crate.name}", LoggingMode.Debug);
            var crateTag = entity.Crate.Tags;
            // check tag of crate to determine where to spawn it
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

    private void OnDestroy()
    {
        Instance = null;
    }
    
    public ParanoiaManager(IntPtr ptr) : base(ptr) { }
}