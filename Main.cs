﻿using MelonLoader;

using NEP.Paranoia.Entities;
using NEP.Paranoia.Managers;
using NEP.Paranoia.Utilities;

using UnityEngine;
using UnityEngine.Video;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NEP.Paranoia
{
	public static class BuildInfo
	{
		public const string Name = "paranoia"; // Name of the Mod.  (MUST BE SET)
		public const string Description = "Stay away from there. Please."; // Description for the Mod.  (Set as null if none)
		public const string Author = "Not Enough Photons"; // Author of the Mod.  (MUST BE SET)
		public const string Company = null; // Company that made the Mod.  (Set as null if none)
		public const string Version = "3.0.0"; // Version of the Mod.  (MUST BE SET)
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
		internal List<AudioClip> teleporterAmbience;
		internal List<AudioClip> paralyzerAmbience;
		internal List<AudioClip> watcherAmbience;
		internal List<AudioClip> doorIdleSounds;
		internal List<AudioClip> doorOpenSounds;
		internal List<AudioClip> darkVoices;
		internal List<AudioClip> radioTunes;

		internal List<VideoClip> videoClips;

		internal AudioClip startingTune;

		internal GameObject shadowPersonObject;
		internal GameObject staringManObject;
		internal GameObject ceilingManObject;
		internal GameObject observerObject;
		internal GameObject teleportingEntityObject;
		internal GameObject paralyzingEntityObject;
		internal GameObject radioObject;
		internal GameObject monitorObject;
		internal GameObject cursedDoorObject;

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

				ParanoiaUtilities utils = new ParanoiaUtilities();

				ParanoiaUtilities.RegisterTypesInIL2CPP();

				if (!System.IO.Directory.Exists("UserData/paranoia"))
				{
					System.IO.Directory.CreateDirectory("UserData/paranoia");
				}

				bundle = AssetBundle.LoadFromFile("UserData/paranoia/paranoia.pack");

				source = bundle.LoadAsset("s").Cast<GameObject>().gameObject.GetComponent<AudioSource>();
				source.dopplerLevel = 0f;
				source.gameObject.hideFlags = HideFlags.DontUnloadUnusedAsset;

				shadowPersonObject = bundle.LoadAsset("ShadowPerson").Cast<GameObject>();
				shadowPersonObject.hideFlags = HideFlags.DontUnloadUnusedAsset;

				staringManObject = bundle.LoadAsset("StaringMan").Cast<GameObject>();
				staringManObject.hideFlags = HideFlags.DontUnloadUnusedAsset;

				ceilingManObject = bundle.LoadAsset("CeilingMan").Cast<GameObject>();
				ceilingManObject.hideFlags = HideFlags.DontUnloadUnusedAsset;

				observerObject = bundle.LoadAsset("Observer").Cast<GameObject>();
				observerObject.hideFlags = HideFlags.DontUnloadUnusedAsset;

				teleportingEntityObject = bundle.LoadAsset("GrayMan").Cast<GameObject>();
				teleportingEntityObject.hideFlags = HideFlags.DontUnloadUnusedAsset;

				paralyzingEntityObject = bundle.LoadAsset("Paralyzer").Cast<GameObject>();
				paralyzingEntityObject.hideFlags = HideFlags.DontUnloadUnusedAsset;

				radioObject = bundle.LoadAsset("PRadio").Cast<GameObject>();
				radioObject.hideFlags = HideFlags.DontUnloadUnusedAsset;

				monitorObject = bundle.LoadAsset("MonitorPlayer").Cast<GameObject>();
				monitorObject.hideFlags = HideFlags.DontUnloadUnusedAsset;

				cursedDoorObject = bundle.LoadAsset("CursedDoor").Cast<GameObject>();
				cursedDoorObject.hideFlags = HideFlags.DontUnloadUnusedAsset;

				ParanoiaUtilities.FixObjectShader(radioObject);
				//FixObjectShader(monitorObject);

				genericAmbience = new List<AudioClip>();
				screamAmbience = new List<AudioClip>();
				chaserAmbience = new List<AudioClip>();
				watcherAmbience = new List<AudioClip>();
				doorIdleSounds = new List<AudioClip>();
				doorOpenSounds = new List<AudioClip>();
				teleporterAmbience = new List<AudioClip>();
				paralyzerAmbience = new List<AudioClip>();
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

					GameObject.Find("MUSICMACHINE (1)").SetActive(false);
					GameObject.Find("AMMODISPENSER").SetActive(false);
					GameObject.Find("HEALTHMACHINE").SetActive(false);
					GameObject.Find("CUSTOMLIGHTMACHINE/LIGHTMACHINE").SetActive(false);
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

		internal void RegisterObject(GameObject bundleObject, string assetName)
		{
			bundleObject = bundle.LoadAsset(assetName).Cast<GameObject>();
			bundleObject.hideFlags = HideFlags.DontUnloadUnusedAsset;
		}

		internal void InitializeGameManager()
		{
			gameManager.radioObject = radioObject;
			gameManager.shadowMan = shadowPersonObject;
			gameManager.observer = observerObject;
			gameManager.staringMan = staringManObject;
			gameManager.ceilingWatcher = ceilingManObject;
			gameManager.teleportingEntity = teleportingEntityObject;
			gameManager.paralyzerEntity = paralyzingEntityObject;
			gameManager.monitorObject = monitorObject;
			gameManager.cursedDoorObject = cursedDoorObject;
			gameManager.clipList = videoClips;
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
                { "amb_dark_voice", darkVoices },
                { "amb_paralysis", paralyzerAmbience },
                { "door_idle", doorIdleSounds },
                { "door_open", doorOpenSounds },
                { "radio_tune", radioTunes }
			};

			foreach(List<AudioClip> list in directory.Values)
            {
				foreach(AudioClip clip in bundle.LoadAllAssets())
                {
					foreach(string value in nameLUT)
                    {
                        if (clip.name.StartsWith(value))
                        {
							directory[value].Add(clip);
                        }
                    }
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

	internal class CustomLoadScreenSupport
	{
		internal static void Initialize()
		{
			Assembly loadScreenAsm = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault((asm) => asm.GetName().Name == "CustomLoadScreens");
		}
	}
}