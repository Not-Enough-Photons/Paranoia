using UnityEngine;
using NEP.Paranoia.ParanoiaUtilities;

namespace NEP.Paranoia.Entities
{
    public struct SpawnCircle
    {
        private const float Deg2Rad = 0.0174532924f;

        public static Vector3 SolveCircle(Vector3 position, float y, float radius, float angle)
        {
            float x = position.x + Mathf.Sin(angle * Deg2Rad) * radius;
            float z = position.z + Mathf.Cos(angle * Deg2Rad) * radius;

            return new Vector3(x, y, z);
        }
    }
}
