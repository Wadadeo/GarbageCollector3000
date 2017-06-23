using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NPlayer;
using NEngine;
using NObjects;
using NObjects.Persistent;

namespace NMap
{
    //TODO: when an ennemy dies, notify this class (call EntityDestroyed())
    public class Generation : MonoBehaviour
    {
        public static Generation singleton;

        [SerializeField]
        private Transform player;
        [SerializeField]
        private ObjectHandler handler;
        [SerializeField]
        private CameraBehaviour cam;

        public float spawnDistance;

        [SerializeField]
        private int minSpawnPerWave;
        [SerializeField]
        private int maxSpawnPerWave;

        [SerializeField]
        private float minTimeBetweenWaves;
        [SerializeField]
        private float maxTimeBetweenWaves;

        [SerializeField]
        private SpaceObjectDataHandler objectData;

        private Dictionary<eObjectType, int> _entitiesCount;
        private float _hostility = 0;
        private float _distanceMultiplier = 1;

        public void init(GameEngine engine)
        {

            engine.hostilityChangedEvent += this.hostilityChanged;
            engine.massChangedEvent += this.massChanged;
        }

        public float massToDistanceMultiplier(int mass)
        {
            return Mathf.Sqrt(Mathf.Sqrt(mass));
        }

        void Awake()
        {
            singleton = this;
            _entitiesCount = new Dictionary<eObjectType, int>();
            eObjectType e = (eObjectType)0;
            while (e < eObjectType.MAX)
            {
                _entitiesCount.Add(e, 0);
                e++;
            }
        }

        void Update()
        {
        }

        public void pause()
        {
            StopAllCoroutines();
        }

        public void resume()
        {
            StartCoroutine(this.Cor_Spawn());
        }

        public void EntityDestroyed(eObjectType type)
        {
            --this._entitiesCount[type];
        }

        #region private

        private void hostilityChanged(float hostility)
        {
            this._hostility = hostility;
        }

        private void massChanged(int mass)
        {
            this._distanceMultiplier = this.massToDistanceMultiplier(mass);
            this.cam.changeCameraScale(this._distanceMultiplier);
        }

        private IEnumerator Cor_Spawn()
        {
            while (true)
            {
                this.spawn();
                yield return new WaitForSeconds(Random.Range(this.minTimeBetweenWaves, this.maxTimeBetweenWaves));
            }
        }

        private float calculateSpawnChances(SpaceObjectData item)
        {
            return item.hostilitySpawnModifier > 0 ? item.spawnChances * this._hostility * item.hostilitySpawnModifier :  item.spawnChances;
        }

        private void spawn()
        {
            int nSpawned = (int)(((float)Random.Range(this.minSpawnPerWave, this.maxSpawnPerWave)) * this._distanceMultiplier);
            if (nSpawned == 0)
                return;
            List<SpaceObjectData> spawnable = new List<SpaceObjectData>();
            float totalSpawnChances = 0;
            foreach (SpaceObjectData item in this.objectData.GetObjects())
            {
                if (this._hostility >= item.requiredHostility)
                {
                    spawnable.Add(item);
                    totalSpawnChances += calculateSpawnChances(item);
                }
            }
            for (int i = 0; i < nSpawned; ++i)
                spawnOne(totalSpawnChances, spawnable);
        }

        private void spawnOne(float totalSpawnChances, List<SpaceObjectData> spawnable)
        {
           float n = Random.Range(0, totalSpawnChances - 1);
           float total = 0;
           foreach (SpaceObjectData item in spawnable)
           {
                total += calculateSpawnChances(item);
                if (n < total)
                {
                    spawnEntity(item.objectType, this.angleToSpawnPoint(Random.Range(0, Mathf.PI)));
                    return;
                }
           }
        }

        private Vector2 angleToSpawnPoint(float angle)
        {
            int sign = Vector3.Cross(new Vector2(-1, 0), (Vector2)this.player.up).z < 0 ? -1 : 1;
            angle += sign * Vector2.Angle(new Vector2(-1, 0), (Vector2)this.player.up) * Mathf.Deg2Rad;
            return (Vector2)this.player.position + this.spawnDistance * this._distanceMultiplier * new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        }

        private void spawnEntity(eObjectType type, Vector2 position)
        {
            ++this._entitiesCount[type];
            SpaceObject entity = this.handler.GetObjectFromPool(type, position);
            entity.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
        }

        #endregion
    }
}
