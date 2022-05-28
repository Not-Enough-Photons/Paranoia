using System.Collections;
using System.Collections.Generic;
using System.Linq;

using NEP.Paranoia.Entities;
using NEP.Paranoia.TickEvents;
using NEP.Paranoia.ParanoiaUtilities;
using static NEP.Paranoia.Managers.Tick;

using StressLevelZero.AI;
using PuppetMasta;

using UnityEngine;

using Newtonsoft.Json;
using StressLevelZero.Rig;

namespace NEP.Paranoia.Managers
{
    public class GameManager
    {
        public GameManager()
        {
            instance = this;

            Initialize();
        }

        public static GameManager instance;
        public TickManager tickManager;

        public List<GameObject> entities;

        public static float insanity;
        public static float insanityRate = 0.025f;

        public static int rngValue = 0;

        private void Initialize()
        {
            tickManager = new TickManager();

            entities = new List<GameObject>();

            string[] entityReg = DataReader.ReadEntityRegistry("entityreg.txt");

            foreach(string ent in entityReg)
            {
                SpawnEntity(ent);
            }
        }

        private GameObject SpawnEntity(string name)
        {
            GameObject directoryEntity = Paranoia.instance.GetEntInDirectory(name);

            if(directoryEntity == null)
            {
                return null;
            }

            string entName = directoryEntity.name.Substring(4);
            bool isAudioEntity = entName.StartsWith("a_");

            GameObject spawnedObject = GameObject.Instantiate(directoryEntity, Vector3.zero, Quaternion.identity);

            if (isAudioEntity)
            {
                entName = entName.Substring(2);
                spawnedObject.AddComponent<AudioMirage>();
            }
            else
            {
                spawnedObject.AddComponent<BaseMirage>();
            }

            spawnedObject.name = entName;
            spawnedObject.hideFlags = HideFlags.DontUnloadUnusedAsset;

            spawnedObject.SetActive(false);

            entities.Add(spawnedObject);

            return spawnedObject;
        }
    }
}