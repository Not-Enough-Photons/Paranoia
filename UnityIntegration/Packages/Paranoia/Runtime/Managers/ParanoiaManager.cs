﻿using System.Collections;
using SLZ.Marrow.Warehouse;
using System.Collections.Generic;
using NEP.Paranoia.Scripts.Entities;
using UnityEngine;
using Random = UnityEngine.Random;
using System;
using NEP.Paranoia.Scripts.InternalBehaviours;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace NEP.Paranoia.Scripts.Managers
{
	[Serializable]
	public class EventSettings
	{
		[Tooltip("The minimum time between events.")]
		public float eventTimerMin = 30f;
		[Tooltip("The maximum time between events.")]
		public float eventTimerMax = 60f;
		[Tooltip("All realtime lights in the scene.")]
		public Light[] lights;
		[Tooltip("Possible locations for NPCs to randomly walk to.")]
		public Transform[] npcMoveLocations;
		[Tooltip("Sounds that play when an NPC gets grabbed.")]
		public AudioClip[] grabSounds;
	}

	[Serializable]
	public class EntitySettings
	{
		[Tooltip("The minimum time between entitiy spawns.")]
		public float entityTimerMin = 60f;
		[Tooltip("The maximum time between entity spawns.")]
		public float entityTimerMax = 80f;
		[Tooltip("Whether or not to include entities from extension mods.")]
		public bool allowExtensionEntities = true;
		[Tooltip("All possible entities that can spawn.")]
		public List<SpawnableCrateReference> entities;
		[Tooltip("Spawn locations for air entities.")]
		public Transform[] airSpawns;
		[Tooltip("Spawn locations for ground entities.")]
		public Transform[] groundSpawns;
		[Tooltip("Spawn locations for audio entities.")]
		public Transform[] audioSpawns;
		[Tooltip("The mirage spawn location.")]
		public Transform mirageSpawn;
	}

	[Serializable]
	public class DoorSettings
	{
		[Tooltip("The minimum time for the door spawn.")]
		public float doorTimerMin = 480f;
		[Tooltip("The maximum time for the door spawn.")]
		public float doorTimerMax = 600f;
		[Tooltip("The door object.")]
		public GameObject door;
		[Tooltip("The door spawn locations.")]
		public Transform[] doorSpawnLocations;
	}

	[Serializable]
	public class ExtraSettings
	{
		public MeshRenderer signMesh;
		public Texture2D signTexture;
		public Texture2D signWarningTexture;
		public AudioSource warningSound;
		public GameObject thefog;
		public Transform doorSpawnLocation;
		public float phase1Timer = 120f;
		public float phase2Timer = 15f;
		public float phase3Timer = 600f;
	}
	
    [AddComponentMenu("Paranoia/Managers/Paranoia Manager")]
	[HelpURL("https://github.com/Not-Enough-Photons/Paranoia/wiki/Managers#paranoia-manager")]
    public class ParanoiaManager : ParanoiaEvent
    {
        public override string Warning => "EVERY VALUE MUST BE SET!";
        public override string Comment => "KEEP MANAGER TYPE AS PARANOIA!";
        
        public static ParanoiaManager Instance { get; private set; }
    
        public ManagerType managerType;
        public EventSettings eventSettings;
        public EntitySettings entitySettings;
        public DoorSettings doorSettings;
        [Header("IGNORE THESE!")]
        public ExtraSettings extraSettings;
    
        private bool _enabled;
        private bool _doorSpawned;
    
        private int _eventsCaused;
        private bool _musicDisabled;
        private int _entitiesSpawned;
        
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
			if (entitySettings.airSpawns.Length > 0) DrawGizmosForArray(entitySettings.airSpawns, Color.blue);
			if (entitySettings.groundSpawns.Length > 0) DrawGizmosForArray(entitySettings.groundSpawns, Color.green);
			if (entitySettings.audioSpawns.Length > 0) DrawGizmosForArray(entitySettings.audioSpawns, Color.red);
			if (doorSettings.doorSpawnLocations.Length > 0) DrawGizmosForArray(doorSettings.doorSpawnLocations, Color.yellow);

			if (entitySettings.mirageSpawn != null)
			{
				Gizmos.color = Color.cyan;
				Gizmos.DrawSphere(entitySettings.mirageSpawn.position, 0.3f);
			}

			if (eventSettings.npcMoveLocations.Length > 0) DrawGizmosForArray(eventSettings.npcMoveLocations, Color.magenta);
		}

		private void DrawGizmosForArray(Transform[] array, Color color)
		{
			Gizmos.color = color;

			foreach (Transform t in array)
			{
				Gizmos.DrawSphere(t.position, 0.3f);
			}
		}	
#endif		
    }
}