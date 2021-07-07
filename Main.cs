using MelonLoader;

using UnityEngine;

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
        public class Tick
        {
            [System.Flags]
            public enum TickType
            {
                TT_LIGHT = 1,
                TT_DARK = 2
            }

            public Tick(float maxTick, TickType tickType)
            {
                this.maxTick = maxTick;
                this.tickType = tickType;

                if (tickType.HasFlag(TickType.TT_LIGHT))
                {
                    instance.ticks.Add(this);
                }
                else if (tickType == TickType.TT_DARK)
                {
                    instance.darkTicks.Add(this);
                }
            }

            internal float tick = 0f;
            internal float maxTick = 5f;

            internal TickType tickType;

            internal System.Action OnTick;

            internal void Update()
            {
                tick += Time.deltaTime;

                if (tick >= maxTick)
                {
                    OnTick?.Invoke();
                    tick = 0f;
                }
            }
        }

        public class SpawnCircle
        {
            public SpawnCircle(Transform originTransform)
            {
                this.originTransform = originTransform;
            }

            public Vector3 circle;

            public Transform originTransform;

            public float radius = 75f;

            private const float Deg2Rad = 0.0174532924f;

            public Vector3 CalculatePlayerCircle(float angle)
            {
                // y position is 1 meter since we need the shadow beings to be on the ground directly
                return new Vector3(
                    originTransform.position.x + Mathf.Sin(angle * Deg2Rad) * radius,
                    originTransform.position.y,
                    originTransform.position.z + Mathf.Cos(angle * Deg2Rad) * radius);
            }

            public Vector3 CalculatePlayerCircle(float angle, float radius)
            {
                // y position is 1 meter since we need the shadow beings to be on the ground directly
                return new Vector3(
                    originTransform.position.x + Mathf.Sin(angle * Deg2Rad) * radius,
                    originTransform.position.y,
                    originTransform.position.z + Mathf.Cos(angle * Deg2Rad) * radius);
            }
        }

        public static Paranoia instance;

        internal AssetBundle bundle;
        internal AudioSource source;

        internal AudioManager audioManager;

        internal List<Tick> ticks;
        internal List<Tick> darkTicks;

        internal List<AudioClip> genericAmbience;
        internal List<AudioClip> screamAmbience;
        internal List<AudioClip> chaserAmbience;
        internal List<AudioClip> watcherAmbience;
        internal List<AudioClip> darkVoices;
        internal List<AudioClip> radioTunes;

        internal AudioClip startingTune;

        internal GameObject shadowPersonObject;
        internal GameObject staringManObject;
        internal GameObject ceilingManObject;
        internal GameObject radioObject;

        internal VLB.VolumetricLightBeam lightBeam;
        internal Light blankBoxLight;
        internal GameObject flashlightObject;

        internal ShadowPerson shadowPerson;
        internal ChaserMirage mirage;
        internal ShadowPersonChaser shadowPersonChaser;

        internal Hallucination hChaserMirage;
        internal Hallucination hShadowPersonChaser;
        internal Hallucination hShadowPerson;
        internal Hallucination hStaringMan;
        internal Hallucination hCeilingMan;
        internal Hallucination hDarkVoice;

        internal GameObject radioClone;

        internal GameObject voiceOffset;

        internal AudioSource radioSource;

        internal Transform playerTrigger;

        internal SpawnCircle playerCircle;

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

        // Light/Dark ticks
        private Tick audioTick;
        private Tick shadowPersonTick;
        private Tick tPoseTick;
        private Tick chaserTick;
        private Tick staringManTick;
        private Tick radioTick;
        private Tick firstTimeRadioTick;
        private Tick aiToOriginTick;
        private Tick aiKillTick;
        private Tick dropItemTick;
        private Tick rngGeneratorTick;

        // Dark ticks only
        private Tick dVoiceMirageTick;

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
                radioObject = bundle.LoadAsset("PRadio").Cast<GameObject>();

                FixObjectShader(radioObject);

                staringManObject.hideFlags = HideFlags.DontUnloadUnusedAsset;
                shadowPersonObject.hideFlags = HideFlags.DontUnloadUnusedAsset;
                ceilingManObject.hideFlags = HideFlags.DontUnloadUnusedAsset;
                radioObject.hideFlags = HideFlags.DontUnloadUnusedAsset;

                genericAmbience = new List<AudioClip>();
                screamAmbience = new List<AudioClip>();
                chaserAmbience = new List<AudioClip>();
                watcherAmbience = new List<AudioClip>();
                darkVoices = new List<AudioClip>();
                radioTunes = new List<AudioClip>();

                CustomRadioModifications.Initialize();

                InitializeTicks();
            }
            catch (System.Exception e)
            {
                throw e;
            }
        }

        public override void OnApplicationQuit()
        {
            Cleanup();
        }

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            try
            {
                if (sceneName.ToLower() == "sandbox_blankbox")
                {
                    isBlankBox = true;

                    blankBoxLight = GameObject.FindObjectOfType<CustomLightMachine>().light;
                    lightBeam = GameObject.FindObjectOfType<CustomLightMachine>().lightBeam;

                    playerTrigger = FindPlayer();

                    voiceOffset = new GameObject("Dark Voice Offset");

                    voiceOffset.transform.SetParent(playerTrigger.transform);
                    voiceOffset.transform.position = Vector3.zero;
                    voiceOffset.transform.localPosition = Vector3.forward * -2f;

                    audioManager = new GameObject("Manager").AddComponent<AudioManager>();
                    ObjectPool pool = audioManager.gameObject.AddComponent<ObjectPool>();

                    PrecacheAudioAssets();

                    audioManager.ambientGeneric = genericAmbience;
                    audioManager.ambientScreaming = screamAmbience;
                    audioManager.ambientChaser = chaserAmbience;
                    audioManager.ambientDarkVoices = darkVoices;

                    audioManager.pool = pool;

                    pool.prefab = source.gameObject;
                    pool.poolSize = 10;

                    GameObject.Find("MUSICMACHINE (1)/Headset_MUSIC/MusicPlayer").SetActive(false);
                    GameObject.Find("Decal_SafeGrav").SetActive(false);

                    //ChangeClipboardText();

                    InitializeEntities();

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

        #region Events/Routines

        internal void TPoseEvent()
        {
            tPoseTick.maxTick = Random.Range(rng, 150);
            bool isRareNumber = rng >= 20 && rng <= 29 || rng >= 25 && rng <= 30;

            if (isRareNumber)
            {
                if (playerTrigger != null)
                {
                    try
                    {
                        UnhollowerBaseLib.Il2CppArrayBase<StressLevelZero.AI.AIBrain> brains = GameObject.FindObjectsOfType<StressLevelZero.AI.AIBrain>();

                        foreach (StressLevelZero.AI.AIBrain brain in brains)
                        {
                            Transform t = brain.transform;

                            if (t.gameObject != null)
                            {
                                if (t.Find("Physics") && t.Find("AiRig"))
                                {
                                    Transform physicsGrp = t.Find("Physics");
                                    Transform aiGrp = t.Find("AiRig");

                                    physicsGrp.gameObject.SetActive(false);
                                    aiGrp.gameObject.SetActive(false);

                                    t.GetComponent<StressLevelZero.Combat.VisualDamageController>().enabled = false;
                                    t.GetComponent<StressLevelZero.AI.AIBrain>().enabled = false;
                                    t.GetComponent<Arena_EnemyReference>().enabled = false;

                                    if (t.Find("Physics/Root_M/Spine_M/Chest_M/Head_M/Jaw_M/Head_JawEndSHJnt") != null)
                                    {
                                        Material eyeMat = t.Find("brettEnemy@neutral/geoGrp/brett_face").GetComponent<SkinnedMeshRenderer>().materials.FirstOrDefault((mat) => mat.name.Contains("mat_Brett_eye"));
                                        eyeMat.color = new Color(0f, 0f, 0f, 0f);
                                        Transform jaw = t.Find("Physics/Root_M/Spine_M/Chest_M/Head_M/Jaw_M/Head_JawEndSHJnt");
                                        jaw.localPosition = new Vector3(jaw.localPosition.x, -0.35f, jaw.localPosition.z);
                                    }

                                    t.localPosition = new Vector3(t.localPosition.x, 0f, t.localPosition.z);

                                    Vector3 lookRotation = Quaternion.LookRotation(FindPlayer().forward).eulerAngles;
                                    t.eulerAngles = new Vector3(t.eulerAngles.x, lookRotation.y, t.eulerAngles.z);
                                }
                            }
                        }

                        MelonCoroutines.Start(CoResetTPosedEnemies(5f));
                    }
                    catch
                    {

                    }

                }
            }
            else
            {
                return;
            }
        }

        internal void SpawnFirstRadio()
        {
            radioClone.SetActive(false);

            radioClone.transform.position = playerCircle.CalculatePlayerCircle(Random.Range(0, 360), 10f);

            radioClone.transform.LookAt(FindPlayer());

            radioSource.clip = startingTune;
            radioSource.spatialBlend = 0.75f;

            firstRadioSpawn = true;

            radioClone.SetActive(true);

            MelonCoroutines.Start(CoRadioHide(radioSource.clip.length));

            firstTimeRadioTick.OnTick -= SpawnFirstRadio;
        }

        internal void SpawnRadio()
        {
            if (firstRadioSpawn) { return; }

            radioClone.SetActive(false);

            radioClone.transform.position = playerCircle.CalculatePlayerCircle(Random.Range(0, 360), 10f);

            radioClone.transform.LookAt(FindPlayer());

            radioSource.clip = radioTunes[Random.Range(0, radioTunes.Count)];
            radioSource.spatialBlend = 1f;

            radioClone.SetActive(true);

            MelonCoroutines.Start(CoRadioHide(radioSource.clip.length));
        }

        internal void MoveAIToPoint(Vector3 point, bool fallThroughWorld)
        {
            try
            {
                UnhollowerBaseLib.Il2CppArrayBase<StressLevelZero.AI.AIBrain> brains = GameObject.FindObjectsOfType<StressLevelZero.AI.AIBrain>();

                foreach (StressLevelZero.AI.AIBrain brain in brains)
                {
                    Transform ai = brain.transform;
                    MelonCoroutines.Start(CoMoveAIToPoint(ai, point, fallThroughWorld));
                }
            }
            catch
            {

            }
        }

        internal void MoveAIToPlayer()
        {
            try
            {
                if (GameObject.FindObjectsOfType<StressLevelZero.AI.AIBrain>() != null)
                {
                    UnhollowerBaseLib.Il2CppArrayBase<StressLevelZero.AI.AIBrain> brains = GameObject.FindObjectsOfType<StressLevelZero.AI.AIBrain>();

                    foreach (StressLevelZero.AI.AIBrain brain in brains)
                    {
                        Transform t = brain.transform;
                        MelonCoroutines.Start(CoMoveAIToPlayer(t));
                    }
                }
            }
            catch
            {

            }
        }

        internal void KillAIRandomly()
        {
            try
            {
                if (GameObject.FindObjectsOfType<StressLevelZero.AI.AIBrain>() != null)
                {
                    UnhollowerBaseLib.Il2CppArrayBase<StressLevelZero.AI.AIBrain> brains = GameObject.FindObjectsOfType<StressLevelZero.AI.AIBrain>();

                    foreach (StressLevelZero.AI.AIBrain brain in brains)
                    {
                        Transform ai = brain.transform;
                        if (ai.GetComponentInParent<StressLevelZero.AI.AIBrain>() != null)
                        {
                            StressLevelZero.AI.AIBrain parent = ai.GetComponentInParent<StressLevelZero.AI.AIBrain>();

                            PuppetMasta.PuppetMaster puppetMaster = parent.GetComponentInChildren<PuppetMasta.PuppetMaster>();
                            PuppetMasta.SubBehaviourHealth hp = parent.GetComponentInChildren<PuppetMasta.BehaviourBaseNav>().health;
                            hp.Kill();
                            puppetMaster.Kill();

                            aiKillTick.maxTick = Random.Range(180f, 280f);
                        }
                    }
                }
            }
            catch
            {

            }
        }

        internal void KillNimbus()
        {
            if (GameObject.FindObjectOfType<StressLevelZero.Props.Weapons.FlyingGun>())
            {
                StressLevelZero.Props.Weapons.FlyingGun nimbus = GameObject.FindObjectOfType<StressLevelZero.Props.Weapons.FlyingGun>();

                nimbus.triggerGrip.ForceDetach();
            }
        }

        internal void KillWasp()
        {
            if (GameObject.FindObjectOfType<PuppetMasta.BehaviourHovercraft>())
            {
                PuppetMasta.BehaviourHovercraft wasp = GameObject.FindObjectOfType<PuppetMasta.BehaviourHovercraft>();
                wasp.KillStart();
            }
        }

        internal void DropHeadItem()
        {
            StressLevelZero.Props.Weapons.HandWeaponSlotReciever slot = GameObject.Find("[RigManager (Default Brett)]/[SkeletonRig (GameWorld Brett)]/Body/skull/Head/HeadSlotContainer/WeaponReciever").GetComponent<StressLevelZero.Props.Weapons.HandWeaponSlotReciever>();

            if (slot.m_WeaponHost != null)
            {
                slot.GetHost().DisableColliders();
                slot.DropWeapon();
            }
        }

        internal void ChangeClipboardText()
        {
            TextMeshPro tmp = GameObject.Find("prop_clipboard_MuseumBasement/TMP").GetComponent<TextMeshPro>();

            tmp.text = "TURN AROUND. NOT IN GAME.";
            tmp.old_text = "TURN AROUND. NOT IN GAME.";
            tmp.m_text = tmp.text;
        }

        internal IEnumerator CoLightFlickerRoutine(int iterations)
        {
            int i = 0;
            bool isOn = false;
            float random = 0f;

            if (GameObject.FindObjectOfType<PropFlashlight>() != null)
            {
                flashlightObject = GameObject.FindObjectOfType<PropFlashlight>().gameObject;
            }

            for (i = 0; i < iterations; i++)
            {
                yield return new WaitForSeconds(0.10f);

                random = Random.Range(1, iterations);

                if (blankBoxLight != null || lightBeam != null)
                {
                    if ((i * random / 2) % 2 == 0)
                    {
                        isOn = false;
                        blankBoxLight.gameObject.SetActive(false);
                        lightBeam.gameObject.SetActive(false);
                        LightmapSettings.lightmaps = new LightmapData[0];
                        LightmapSettings.lightProbes.bakedProbes = null;
                    }
                    else
                    {
                        isOn = true;
                        blankBoxLight.gameObject.SetActive(true);
                        lightBeam.gameObject.SetActive(true);
                        LightmapSettings.lightmaps = lightmaps;
                        LightmapSettings.lightProbes.bakedProbes = new UnhollowerBaseLib.Il2CppStructArray<UnityEngine.Rendering.SphericalHarmonicsL2>(bakedProbes.Count);
                        LightmapSettings.lightProbes.bakedProbes = bakedProbes;
                    }

                    DynamicGI.UpdateEnvironment();
                }

                if (flashlightObject != null && flashlightObject.GetComponent<PropFlashlight>() != null)
                {
                    if ((i * random / 2) % 2 == 0)
                    {
                        flashlightObject?.GetComponent<PropFlashlight>().LightStuff.SetActive(false);
                    }
                    else
                    {
                        flashlightObject?.GetComponent<PropFlashlight>().LightStuff.SetActive(true);
                    }
                }
            }

            if (!isOn)
            {
                isDark = true;
            }

            yield return null;
        }

        internal IEnumerator CoRadioHide(float time)
        {
            yield return new WaitForSeconds(time);

            firstRadioSpawn = false;

            if (radioClone.activeInHierarchy)
            {
                radioClone.SetActive(false);
            }
        }

        internal IEnumerator CoResetTPosedEnemies(float seconds)
        {
            yield return new WaitForSeconds(seconds);

            UnhollowerBaseLib.Il2CppArrayBase<StressLevelZero.AI.AIBrain> brains = GameObject.FindObjectsOfType<StressLevelZero.AI.AIBrain>();

            for (int i = 0; i < brains.Count; i++)
            {
                Transform t = brains[i].transform;
                Transform physicsGrp = t.Find("Physics");
                Transform aiGrp = t.Find("AiRig");

                physicsGrp.gameObject.SetActive(true);
                aiGrp.gameObject.SetActive(true);

                t.GetComponent<StressLevelZero.Combat.VisualDamageController>().enabled = true;
                t.GetComponent<StressLevelZero.AI.AIBrain>().enabled = true;
                t.GetComponent<Arena_EnemyReference>().enabled = true;

                if (t.Find("Physics/Root_M/Spine_M/Chest_M/Head_M/Jaw_M/Head_JawEndSHJnt") != null)
                {
                    Material eyeMat = t.Find("brettEnemy@neutral/geoGrp/brett_face").GetComponent<SkinnedMeshRenderer>().materials.FirstOrDefault((mat) => mat.name.Contains("mat_Brett_eye"));
                    Transform jaw = t.Find("Physics/Root_M/Spine_M/Chest_M/Head_M/Jaw_M/Head_JawEndSHJnt");
                    jaw.localPosition = new Vector3(jaw.localPosition.x, -0.044f, jaw.localPosition.z);
                }

                t.gameObject.SetActive(false);
            }
        }

        internal IEnumerator CoMoveAIToPoint(Transform ai, Vector3 point, bool fallThroughWorld)
        {
            if (ai != null)
            {
                if (ai.GetComponentInParent<StressLevelZero.AI.AIBrain>() != null)
                {
                    Transform parent = ai.GetComponentInParent<StressLevelZero.AI.AIBrain>().transform;

                    if (parent.GetComponentInChildren<PuppetMasta.BehaviourBaseNav>())
                    {
                        PuppetMasta.BehaviourBaseNav baseNav = parent.GetComponentInChildren<PuppetMasta.BehaviourBaseNav>();

                        baseNav.sensors.hearingSensitivity = 0f;
                        baseNav.sensors.visionFov = 0f;
                        baseNav.sensors._visionSphere.enabled = false;
                        baseNav.breakAgroHomeDistance = 0f;

                        if (baseNav.mentalState != PuppetMasta.BehaviourBaseNav.MentalState.Rest)
                        {
                            baseNav.SwitchMentalState(PuppetMasta.BehaviourBaseNav.MentalState.Rest);
                        }

                        baseNav.SetHomePosition(point, true);

                        while (Vector3.Distance(baseNav.transform.position, point) > 2f) { yield return null; }

                        if (fallThroughWorld)
                        {
                            foreach (Collider c in ai.GetComponentsInChildren<Collider>())
                            {
                                c.enabled = false;
                            }

                            yield return new WaitForSeconds(1f);

                            foreach (Collider c in ai.GetComponentsInChildren<Collider>())
                            {
                                c.enabled = true;
                            }

                            ai.gameObject.SetActive(false);
                        }

                        yield return null;
                    }
                }
                else
                {
                    yield return null;
                }
            }
        }

        internal IEnumerator CoMoveAIToPlayer(Transform ai)
        {
            if (playerTrigger != null)
            {
                if (ai.GetComponentInParent<StressLevelZero.AI.AIBrain>() != null)
                {
                    Transform parent = ai.GetComponentInParent<StressLevelZero.AI.AIBrain>().transform;
                    PuppetMasta.BehaviourBaseNav baseNav = parent.GetComponentInChildren<PuppetMasta.BehaviourBaseNav>();

                    baseNav.sensors.hearingSensitivity = 0f;
                    baseNav.sensors.visionFov = 0f;
                    baseNav.sensors._visionSphere.enabled = false;
                    baseNav.breakAgroHomeDistance = 0f;

                    if (baseNav.mentalState != PuppetMasta.BehaviourBaseNav.MentalState.Rest)
                    {
                        baseNav.SwitchMentalState(PuppetMasta.BehaviourBaseNav.MentalState.Rest);
                    }

                    baseNav.SetHomePosition(FindPlayer().position, true);

                    while (Vector3.Distance(baseNav.transform.position, FindPlayer().position) > 0.25f) { yield return null; }

                    yield return null;
                }
                else
                {
                    yield return null;
                }
            }
        }

        #endregion

        #region Utility

        internal Hallucination CreateHallucination(Vector3 position, Hallucination.HallucinationType hType, Hallucination.HallucinationFlags hFlags, Hallucination.HallucinationClass hClass, float distanceToDisappear, float chaseSpeed, float delayTime)
        {
            Hallucination hallucination = new GameObject($"{hType} {hClass} Hallucination").AddComponent<Hallucination>();
            SpriteBillboard billboard = hallucination.gameObject.AddComponent<SpriteBillboard>();

            billboard.target = instance.FindPlayer();
            hallucination.target = instance.FindPlayer();
            hallucination.cameraTarget = instance.FindPlayer();
            hallucination.audioManager = GameObject.FindObjectOfType<AudioManager>();

            if (hType.HasFlag(Hallucination.HallucinationType.Auditory))
            {
                AudioSource source = hallucination.gameObject.AddComponent<AudioSource>();
                hallucination.source = source;
                source.dopplerLevel = 0;
            }

            hallucination.transform.position = position;
            hallucination.gameObject.SetActive(false);
            hallucination.Initialize(hType, hFlags, hClass, distanceToDisappear, chaseSpeed, delayTime);
            return hallucination;
        }

        internal Hallucination CreateHallucination(Vector3 position, GameObject prefab, Hallucination.HallucinationType hType, Hallucination.HallucinationFlags hFlags, Hallucination.HallucinationClass hClass, float distanceToDisappear, float chaseSpeed, float delayTime)
        {
            Hallucination hallucination = new GameObject($"{hType} {hClass} Hallucination").AddComponent<Hallucination>();
            SpriteBillboard billboard = hallucination.gameObject.AddComponent<SpriteBillboard>();

            billboard.target = instance.FindPlayer();
            hallucination.target = instance.FindPlayer();
            hallucination.cameraTarget = instance.FindPlayer();
            hallucination.audioManager = GameObject.FindObjectOfType<AudioManager>();

            if (prefab != null)
            {
                GameObject.Instantiate(prefab, hallucination.transform);
            }

            if (hType.HasFlag(Hallucination.HallucinationType.Auditory))
            {
                AudioSource source = hallucination.gameObject.AddComponent<AudioSource>();
                hallucination.source = source;
                source.dopplerLevel = 0;
            }

            hallucination.gameObject.transform.position = position;
            hallucination.gameObject.SetActive(false);
            hallucination.Initialize(hType, hFlags, hClass, distanceToDisappear, chaseSpeed, delayTime);
            return hallucination;
        }

        private bool Is3AM()
        {
            // 24 hour time, for consistency!
            int currentHour = int.Parse(System.DateTime.Now.ToString("HH", System.Globalization.CultureInfo.InvariantCulture));

            // 3 AM
            if (currentHour == 03)
            {
                return true;
            }

            return false;
        }

        public Transform FindPlayer()
        {
            // Code lifted from the Boneworks Modding Toolkit.

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

        private void RegisterTypesInIL2CPP()
        {
            UnhollowerRuntimeLib.ClassInjector.RegisterTypeInIl2Cpp<AudioManager>();
            UnhollowerRuntimeLib.ClassInjector.RegisterTypeInIl2Cpp<ObjectPool>();
            UnhollowerRuntimeLib.ClassInjector.RegisterTypeInIl2Cpp<ChaserMirage>();
            UnhollowerRuntimeLib.ClassInjector.RegisterTypeInIl2Cpp<ShadowPerson>();
            UnhollowerRuntimeLib.ClassInjector.RegisterTypeInIl2Cpp<ShadowPersonChaser>();
            UnhollowerRuntimeLib.ClassInjector.RegisterTypeInIl2Cpp<Hallucination>();
            UnhollowerRuntimeLib.ClassInjector.RegisterTypeInIl2Cpp<SpriteBillboard>();
        }

        private void InitializeTicks()
        {
            ticks = new List<Tick>();
            darkTicks = new List<Tick>();

            audioTick = new Tick(60f, Tick.TickType.TT_LIGHT | Tick.TickType.TT_DARK);
            shadowPersonTick = new Tick(90f, Tick.TickType.TT_LIGHT | Tick.TickType.TT_DARK);
            tPoseTick = new Tick(120f, Tick.TickType.TT_LIGHT | Tick.TickType.TT_DARK);
            chaserTick = new Tick(90f, Tick.TickType.TT_LIGHT | Tick.TickType.TT_DARK);
            staringManTick = new Tick(180f, Tick.TickType.TT_LIGHT | Tick.TickType.TT_DARK);
            radioTick = new Tick(190f, Tick.TickType.TT_LIGHT | Tick.TickType.TT_DARK);
            firstTimeRadioTick = new Tick(30f, Tick.TickType.TT_LIGHT | Tick.TickType.TT_DARK);
            aiToOriginTick = new Tick(260f, Tick.TickType.TT_LIGHT | Tick.TickType.TT_DARK);
            aiKillTick = new Tick(240f, Tick.TickType.TT_LIGHT | Tick.TickType.TT_DARK);
            dropItemTick = new Tick(15f, Tick.TickType.TT_LIGHT | Tick.TickType.TT_DARK);
            rngGeneratorTick = new Tick(5f, Tick.TickType.TT_LIGHT | Tick.TickType.TT_DARK);

            Tick lightFlickerTick = new Tick(90f, Tick.TickType.TT_LIGHT | Tick.TickType.TT_DARK);
            lightFlickerTick.OnTick += new System.Action(() =>
            {
                KillNimbus();
                KillWasp();
                lightFlickerTick.maxTick = Random.Range(120f, 180f);
                MelonCoroutines.Start(CoLightFlickerRoutine(Random.Range(15, 25)));
            });

            Tick ceilingManTick = new Tick(30f, Tick.TickType.TT_DARK);

            dVoiceMirageTick = new Tick(30f, Tick.TickType.TT_DARK);

            audioTick.OnTick += AudioRoutine;
            shadowPersonTick.OnTick += SpawnShadowPerson;
            tPoseTick.OnTick += TPoseEvent;
            chaserTick.OnTick += SpawnChaserMirage;
            staringManTick.OnTick += SpawnStaringMan;
            radioTick.OnTick += new System.Action(() => { SpawnRadio(); MoveAIToPoint(radioClone.transform.position, false); });
            firstTimeRadioTick.OnTick += SpawnFirstRadio;
            aiToOriginTick.OnTick += new System.Action(() => MoveAIToPoint(Vector3.zero, true));
            aiKillTick.OnTick += KillAIRandomly;
            dropItemTick.OnTick += DropHeadItem;
            ceilingManTick.OnTick += SpawnCeilingMan;
            rngGeneratorTick.OnTick += new System.Action(() => rng = Random.Range(23, 150));

            dVoiceMirageTick.OnTick += SpawnDarkVoice;
        }

        private void Cleanup()
        {
            audioTick.OnTick -= AudioRoutine;
            shadowPersonTick.OnTick -= SpawnShadowPerson;
            tPoseTick.OnTick -= TPoseEvent;
            chaserTick.OnTick -= SpawnChaserMirage;
            staringManTick.OnTick -= SpawnStaringMan;
            radioTick.OnTick -= SpawnRadio;
            aiToOriginTick.OnTick -= new System.Action(() => { SpawnRadio(); MoveAIToPoint(radioClone.transform.position, false); });
            aiKillTick.OnTick -= KillAIRandomly;
            rngGeneratorTick.OnTick -= new System.Action(() => rng = Random.Range(23, 150));
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

        private void InitializeEntities()
        {
            try
            {
                mirage = new GameObject("Mirage").AddComponent<ChaserMirage>();
                mirage.gameObject.SetActive(false);

                hShadowPerson = CreateHallucination(Vector3.up, shadowPersonObject, Hallucination.HallucinationType.Visual, Hallucination.HallucinationFlags.None, Hallucination.HallucinationClass.Watcher, 30f, 0f, 0f);
                MelonLogger.Msg($"Initialized {hShadowPerson.name}");
                hShadowPersonChaser = CreateHallucination(Vector3.up, shadowPersonObject, Hallucination.HallucinationType.Visual, Hallucination.HallucinationFlags.None, Hallucination.HallucinationClass.Chaser, 1f, 50f, 5f);
                MelonLogger.Msg($"Initialized {hShadowPersonChaser.name}");
                hCeilingMan = CreateHallucination(Vector3.up, ceilingManObject, Hallucination.HallucinationType.Auditory | Hallucination.HallucinationType.Visual, Hallucination.HallucinationFlags.HideWhenSeen, Hallucination.HallucinationClass.Watcher, 30f, 0f, 0f);
                MelonLogger.Msg($"Initialized {hCeilingMan.name}");
                hChaserMirage = CreateHallucination(Vector3.up, new GameObject("Mirage"), Hallucination.HallucinationType.Auditory, Hallucination.HallucinationFlags.None, Hallucination.HallucinationClass.Chaser, 1f, 50f, 0f);
                MelonLogger.Msg($"Initialized {hChaserMirage.name}");
                hStaringMan = CreateHallucination(Vector3.up, staringManObject, Hallucination.HallucinationType.Visual, Hallucination.HallucinationFlags.None, Hallucination.HallucinationClass.Chaser, 30f, 1f, 0f);
                MelonLogger.Msg($"Initialized {hStaringMan.name}");
                hDarkVoice = CreateHallucination(Vector3.up, new GameObject(), Hallucination.HallucinationType.Auditory, Hallucination.HallucinationFlags.None, Hallucination.HallucinationClass.DarkVoice, 0f, 0f, 0f);
                MelonLogger.Msg($"Initialized {hDarkVoice.name}");

                radioClone = GameObject.Instantiate(radioObject);

                radioSource = radioClone.GetComponentInChildren<AudioSource>();

                radioClone.SetActive(false);

                playerCircle = new SpawnCircle(FindPlayer());

                GameObject ceilingManSpawnPoint = new GameObject("Ceiling Man Spawn Point");
                ceilingManSpawnPoint.transform.position = Vector3.up * 75f;
                ceilingManSpawnCircle = new SpawnCircle(ceilingManSpawnPoint.transform);

                for (int i = 0; i < spawnCircles.Length; i++)
                {
                    spawnCircles[i] = new SpawnCircle(new GameObject("Spawn Circle").transform);
                    spawnCircles[i].radius = 100f;
                    spawnCircles[i].originTransform.position = spawnPoints[i];
                }
            }
            catch (System.Exception e)
            {
                MelonLogger.Msg(e);
            }
        }

        #endregion
    }

    internal class CustomRadioModifications
    {
        internal static string[] files;

        internal static void Initialize()
        {
            if (!HasCustomRadioDirectory() || !HasCustomRadios()) { return; }

            string[] fileNames = System.IO.Directory.GetFiles("UserData/CustomRadios");

            for (int i = 0; i < fileNames.Length; i++)
            {
                fileNames[i] = fileNames[i].Replace(@"UserData/CustomRadios\", string.Empty).Replace(".wav", string.Empty);
            }

            files = fileNames;
        }

        internal static void SwapCustomRadios()
        {
            if (files.Length == 0 || files.Length == -1) { return; }

            try
            {
                if (GameObject.FindObjectsOfType<AudioPlayer>() != null)
                {
                    foreach (AudioPlayer player in GameObject.FindObjectsOfType<AudioPlayer>())
                    {
                        foreach (string file in files)
                        {
                            if (player.source.clip != null || player.source != null)
                            {
                                if (player.source.clip.name.ToLower() == $"{file.ToLower()} radio")
                                {
                                    player.source.Stop();
                                }
                            }
                        }
                    }
                }
            }
            catch { }
        }

        internal static bool HasCustomRadioDirectory()
        {
            return System.IO.Directory.Exists("UserData/CustomRadios");
        }

        internal static bool HasCustomRadios()
        {
            return System.AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(asm => asm.GetName().Name == "CustomRadios") != null;
        }
    }
}