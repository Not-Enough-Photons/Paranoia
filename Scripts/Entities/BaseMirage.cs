using System;
using UnityEngine;

using NEP.Paranoia.Managers;

namespace NEP.Paranoia.Entities
{
    [MelonLoader.RegisterTypeInIl2Cpp]
    public class BaseMirage : MonoBehaviour
    {
        public BaseMirage(IntPtr ptr) : base(ptr) { }

        [Serializable]
        public struct Stats
        {
            // -- GENERIC STATS -- 
            public float spawnRadius;
            public float spawnAngle;
            public float yOffset;
            public float time;
            public float hideDist;
            public float damage;
            public float moveSpeed;
            public float timeTeleport;
            public float[] position;

            public string entityFlags;
            public string spawnFlags;

            // -- AUDIO MIRAGES ONLY! -- 
            public float doppler;
            public float spatialBlend;
            public bool looping;
            public bool audioTeleport;
            public string[] clips;

            public static void ParseEntityFlags(BaseMirage mirage, string flags)
            {
                string[] split = flags.Split('|');

                foreach (string flag in split)
                {
                    print(flag);

                    object objParsed = Enum.Parse(typeof(EntityFlags), flag);
                    mirage.entityFlags ^= (EntityFlags)objParsed;
                }
            }

            public static void ParseSpawnFlags(BaseMirage mirage, string flags)
            {
                string[] split = flags.Split('|');
                foreach (string flag in split)
                {
                    object objParsed = Enum.Parse(typeof(SpawnFlags), flag);
                    mirage.spawnFlags ^= (SpawnFlags)objParsed;
                }
            }
        }

        public EntityFlags entityFlags;
        public SpawnFlags spawnFlags;

        public Transform target;

        protected Stats stats;

        private Vector3 targetCircle;

        private float randAngle;

        private float t_Teleport;
        private float t_Wait;

        protected virtual void Awake()
        {
            if(this is AudioMirage)
            {
                name = name.Substring(6);
            }
            else
            {
                name = name.Substring(4);
            }

            int indexOf = name.IndexOf("(Clone)");
            name = name.Remove(indexOf);

            stats = DataReader.ReadStats(name);

            Stats.ParseEntityFlags(this, stats.entityFlags);
            Stats.ParseSpawnFlags(this, stats.spawnFlags);

            target = ModThatIsNotMod.Player.GetPlayerHead().transform;

            gameObject.hideFlags = HideFlags.DontUnloadUnusedAsset;

            gameObject.SetActive(true);
        }

        protected virtual void OnEnable()
        {
            transform.position = Vector3.up * 0.5f;

            if (stats.position != null)
            {
                Vector3 newPos = new Vector3(stats.position[0], stats.position[1], stats.position[2]);
                transform.position = newPos;
            }

            if (spawnFlags.HasFlag(SpawnFlags.SpawnAroundTarget))
            {
                randAngle = UnityEngine.Random.Range(0f, 360f);

                targetCircle = SpawnCircle.SolveCircle(target.position, 1f, 100f, randAngle);
                transform.position = targetCircle;
            }

            if (spawnFlags.HasFlag(SpawnFlags.None))
            {
                return;
            }

            if (spawnFlags.HasFlag(SpawnFlags.SpawnAtPoints))
            {
                // Spawn at defined spawn points
            }
        }

        protected virtual void OnDisable()
        {
            t_Teleport = 0f;
            t_Wait = 0f;
        }

        protected virtual void Update()
        {
            if (entityFlags.HasFlag(EntityFlags.HideWhenClose))
            {
                if (Vector3.Distance(transform.position, target.position) < stats.hideDist)
                {
                    gameObject.SetActive(false);
                }
            }

            if (entityFlags.HasFlag(EntityFlags.Wait))
            {
                t_Wait += Time.deltaTime;

                if (t_Wait < stats.time)
                {
                    return;
                }
            }

            if (entityFlags.HasFlag(EntityFlags.LookAtTarget))
            {
                if (target == null)
                {
                    return;
                }

                transform.LookAt(target);
            }

            if (entityFlags.HasFlag(EntityFlags.Moving))
            {
                if (target == null)
                {
                    return;
                }

                transform.position += (transform.forward * stats.moveSpeed) * Time.deltaTime;
            }

            if (entityFlags.HasFlag(EntityFlags.Teleporting))
            {
                t_Teleport += Time.deltaTime;

                if (t_Teleport >= stats.timeTeleport)
                {
                    OnTeleport();
                    t_Teleport = 0f;
                }
            }
        }

        protected virtual void OnTeleport()
        {
            transform.position += transform.forward * stats.moveSpeed;
        }
    }

}