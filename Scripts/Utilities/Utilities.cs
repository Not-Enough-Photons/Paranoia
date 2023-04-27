using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

using NEP.Paranoia.Entities;
using NEP.Paranoia.Managers;

using SLZ.AI;
using SLZ.Rig;
using SLZ.Props.Weapons;
using SLZ.Interaction;
using SLZ.VRMK;

using PuppetMasta;

using RealisticEyeMovements;

using UnhollowerBaseLib;
using UnhollowerRuntimeLib;

using UnityEngine;
using UnityEngine.Audio;
using Object = UnityEngine.Object;

namespace NEP.Paranoia.ParanoiaUtilities
{
    public class Utilities
    {
        public static Assembly GetAssembly(string assemblyName)
        {
            return null;
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
            return BoneLib.Player.rigManager.GetComponentInChildren<TriggerRefProxy>();
        }

        public static BaseMirage GetMirage(string name)
        {
            BaseMirage mirage = GameManager.entities.Find((match) => match.name == name).GetComponent<BaseMirage>();
            return mirage;
        }

        public static AudioMixer GetAudioMixer()
        {
            return Object.FindObjectOfType<Audio_Manager>().audioMixer;
        }

        public static PhysicsRig GetPhysicsRig()
        {
            return UnityEngine.Object.FindObjectOfType<PhysicsRig>();
        }

        public static ControllerRig GetControllerRig()
        {
            return UnityEngine.Object.FindObjectOfType<SteamControllerRig>();
        }

        public static Il2CppArrayBase<AIBrain> FindAIBrains()
        {
            return Object.FindObjectsOfType<AIBrain>();
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

        public static Gun GetGunInHand(SLZ.Handedness hand)
        {
            PhysicsRig physRig = GetPhysicsRig();

            Hand _hand = hand == SLZ.Handedness.RIGHT ? physRig.rightHand : physRig.leftHand;

            if(_hand == null) { return null; }

            Gun gun = BoneLib.Player.GetGunInHand(_hand);

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

        public static Transform endRoomEyesSpawn;
        public static Transform endRoomPlayerSpawn;

        public static GameObject collectAllSign;

        public static FogSettings baseFog;
        public static FogSettings darkFog;

        public static Bounds blankboxBounds => GetLevelBounds();

        public static Material[] staticPlaneMaterials;
        public static float[] staticPlaneCubeMapScalars;

        private static int miscButtonPresses;
        private static int miscButtonRNG;
        private static bool miscButtonReachedEvent;
        private static bool miscRNGGenerated;

        public static void Initialize()
        {
            rendererMaterials = CacheAllRendererMaterials(Object.FindObjectsOfType<Renderer>());
            lightmaps = LightmapSettings.lightmaps;
            bakedProbes = LightmapSettings.lightProbes.bakedProbes;
            bakedProbes = LightmapSettings.lightProbes.bakedProbes;
            staticPlaneObjects = FindGameObjectsWithLayer("Static");
            staticPlaneMaterials = CacheMaterialsFromPlanes(staticPlaneObjects);
            staticPlaneCubeMapScalars = CacheCubeMapScalars(staticPlaneMaterials);
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

        public static void DisableCubemapScalars(Material[] materials)
        {
            foreach(Material mat in materials)
            {
                mat.SetFloat("g_flCubeMapScalar", 0f);
            }
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

    public class Patches
    {
        [HarmonyLib.HarmonyPatch(typeof(SLZ.Combat.Projectile))]
        [HarmonyLib.HarmonyPatch(nameof(SLZ.Combat.Projectile.Awake))]
        public static class OnProjectileCollision
        {
            public static void Postfix(SLZ.Combat.Projectile __instance)
            {
                __instance.onCollision.AddListener(new Action<Collider, Vector3, Vector3>((col, position, normal) =>
                {
                    Hit(col, position, normal);
                }));
            }

            private static void Hit(Collider collider, Vector3 positionWorld, Vector3 normal)
            {
                BaseMirage mirageHit = collider.GetComponentInParent<BaseMirage>();

                if (mirageHit != null)
                {
                    mirageHit.OnProjectileHit();
                }
            }
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
