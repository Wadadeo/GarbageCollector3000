using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NObjects;
using NObjects.Persistent;

namespace NEngine
{
    public class ObjectHandler : MonoBehaviour
    {
        public static ObjectHandler singleton;

        [Header("Object pooling")]
        [SerializeField]
        private SpaceObjectDataHandler objectData;
        [SerializeField]
        private Transform pooledTransform;

        private Dictionary<eObjectType, SpaceObject> objectsToPool;
        private Dictionary<eObjectType, List<SpaceObject>> pools;

        #region MonoBehaviour

        void Awake()
        {
            singleton = this;
            objectsToPool = new Dictionary<eObjectType, SpaceObject>();
            pools = new Dictionary<eObjectType, List<SpaceObject>>();
        }

        void Start()
        {
            SpaceObject tmp;
            eObjectType eTmp;
      //     if (!checkUp()) //////////////////
      //         Debug.LogError("Check up failed, please give correct parameters to the ObjectHandler");
            List<SpaceObjectData> tmpData = objectData.GetObjects();
            for (int i = 0; i < tmpData.Count; i++)
            {
                eTmp = tmpData[i].objectType;
                objectsToPool.Add(eTmp, tmpData[i].objectPrefab);
                pools.Add(eTmp, new List<SpaceObject>());
                for (int j = 0; j < tmpData[i].initialNumberOfInstances; j++)
                {
                    tmp = Instantiate(objectsToPool[eTmp], pooledTransform);
                    tmp.gameObject.SetActive(false);
                    pools[eTmp].Add(tmp);
                }
            }
        }

        #endregion

        #region Public

        /// <summary>
        /// Creates or gets an object from the pool and puts it to the given coordinates
        /// </summary>
        /// <param name="index">The object type to get</param>
        /// <param name="coordinates">The new position of the object</param>
        /// <returns>A new clean SpaceObject</returns>
        public SpaceObject GetObjectFromPool(eObjectType index, Vector2 coordinates)
        {
            if ((int)index >= pools.Count)
            {
                Debug.LogError("The type of object " + index + " is past the index");
                return null;
            }
            foreach (SpaceObject item in pools[index])
            {
                if (item.gameObject.activeInHierarchy == false)
                {
                    item.transform.position = coordinates;
                    item.gameObject.SetActive(true);
                    item.transform.SetParent(null);
                    item.GetComponent<SpaceObject>().Init();
                    return item;
                }
            }
            SpaceObject tmp = Instantiate(objectsToPool[index], coordinates, Quaternion.identity);
            pools[index].Add(tmp);
            tmp.Init();
            return tmp;
        }

        /// <summary>
        /// Puts the object to the pool and deactivates it, no garbage collection call
        /// </summary>
        /// <param name="obj">The object to pool</param>
        public void PoolObject(SpaceObject obj)
        {
            NMap.Generation.singleton.EntityDestroyed(obj.GetObjectType());
            obj.gameObject.SetActive(false);
            obj.StopAllCoroutines();
            obj.transform.SetParent(pooledTransform);
        }

        #endregion
    }
}
