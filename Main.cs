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
        public const string Description = "They're everywhere."; // Description for the Mod.  (Set as null if none)
        public const string Author = "Not Enough Photons"; // Author of the Mod.  (MUST BE SET)
        public const string Company = "Not Enough Photons"; // Company that made the Mod.  (Set as null if none)
        public const string Version = "4.0.0"; // Version of the Mod.  (MUST BE SET)
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

        public List<AudioClip> mainClips;

        public List<Texture2D> decorTextures = new List<Texture2D>();

        private string dataPath = MelonUtils.UserDataDirectory + "/paranoia";

        /// <summary>
        /// Gets an entity in the entity list.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public GameObject GetEntInDirectory(string name)
        {
            if(baseEntities[name] == null)
            {
                return null;
            }

            return baseEntities[name];
        }

        /// <summary>
        /// Gets a clip in a target dictionary.
        /// </summary>
        /// <param name="list"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public AudioClip GetClipInDirectory(string name)
        {
            return mainClips.FirstOrDefault((clip) => clip.name == name);
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

                mainClips = new List<AudioClip>();

                Utilities utils = new Utilities();
                MapUtilities mapUtils = new MapUtilities();

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

            gameManager = new GameManager();

            if (currentScene.ToLower() == "scene_blankbox")
            {
                MapUtilities.currentLevel = MapLevel.Blankbox;
                
            }

            /*switch (currentScene.ToLower())
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
            */
            
        }

        public override void OnUpdate()
        {
            if(gameManager != null)
            {
                gameManager.tickManager.Update();
            }
        }

        internal void RegisterObject(GameObject bundleObject, string assetName)
        {
            bundleObject = bundle.LoadAsset(assetName).Cast<GameObject>();
            bundleObject.hideFlags = HideFlags.DontUnloadUnusedAsset;
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
            Il2CppReferenceArray<UnityEngine.Object> assets = bundle.LoadAllAssets();

            for (int j = 0; j < assets.Count; j++)
            {
                if (assets[j].TryCast<AudioClip>() == null) { continue; }

                AudioClip clip = assets[j].Cast<AudioClip>();
                clip.hideFlags = HideFlags.DontUnloadUnusedAsset;

                mainClips.Add(clip);
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