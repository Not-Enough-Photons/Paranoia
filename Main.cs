using MelonLoader;

using NEP.Paranoia.Entities;
using NEP.Paranoia.Managers;
using NEP.Paranoia.Utilities;

using UnityEngine;
using UnityEngine.Video;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using UnhollowerBaseLib;

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

		public AssetBundle bundle;
		
		public ParanoiaGameManager gameManager;
		public AudioManager audioManager;

		private Dictionary<string, GameObject> baseEntities;

		public List<AudioClip> genericAmbience;
		public List<AudioClip> screamAmbience;
		public List<AudioClip> chaserAmbience;
		public List<AudioClip> teleporterAmbience;
		public List<AudioClip> paralyzerAmbience;
		public List<AudioClip> deafenSounds;
		public List<AudioClip> grabSounds;
		public List<AudioClip> watcherAmbience;
		public List<AudioClip> doorIdleSounds;
		public List<AudioClip> doorOpenSounds;
		public List<AudioClip> darkVoices;
		public List<AudioClip> radioTunes;

		public List<VideoClip> videoClips;

		public AudioClip startingTune;

		public string[] supportedMaps = new string[]
		{
			"sandbox_blankbox",
			"sandbox_museumbasement"
		};

		public bool isBlankBox;

		public GameObject GetEntInDirectory(string name)
        {
			return baseEntities[name];
        }

		public override void OnApplicationStart()
		{
			try
			{
				if (instance == null)
				{
					instance = this;
				}

                Utilities.Utilities utils = new Utilities.Utilities();
				MapUtilities mapUtils = new MapUtilities();

                Utilities.Utilities.RegisterTypesInIL2CPP();

				if (!System.IO.Directory.Exists("UserData/paranoia"))
				{
					System.IO.Directory.CreateDirectory("UserData/paranoia");
				}

				bundle = AssetBundle.LoadFromFile("UserData/paranoia/paranoia.pack");

				genericAmbience = new List<AudioClip>();
				screamAmbience = new List<AudioClip>();
				chaserAmbience = new List<AudioClip>();
				watcherAmbience = new List<AudioClip>();
				deafenSounds = new List<AudioClip>();
				grabSounds = new List<AudioClip>();
				doorIdleSounds = new List<AudioClip>();
				doorOpenSounds = new List<AudioClip>();
				teleporterAmbience = new List<AudioClip>();
				paralyzerAmbience = new List<AudioClip>();
				darkVoices = new List<AudioClip>();
				radioTunes = new List<AudioClip>();

				videoClips = new List<VideoClip>();

				PrecacheEntityObjects();
				PrecacheAudioAssets();

			}
			catch (System.Exception e)
			{
				throw e;
			}
		}

		public override void OnSceneWasLoaded(int buildIndex, string sceneName)
		{
            switch (sceneName.ToLower())
            {
				case "sandbox_blankbox":
					gameManager = new GameObject("Game Manager").AddComponent<ParanoiaGameManager>();
					GameObject.Find("MUSICMACHINE (1)").SetActive(false);
					GameObject.Find("AMMODISPENSER").SetActive(false);
					GameObject.Find("HEALTHMACHINE").SetActive(false);
					GameObject.Find("CUSTOMLIGHTMACHINE/LIGHTMACHINE").SetActive(false);
					GameObject.Find("Decal_SafeGrav").SetActive(false);

					isBlankBox = true;
					break;
				case "sandbox_museumbasement":
					gameManager = new GameObject("Game Manager").AddComponent<ParanoiaGameManager>();
					break;
            }
		}

        public override void OnSceneWasUnloaded(int buildIndex, string sceneName)
        {
			gameManager = null;
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
			string[] nameLUT = new string[]
			{
				"amb_generic",
				"amb_scream",
				"amb_chaser",
				"amb_watcher",
				"amb_teleportingthing",
				"amb_dark_voice",
				"amb_paralysis",
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
				{ "amb_dark_voice", darkVoices },
				{ "amb_paralysis", paralyzerAmbience },
				{ "amb_deafen", deafenSounds },
				{ "player_grab", grabSounds },
				{ "door_idle", doorIdleSounds },
				{ "door_open", doorOpenSounds },
				{ "radio_tune", radioTunes }
			};

			Il2CppReferenceArray<UnityEngine.Object> assets = bundle.LoadAllAssets();

			for(int i = 0; i < directory.Keys.Count; i++)
            {
				for(int j = 0; j < assets.Count; j++)
                {
					if(assets[j].TryCast<AudioClip>() == null) { continue; }

					AudioClip clip = assets[j].Cast<AudioClip>();
					clip.hideFlags = HideFlags.DontUnloadUnusedAsset;

                    if (clip.name.StartsWith(nameLUT[i]))
                    {
						directory[nameLUT[i]].Add(clip);
					}
                }
            }
		}
	}
}