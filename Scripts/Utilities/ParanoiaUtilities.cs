using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using NEP.Paranoia.Entities;
using NEP.Paranoia.Managers;

using StressLevelZero.AI;
using PuppetMasta;
using StressLevelZero.Rig;
using StressLevelZero.Props.Weapons;
using StressLevelZero.Interaction;

using UnhollowerBaseLib;
using UnhollowerRuntimeLib;

using UnityEngine;
using Object = UnityEngine.Object;

using Valve.VR;

namespace NEP.Paranoia.Utilities
{
    public class ParanoiaUtilities
    {
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

        /// <summary>
        /// Finds the GameWorld rig. 
        /// Useful for cloning the bones to make a fake copy of the player.
        /// </summary>
        /// <returns></returns>
        public static SkeletonRig GetGameWorldRig()
        {
            RigManager rigManager = ModThatIsNotMod.Player.GetRigManager().GetComponent<RigManager>();

            return rigManager.gameWorldSkeletonRig;
        }

        public static PhysicsRig GetPhysicsRig()
        {
            return UnityEngine.Object.FindObjectOfType<PhysicsRig>();
        }

        public static AIBrain[] FindAIBrains()
        {
            AIBrain[] result = Object.FindObjectsOfType<AIBrain>();
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

        public static Gun GetGunInHand(StressLevelZero.Handedness hand)
        {
            PhysicsRig physRig = GetPhysicsRig();

            Hand _hand = hand == StressLevelZero.Handedness.RIGHT ? physRig.rightHand : physRig.leftHand;

            if(_hand == null) { return null; }

            Gun gun = ModThatIsNotMod.Player.GetGunInHand(_hand);

            if(gun == null) { return null; }

            return gun;
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

        public static Tick GetTick(string name)
        {
            return ParanoiaGameManager.instance.GetType().GetField(name, System.Reflection.BindingFlags.NonPublic).GetValue(nameof(name)) as Tick;
        }

        public static DateTime CalculateTargetTimeDifference(DateTime targetTime)
        {
            TimeSpan difference = DateTime.Now - targetTime;

            return new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, difference.Hours, difference.Minutes, difference.Seconds);
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
            ClassInjector.RegisterTypeInIl2Cpp<StaringMan>();
            ClassInjector.RegisterTypeInIl2Cpp<TeleportingEntity>();
        }
    }

    public class ParanoiaMapUtilities
    {
        public static Il2CppReferenceArray<LightmapData> lightmaps;
        public static Il2CppStructArray<UnityEngine.Rendering.SphericalHarmonicsL2> bakedProbes;
        public static GameObject[] staticPlaneObjects;
        public static GameObject staticCeiling;
        public static GameObject mainLight;
        public static VLB.VolumetricLightBeam[] lightBeams;

        public static Material[] staticPlaneMaterials;
        public static float[] staticPlaneCubeMapScalars;

        public static void Initialize()
        {
            lightmaps = LightmapSettings.lightmaps;
            bakedProbes = LightmapSettings.lightProbes.bakedProbes;
            staticPlaneObjects = FindGameObjectsWithLayer("Static");
            staticPlaneMaterials = CacheMaterialsFromPlanes(staticPlaneObjects);
            staticPlaneCubeMapScalars = CacheCubeMapScalars(staticPlaneMaterials);
            staticCeiling = GameObject.Find("------STATICENV------");
            mainLight = GameObject.Find("REALTIMELIGHT");
            lightBeams = UnityEngine.Object.FindObjectsOfType<VLB.VolumetricLightBeam>();
        }

        /// <summary>
        /// Finds a group of game objects on a given layer.
        /// </summary>
        /// <param name="layerName"></param>
        /// <returns></returns>
        private static GameObject[] FindGameObjectsWithLayer(string layerName)
        {
            GameObject[] objectsInScene = GameObject.FindObjectsOfType<GameObject>();
            List<GameObject> layerObjects = new List<GameObject>();

            for (int i = 0; i < objectsInScene.Length; i++)
            {
                if (objectsInScene[i].layer == LayerMask.NameToLayer("Static"))
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
}
