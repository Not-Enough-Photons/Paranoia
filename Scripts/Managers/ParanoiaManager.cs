using System;
using MelonLoader;
using System.Collections;
using System.Collections.Generic;
using BoneLib;
using Paranoia.Helpers;
using SLZ.Marrow.Warehouse;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Paranoia.Managers
{
    public class ParanoiaManager : MonoBehaviour
    {
        public float eventTimerMin = 30f;
        public float eventTimerMax = 60f;
        private readonly List<Light> flicker = new List<Light>();
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

        public void AddLights(Light[] newLights)
        {
            flicker.AddRange(newLights);
            lights = flicker.ToArray();
        }
        
        /// <summary>
        /// Enables all tick coroutines.
        /// </summary>
        public void Enable()
        {
            if (_enabled) return;
            _enabled = true;
            MelonCoroutines.Start(EntityTick());
            MelonCoroutines.Start(EventTick());
            if (_doorSpawned) return;
            MelonCoroutines.Start(DoorTick());
        }
        /// <summary>
        /// Disables all tick coroutines.
        /// <br/>This is generally not needed, as the manager is gone when the level is unloaded, but it's just there for further control.
        /// </summary>
        public void Disable()
        {
            if (!_enabled) return;
            _enabled = false;
            MelonCoroutines.Stop(EntityTick());
            MelonCoroutines.Stop(EventTick());
            if (_doorSpawned) return;
            MelonCoroutines.Stop(DoorTick());
        }
        /// <summary>
        /// Entity Tick runs every X seconds, where X is generated from a random range between serialized fields entityTimerMin and entityTimerMax.
        /// <br/>Once that time is up, a random entity crate is chosen from the serialized field "entities" and is spawned as a crate.
        /// </summary>
        private IEnumerator EntityTick()
        {
            ModConsole.Msg("Entity tick started", LoggingMode.DEBUG);
            while (_enabled)
            {
                ModConsole.Msg("Entity tick begin", LoggingMode.DEBUG);
                var time = Random.Range(entityTimerMin, entityTimerMax);
                yield return new WaitForSeconds(time);
                ModConsole.Msg("Entity tick spawn phase", LoggingMode.DEBUG);
                var entity = entities[Random.Range(0, entities.Length)];
                ModConsole.Msg($"Chosen entity: {entity.Crate.name}", LoggingMode.DEBUG);
                var crateTag = entity.Crate.Tags;
                switch (crateTag.Contains("Air") ? "Air" : crateTag.Contains("Ground") ? "Ground" : crateTag.Contains("Special") ? "Special" : crateTag.Contains("Audio") ? "Audio" : "None")
                {
                    case "Air":
                    {
                        ModConsole.Msg("Entity had Air tag", LoggingMode.DEBUG);
                        var location = airSpawns[Random.Range(0, airSpawns.Length)];
                        HelperMethods.SpawnCrate(entity, location.position, Quaternion.identity, Vector3.one, false, go => { });
                        break;
                    }
                    case "Ground":
                    {
                        ModConsole.Msg("Entity had Ground tag", LoggingMode.DEBUG);
                        var location = groundSpawns[Random.Range(0, groundSpawns.Length)];
                        HelperMethods.SpawnCrate(entity, location.position, Quaternion.identity, Vector3.one, false, go => { });
                        break;
                    }
                    case "Special":
                    {
                        ModConsole.Msg("Entity had Special tag", LoggingMode.DEBUG);
                        HelperMethods.SpawnCrate(entity, mirageSpawn.position, Quaternion.identity, Vector3.one,  false, go => { });
                        break;
                    }
                    case "Audio":
                    {
                        ModConsole.Msg("Entity had Audio tag", LoggingMode.DEBUG);
                        var location = audioSpawns[Random.Range(0, audioSpawns.Length)];
                        HelperMethods.SpawnCrate(entity, location.position, Quaternion.identity, Vector3.one, false, go => { });
                        break;
                    }
                    case "None":
                    {
                        ModConsole.Warning("You idiot. You absolute buffoon. You have a crate with no tag. It has no way of spawning. You moron.");
                        ModConsole.Warning("I'm gonna slap this guy at a random ground spawn. I hope you're happy.");
                        var location = groundSpawns[Random.Range(0, groundSpawns.Length)];
                        HelperMethods.SpawnCrate(entity, location.position, Quaternion.identity, Vector3.one, false, go => { });
                        break;
                    }
                    default:
                    {
                        MelonLogger.Error("Something broke. Tag was set, but wasn't read.");
                        break;
                    } 
                }
            }
        }
        /// <summary>
        ///  Event Tick runs every X seconds, where X is generated from a random range between serialized fields eventTimerMin and eventTimerMax.
        ///  <br/>Once that time is up, a random event is chosen from the switch statement below.
        /// </summary>
        private IEnumerator EventTick()
        {
            ModConsole.Msg("Event tick started", LoggingMode.DEBUG);
            while (_enabled)
            {
                ModConsole.Msg("Event tick begin", LoggingMode.DEBUG);
                var time = Random.Range(eventTimerMin, eventTimerMax);
                yield return new WaitForSeconds(time);
                ModConsole.Msg("Event tick event phase", LoggingMode.DEBUG);
                // When adding new events, make sure to add them to the switch statement below. Increment the random range by 1, and add a new case.
                var rand = Random.Range(1, 15);
                switch (rand)
                {
                    case 1:
                        ModConsole.Msg("Chosen event: DragRandomNpc", LoggingMode.DEBUG);
                        Events.DragRandomNpc.Activate(grabSounds);
                        break;
                    case 2:
                        ModConsole.Msg("Chosen event: DragNpcToCeiling", LoggingMode.DEBUG);
                        Events.DragNpcToCeiling.Activate(grabSounds);
                        break;
                    case 3:
                        ModConsole.Msg("Chosen event: KillAI", LoggingMode.DEBUG);
                        Events.KillAI.Activate();
                        break;
                    case 4:
                        ModConsole.Msg("Chosen event: LaughAtPlayer", LoggingMode.DEBUG);
                        Events.LaughAtPlayer.Activate();
                        break;
                    case 5:
                        ModConsole.Msg("Chosen event: MoveAIToPlayer", LoggingMode.DEBUG);
                        Events.MoveAIToPlayer.Activate();
                        break;
                    case 6:
                        ModConsole.Msg("Chosen event: MoveAIToRadio", LoggingMode.DEBUG);
                        var location = groundSpawns[Random.Range(0, groundSpawns.Length)];
                        Events.MoveAIToRadio.Activate("NotEnoughPhotons.Paranoia.Spawnable.Radio", location);
                        break;
                    case 7:
                        ModConsole.Msg("Chosen event: FireGunInHand", LoggingMode.DEBUG);
                        Events.FireGunInHand.Activate();
                        break;
                    case 8:
                        ModConsole.Msg("Chosen event: FireGun", LoggingMode.DEBUG);
                        Events.FireGun.Activate();
                        break;
                    case 9:
                        ModConsole.Msg("Chosen Event: FlickerFlashlights", LoggingMode.DEBUG);
                        Events.FlickerFlashlights.Activate();
                        break;
                    case 10:
                        ModConsole.Msg("Chosen event: FlingRandomObject", LoggingMode.DEBUG);
                        Events.FlingRandomObject.Activate();
                        break;
                    case 11:
                        ModConsole.Msg("Chosen event: LightFlicker", LoggingMode.DEBUG);
                        Events.LightFlicker.Activate(lights);
                        break;
                    case 12:
                        ModConsole.Msg("Chosen event: GrabPlayer", LoggingMode.DEBUG);
                        Events.GrabPlayer.Activate(grabSounds);
                        break;
                    case 13:
                        ModConsole.Msg("Chosen event: Crabtroll", LoggingMode.DEBUG);
                        Events.Crabtroll.Activate();
                        break;
                    case 14:
                        ModConsole.Msg("Chosen event: MoveAIToSpecificLocation", LoggingMode.DEBUG);
                        var location3 = npcMoveLocations[Random.Range(0, npcMoveLocations.Length)];
                        Events.MoveAIToSpecificLocation.Activate(location3);
                        break;
                    case 15:
                        ModConsole.Msg("Chosen event: FakeFireGun", LoggingMode.DEBUG);
                        Events.FakeFireGun.Activate();
                        break;
                    default:
                        ModConsole.Error("Something broke. Random number couldn't be read. Falling back to DragRandomNpc.");
                        Events.DragRandomNpc.Activate(grabSounds);
                        break;
                }
            }
        }
        /// <summary>
        /// Door Tick runs every X seconds, where X is generated from a random range between serialized fields doorTimerMin and doorTimerMax.
        /// <br/>Once that time is up, the door prefab (serialized as "door") is spawned at a random location from the serialized field "doorSpawnLocations".
        /// </summary>
        private IEnumerator DoorTick()
        {
            ModConsole.Msg("Door tick started", LoggingMode.DEBUG);
            var time = Random.Range(doorTimerMin, doorTimerMax);
            yield return new WaitForSeconds(time);
            ModConsole.Msg("Door tick door phase", LoggingMode.DEBUG);
            var location = doorSpawnLocations[Random.Range(0, doorSpawnLocations.Length)];
            Instantiate(door, location.position, location.rotation);
            _doorSpawned = true;
        }

        public ParanoiaManager(IntPtr ptr) : base(ptr) { }
    }
}