#if MELONLOADER
using System;
using MelonLoader;
using BoneLib;
#endif
using System.Collections;
using UnityEngine;

namespace Paranoia.Entities
{
#if UNITY_EDITOR
    [AddComponentMenu("Paranoia/Entities/Mirage")]
#endif
    public class Mirage : MonoBehaviour
    {
#if MELONLOADER
        public float movementSpeed;
        public float despawnTime;
        public bool shootable;
        public float minX = -195f;
        public float minZ = -195f;
        public float maxX = 195f;
        public float maxZ = 195f;
        private Transform _player;
        private Transform This => transform;
        private Vector3 _targetPosition;
#else
        [Header("Mirage Settings")]
        [Tooltip("How fast the mirage moves")]
        public float movementSpeed;
        [Tooltip("How long until the mirage despawns")]
        public float despawnTime;
        [Tooltip("If this is enabled, the mirage will not despawn after the amounted time. You will have to implement shooting events yourself with a generic attack reciever.")]
        public bool shootable;
        [Header("Bounds Settings")]
        [Tooltip("The minimum X value the mirage can move to")]
        public float minX = -195f;
        [Tooltip("The minimum Z value the mirage can move to")]
        public float minZ = -195f;
        [Tooltip("The maximum X value the mirage can move to")]
        public float maxX = 195f;
        [Tooltip("The maximum Z value the mirage can move to")]
        public float maxZ = 195f;
        private Transform _player;
        private Transform This => transform;
        private Vector3 _targetPosition;
#endif
#if MELONLOADER
        private void Start()
        {
            _player = Player.playerHead;
            SetTargetPosition();
            if (shootable) return;
            MelonCoroutines.Start(DespawnSelf(despawnTime));
        }

        private void Update()
        {
            This.position = Vector3.Lerp(transform.position, _targetPosition, movementSpeed * Time.deltaTime);
            This.LookAt(_player);
            if (Vector3.Distance(transform.position, _targetPosition) < 0.1f)
            {
                SetTargetPosition();
            }
        }

        private void SetTargetPosition()
        {
            var randX = UnityEngine.Random.Range(minX, maxX);
            var randZ = UnityEngine.Random.Range(minZ, maxZ);
            _targetPosition = new Vector3(randX, transform.position.y, randZ);
        }
        
        private IEnumerator DespawnSelf(float delay)
        {
            yield return new WaitForSeconds(delay);
            MelonLogger.Msg("Mirage despawned");
            Destroy(gameObject);
        }
#else
        private void Start()
        {

        }

        private void Update()
        {

        }

        private void SetTargetPosition()
        {

        }

        private IEnumerator DespawnSelf(float delay)
        {
            yield return new WaitForSeconds(delay);
        }
#endif

#if MELONLOADER
        public Mirage(IntPtr ptr) : base(ptr) { }
#endif
    }
}