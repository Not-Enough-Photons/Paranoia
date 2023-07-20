using System;
using System.Collections.Generic;
using UnityEngine;

using NEP.Paranoia.Managers;

namespace NEP.Paranoia.Entities
{
    [MelonLoader.RegisterTypeInIl2Cpp]
    public class Mirage : MonoBehaviour
    {
        public Mirage(IntPtr ptr) : base(ptr) { }

        [Serializable]
        public struct Stats
        {
            // -- GENERIC STATS -- 
            public float[] position;
            public float[] scale;
            public float spawnRadius;
            public float spawnAngle;
            public float yOffset;
            public float time;
            public float hideDist;
            public float damage;
            public float moveSpeed;
            public float timeTeleport;

            public string reachedPlayerEvent;
            private TickEvents.ParanoiaEvent _reachedPlayerEvent;

            public float fade;

            public string usePrefab;

            public string[] textures;

            public string entityFlags;
            public string spawnFlags;

            // -- AUDIO MIRAGES ONLY! -- 
            public float doppler;
            public float spatialBlend;
            public bool looping;
            public bool audioTeleport;
            public string[] clips;

            public static void ParseEntityFlags(Mirage mirage, string flags)
            {
                if(mirage == null)
                {
                    return;
                }

                if (string.IsNullOrEmpty(flags))
                {
                    return;
                }

                string[] split = flags.Split('|');

                foreach (string flag in split)
                {
                    object objParsed = Enum.Parse(typeof(EntityFlags), flag);
                    mirage.entityFlags ^= (EntityFlags)objParsed;
                }
            } 

            public static void ParseSpawnFlags(Mirage mirage, string flags)
            {
                if (mirage == null)
                {
                    return;
                }

                if (string.IsNullOrEmpty(flags))
                {
                    return;
                }

                string[] split = flags.Split('|');
                foreach (string flag in split)
                {
                    object objParsed = Enum.Parse(typeof(SpawnFlags), flag);
                    mirage.spawnFlags ^= (SpawnFlags)objParsed;
                }
            }

            public static void ParseReachedEvent(Mirage mirage, string eventName)
            {
                if (mirage == null)
                {
                    return;
                }

                if (string.IsNullOrEmpty(eventName))
                {
                    return;
                }

                Type eventType = Type.GetType("NEP.Paranoia.TickEvents.Events." + eventName);

                if(eventType == null)
                {
                    return;
                }

                TickEvents.ParanoiaEvent pEvent = Activator.CreateInstance(eventType) as TickEvents.ParanoiaEvent;
                mirage.stats._reachedPlayerEvent = pEvent;
            }

            public TickEvents.ParanoiaEvent GetEvent()
            {
                return _reachedPlayerEvent;
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
        private float spin;

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
            Stats.ParseReachedEvent(this, stats.reachedPlayerEvent);

            if(stats.textures != null)
            {
                SetupTextures(stats.textures);
            }

            target = BoneLib.Player.playerHead;

            meshRenderer = GetComponentInChildren<MeshRenderer>();

            gameObject.hideFlags = HideFlags.DontUnloadUnusedAsset;

            gameObject.SetActive(false);
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
                if(meshRenderer != null && meshRenderer.name == "Quad")
                {
                    int random = UnityEngine.Random.Range(0, textures.Count);
                    meshRenderer.sharedMaterial.mainTexture = textures[random];
                }
            }
            else
            {
                if(meshRenderer != null && meshRenderer.name == "Quad")
                {
                    Texture2D missingTexture = Paranoia.instance.GetTextureInList("tex_missing_texture");
                    meshRenderer.sharedMaterial.mainTexture = missingTexture;
                }
            }

            if (spawnFlags.HasFlag(SpawnFlags.SpawnAroundTarget))
            {
                randAngle = UnityEngine.Random.Range(0f, 360f);

                if (stats.yOffset != 0f)
                {
                    targetCircle = SpawnCircle.SolveCircle(target.position, stats.yOffset, stats.spawnRadius, randAngle);
                    transform.position = targetCircle;
                }
                else
                {
                    targetCircle = SpawnCircle.SolveCircle(target.position, 1f, stats.spawnRadius, randAngle);
                    transform.position = targetCircle;
                }
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

                    if (!string.IsNullOrEmpty(stats.reachedPlayerEvent))
                    {
                        stats.GetEvent()?.Start();
                    }
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

            if (entityFlags.HasFlag(EntityFlags.HideWhenSeen))
            {
                if(target == null)
                {
                    return;
                }

                Vector3 targetForward = target.forward;
                Vector3 forward = transform.forward;
                float angle = Vector3.SignedAngle(forward, targetForward, Vector3.up);

                if(angle > 135f)
                {
                    gameObject.SetActive(false);
                }
            }

            if (entityFlags.HasFlag(EntityFlags.SpinAroundTarget))
            {
                if(target == null)
                {
                    return;
                }

                spin += Time.deltaTime;
                Vector3 circle = SpawnCircle.SolveCircle(target.position, target.position.y, stats.spawnRadius, spin);
                transform.position = circle;
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