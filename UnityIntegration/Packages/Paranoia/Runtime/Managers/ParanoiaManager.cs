using System.Collections;
using NEP.Paranoia.Helpers;
using SLZ.Marrow.Warehouse;
using System.Collections.Generic;
using NEP.Paranoia.Entities;
using UnityEngine;
using Random = UnityEngine.Random;
using SLZ.SFX;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace NEP.Paranoia.Managers
{
    [AddComponentMenu("Paranoia/Managers/Paranoia Manager")]
    public class ParanoiaManager : ParanoiaEvent
    {
        public override string Warning => "EVERY VALUE MUST BE SET!";
        public override string Comment => "DO NOT BOTHER WITH MUSEUM/BASELINE SETTINGS!";
        
        [HideInInspector]
        public static ParanoiaManager Instance { get; private set; }
        private readonly List<Event> _events = new();
    
        #region Main Settings
		[Header("Main Settings")]
		public string warning = "You should leave this as Paranoia.";
		[Tooltip("The manager type. Generally, you should only pick Paranoia. The others are internally used and aren't meant for public use.")]
        public ManagerType managerType;
        
        [Space]
        
        [Header("Event Settings")]
        [Tooltip("Minimum time before an event happens.")]
        public float eventTimerMin = 30f;
        [Tooltip("Maximum time before an event happens.")]
        public float eventTimerMax = 60f;
        [Tooltip("All realtime lights.")]
        public Light[] lights;
        [Tooltip("Places where NPCs may move to randomly.")]
        public Transform[] npcMoveLocations;
        [Tooltip("Sounds for when an NPC gets grabbed.")]
        public AudioClip[] grabSounds;
        
        [Space]
        
        [Header("Entity Settings")]
        [Tooltip("Minimum time before an entity spawns.")]
        public float entityTimerMin = 60f;
        [Tooltip("Maximum time before an entity spawns.")]
        public float entityTimerMax = 80f;
        [Tooltip("All entities that can spawn.")]
        public SpawnableCrateReference[] entities;
        [Tooltip("Spawn locations for air entities.")]
        public Transform[] airSpawns;
        [Tooltip("Spawn locations for ground entities.")]
        public Transform[] groundSpawns;
        [Tooltip("Spawn locations for audio entities.")]
        public Transform[] audioSpawns;
        [Tooltip("Spawn locations for mirages.")]
        public Transform mirageSpawn;
        
		[Space]
        
        [Header("Door Settings")]
        [Tooltip("Minimum time before the door spawns.")]
        public float doorTimerMin = 480f;
        [Tooltip("Maximum time before the door spawns.")]
        public float doorTimerMax = 600f;
        [Tooltip("The door object.")]
        public GameObject door;
        [Tooltip("Spawn locations for the door.")]
        public Transform[] doorSpawnLocations;
        private bool _enabled;
        private bool _doorSpawned;
    
        #endregion

        [Space] 
        
        public string warning2 = "You don't need to touch anything below this warning.";
        
        [Space]
        
        #region Museum Settings
        [HideInInspector]
        public MeshRenderer signMesh;
        [Header("Museum Settings")]
        [Tooltip("The texture to change the sign to.")]
        public Texture2D signTexture;
        [Tooltip("The texture to change the sign to when the door spawns.")]
        public Texture2D signWarningTexture;
        [Tooltip("The sound that plays when the door spawns.")]
        public AudioSource warningSound;
        [HideInInspector]
        public ZoneMusic zoneMusic;
        [Tooltip("The object with the global volume with the fog.")]
        public GameObject globalVolume;
        [Tooltip("The door spawn location.")]
        public Transform doorSpawnLocation;
        [Tooltip("Time until phase 1 (eye appear), in seconds")]
        public float phase1Timer = 120f;
        [Tooltip("Time until phase 2 (eye disappear), in seconds")]
        public float phase2Timer = 15f;
        [Tooltip("Time until phase 3 (door spawn), in seconds")]
        public float phase3Timer = 600f;
    
        #endregion
    
        [Space]
        
        #region Baseline Settings
        
        [Header("Baseline Settings")]
        
        [Tooltip("The fog object.")]
        public GameObject thefog;
        private int _eventsCaused;
        private bool _musicDisabled;
        private int _entitiesSpawned;
    
        #endregion
        
        private void Awake()
        {
        
        }

        public void Enable()
        {
        
        }
    
        public void Disable()
        {
        
        }
    
        private IEnumerator EntityTick()
        {
            yield return null;
        }
    
        private IEnumerator EventTick()
        {
            yield return null;
        }
    
        private IEnumerator DoorTick()
        {
            yield return null;
        }
    
        private void OnDestroy()
        {
            
        }
    
        #region Other Managers
    
        private IEnumerator MuseumTick()
        {
            yield return null;
        }
    
		private IEnumerator BaselineEntityTick()
		{
			yield return null;
		}
    
		#endregion
		
#if UNITY_EDITOR
		private void OnDrawGizmos()
		{
			DrawGizmosForArray(airSpawns, Color.blue);
			DrawGizmosForArray(groundSpawns, Color.green);
			DrawGizmosForArray(audioSpawns, Color.red);
			DrawGizmosForArray(doorSpawnLocations, Color.yellow);

			if (mirageSpawn != null)
			{
				Gizmos.color = Color.cyan;
				Gizmos.DrawSphere(mirageSpawn.position, 0.3f);
			}

			DrawGizmosForArray(npcMoveLocations, Color.magenta);
		}

		private void DrawGizmosForArray(Transform[] array, Color color)
		{
			Gizmos.color = color;

			foreach (Transform t in array)
			{
				Gizmos.DrawSphere(t.position, 0.3f);
			}
		}		
    }
#endif
}