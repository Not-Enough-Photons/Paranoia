using UnityEngine;
using NEP.Paranoia.ParanoiaUtilities;

namespace NEP.Paranoia.Entities
{
	public class SpawnCircle
	{
        public SpawnCircle(Transform originTransform)
        {
            this.originTransform = originTransform;
        }

        public Vector3 circle;

        public Transform originTransform;

        public float radius = 75f;

        private const float Deg2Rad = 0.0174532924f;

        public Vector3 CalculatePlayerCircle(float angle)
        {
            // y position is 1 meter since we need the shadow beings to be on the ground directly
            return new Vector3(
                originTransform.position.x + Mathf.Sin(angle * Deg2Rad) * radius,
                0.5f,
                originTransform.position.z + Mathf.Cos(angle * Deg2Rad) * radius);
        }

        public Vector3 CalculatePlayerCircle(float angle, float radius)
        {
            // y position is 1 meter since we need the shadow beings to be on the ground directly
            return new Vector3(
                originTransform.position.x + Mathf.Sin(angle * Deg2Rad) * radius,
                0.5f,
                originTransform.position.z + Mathf.Cos(angle * Deg2Rad) * radius);
        }

        public Vector3 CalculatePlayerCircle(float angle, float radius, float yOffset)
        {
            Bounds levelBounds = MapUtilities.GetLevelBounds();

            Vector3 pos = new Vector3(
                originTransform.position.x + Mathf.Sin(angle * Deg2Rad) * radius,
                yOffset,
                originTransform.position.z + Mathf.Cos(angle * Deg2Rad) * radius);

            // y position is 1 meter since we need the shadow beings to be on the ground directly
            return pos;
        }
    }
}
