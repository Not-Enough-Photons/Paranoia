using SLZ.Marrow.Warehouse;
using UnityEngine;

namespace Paranoia.Entities
{
    /// <summary>
    /// Used for the managers, percent chance of something spawning.
    /// </summary>
    public class ParanoiaEntity : ScriptableObject
    {
        public enum EntityType
        {
            Ground,
            Air,
            Audio,
            Special
        }
        public SpawnableCrateReference crateReference;
        public EntityType entityType;
        public int percentChance = 100;
    }
}