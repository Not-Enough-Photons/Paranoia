using UnityEngine;

using NEP.Paranoia.Managers;

namespace NEP.Paranoia.Entities
{
    public class BaseHallucination : MonoBehaviour
    {
        public BaseHallucination(System.IntPtr ptr) : base(ptr) { }

        public struct Settings
        {
            public string baseFlags;
            public string startFlags;

            public bool useRandomSpawnAngle;
            public float spawnRadius;
            public float spawnAngle;
            public float yOffset;

            public bool usesDelay;
            public float maxTime;

            public float moveSpeed;
            public float maxTeleportDelay;

            public float disableDistance;
            public float lookAtDisableDistance;
            public float damage;
        }

        [System.Flags]
        public enum HallucinationFlags
        {
            None,
            HideWhenSeen = (1 << 0),
            HideWhenClose = (1 << 1),
            LookAtTarget = (1 << 2),
            Moving = (1 << 3),
            Damaging = (1 << 4),
            DamageThenHide = (1 << 5),
            SpinAroundPlayer = (1 << 6),
            Teleporting = (1 << 7),
            MoveWhenNotSeen = (1 << 8)
        }

        [System.Flags]
        public enum StartFlags
        {
            None = 0,
            SpawnAroundPlayer = (1 << 0),
            SpawnAtPoints = (1 << 1),
            LookAtTarget = (1 << 2)
        }

        public Settings settings { get; set; }

        public HallucinationFlags flags { get; protected set; }
        public StartFlags startFlags { get; protected set; }

        public bool useRandomSpawnAngle { get; protected set; }
        public bool usesDelay { get; protected set; }

        public float spawnRadius { get; protected set; } = 100f;
        public float spawnAngle { get; protected set; }
        public float yOffset { get; protected set; }
        public float maxTime { get; protected set; } = 1f;
        public float moveSpeed { get; protected set; } = 1f;
        public float maxTeleportDelay { get; protected set; } = 1f;
        public float disableDistance { get; protected set; } = 1f;
        public float lookAtDisableDistance { get; protected set; } = 1f;
        public float damage { get; protected set; } = 0.1f;

        public Transform playerTarget;
        public Transform playerTargetHead;

        public Vector3[] spawnPoints { get; protected set; }

        protected readonly string baseJsonPath = "UserData/paranoia/json/BaseHallucination/";

        private float startDelayTimer;
        private float teleportDelayTimer;

        private bool reachedDelayTimer;
        private bool reachedTeleportDelayTimer = false;

        protected bool GetReachedTeleportDelay()
        {
            return reachedTeleportDelayTimer;
        }

        public virtual void ReadValuesFromJSON(string json)
        {
            settings = Newtonsoft.Json.JsonConvert.DeserializeObject<Settings>(json);

            string baseFlagsDirty = settings.baseFlags.Replace(" ", string.Empty);
            string[] flagsTxt = baseFlagsDirty.Split('|');

            string startFlagsDirty = settings.startFlags.Replace(" ", string.Empty);
            string[] startFlagsTxt = startFlagsDirty.Split('|');

            foreach (string value in flagsTxt) { flags ^= (HallucinationFlags)System.Enum.Parse(typeof(HallucinationFlags), value); }
            foreach (string value in startFlagsTxt) { startFlags ^= (StartFlags)System.Enum.Parse(typeof(StartFlags), value); }

            useRandomSpawnAngle = settings.useRandomSpawnAngle;
            spawnRadius = settings.spawnRadius;
            spawnAngle = settings.spawnAngle;
            yOffset = settings.yOffset;

            usesDelay = settings.usesDelay;
            maxTime = settings.maxTime;

            moveSpeed = settings.moveSpeed;
            maxTeleportDelay = settings.maxTeleportDelay;

            disableDistance = settings.disableDistance;
            lookAtDisableDistance = settings.lookAtDisableDistance;
            damage = settings.damage;
        }

