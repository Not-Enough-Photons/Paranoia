using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NotEnoughPhotons.paranoia
{
    public class ObjectPool : MonoBehaviour
    {
        public ObjectPool(System.IntPtr ptr) : base(ptr) { }

        /// <summary>
        /// Pool size (How many UI objects do we want?)
        /// </summary>
        public int poolSize = 10;

        /// <summary>
        /// The prefab you want copied in the pool list
        /// </summary>
        public GameObject prefab;

        /// <summary>
        /// List of pooled objects
        /// </summary>
        public List<GameObject> objects;

        /// <summary>
        /// Pooled objects currently instantiated in the hierarchy
        /// </summary>
        public List<GameObject> pooledObjects;

		private void Awake()
		{
            objects = new List<GameObject>(poolSize);
            pooledObjects = new List<GameObject>(poolSize);

            if(prefab == null) { return; }

            GrowPool(poolSize);
		}

        [UnhollowerBaseLib.Attributes.HideFromIl2Cpp]
        public void GrowPool(int amount)
		{
            GrowPool(amount, false);
		}

        [UnhollowerBaseLib.Attributes.HideFromIl2Cpp]
        public void GrowPool(int amount, bool startActive)
        {
            for (int i = 0; i < amount; i++)
            {
                objects.Add(prefab);
                GameObject currentPooledObject = Instantiate(prefab, this.transform);
                pooledObjects.Add(currentPooledObject);
                currentPooledObject.SetActive(startActive);
            }
        }

        [UnhollowerBaseLib.Attributes.HideFromIl2Cpp]
        public GameObject GetObjectAtIndex(int index)
		{
            return objects[index];
		}

        [UnhollowerBaseLib.Attributes.HideFromIl2Cpp]
        public GameObject GetFirstObjectActive(bool active)
		{
			if (active == true)
			{
                return pooledObjects.FirstOrDefault((obj) => obj.activeInHierarchy);
            }
			else
			{
                return pooledObjects.FirstOrDefault((obj) => !obj.activeInHierarchy);
            }
		}
	}
}
