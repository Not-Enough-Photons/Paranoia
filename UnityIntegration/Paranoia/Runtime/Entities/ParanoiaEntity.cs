using SLZ.Marrow.Warehouse;
using UnityEngine;

namespace Paranoia.Entities
{
    public class ParanoiaEntity : ScriptableObject
    {
        public enum EntityType
        {
            Ground,
            Air,
            Audio,
            Special
        }
        [Tooltip("The crate of the entity.")]
        public SpawnableCrateReference crateReference;
        [Tooltip("The type of entity. Determines their spawn. Ground means they'll spawn at a ground location, Air means they'll spawn at an air location, Audio means they'll spawn at an audio location, and Special means they'll spawn at the mirage location.")]
        public EntityType entityType;
        [Tooltip("The percent chance that this entity is picked.")]
        public int percentChance = 100;
    }
}