using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NObjects
{
    namespace Persistent
    {
        [CreateAssetMenu(fileName = "Space Object List", menuName = "new space object List")]
        public class SpaceObjectDataHandler : ScriptableObject
        {
            [Header("Object List")]
            [SerializeField]
            private List<SpaceObjectData> objects;

            public List<SpaceObjectData> GetObjects()
            {
                return objects;
            }
        }
    }
}
