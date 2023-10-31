using System;
using System.Collections;
using System.Collections.Generic;
using Paranoia.Helpers;
using SLZ.Marrow.Warehouse;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Paranoia.Managers
{
    [AddComponentMenu("Paranoia/Managers/Museum Manager")]
    public class MuseumManager : ParanoiaEvent
    {
        public override string Warning => "DO NOT USE THIS! THIS IS USED ONLY EVER IN MUSEUM BASEMENT!";
        [Header("Museum Tick")]
        [Tooltip("The mesh renderer of the sign.")]
        public MeshRenderer signMesh;
        [Tooltip("The texture of the sign.")]
        public Texture2D signTexture;
        [Tooltip("The texture of the sign when the door is open.")]
        public Texture2D signWarningTexture;
        [Tooltip("The global volume with the fog.")]
        public GameObject globalVolume;
        [Tooltip("The door prefab that will spawn.")]
        public GameObject door;
        [Tooltip("The location that the door may spawn.")]
        public Transform doorSpawnLocation;
        [Tooltip("The amount of time before the sign changes.")]
        public float phase1Timer = 120f;
        [Tooltip("The amount of time before sign hides and the fog goes away.")]
        public float phase2Timer = 15f;
        [Tooltip("The amount of time before the door spawns and the fog returns.")]
        public float phase3Timer = 600f;
        
        [Header("Event Tick")]
        [Tooltip("The minimum amount of time between events.")]
        public float eventTimerMin = 30f;
        [Tooltip("The maximum amount of time between events.")]
        public float eventTimerMax = 60f;
        [Tooltip("The list of locations that NPCs may be moved to.")]
        public Transform[] npcMoveLocations;
        [Tooltip("The sounds used for the grab events.")]
        public AudioClip[] grabSounds;
        [Tooltip("The places the radio can spawn at")]
        public Transform[] radioSpawns;
        private bool _enabled;
        
        public void Enable()
        {

        }

        public void Disable()
        {

        }

        private IEnumerator MuseumTick()
        {
            yield return null;
        }

        private IEnumerator EventTick()
        {
            yield return null;
        }
    }
}