using MelonLoader;
using NEP.Paranoia.Managers;
using NEP.Paranoia.ParanoiaUtilities;
using System.Collections.Generic;
using System.Linq;
using UnhollowerBaseLib;
using UnityEngine;

namespace NEP.Paranoia
{
    public static class BuildInfo
    {
        public const string Name = "paranoia"; // Name of the Mod.  (MUST BE SET)
        public const string Description = "Stay away from there. Please."; // Description for the Mod.  (Set as null if none)
        public const string Author = "Not Enough Photons"; // Author of the Mod.  (MUST BE SET)
        public const string Company = "Not Enough Photons"; // Company that made the Mod.  (Set as null if none)
        public const string Version = "3.0.2"; // Version of the Mod.  (MUST BE SET)
        public const string DownloadLink = null; // Download Link for the Mod.  (Set as null if none)
    }

    public class Paranoia : MelonMod
    {
        public static Paranoia instance;

        public AssetBundle bundle;

        public GameManager gameManager;

        public string currentScene;
        public string nextScene;

        private Dictionary<string, GameObject> baseEntities;

        public List<AudioClip> genericAmbience = new List<AudioClip>();
        public List<AudioClip> screamAmbience = new List<AudioClip>();

        public List<AudioClip> chaserAmbience = new List<AudioClip>();
        public List<AudioClip> teleporterAmbience = new List<AudioClip>();
        public List<AudioClip> paralyzerAmbience = new List<AudioClip>();
        public List<AudioClip> watcherAmbience = new List<AudioClip>();
        public List<AudioClip> cryingAmbience = new List<AudioClip>();
        public List<AudioClip> darkVoices = new List<AudioClip>();

        public List<AudioClip> deafenSounds = new List<AudioClip>();
        public List<AudioClip> grabSounds = new List<AudioClip>();
        public List<AudioClip> doorIdleSounds = new List<AudioClip>();
        public List<AudioClip> doorOpenSounds = new List<AudioClip>();
        public List<AudioClip> radioTunes = new List<AudioClip>();

        public List<Texture2D> decorTextures = new List<Texture2D>();

        private string dataPath = MelonUtils.UserDataDirectory + "/paranoia";

        public bool _debugMode = false;

        public string[] supportedMaps = new string[]
        {
            "sandbox_blankbox",
            "sandbox_museumbasement"
        };

        public bool isTargetLevel;

        /// <summary>
        /// Gets an entity in the entity list.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public GameObject GetEntInDirectory(string name)
        {
            return baseEntities[name];
        }

        /// <summary>
        /// Gets a clip in a target dictionary.
        /// </summary>
        /// <param name="list"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public AudioClip GetClipInDirectory(List<AudioClip> list, string name)
        {
            return list.FirstOrDefault((clip) => clip.name == name);
        }

        public Texture2D GetTextureInList(string name)
        {
            return decorTextures.FirstOrDefault((texture) => texture.name == name);
        }

        public override void OnApplicationStart()
        {
            try
            {
                if (instance == null)
                {
                    instance = this;
                }

                Utilities utils = new Utilities();
                MapUtilities mapUtils = new MapUtilities();

                Utilities.RegisterTypesInIL2CPP();

                if (!System.IO.Directory.Exists(dataPath))
                {
                    System.IO.Directory.CreateDirectory(dataPath);
                }

                bundle = AssetBundle.LoadFromFile(dataPath + "/paranoia.pack");

                PrecacheEntityObjects();
                PrecacheAudioAssets();
                PrecacheTextureAssets();
            }
            catch (System.Exception e)
            {
                throw e;
            }
        }

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            currentScene = sceneName;

            switch (currentScene.ToLower())
            {
                case "scene_breakroom": MapUtilities.currentLevel = MapLevel.Breakroom; break;
                case "scene_museum": MapUtilities.currentLevel = MapLevel.Museum; break;
                case "scene_streets": MapUtilities.currentLevel = MapLevel.Streets; break;
                case "scene_runoff": MapUtilities.currentLevel = MapLevel.Runoff; break;
                case "scene_sewerstation": MapUtilities.currentLevel = MapLevel.Sewers; break;
                case "scene_warehouse": MapUtilities.currentLevel = MapLevel.Warehouse; break;
                case "scene_subwaystation": MapUtilities.currentLevel = MapLevel.CentralStation; break;
                case "scene_tower": MapUtilities.currentLevel = MapLevel.Tower; break;
                case "scene_towerboss": MapUtilities.currentLevel = MapLevel.TimeTower; break;
                case "scene_dungeon": MapUtilities.currentLevel = MapLevel.Dungeon; break;
                case "scene_arena": MapUtilities.currentLevel = MapLevel.Arena; break;
                case "scene_throneroom": MapUtilities.currentLevel = MapLevel.ThroneRoom; break;
                case "scene_tuscany": MapUtilities.currentLevel = MapLevel.Tuscany; break;
                case "scene_redactedchamber": MapUtilities.currentLevel = MapLevel.RedactedChamber; break;
                case "sandbox_handgunbox": MapUtilities.currentLevel = MapLevel.HandgunRange; break;
                case "scene_hoverjunkers": MapUtilities.currentLevel = MapLevel.HoverJunkers; break;
                case "sandbox_blankbox": MapUtilities.currentLevel = MapLevel.Blankbox;
                    MelonCoroutines.Start(CoCustomMapsRoutine()); break;
                case "sandbox_museumbasement": MapUtilities.currentLevel = MapLevel.MuseumBasement;
                    break;
                case "custom_map_bbl": MapUtilities.currentLevel = MapLevel.CustomMap; MelonCoroutines.Start(CoCustomMapsRoutine()); break;
                default: break;
            }