        protected virtual void Awake()
        {
            gameObject.hideFlags = HideFlags.DontUnloadUnusedAsset;

            playerTarget = ModThatIsNotMod.Player.GetPlayerHead().transform;
            playerTargetHead = ModThatIsNotMod.Player.GetPlayerHead().transform;

            gameObject.SetActive(false);
        }

        protected virtual void OnEnable()
        {
            transform.position = Vector3.up * 1f;

            if (startFlags.HasFlag(StartFlags.SpawnAroundPlayer))
            {
                if (useRandomSpawnAngle)
                {
                    transform.position = Paranoia.instance.gameManager.playerCircle.CalculatePlayerCircle(Random.Range(0, 360), spawnRadius, yOffset);
                }
                else
                {
                    transform.position = Paranoia.instance.gameManager.playerCircle.CalculatePlayerCircle(spawnAngle, spawnRadius, yOffset);
                }
            }

            if (startFlags.HasFlag(StartFlags.SpawnAtPoints))
            {
                if(spawnPoints == null) { return; }
                if(spawnPoints.Length == 0) { return; }

                transform.position = spawnPoints[Random.Range(0, spawnPoints.Length)];
            }

            if (startFlags.HasFlag(StartFlags.LookAtTarget))
            {
                Vector3 lookAtEuler = Quaternion.LookRotation(playerTarget.position - -transform.forward).eulerAngles;
                transform.eulerAngles = Vector3.up * lookAtEuler.y;
            }
        }

        protected virtual void Update()
        {
            if (playerTarget == null) { return; }
            if (playerTargetHead == null) { return; }

            if (flags.HasFlag(HallucinationFlags.HideWhenSeen))
            {
                if (Vector3.Dot(playerTargetHead.forward, transform.position) < lookAtDisableDistance)
                {
                    gameObject.SetActive(false);
                }
            }

            if (flags.HasFlag(HallucinationFlags.HideWhenClose))
            {
                if (Vector3.Distance(playerTarget.position, transform.position) < disableDistance)
                {
                    gameObject.SetActive(false);
                }
            }

            if (flags.HasFlag(HallucinationFlags.LookAtTarget))
            {
                transform.LookAt(playerTarget);
            }

            if (usesDelay)
            {
                startDelayTimer += Time.deltaTime;

                if (startDelayTimer >= maxTime)
                {
                    reachedDelayTimer = true;
                }

                if (!reachedDelayTimer) { return; }
            }

            if (flags.HasFlag(HallucinationFlags.Moving))
            {
                transform.position += transform.forward * (moveSpeed * Time.deltaTime);
            }

            if (flags.HasFlag(HallucinationFlags.DamageThenHide))
            {
                if (Vector3.Distance(playerTarget.position, transform.position) < disableDistance)
                {
                    playerTarget.GetComponentInParent<Player_Health>().TAKEDAMAGE(damage);
                    gameObject.SetActive(false);
                }
            }

            if (flags.HasFlag(HallucinationFlags.SpinAroundPlayer))
            {
                transform.position = Paranoia.instance.gameManager.playerCircle.CalculatePlayerCircle(Time.time, spawnRadius);
            }

            if (flags.HasFlag(HallucinationFlags.Teleporting))
            {
                teleportDelayTimer += Time.deltaTime;

                if (reachedTeleportDelayTimer)
                {
                    transform.position += transform.forward * moveSpeed;
                }

                reachedTeleportDelayTimer = false;

                if (teleportDelayTimer >= maxTeleportDelay)
                {
                    reachedTeleportDelayTimer = true;
                    teleportDelayTimer = 0f;
                }
            }
        }

        protected virtual void OnBecameVisible()
        {
            if (flags.HasFlag(HallucinationFlags.MoveWhenNotSeen))
            {
                teleportDelayTimer += Time.deltaTime;

                if (reachedTeleportDelayTimer)
                {
                    transform.position += transform.forward * moveSpeed;
                }

                reachedTeleportDelayTimer = false;

                if (teleportDelayTimer >= maxTeleportDelay)
                {
                    reachedTeleportDelayTimer = true;
                    teleportDelayTimer = 0f;
                }
            }
        }
    }
}