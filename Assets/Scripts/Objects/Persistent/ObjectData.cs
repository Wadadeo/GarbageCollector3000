using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NObjects
{
    namespace Persistent
    {
        [CreateAssetMenu(menuName = "new object", fileName = "object")]
        public class ObjectData : ScriptableObject
        {
            [Header("Essentials")]
            [Range(1f, 10f)]
            [Tooltip("How much damage can it take until destruction")]
            public byte health = 1;
            [Range(0f, 20f)]
            [Tooltip("damage applied to any object at collision")]
            public byte damage = 0;
            [Range(0f, 100f)]
            [Tooltip("the assimilation difficulty of the object, 0 means no assimilation")]
            public int mass = 1;
            [Header("Customisation")]
            public ParticleSystem explosionPrefab;
        }
    }
}