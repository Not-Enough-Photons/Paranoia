using System;
using MelonLoader;
using System.Collections;
using Paranoia.Helpers;
using SLZ.Marrow.Warehouse;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Paranoia.Managers
{
    public class ParanoiaManager : MonoBehaviour
    {
        public float eventTimerMin = 30f;
        public float eventTimerMax = 60f;
        public GameObject lights;
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
        
        public void Enable()
        {
            if (_enabled) return;
            _enabled = true;
            MelonCoroutines.Start(EntityTick());
            MelonCoroutines.Start(EventTick());
            if (_doorSpawned) return;
            MelonCoroutines.Start(DoorTick());
        }

        public void Disable()
        {
            if (!_enabled) return;
            _enabled = false;
            MelonCoroutines.Stop(EntityTick());
            MelonCoroutines.Stop(EventTick());
            if (_doorSpawned) return;
            MelonCoroutines.Stop(DoorTick());
        }

        private IEnumerator EntityTick()
        {
            MelonLogger.Msg("Entity tick started");
            while (_enabled)
            {
                MelonLogger.Msg("Entity tick begin");
                var time = Random.Range(entityTimerMin, entityTimerMax);
                yield return new WaitForSeconds(time);
                MelonLogger.Msg("Entity tick spawn phase");
                var entity = entities[Random.Range(0, entities.Length)];
                MelonLogger.Msg($"Chosen entity: {entity.Crate.name}");
                var crateTag = entity.Crate.Tags;
                switch (crateTag.Contains("Air") ? "Air" : crateTag.Contains("Ground") ? "Ground" : crateTag.Contains("Special") ? "Special" : crateTag.Contains("Audio") ? "Audio" : "None")
                {
                    case "Air":
                    {
                        var location = airSpawns[Random.Range(0, airSpawns.Length)];
                        Warehouse.Spawn(entity, location.position, Quaternion.identity, false, go => { });
                        break;
                    }
                    case "Ground":
                    {
                        var location = groundSpawns[Random.Range(0, groundSpawns.Length)];
                        Warehouse.Spawn(entity, location.position, Quaternion.identity, false, go => { });
                        break;
                    }
                    case "Special":
                    {
                        Warehouse.Spawn(entity, mirageSpawn.position, Quaternion.identity, false, go => { });
                        break;
                    }
                    case "Audio":
                    {
                        var location = audioSpawns[Random.Range(0, audioSpawns.Length)];
                        Warehouse.Spawn(entity, location.position, Quaternion.identity, false, go => { });
                        break;
                    }
                    case "None":
                    {
                        MelonLogger.Msg("You forgot to set a tag lol");
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
        
        private IEnumerator EventTick()
        {
            MelonLogger.Msg("Event tick started");
            while (_enabled)
            {
                MelonLogger.Msg("Event tick begin");
                var time = Random.Range(eventTimerMin, eventTimerMax);
                yield return new WaitForSeconds(time);
                MelonLogger.Msg("Event tick event phase");
                var rand = Random.Range(1, 15);
                switch (rand)
                {
                    case 1:
                        MelonLogger.Msg("Chosen event: DragRandomNpc");
                        Events.DragRandomNpc.Activate(gameObject);
                        break;
                    case 2:
                        MelonLogger.Msg("Chosen event: DragNpcToCeiling");
                        Events.DragNpcToCeiling.Activate(gameObject);
                        break;
                    case 3:
                        MelonLogger.Msg("Chosen event: KillAI");
                        Events.KillAI.Activate();
                        break;
                    case 4:
                        MelonLogger.Msg("Chosen event: LaughAtPlayer");
                        Events.LaughAtPlayer.Activate();
                        break;
                    case 5:
                        MelonLogger.Msg("Chosen event: MoveAIToPlayer");
                        Events.MoveAIToPlayer.Activate();
                        break;
                    case 6:
                        MelonLogger.Msg("Chosen event: MoveAIToRadio");
                        var location = groundSpawns[Random.Range(0, groundSpawns.Length)];
                        Events.MoveAIToRadio.Activate("NotEnoughPhotons.Paranoia.Spawnable.Radio", location);
                        break;
                    case 7:
                        MelonLogger.Msg("Chosen event: FireGunInHand");
                        Events.FireGunInHand.Activate();
                        break;
                    case 8:
                        MelonLogger.Msg("Chosen event: FireGun");
                        Events.FireGun.Activate();
                        break;
                    case 9:
                        MelonLogger.Msg("Chosen Event: FlickerFlashlights");
                        Events.FlickerFlashlights.Activate();
                        break;
                    case 10:
                        MelonLogger.Msg("Chosen event: FlingRandomObject");
                        Events.FlingRandomObject.Activate();
                        break;
                    case 11:
                        MelonLogger.Msg("Chosen event: LightFlicker");
                        Events.LightFlicker.Activate(lights);
                        break;
                    case 12:
                        MelonLogger.Msg("Chosen event: GrabPlayer");
                        Events.GrabPlayer.Activate(gameObject);
                        break;
                    case 13:
                        MelonLogger.Msg("Chosen event: Crabtroll");
                        Events.Crabtroll.Activate();
                        break;
                    case 14:
                        MelonLogger.Msg("Chosen event: MoveAIToSpecificLocation");
                        var location3 = npcMoveLocations[Random.Range(0, npcMoveLocations.Length)];
                        Events.MoveAIToSpecificLocation.Activate(location3);
                        break;
                    case 15:
                        MelonLogger.Msg("Chosen event: FakeFireGun");
                        Events.FakeFireGun.Activate();
                        break;
                    default:
                        MelonLogger.Error("Something broke. Random number couldn't be read. Falling back to DragRandomNpc.");
                        Events.DragRandomNpc.Activate(gameObject);
                        break;
                }
            }
        }

        private IEnumerator DoorTick()
        {
            MelonLogger.Msg("Door tick started");
            var time = Random.Range(doorTimerMin, doorTimerMax);
            yield return new WaitForSeconds(time);
            MelonLogger.Msg("Door tick door phase");
            var location = doorSpawnLocations[Random.Range(0, doorSpawnLocations.Length)];
            Instantiate(door, location.position, location.rotation);
            _doorSpawned = true;
        }

        public ParanoiaManager(IntPtr ptr) : base(ptr) { }
    }
}