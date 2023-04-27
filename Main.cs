using MelonLoader;
using NEP.Paranoia.Managers;
using NEP.Paranoia.ParanoiaUtilities;
using System.Collections.Generic;
using System.Linq;
using UnhollowerBaseLib;
using UnityEngine;

using BoneLib;
using static SLZ.UI.SceneAmmoUI;

namespace NEP.Paranoia
{
    public static class BuildInfo
    {
        public const string Name = "paranoia"; // Name of the Mod.  (MUST BE SET)
        public const string Description = "..."; // Description for the Mod.  (Set as null if none)
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
        public TickManager tickManager;

        public MapLevel mapLevel;
        public string currentScene;
        public string nextScene;

        private Dictionary<string, GameObject> baseEntities;

        public List<AudioClip> mainClips;

        public List<Texture2D> textures = new List<Texture2D>();

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
            return textures.FirstOrDefault((texture) => texture.name == name);
        }

        public override void OnInitializeMelon()
        {
            try
            {
                if (instance == null)
                {
                    instance = this;
                }

                textures = new List<Texture2D>();
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

                BoneLib.Hooking.OnLevelInitialized += OnSceneInitialized;
            }
            catch (System.Exception e)
            {
                throw e;
            }
        }

        public override void OnUpdate()
        {
            if(gameManager != null)
            {
                gameManager.tickManager?.Update();
            }
        }

        public void OnSceneInitialized(LevelInfo info)
        {
            currentScene = info.title;

            gameManager = new GameManager();

            mapLevel = MapUtilities.currentLevel;

            MapUtilities.Initialize();
        }

        public override void OnSceneWasUnloaded(int buildIndex, string sceneName)
        {
            if(gameManager != null)
            {
                GameManager.insanity = 0;
                GameManager.rngValue = 0;
                GameManager.miscRng = 0;
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

                textures.Add(texture);
            }
        }
    }
}