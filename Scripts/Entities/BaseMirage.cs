using System;
using System.Collections.Generic;
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
            public float[] position;
            public float[] scale;
            public float[] spawnPoints;
            public float spawnRadius;
            public float spawnAngle;
            public float yOffset;
            public float time;
            public float hideDist;
            public float damage;
            public float moveSpeed;
            public float timeTeleport;
            public float fade;

            public string[] textures;

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

        private List<Texture2D> textures;
        private MeshRenderer meshRenderer;

        private Vector3 targetCircle;

        private float randAngle;

        private float t_Teleport;
        private float t_Wait;

        public void OnProjectileHit()
        {
            if (!entityFlags.HasFlag(EntityFlags.HideWhenHit))
            {
                return;
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

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

            if(stats.textures != null)
            {
                SetupTextures(stats.textures);
            }

            target = ModThatIsNotMod.Player.GetPlayerHead().transform;

            meshRenderer = GetComponentInChildren<MeshRenderer>();

            gameObject.hideFlags = HideFlags.DontUnloadUnusedAsset;

            gameObject.SetActive(true);
        }

        protected virtual void OnEnable()
        {
            if (stats.position != null)
            {
                Vector3 newPos = new Vector3();

                newPos[0] = stats.position[0];
                newPos[1] = stats.position[1];
                newPos[2] = stats.position[2];

                transform.position = newPos;
            }
            else
            {
                transform.position = Vector3.up * 0.5f;
            }

            if(stats.scale != null)
            {
                Vector3 newScale = new Vector3();

                newScale[0] = stats.scale[0];
                newScale[1] = stats.scale[1];
                newScale[2] = stats.scale[2];

                transform.localScale = newScale;
            }
            else
            {
                transform.localScale = Vector3.one;
            }

            if(stats.textures != null)
            {
                if(meshRenderer != null)
                {
                    int random = UnityEngine.Random.Range(0, textures.Count);
                    meshRenderer.sharedMaterial.mainTexture = textures[random];
                }
            }
            else
            {
                if(meshRenderer != null)
                {
                    Texture2D missingTexture = Paranoia.instance.GetTextureInList("tex_missing_texture");
                    meshRenderer.sharedMaterial.mainTexture = missingTexture;
                }
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

        protected virtual void SetupTextures(string[] textureList)
        {
            textures = new List<Texture2D>();

            foreach(string texture in textureList)
            {
                Texture2D tex = Paranoia.instance.GetTextureInList(texture);

                if(tex != null)
                {
                    textures?.Add(tex);
                }
                else
                {
                    tex = Paranoia.instance.GetTextureInList("tex_missing_texture");
                    textures?.Add(tex);
                }
            }
        }
    }

}