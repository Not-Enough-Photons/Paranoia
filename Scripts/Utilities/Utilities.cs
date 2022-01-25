using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

using NEP.Paranoia.Entities;
using NEP.Paranoia.Managers;

using StressLevelZero.AI;
using StressLevelZero.Rig;
using StressLevelZero.Props.Weapons;
using StressLevelZero.Interaction;
using StressLevelZero.VRMK;

using PuppetMasta;

using TMPro;

using RealisticEyeMovements;

using UnhollowerBaseLib;
using UnhollowerRuntimeLib;

using UnityEngine;
using UnityEngine.Audio;
using Object = UnityEngine.Object;

using Valve.VR;

namespace NEP.Paranoia.ParanoiaUtilities
{
    public class Utilities
    {
        public static Assembly GetAssembly(string assemblyName)
        {
            return null;
        }

        /// <summary>
        /// Gets the current HMD that SteamVR is using.
        /// </summary>
        /// <returns>The headset in use.</returns>
        public static HMDType GetHMD()
        {
            HMDType hmdModel;

            switch (SteamVR.instance.hmd_ModelNumber)
            {
                case "Index":
                    hmdModel = HMDType.Index;
                    break;
                case "Vive":
                    hmdModel = HMDType.Vive;
                    break;
                case "VIVE_Pro MV":
                    hmdModel = HMDType.VivePro;
                    break;
                case "vive_cosmos":
                    hmdModel = HMDType.ViveCosmos;
                    break;
                case "Oculus Quest":
                    hmdModel = HMDType.Quest;
                    break;
                case "Oculus Rift CV1":
                    hmdModel = HMDType.Rift;
                    break;
                case "Oculus Rift S":
                    hmdModel = HMDType.RiftS;
                    break;
                default:
                    hmdModel = HMDType.Unknown;
                    break;
            }

            return hmdModel;
        }

        /// <summary>
        /// Finds the head of the player.
        /// </summary>
        /// <returns></returns>
        public static Transform FindHead()
        {
            return GameObject.Find("[RigManager (Default Brett)]/[PhysicsRig]/").transform;
        }

        /// <summary>
        /// Finds the player. Taken from the Boneworks Modding Toolkit.
        /// </summary>
        /// <returns></returns>
        public static Transform FindPlayer()
        {
            GameObject[] array = GameObject.FindGameObjectsWithTag("Player");

            for (int i = 0; i < array.Length; i++)
            {
                bool isTrigger = array[i].name == "PlayerTrigger";

                if (isTrigger)
                {
                    return array[i].transform;
                }
            }

            return null;
        }

        public static TriggerRefProxy GetPlayerProxy()
        {
            return ModThatIsNotMod.Player.GetRigManager().GetComponentInChildren<TriggerRefProxy>();
        }

        public static BaseHallucination GetHallucination(string name)
        {
            System.Type type = Paranoia.instance.gameManager.GetType();
            FieldInfo info = type.GetField(name);
            
            object obj = info?.GetValue(null);

            if(obj == null) { return null; }

            BaseHallucination hallucination = obj as BaseHallucination;

            return hallucination;
        }

        public static AudioMixer GetAudioMixer()
        {
            return Object.FindObjectOfType<Audio_Manager>().audioMixer;
        }

        public static GameWorldSkeletonRig ClonePlayerBody(Vector3 position, Quaternion rotation)
        {
            GameObject rig = Object.Instantiate(GetGameWorldRig().gameObject, Vector3.up * 5f, Quaternion.identity);
            return rig.GetComponent<GameWorldSkeletonRig>();
        }

        public static GameObject GetRigManager()
        {
            return ModThatIsNotMod.Player.GetRigManager();
        }

        /// <summary>
        /// Finds the GameWorld rig. 
        /// Useful for cloning the bones to make a fake copy of the player.
        /// </summary>
        /// <returns></returns>
        public static GameWorldSkeletonRig GetGameWorldRig()
        {
            RigManager rigManager = ModThatIsNotMod.Player.GetRigManager().GetComponent<RigManager>();

            return rigManager.gameWorldSkeletonRig as GameWorldSkeletonRig;
        }

        public static PhysicsRig GetPhysicsRig()
        {
            return UnityEngine.Object.FindObjectOfType<PhysicsRig>();
        }

        public static ControllerRig GetControllerRig()
        {
            return UnityEngine.Object.FindObjectOfType<SteamControllerRig>();
        }

        public static AIBrain[] FindAIBrains()
        {
            AIBrain[] result = Object.FindObjectsOfType<AIBrain>();

            if (result == null || result.Length < 0)
            {
                return null;
            }

            return result;
        }

