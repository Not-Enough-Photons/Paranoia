namespace NEP.Paranoia.Managers;

[Serializable]
public class EventSettings
{
    public float eventTimerMin = 30f;
    public float eventTimerMax = 60f;
    public Light[] lights;
    public Transform[] npcMoveLocations;
    public AudioClip[] grabSounds;
}

[Serializable]
public class EntitySettings
{
    public float entityTimerMin = 60f;
    public float entityTimerMax = 80f;
    public SpawnableCrateReference[] entities;
    public Transform[] airSpawns;
    public Transform[] groundSpawns;
    public Transform[] audioSpawns;
    public Transform mirageSpawn;
}

[Serializable]
public class DoorSettings
{
    public float doorTimerMin = 480f;
    public float doorTimerMax = 600f;
    public GameObject door;
    public Transform[] doorSpawnLocations;
}

[Serializable]
public class ExtraSettings
{
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
}

public class ParanoiaManager : MonoBehaviour
{
    public static ParanoiaManager Instance { get; private set; }
    internal readonly List<Event> events = new();
    
    public ManagerType managerType;
    public EventSettings eventSettings;
    public EntitySettings entitySettings;
    public DoorSettings doorSettings;
    public ExtraSettings extraSettings;
    
    private bool _enabled;
    private bool _doorSpawned;
    
    private int _eventsCaused;
    private bool _musicDisabled;
    private int _entitiesSpawned;
    
