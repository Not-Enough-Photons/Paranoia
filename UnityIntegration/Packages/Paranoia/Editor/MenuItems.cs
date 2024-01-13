using UnityEngine;
using UnityEditor;
using NEP.Paranoia.Scripts.Managers;

namespace NEP.Paranoia.Editor
{
    public class MenuItems
    {
        [MenuItem("GameObject/Paranoia/Paranoia Manager", false, 10)]
        private static void CreateParanoiaManager(MenuCommand menuCommand)
        {
            var go = new GameObject("ParanoiaManager");
            GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
            var manager = go.AddComponent<ParanoiaManager>();
            // Spawnpoint parent creation
            var doorSpawns = new GameObject("DoorSpawns");
            doorSpawns.transform.parent = go.transform;
            var groundSpawns = new GameObject("GroundSpawns");
            groundSpawns.transform.parent = go.transform;
            var airSpawns = new GameObject("AirSpawns");
            airSpawns.transform.parent = go.transform;
            var audioSpawns = new GameObject("AudioSpawns");
            audioSpawns.transform.parent = go.transform;
            var mirageSpawn = new GameObject("MirageSpawn");
            mirageSpawn.transform.parent = go.transform;
            var walkLocations = new GameObject("WalkLocations");
            walkLocations.transform.parent = go.transform;
            // Spawnpoint creation
            var doorSpawn1 = new GameObject("Spawn1");
            doorSpawn1.transform.parent = doorSpawns.transform;
            var groundSpawn1 = new GameObject("Spawn1");
            groundSpawn1.transform.parent = groundSpawns.transform;
            var airSpawn1 = new GameObject("Spawn1");
            airSpawn1.transform.parent = airSpawns.transform;
            var audioSpawn1 = new GameObject("Spawn1");
            audioSpawn1.transform.parent = audioSpawns.transform;
            var mirageSpawn1 = new GameObject("Spawn");
            mirageSpawn1.transform.parent = mirageSpawn.transform;
            var walkLocation1 = new GameObject("Location1");
            walkLocation1.transform.parent = walkLocations.transform;
            Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
            Selection.activeObject = go;
        }
    }
}