        public static AIBrain[] FindAIBrains(out BehaviourBaseNav[] navs)
        {
            AIBrain[] result = Object.FindObjectsOfType<AIBrain>();
            navs = FindBaseNavs(result);

            return result;
        }

        public static BehaviourBaseNav[] FindBaseNavs(AIBrain[] brains)
        {
            List<BehaviourBaseNav> baseNavs = new List<BehaviourBaseNav>();

            brains.ToList().ForEach((brain) =>
            {
                baseNavs.Add(brain?.behaviour);
            });

            return baseNavs.ToArray();
        }

        public static Rigidbody[] FindObjectsBehindHead(Transform head, string layer)
        {
            Rigidbody[] rbs = UnityEngine.Object.FindObjectsOfType<Rigidbody>();
            List<Rigidbody> objectList = new List<Rigidbody>();

            if(rbs == null) { return null; }

            foreach(Rigidbody rb in rbs)
            {
                if(Vector3.Dot(head.forward, rb.position) <= 1f)
                {
                    if (rb.gameObject.layer == LayerMask.NameToLayer(layer))
                    {
                        objectList.Add(rb);
                    }
                }
            }

            return objectList.ToArray();
        }

        public static LookTargetController[] GetLookAtControllers()
        {
            List<LookTargetController> lookTargets = new List<LookTargetController>();
            AIBrain[] brains = FindAIBrains();

            foreach(AIBrain brain in brains)
            {
                LookTargetController lookTarget = brain.GetComponentInChildren<LookTargetController>();
                lookTargets?.Add(lookTarget);
            }

            return lookTargets.ToArray();
        }

        public static bool CheckForSpawnPanel(UIRig rig)
        {
            if (rig == null) { return false; }

            return rig.popUpMenu.spawnablesPanelView.gameObject.active;
        }

        public static Gun GetGunInHand(StressLevelZero.Handedness hand)
        {
            PhysicsRig physRig = GetPhysicsRig();

            Hand _hand = hand == StressLevelZero.Handedness.RIGHT ? physRig.rightHand : physRig.leftHand;

            if(_hand == null) { return null; }

            Gun gun = ModThatIsNotMod.Player.GetGunInHand(_hand);

            if(gun == null) { return null; }

            return gun;
        }

        public static bool Verify()
        {
            string gamePath = MelonLoader.MelonUtils.GameDirectory;
            string cut = gamePath.Substring(2);

            return cut.Contains("BONEWORKS.v1.6") // Obvious pirated copy
                || !cut.Contains("steamapps")
                || !cut.Contains("Steam") // Steam checks
                || !cut.Contains("stress-level-zero-inc-boneworks"); // Oculus version
        }

        /// <summary>
        /// Checks if our system clock hour is equal to the hour we set.
        /// </summary>
        /// <param name="hour"></param>
        /// <returns></returns>
        public static bool IsTargetHour(int hour)
        {
            // 24 hour time, for consistency!
            int currentHour = GetSystemHour();

            // Is this not the right hour?
            if (currentHour != hour)
            {
                return false;
            }

            return true;
        }

        public static DateTime CalculateTargetTimeDifference(DateTime targetTime)
        {
            TimeSpan difference = DateTime.Now - targetTime;

            return new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, difference.Hours, difference.Minutes, difference.Seconds);
        }

        public static string GetParameterString(string method)
        {
            string[] split = method.Split('(', ')');

            return split[1];
        }

        public static string GetMethodNameString(string method)
        {
            string[] split = method.Split('(');
            return split[0];
        }

        /// <summary>
        /// Gets current system hour.
        /// </summary>
        /// <returns></returns>
        internal static int GetSystemHour()
        {
            return int.Parse(DateTime.Now.ToString("HH", CultureInfo.InvariantCulture));
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

        internal static void RegisterTypesInIL2CPP()
        {
            ClassInjector.RegisterTypeInIl2Cpp<AudioManager>();
            ClassInjector.RegisterTypeInIl2Cpp<BaseHallucination>();
            ClassInjector.RegisterTypeInIl2Cpp<AudioHallucination>();

            ClassInjector.RegisterTypeInIl2Cpp<Ambience>();
            ClassInjector.RegisterTypeInIl2Cpp<CursedDoorController>();
            ClassInjector.RegisterTypeInIl2Cpp<CeilingMan>();
            ClassInjector.RegisterTypeInIl2Cpp<Chaser>();
            ClassInjector.RegisterTypeInIl2Cpp<DarkVoice>();
            ClassInjector.RegisterTypeInIl2Cpp<SjasFace>();
            ClassInjector.RegisterTypeInIl2Cpp<CryingEntity>();
            ClassInjector.RegisterTypeInIl2Cpp<InvisibleForce>();
            ClassInjector.RegisterTypeInIl2Cpp<MonitorVideo>();
            ClassInjector.RegisterTypeInIl2Cpp<Observer>();
            ClassInjector.RegisterTypeInIl2Cpp<ObjectPool>();
            ClassInjector.RegisterTypeInIl2Cpp<ParanoiaGameManager>();
            ClassInjector.RegisterTypeInIl2Cpp<FordScaling>();
            ClassInjector.RegisterTypeInIl2Cpp<Paralyzer>();
            ClassInjector.RegisterTypeInIl2Cpp<Radio>();
            ClassInjector.RegisterTypeInIl2Cpp<ShadowPerson>();
            ClassInjector.RegisterTypeInIl2Cpp<ShadowPersonChaser>();
            ClassInjector.RegisterTypeInIl2Cpp<FastStaringMan>();
            ClassInjector.RegisterTypeInIl2Cpp<StaringMan>();
            ClassInjector.RegisterTypeInIl2Cpp<TeleportingEntity>();
        }
    }