    private void Awake()
    {
        Instance = this;
        
        #region AI
        
        events.Add(new Crabtroll());
        events.Add(new DragNpcToCeiling());
        events.Add(new DragRandomNpc());
        events.Add(new KillAI());
        events.Add(new LaughAtPlayer());
        events.Add(new MoveAIToPlayer());
        events.Add(new MoveAIToRadio());
        events.Add(new MoveAIToSpecificLocation());
        
        #endregion
        
        #region Player
        
        events.Add(new FakeFireGun());
        events.Add(new FireGunInHand());
        events.Add(new GrabPlayer());
        
        #endregion
        
        #region World
        
        events.Add(new FireGun());
        events.Add(new FlickerFlashlights());
        events.Add(new FlingRandomObject());
        events.Add(new LightFlicker());
        
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
            var time = Random.Range(entitySettings.entityTimerMin, entitySettings.entityTimerMax);
            yield return new WaitForSeconds(time);
            ModConsole.Msg("Entity tick spawn phase", LoggingMode.Debug);
            var entity = Utilities.GetRandomEntity(entitySettings.entities);
            ModConsole.Msg($"Chosen entity: {entity.Crate.name}", LoggingMode.Debug);
            var crateTag = entity.Crate.Tags;
            switch (crateTag.Contains("Air") ? "Air" : crateTag.Contains("Ground") ? "Ground" : crateTag.Contains("Special") ? "Special" : crateTag.Contains("Audio") ? "Audio" : "None")
            {
                case "Air":
                {
                    ModConsole.Msg("Entity had Air tag", LoggingMode.Debug);
                    var location = entitySettings.airSpawns[Random.Range(0, entitySettings.airSpawns.Length)];
                    HelperMethods.SpawnCrate(entity, location.position, Quaternion.identity, Vector3.one, false, _ => { });
                    break;
                }
                case "Ground":
                {
                    ModConsole.Msg("Entity had Ground tag", LoggingMode.Debug);
                    var location = entitySettings.groundSpawns[Random.Range(0, entitySettings.groundSpawns.Length)];
                    HelperMethods.SpawnCrate(entity, location.position, Quaternion.identity, Vector3.one, false, _ => { });
                    break;
                }
                case "Special":
                {
                    ModConsole.Msg("Entity had Special tag", LoggingMode.Debug);
                    HelperMethods.SpawnCrate(entity, entitySettings.mirageSpawn.position, Quaternion.identity, Vector3.one,  false, _ => { });
                    break;
                }
                case "Audio":
                {
                    ModConsole.Msg("Entity had Audio tag", LoggingMode.Debug);
                    var location = entitySettings.audioSpawns[Random.Range(0, entitySettings.audioSpawns.Length)];
                    HelperMethods.SpawnCrate(entity, location.position, Quaternion.identity, Vector3.one, false, _ => { });
                    break;
                }
                case "None":
                {
                    ModConsole.Error($"You forgot to tag Entity {entity.Crate.name}! Spawning at a ground location. Your fault if it's wrong.");
                    var location = entitySettings.groundSpawns[Random.Range(0, entitySettings.groundSpawns.Length)];
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
            var time = Random.Range(eventSettings.eventTimerMin, eventSettings.eventTimerMax);
            // Wait for that time
            yield return new WaitForSeconds(time);
            ModConsole.Msg("Event tick event phase", LoggingMode.Debug);
            // Choose a random event
            var rand = Random.Range(0, events.Count);
            var chosenEvent = events[rand];
            if (chosenEvent.CanInvoke()) chosenEvent.Invoke();
            else
            {
                // if it can't invoke, try to get one that can.
                while (!chosenEvent.CanInvoke())
                {
                    var randAgain = Random.Range(0, events.Count);
                    // ensure that it's not just picking the same one again
                    while (randAgain == rand) randAgain = Random.Range(0, events.Count);
                    chosenEvent = events[randAgain];
                    chosenEvent.Invoke();
                }
            }
        }
    }
    
    private IEnumerator DoorTick()
    {
        ModConsole.Msg("Door tick started", LoggingMode.Debug);
        var time = Random.Range(doorSettings.doorTimerMin, doorSettings.doorTimerMax);
        yield return new WaitForSeconds(time);
        ModConsole.Msg("Door tick door phase", LoggingMode.Debug);
        var location = doorSettings.doorSpawnLocations[Random.Range(0, doorSettings.doorSpawnLocations.Length)];
        Instantiate(doorSettings.door, location.position, location.rotation);
        _doorSpawned = true;
    }
    
    private void OnDestroy()
    {
        Instance = null;
    }
    
    #region Other Managers
    
    private IEnumerator MuseumTick()
    {
        yield return new WaitForSeconds(extraSettings.phase1Timer);
        extraSettings.zoneMusic.StopMusic(3f);
        MuseumEvents.ChangeSign(extraSettings.signMesh, extraSettings.signTexture);
        extraSettings.thefog.SetActive(true);
        yield return new WaitForSeconds(extraSettings.phase2Timer);
        extraSettings.zoneMusic.PlayMusic(3f);
        MuseumEvents.HideSign(extraSettings.signMesh);
        extraSettings.thefog.SetActive(false);
        yield return new WaitForSeconds(extraSettings.phase3Timer);
        extraSettings.zoneMusic.StopMusic(3f);
        extraSettings.thefog.SetActive(true);
        extraSettings.warningSound.Play();
        MuseumEvents.UnhideSign(extraSettings.signMesh);
        MuseumEvents.ChangeSign(extraSettings.signMesh, extraSettings.signWarningTexture);
        Instantiate(doorSettings.door, extraSettings.doorSpawnLocation.position, extraSettings.doorSpawnLocation.rotation);
    }
    
    private IEnumerator BaselineEntityTick()
    {
        ModConsole.Msg("Entity tick started", LoggingMode.Debug);
        while (_enabled)
        {
            ModConsole.Msg("Entity tick begin", LoggingMode.Debug);
            var time = Random.Range(entitySettings.entityTimerMin, entitySettings.entityTimerMax);
            yield return new WaitForSeconds(time);
            ModConsole.Msg("Entity tick spawn phase", LoggingMode.Debug);
            if (_entitiesSpawned >= 3) _entitiesSpawned++;
            if (_entitiesSpawned == 3)
            {
                extraSettings.thefog.gameObject.SetActive(true);
                extraSettings.zoneMusic.StopMusic(1f);
            }
            var entity = Utilities.GetRandomEntity(entitySettings.entities);
            ModConsole.Msg($"Chosen entity: {entity.Crate.name}", LoggingMode.Debug);
            var crateTag = entity.Crate.Tags;
            switch (crateTag.Contains("Air") ? "Air" : crateTag.Contains("Ground") ? "Ground" : crateTag.Contains("Special") ? "Special" : crateTag.Contains("Audio") ? "Audio" : "None")
            {
                case "Air":
                {
                    ModConsole.Msg("Entity had Air tag", LoggingMode.Debug);
                    var location = entitySettings.airSpawns[Random.Range(0, entitySettings.airSpawns.Length)];
                    HelperMethods.SpawnCrate(entity, location.position, Quaternion.identity, Vector3.one, false, _ => { });
                    break;
                }
                case "Ground":
                {
                    ModConsole.Msg("Entity had Ground tag", LoggingMode.Debug);
                    var location = entitySettings.groundSpawns[Random.Range(0, entitySettings.groundSpawns.Length)];
                    HelperMethods.SpawnCrate(entity, location.position, Quaternion.identity, Vector3.one, false, _ => { });
                    break;
                }
                case "Special":
                {
                    ModConsole.Msg("Entity had Special tag", LoggingMode.Debug);
                    HelperMethods.SpawnCrate(entity, entitySettings.mirageSpawn.position, Quaternion.identity, Vector3.one,  false, _ => { });
                    break;
                }
                case "Audio":
                {
                    ModConsole.Msg("Entity had Audio tag", LoggingMode.Debug);
                    var location = entitySettings.audioSpawns[Random.Range(0, entitySettings.audioSpawns.Length)];
                    HelperMethods.SpawnCrate(entity, location.position, Quaternion.identity, Vector3.one, false, _ => { });
                    break;
                }
                case "None":
                {
                    ModConsole.Error($"You forgot to tag Entity {entity.Crate.name}! Spawning at a ground location. Your fault if it's wrong.");
                    var location = entitySettings.groundSpawns[Random.Range(0, entitySettings.groundSpawns.Length)];
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