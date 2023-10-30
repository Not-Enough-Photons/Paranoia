using System;
using System.Collections;
using System.Collections.Generic;
using Paranoia.Helpers;
using SLZ.Marrow.Warehouse;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Paranoia.Managers
{
    [AddComponentMenu("Paranoia/Museum Manager")]
    public class MuseumManager : ParanoiaEvent
    {
        public override string Warning => "DO NOT USE THIS! THIS IS USED ONLY EVER INTERNALLY!";
        [Header("Sign Tick")]
        [Tooltip("The mesh renderer that will be used for the sign.")]
        public MeshRenderer signMesh;
        [Tooltip("The texture that will be used for the sign.")]
        public Texture2D signTexture;
        [Tooltip("The minimum amount of time between sign events.")]
        public float signChangeTimerMin = 30f;
        [Tooltip("The maximum amount of time between sign events.")]
        public float signChangeTimerMax = 60f;
        [Tooltip("The minimum amount of time between sign deletion.")]
        public float signDeleteTimerMin = 60f;
        [Tooltip("The maximum amount of time between sign deletion.")]
        public float signDeleteTimerMax = 75f;
        [Header("Fog Tick")]
        [Tooltip("The global volume that will be used for the fog.")]
        public GameObject globalVolume;
        [Tooltip("The minimum amount of time between fog events.")]
        public float fogTimerMin = 120f;
        [Tooltip("The maximum amount of time between fog events.")]
        public float fogTimerMax = 240f;
        [Header("Event Tick")]
        [Tooltip("The minimum amount of time between events.")]
        public float eventTimerMin = 30f;
        [Tooltip("The maximum amount of time between events.")]
        public float eventTimerMax = 60f;
        [Tooltip("The lights that will flicker during the LightFlicker event.")]
        public Light[] lights;
        [HideInInspector]
        public List<Light> _lights;
        [Tooltip("The list of locations that NPCs may be moved to.")]
        public Transform[] npcMoveLocations;
        [Tooltip("The sounds used for the grab events.")]
        public AudioClip[] grabSounds;
        [Header("Entity Tick")]
        [Tooltip("The minimum amount of time between entities spawning.")]
        public float entityTimerMin = 60f;
        [Tooltip("The maximum amount of time between entities spawning.")]
        public float entityTimerMax = 80f;
        [Tooltip("The list of entities that might spawn.")]
        public SpawnableCrateReference[] entities;
        [Tooltip("The list of locations that entities may spawn in the air.")]
        public Transform[] airSpawns;
        [Tooltip("The list of locations that entities may spawn on the ground.")]
        public Transform[] groundSpawns;
        [Tooltip("The list of locations that audio events will spawn at")]
        public Transform[] audioSpawns;
        [Tooltip("The location that the mirage will spawn.")]
        public Transform mirageSpawn;
        [Header("Door Tick")]
        [Tooltip("The minimum amount of time between the door spawning.")]
        public float doorTimerMin = 480f;
        [Tooltip("The maximum amount of time between the door spawning.")]
        public float doorTimerMax = 600f;
        [Tooltip("The door prefab that will spawn.")]
        public GameObject door;
        [Tooltip("The list of locations that the door may spawn.")]
        public Transform[] doorSpawnLocations;
        private bool _enabled;
        private bool _doorSpawned;
        
        public void AddLightsToArray()
        {

        }
        
        public void Enable()
        {

        }

        public void Disable()
        {

        }

        private IEnumerator SignTick()
        {
            yield return null;
        }
        
        private IEnumerator FogTick()
        {
            yield return null;
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
    }
}