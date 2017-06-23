using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NObjects
{
    public enum eObjectType
    {
        Meteorite,
        Satellite,
        Satellite2,
        Asteroid,
		BlackHole,
        Interceptor,
		Destroyer,
        Mine,
        RadioactiveBarrel,
        MAX
    }

    namespace Persistent
    {
        [CreateAssetMenu(fileName = "Space Object", menuName = "new space object")]
        public class SpaceObjectData : ObjectData
        {
            [Header("Engine")]
            [Space()]
            [Header("Spawn information")]
            public SpaceObject objectPrefab;
            [Tooltip("Start number of pooled objects")]
            public int initialNumberOfInstances = 0;
            [Header("Generation Information")]
            public int spawnChances = 1;
            [Range(0f, 1f)]
            public float requiredHostility = 0;
            [Tooltip("The higher it is, the more often the object will spawn when hostility goes up")]
            public float hostilitySpawnModifier = 0;
            public eObjectType objectType;
            [Space()]
            [Header("Gameplay Characteristics")]
            [Range(0f, 3f)]
            [Tooltip(" 0 means it is simply on orbit, 1 2 and 3 means it goes towards the player in different speeds")]
            public int speed = 0;
            [Tooltip("Percentage of hostility modified when assimilated")]
            [Range(-1f, 1f)]
            public float HostilityModifier;
            [Header("Object Characteristics")]
            public string objectName = "Unknown";
            [Multiline]
            public string objectDescription = "A Space Object";
            public Sprite objectIcon;
            [Range(-1, 60)]
            [Tooltip("-1 means no timer, timer launches when the object becomes invisible to the camera")]
            public int aliveTime = 15;
        }
    }
}