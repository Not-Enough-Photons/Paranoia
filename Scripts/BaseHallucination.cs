using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NotEnoughPhotons.paranoia
{
    public class BaseHallucination : MonoBehaviour
    {
        public BaseHallucination(System.IntPtr ptr) : base(ptr) { }

        [System.Flags]
        public enum HallucinationFlags
        {
            None = 0,
            HideWhenSeen = (1 << 0),
            HideWhenClose = (1 << 1),
            LookAtTarget = (1 << 2),
            Moving = (1 << 3),
            Damaging = (1 << 4)
        }

        [System.Flags]
        public enum StartFlags
        {
            None = 0,
            SpawnAroundPlayer = (1 << 0),
            SpawnAtPoints = (1 << 1)
        }

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

        protected float m_disableDistance = 1f;
        protected float m_lookAtDisableDistance = 1f;
        protected float m_damage = 0.1f;

        protected Transform m_playerTarget;
        protected Transform m_playerTargetHead;

        protected Vector3[] m_spawnPoints;

        private float timer;
        private bool reachedTimer;

        protected virtual void Awake()
        {
            m_playerTarget = ModThatIsNotMod.Player.GetPlayerHead().transform;
            m_playerTargetHead = ModThatIsNotMod.Player.GetPlayerHead().transform;
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
                if(m_spawnPoints.Length == -1 || m_spawnPoints == null) { return; }

                transform.position = m_spawnPoints[Random.Range(0, m_spawnPoints.Length)];
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
                timer += Time.deltaTime;

                if (timer >= m_maxTime)
                {
                    reachedTimer = true;
                }

                if (!reachedTimer) { return; }
            }

            if (m_flags.HasFlag(HallucinationFlags.Moving))
            {
                transform.position += transform.forward * (m_moveSpeed * Time.deltaTime);
            }

            if (m_flags.HasFlag(HallucinationFlags.Damaging))
            {
                // do damaging stuff
            }
        }
    }

}