using System.Collections;
using System.Collections.Generic;
using System.Linq;

using NEP.Paranoia.Entities;
using NEP.Paranoia.TickEvents;
using NEP.Paranoia.TickEvents.Mirages;
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

        private void Initialize()
        {
            tickManager = new TickManager();

            GameObject audioChaser = Paranoia.instance.GetEntInDirectory("ent_a_Chaser");
        }

        private void SpawnEntity(string name)
        {
            GameObject directoryEntity = Paranoia.instance.GetEntInDirectory(name);

            string entName = directoryEntity.name.Substring(3);
            bool isAudioEntity = entName.StartsWith("a_");

            GameObject spawnedObject = GameObject.Instantiate(directoryEntity, Vector3.zero, Quaternion.identity);

            if (isAudioEntity)
            {
                spawnedObject.AddComponent<AudioMirage>();
            }
            else
            {
                spawnedObject.AddComponent<BaseMirage>();
            }
        }
    }
}