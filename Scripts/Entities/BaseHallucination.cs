using UnityEngine;

using NotEnoughPhotons.Paranoia.Managers;

namespace NotEnoughPhotons.Paranoia.Entities
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
            Teleporting = (1 << 7)
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

        public HallucinationFlags flags
        { 
            get { return m_flags; } 
            set { m_flags = value; }
        }

        public StartFlags startFlags
        {
            get { return m_startFlags; }
            set { m_startFlags = value; }
        }

        public bool useRandomSpawnAngle
        {
            get { return m_useRandomSpawnAngle; }
            set {  m_useRandomSpawnAngle = value; }
        }

        public float spawnRadius
        {
            get { return m_spawnRadius; }
            set { m_spawnRadius = value; }
        }

        public float spawnAngle
        {
            get { return m_spawnAngle; }
            set { m_spawnAngle = value; }
        }

        public bool usesDelay
        {
            get { return m_usesDelay; }
            set { m_usesDelay = value; }
        }
        public float maxTime
        {
            get { return m_maxTime; }
            set { m_maxTime = value; }
        }

        public float moveSpeed
        {
            get { return m_moveSpeed; }
            set { m_moveSpeed = value; }
        }

        public float maxTeleportDelay
        {
            get { return m_maxTeleportDelay; }
            set { m_maxTeleportDelay = value; }
        }

        public float disableDistance
        {
            get { return m_disableDistance; }
            set { m_disableDistance = value; }
        }
        public float lookAtDisableDistance
        {
            get { return m_lookAtDisableDistance; }
            set { m_lookAtDisableDistance = value; }
        }
        public float damage
        {
            get { return m_damage; }
            set { m_damage = value; }
        }

        public Transform playerTarget
        {
            get { return m_playerTarget; }
            set { m_playerTarget = value; }
        }
        public Transform playerTargetHead
        {
            get { return m_playerTargetHead; }
            set { m_playerTargetHead = value; }
        }

        public Vector3[] spawnPoints
        {
            get { return m_spawnPoints; }
            set { m_spawnPoints = value; }
        }

        protected HallucinationFlags m_flags;

        protected StartFlags m_startFlags;

        protected bool m_useRandomSpawnAngle;
        protected float m_spawnRadius = 100f;
        protected float m_spawnAngle;

        protected bool m_usesDelay;
        protected float m_maxTime = 1f;

        protected float m_moveSpeed = 1f;
        protected float m_maxTeleportDelay = 1f;

        protected float m_disableDistance = 1f;
        protected float m_lookAtDisableDistance = 1f;
        protected float m_damage = 0.1f;

        protected Transform m_playerTarget;
        protected Transform m_playerTargetHead;

        protected Vector3[] m_spawnPoints;

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

            m_useRandomSpawnAngle = settings.useRandomSpawnAngle;
            m_spawnRadius = settings.spawnRadius;
            m_spawnAngle = settings.spawnAngle;

            m_usesDelay = settings.usesDelay;
            m_maxTime = settings.maxTime;

            m_moveSpeed = settings.moveSpeed;
            m_maxTeleportDelay = settings.maxTeleportDelay;

            m_disableDistance = settings.disableDistance;
            m_lookAtDisableDistance = settings.lookAtDisableDistance;
            m_damage = settings.damage;
        }

        protected virtual void Awake()
        {
            m_playerTarget = ModThatIsNotMod.Player.GetPlayerHead().transform;
            m_playerTargetHead = ModThatIsNotMod.Player.GetPlayerHead().transform;

            gameObject.SetActive(false);
        }

        protected virtual void OnEnable()
        {
            transform.position = Vector3.up * 1f;

            if (m_startFlags.HasFlag(StartFlags.SpawnAroundPlayer))
            {
                if (m_useRandomSpawnAngle)
                {
                    transform.position = ParanoiaGameManager.instance.playerCircle.CalculatePlayerCircle(Random.Range(0, 360), m_spawnRadius);
                }
                else
                {
                    transform.position = ParanoiaGameManager.instance.playerCircle.CalculatePlayerCircle(m_spawnAngle, m_spawnRadius);
                }
            }

            if (m_startFlags.HasFlag(StartFlags.SpawnAtPoints))
            {
                if(m_spawnPoints == null) { return; }
                if(m_spawnPoints.Length == 0) { return; }

                transform.position = m_spawnPoints[Random.Range(0, m_spawnPoints.Length)];
            }

            if (m_startFlags.HasFlag(StartFlags.LookAtTarget))
            {
                Vector3 lookAtEuler = Quaternion.LookRotation(playerTarget.position - -transform.forward).eulerAngles;
                transform.eulerAngles = Vector3.up * lookAtEuler.y;
            }
        }

        protected virtual void Update()
        {
            if (m_playerTarget == null) { return; }
            if (m_playerTargetHead == null) { return; }

            if (m_flags.HasFlag(HallucinationFlags.HideWhenSeen))
            {
                if (Vector3.Dot(m_playerTargetHead.forward, transform.position) < m_lookAtDisableDistance)
                {
                    gameObject.SetActive(false);
                }
            }

            if (m_flags.HasFlag(HallucinationFlags.HideWhenClose))
            {
                if (Vector3.Distance(m_playerTarget.position, transform.position) < m_disableDistance)
                {
                    gameObject.SetActive(false);
                }
            }

            if (m_flags.HasFlag(HallucinationFlags.LookAtTarget))
            {
                transform.LookAt(m_playerTarget);
            }

            if (m_usesDelay)
            {
                startDelayTimer += Time.deltaTime;

                if (startDelayTimer >= m_maxTime)
                {
                    reachedDelayTimer = true;
                }

                if (!reachedDelayTimer) { return; }
            }

            if (m_flags.HasFlag(HallucinationFlags.Moving))
            {
                transform.position += transform.forward * (m_moveSpeed * Time.deltaTime);
            }

            if (m_flags.HasFlag(HallucinationFlags.DamageThenHide))
            {
                if (Vector3.Distance(m_playerTarget.position, transform.position) < m_disableDistance)
                {
                    playerTarget.GetComponentInParent<Player_Health>().TAKEDAMAGE(m_damage);
                    gameObject.SetActive(false);
                }
            }

            if (m_flags.HasFlag(HallucinationFlags.SpinAroundPlayer))
            {
                transform.position = ParanoiaGameManager.instance.playerCircle.CalculatePlayerCircle(Time.time, m_spawnRadius);
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
    }

}