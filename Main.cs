using MelonLoader;

using UnityEngine;
using UnityEngine.Video;

using TMPro;

using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace NotEnoughPhotons.paranoia
{
    public static class BuildInfo
    {
        public const string Name = "paranoia"; // Name of the Mod.  (MUST BE SET)
        public const string Description = "Stay away from there."; // Description for the Mod.  (Set as null if none)
        public const string Author = "Not Enough Photons"; // Author of the Mod.  (MUST BE SET)
        public const string Company = null; // Company that made the Mod.  (Set as null if none)
        public const string Version = "2.0.0"; // Version of the Mod.  (MUST BE SET)
        public const string DownloadLink = null; // Download Link for the Mod.  (Set as null if none)
    }

    public class Paranoia : MelonMod
    {
        public static Paranoia instance;

        internal AssetBundle bundle;
        internal AudioSource source;

        internal ParanoiaGameManager gameManager;
        internal AudioManager audioManager;

        internal List<AudioClip> genericAmbience;
        internal List<AudioClip> screamAmbience;
        internal List<AudioClip> chaserAmbience;
        internal List<AudioClip> watcherAmbience;
        internal List<AudioClip> darkVoices;
        internal List<AudioClip> radioTunes;

        internal List<VideoClip> videoClips;

        internal AudioClip startingTune;

        internal GameObject shadowPersonObject;
        internal GameObject staringManObject;
        internal GameObject ceilingManObject;
        internal GameObject observerObject;
        internal GameObject radioObject;
        internal GameObject monitorObject;

        internal GameObject voiceOffset;

        internal Transform playerTrigger;

        internal Vector3[] spawnPoints = new Vector3[3]
        {
            new Vector3(-53.9f, 1f, -55.1f),
            new Vector3(-53.7f, 1f, 32.1f),
            new Vector3(52.1f, 1f, 54.4f)
        };

        internal SpawnCircle[] spawnCircles = new SpawnCircle[3];

        internal SpawnCircle ceilingManSpawnCircle;

        internal UnhollowerBaseLib.Il2CppReferenceArray<LightmapData> lightmaps;
        internal UnhollowerBaseLib.Il2CppStructArray<UnityEngine.Rendering.SphericalHarmonicsL2> bakedProbes;

        internal bool isBlankBox;

        internal bool firstRadioSpawn = false;

        internal bool isDark = false;

        internal int rng = 1;

        public override void OnApplicationStart()
        {
            try
            {
                if (instance == null)
                {
                    instance = this;
                }

                RegisterTypesInIL2CPP();

                bundle = AssetBundle.LoadFromFile("UserData/paranoia/paranoia.pack");

                source = bundle.LoadAsset("s").Cast<GameObject>().gameObject.GetComponent<AudioSource>();
                source.dopplerLevel = 0f;
                source.gameObject.hideFlags = HideFlags.DontUnloadUnusedAsset;

                shadowPersonObject = bundle.LoadAsset("ShadowPerson").Cast<GameObject>();
                staringManObject = bundle.LoadAsset("StaringMan").Cast<GameObject>();
                ceilingManObject = bundle.LoadAsset("CeilingMan").Cast<GameObject>();
                observerObject = bundle.LoadAsset("Observer").Cast<GameObject>();
                radioObject = bundle.LoadAsset("PRadio").Cast<GameObject>();
                monitorObject = bundle.LoadAsset("MonitorPlayer").Cast<GameObject>();

                FixObjectShader(radioObject);
                //FixObjectShader(monitorObject);

                staringManObject.hideFlags = HideFlags.DontUnloadUnusedAsset;
                shadowPersonObject.hideFlags = HideFlags.DontUnloadUnusedAsset;
                ceilingManObject.hideFlags = HideFlags.DontUnloadUnusedAsset;
                radioObject.hideFlags = HideFlags.DontUnloadUnusedAsset;
                monitorObject.hideFlags = HideFlags.DontUnloadUnusedAsset;

                genericAmbience = new List<AudioClip>();
                screamAmbience = new List<AudioClip>();
                chaserAmbience = new List<AudioClip>();
                watcherAmbience = new List<AudioClip>();
                darkVoices = new List<AudioClip>();
                radioTunes = new List<AudioClip>();

                videoClips = new List<VideoClip>();
            }
            catch (System.Exception e)
            {
                throw e;
            }
        }

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            try
            {
                if (sceneName.ToLower() == "sandbox_blankbox")
                {
                    isBlankBox = true;

                    PrecacheAudioAssets();

                    PrecacheVideoAssets();

                    playerTrigger = ParanoiaUtilities.FindPlayer();

                    gameManager = new GameObject("Game Manager").AddComponent<ParanoiaGameManager>();

                    InitializeGameManager();

                    voiceOffset = new GameObject("Dark Voice Offset");

                    voiceOffset.transform.SetParent(playerTrigger.transform);
                    voiceOffset.transform.position = Vector3.zero;
                    voiceOffset.transform.localPosition = Vector3.forward * -2f;

                    audioManager = new GameObject("Audio Manager").AddComponent<AudioManager>();
                    ObjectPool pool = audioManager.gameObject.AddComponent<ObjectPool>();

                    audioManager.ambientGeneric = genericAmbience;
                    audioManager.ambientScreaming = screamAmbience;
                    audioManager.ambientChaser = chaserAmbience;
                    audioManager.ambientDarkVoices = darkVoices;
                    audioManager.radioTunes = radioTunes;
                    audioManager.startingTune = startingTune;

                    audioManager.pool = pool;

                    pool.prefab = source.gameObject;
                    pool.poolSize = 10;

                    GameObject.Find("MUSICMACHINE (1)/Headset_MUSIC/MusicPlayer").SetActive(false);
                    GameObject.Find("Decal_SafeGrav").SetActive(false);

                    lightmaps = LightmapSettings.lightmaps;
                    bakedProbes = LightmapSettings.lightProbes.bakedProbes;
                }
                else
                {
                    isBlankBox = false;
                }
            }
            catch (System.Exception e)
            {
                throw e;
            }
        }

        internal void InitializeGameManager()
        {
            gameManager.radioObject = radioObject;
            gameManager.shadowMan = shadowPersonObject;
            gameManager.observer = observerObject;
            gameManager.staringMan = staringManObject;
            gameManager.ceilingWatcher = ceilingManObject;
            gameManager.monitorObject = monitorObject;
            gameManager.clipList = videoClips;
        }

        internal static void FixObjectShader(GameObject obj)
        {
            if (obj != null)
            {
                Shader valveShader = Shader.Find("Valve/vr_standard");

                foreach (SkinnedMeshRenderer smr in obj.GetComponentsInChildren<SkinnedMeshRenderer>())
                {
                    try
                    {
                        foreach (Material m in smr.sharedMaterials)
                            m.shader = valveShader;
                    }
                    catch
                    {
                        continue;
                    }
                }

                foreach (MeshRenderer smr in obj.GetComponentsInChildren<MeshRenderer>())
                {
                    try
                    {
                        foreach (Material m in smr.sharedMaterials)
                            m.shader = valveShader;
                    }
                    catch
                    {
                        continue;
                    }
                }
            }
        }

        private void RegisterTypesInIL2CPP()
        {
            UnhollowerRuntimeLib.ClassInjector.RegisterTypeInIl2Cpp<AudioManager>();
            UnhollowerRuntimeLib.ClassInjector.RegisterTypeInIl2Cpp<ObjectPool>();
            UnhollowerRuntimeLib.ClassInjector.RegisterTypeInIl2Cpp<Hallucination>();
            UnhollowerRuntimeLib.ClassInjector.RegisterTypeInIl2Cpp<ParanoiaGameManager>();
            UnhollowerRuntimeLib.ClassInjector.RegisterTypeInIl2Cpp<MonitorVideo>();
            UnhollowerRuntimeLib.ClassInjector.RegisterTypeInIl2Cpp<PBillboard>();
        }

        private void PrecacheAudioAssets()
        {
            for (int i = 0; i < bundle.LoadAllAssets().Count; i++)
            {
                if (bundle.LoadAllAssets()[i].name.StartsWith("amb_generic"))
                {
                    genericAmbience.Add(bundle.LoadAllAssets()[i].Cast<AudioClip>());
                }
                else if (bundle.LoadAllAssets()[i].name.StartsWith("amb_scream"))
                {
                    screamAmbience.Add(bundle.LoadAllAssets()[i].Cast<AudioClip>());
                }
                else if (bundle.LoadAllAssets()[i].name.StartsWith("amb_chaser"))
                {
                    chaserAmbience.Add(bundle.LoadAllAssets()[i].Cast<AudioClip>());
                }
                else if (bundle.LoadAllAssets()[i].name.StartsWith("amb_watcher"))
				{
                    watcherAmbience.Add(bundle.LoadAllAssets()[i].Cast<AudioClip>());
                }
                else if (bundle.LoadAllAssets()[i].name.StartsWith("amb_dark_voice"))
                {
                    darkVoices.Add(bundle.LoadAllAssets()[i].Cast<AudioClip>());
                }
                else if (bundle.LoadAllAssets()[i].name.StartsWith("radio_tune"))
                {
                    radioTunes.Add(bundle.LoadAllAssets()[i].Cast<AudioClip>());
                }
                else if (bundle.LoadAllAssets()[i].name.StartsWith("radio_start"))
                {
                    startingTune = bundle.LoadAllAssets()[i].Cast<AudioClip>();
                }
            }
        }

        private void PrecacheVideoAssets()
        {
            for(int i = 0; i < bundle.LoadAllAssets().Count; i++)
            {
                if (bundle.LoadAllAssets()[i].name.StartsWith("video_screen"))
                {
                    videoClips.Add(bundle.LoadAllAssets()[i].Cast<VideoClip>());
                }
            }
        }
    }
}