    public class MapUtilities
    {
        public struct FogSettings
        {
            public float startDistance;
            public float endDistance;
            public float heightFogThickness;
            public float heightFogFalloff;
            public Color heightFogColor;
        }

        public static MapLevel currentLevel;

        public static Il2CppReferenceArray<LightmapData> lightmaps;
        public static Il2CppStructArray<UnityEngine.Rendering.SphericalHarmonicsL2> bakedProbes;
        public static GameObject[] staticPlaneObjects;
        public static GameObject staticCeiling;
        public static GameObject mainLight;
        public static Material[] rendererMaterials;
        public static VLB.VolumetricLightBeam[] lightBeams;
        public static ValveFog fog;

        public static Transform endRoomEyesSpawn;
        public static Transform endRoomPlayerSpawn;

        public static GameObject collectAllSign;

        public static FogSettings baseFog;
        public static FogSettings darkFog;

        public static Bounds blankboxBounds => GetLevelBounds();

        public static TextMeshPro clipboardText;

        public static Material[] staticPlaneMaterials;
        public static float[] staticPlaneCubeMapScalars;

        public static void Initialize()
        {
            rendererMaterials = CacheAllRendererMaterials(Object.FindObjectsOfType<Renderer>());
            lightmaps = LightmapSettings.lightmaps;
            bakedProbes = LightmapSettings.lightProbes.bakedProbes;
            staticPlaneObjects = FindGameObjectsWithLayer("Static");
            staticPlaneMaterials = CacheMaterialsFromPlanes(staticPlaneObjects);
            staticPlaneCubeMapScalars = CacheCubeMapScalars(staticPlaneMaterials);
            fog = Object.FindObjectOfType<ValveFog>();

            Transform endRoom = ParanoiaGameManager.endRoom.transform;
            endRoomEyesSpawn = endRoom.Find("SpawnPoint");
            endRoomPlayerSpawn = endRoom.Find("PlayerTeleportPoint");

            InitializeLevel(currentLevel);

            baseFog = new FogSettings()
            {
                startDistance = fog.startDistance,
                endDistance = fog.endDistance,
                heightFogThickness = fog.heightFogThickness,
                heightFogFalloff = fog.heightFogFalloff,
                heightFogColor = fog.heightFogColor
            };

            darkFog = new FogSettings()
            {
                startDistance = 1500f,
                endDistance = 5f,
                heightFogThickness = 1f,
                heightFogFalloff = 1.17f,
                heightFogColor = Color.black
            };
        }

        private static void InitializeLevel(MapLevel level)
        {
            switch (level)
            {
                case MapLevel.MuseumBasement:
                    clipboardText = GameObject.Find("prop_clipboard_MuseumBasement/TMP").GetComponent<TextMeshPro>();
                    GameObject.Find("MUSICMACHINE (1)").SetActive(false);
                    GameObject.Find("AMMODISPENSER").SetActive(false);
                    GameObject.Find("HEALTHMACHINE").SetActive(false);
                    GameObject.Find("Decal_SafeGrav").SetActive(false);
                    GameObject.Find("decal_playroom (1)").SetActive(false);
                    GameObject.Find("decal_playroom").SetActive(false);
                    GameObject.Find("holographic_sign_SandboxMuseum").SetActive(false);
                    GameObject.Find("DISPLAY_UNLOCKABLES").SetActive(false);
                    collectAllSign = GameObject.Find("holographic_sign_CollectThemAll");
                    break;
                case MapLevel.Blankbox:
                    GameObject.Find("MUSICMACHINE (1)").SetActive(false);
                    GameObject.Find("AMMODISPENSER").SetActive(false);
                    GameObject.Find("HEALTHMACHINE").SetActive(false);
                    GameObject.Find("CUSTOMLIGHTMACHINE/LIGHTMACHINE").SetActive(false);
                    GameObject.Find("Decal_SafeGrav").SetActive(false);

                    staticCeiling = GameObject.Find("------STATICENV------");
                    staticCeiling.SetActive(false);
                    mainLight = GameObject.Find("REALTIMELIGHT");
                    clipboardText = GameObject.Find("prop_clipboard_MuseumBasement/TMP").GetComponent<TextMeshPro>();
                    lightBeams = UnityEngine.Object.FindObjectsOfType<VLB.VolumetricLightBeam>();
                    break;
            }
        }