            gameManager = new GameObject("Game Manager").AddComponent<GameManager>();
        }

        public override void OnSceneWasUnloaded(int buildIndex, string sceneName)
        {
            if (sceneName == nextScene)
            {
                if (gameManager != null && !gameManager.WasCollected)
                {
                    gameManager.Cleanup();
                    UnityEngine.Object.Destroy(gameManager);
                }
            }
        }

        internal void RegisterObject(GameObject bundleObject, string assetName)
        {
            bundleObject = bundle.LoadAsset(assetName).Cast<GameObject>();
            bundleObject.hideFlags = HideFlags.DontUnloadUnusedAsset;
        }

        private System.Collections.IEnumerator CoCustomMapsRoutine()
        {
            yield return new WaitForSecondsRealtime(5f);

            gameManager = new GameObject("Game Manager").AddComponent<GameManager>();

            yield return null;
        }

        private void PrecacheEntityObjects()
        {
            baseEntities = new Dictionary<string, GameObject>();
            Il2CppReferenceArray<UnityEngine.Object> assets = bundle.LoadAllAssets();

            for (int i = 0; i < assets.Count; i++)
            {
                if (assets[i].TryCast<GameObject>() != null && assets[i].name.StartsWith("ent_"))
                {
                    GameObject entAsset = assets[i].TryCast<GameObject>();
                    entAsset.hideFlags = HideFlags.DontUnloadUnusedAsset;
                    baseEntities.Add(entAsset.name, entAsset);
                }
            }
        }

        private void PrecacheAudioAssets()
        {
            string[] nameLUT = new string[]
            {
                "amb_generic",
                "amb_scream",
                "amb_chaser",
                "amb_watcher",
                "amb_teleportingthing",
                "amb_dark_voice",
                "amb_paralysis",
                "amb_crying",
                "amb_deafen",
                "player_grab",
                "door_idle",
                "door_open",
                "radio_tune"
            };

            Dictionary<string, List<AudioClip>> directory = new Dictionary<string, List<AudioClip>>()
            {
                { "amb_generic", genericAmbience },
                { "amb_scream", screamAmbience },
                { "amb_chaser", chaserAmbience },
                { "amb_watcher", watcherAmbience },
                { "amb_teleportingthing", teleporterAmbience },
                { "amb_crying", cryingAmbience },
                { "amb_dark_voice", darkVoices },
                { "amb_paralysis", paralyzerAmbience },
                { "amb_deafen", deafenSounds },
                { "player_grab", grabSounds },
                { "door_idle", doorIdleSounds },
                { "door_open", doorOpenSounds },
                { "radio_tune", radioTunes }
            };

            Il2CppReferenceArray<UnityEngine.Object> assets = bundle.LoadAllAssets();

            for (int i = 0; i < directory.Keys.Count; i++)
            {
                for (int j = 0; j < assets.Count; j++)
                {
                    if (assets[j].TryCast<AudioClip>() == null) { continue; }

                    AudioClip clip = assets[j].Cast<AudioClip>();
                    clip.hideFlags = HideFlags.DontUnloadUnusedAsset;

                    if (clip.name.StartsWith(nameLUT[i]))
                    {
                        directory[nameLUT[i]].Add(clip);
                    }
                }
            }
        }

        private void PrecacheTextureAssets()
        {
            Il2CppReferenceArray<UnityEngine.Object> assets = bundle.LoadAllAssets();

            for (int i = 0; i < assets.Count; i++)
            {
                if (assets[i].TryCast<Texture2D>() == null) { continue; }

                Texture2D texture = assets[i].Cast<Texture2D>();
                texture.hideFlags = HideFlags.DontUnloadUnusedAsset;

                decorTextures.Add(texture);
            }
        }
    }
}