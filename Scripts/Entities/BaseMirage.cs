using UnityEngine;

using System;

namespace NEP.Paranoia.Entities
{
    public class BaseMirage : MonoBehaviour
    {
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

            public string entityFlags;
            public string spawnFlags;

            // -- AUDIO MIRAGES ONLY! -- 
            public float doppler;
            public float spatialBlend;
            public bool looping;
            public bool audioTeleport;

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
                    object objParsed = Enum.Parse(typeof(EntityFlags), flag);
                    mirage.spawnFlags ^= (SpawnFlags)objParsed;
                }
            }
        }

        public EntityFlags entityFlags;
        public SpawnFlags spawnFlags;

        public Transform target;

        protected Stats stats;

        private float t_Teleport;
        private float t_Wait;

        protected virtual void Awake()
        {
            stats = Managers.DataReader.ReadStats(gameObject.name);

            Stats.ParseEntityFlags(this, stats.entityFlags);
            Stats.ParseSpawnFlags(this, stats.spawnFlags);

            target = Camera.main.transform;

            gameObject.hideFlags = HideFlags.DontUnloadUnusedAsset;

            gameObject.SetActive(true);
        }

        protected virtual void OnEnable()
        {
            transform.position = Vector3.up * 0.5f;

            if (spawnFlags.HasFlag(SpawnFlags.None))
            {
                return;
            }

            if (spawnFlags.HasFlag(SpawnFlags.SpawnAroundPlayer))
            {
                // Set spawn angle
                transform.position = SpawnCircle.SolveCircle(target.position, 1f, 100f, UnityEngine.Random.Range(0f, 360f));
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