        public static void ChangeHoloSign(GameObject holoSign, Texture2D texture)
        {
            MeshRenderer renderer = holoSign.GetComponent<MeshRenderer>();

            if(renderer == null) { return; }

            Material rendererMaterial = renderer.material;
            rendererMaterial.mainTexture = texture;
        }

        public static Material[] CacheAllRendererMaterials(Renderer[] renderers)
        {
            List<Material> mats = new List<Material>();

            foreach(Renderer renderer in renderers)
            {
                mats?.Add(renderer.material);
            }

            return mats.ToArray();
        }

        public static void SwitchFog(FogSettings start, FogSettings end, float lerp, float maxTime)
        {
            MelonLoader.MelonCoroutines.Start(CoSwitchFog(start, end, lerp, maxTime));
        }

        private static IEnumerator CoSwitchFog(FogSettings start, FogSettings end, float lerp, float maxTime)
        {
            float time = 0f;

            while(time < maxTime)
            {
                time += Time.deltaTime;

                fog.startDistance = Mathf.MoveTowards(fog.startDistance, -end.startDistance, lerp * Time.deltaTime);
                fog.heightFogThickness = Mathf.MoveTowards(fog.heightFogThickness, end.heightFogThickness, (lerp * 0.00001f) * Time.deltaTime);
                fog.heightFogFalloff = Mathf.MoveTowards(fog.heightFogFalloff, end.heightFogFalloff, (lerp / 20f) * Time.deltaTime);
                fog.heightFogColor = Color.Lerp(fog.heightFogColor, end.heightFogColor, lerp * Time.deltaTime);

                fog.UpdateConstants();

                yield return null;
            }

            yield return null;
        }

        public static void DisableCubemapScalars(Material[] materials)
        {
            foreach(Material mat in materials)
            {
                mat.SetFloat("g_flCubeMapScalar", 0f);
            }
        }

        public static void SetClipboardText(string text)
        {
            clipboardText.text = text;
        }

        public static Bounds GetLevelBounds()
        {
            Vector3 offset = Vector3.up * 42f;
            Vector3 volume = new Vector3(163.2f, 84.3f, 163.67f);

            Bounds levelBounds = new Bounds(Vector3.up * 42f, volume / 2f);

            return levelBounds;
        }

        /// <summary>
        /// Finds a group of game objects on a given layer.
        /// </summary>
        /// <param name="layerName"></param>
        /// <returns></returns>
        public static GameObject[] FindGameObjectsWithLayer(string layerName)
        {
            GameObject[] objectsInScene = GameObject.FindObjectsOfType<GameObject>();
            List<GameObject> layerObjects = new List<GameObject>();

            for (int i = 0; i < objectsInScene.Length; i++)
            {
                if (objectsInScene[i].layer == LayerMask.NameToLayer(layerName))
                {
                    layerObjects?.Add(objectsInScene[i]);
                }
            }

            return layerObjects.ToArray();
        }

        private static Material[] CacheMaterialsFromPlanes(GameObject[] planeObjects)
        {
            List<Material> mats = new List<Material>();

            for (int i = 0; i < planeObjects.Length; i++)
            {
                if (planeObjects[i].GetComponent<Renderer>())
                {
                    Renderer renderer = planeObjects[i].GetComponent<Renderer>();
                    mats.Add(renderer.material);
                }
            }

            return mats.ToArray();
        }

        private static float[] CacheCubeMapScalars(Material[] materials)
        {
            List<float> scalars = new List<float>();

            for(int i = 0; i < materials.Length; i++)
            {
                scalars.Add(materials[i].GetFloat("g_flCubeMapScalar"));
            }

            return scalars.ToArray();
        }
    }

    public class EntanglementUtilities
    {
        public static void Initialize()
        {
            UnpackModules();
        }

        private static void UnpackModules()
        {
            Assembly assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault((asm) => asm.GetName().Name == "Entanglement");

            if(assembly == null) { return; }


        }
    }
}
