using UnityEngine;

namespace NObjects
{
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    public abstract class IObject : MonoBehaviour
    {
        protected Collider2D _collider;
        protected Transform _transform;
        protected Rigidbody2D _body;

        [Header("Persistent Data")]
        [SerializeField]
        protected Persistent.ObjectData objectData;
        [Header("Gameplay ")]
        [SerializeField]
        protected byte currentHealth;
        [SerializeField]
        private int currentMass;
        protected bool isDestroyed;
        protected Transform mainTransform;

        protected Vector3 initialScale;

        #region MonoBehaviour

        protected virtual void Awake()
        {
            _collider = GetComponent<Collider2D>();
            _transform = transform;
            _body = GetComponent<Rigidbody2D>();
            initialScale = _transform.localScale;
        }

        #endregion

        #region Public

        /// <summary>
        /// Initialize the object for correct cleanup after instantiation or pooling
        /// Also Resumes the object
        /// </summary>
        public virtual void Init()
        {
            currentHealth = (byte) objectData.health;
            currentMass = objectData.mass;
            isDestroyed = false;
            mainTransform = _transform;
            _transform.localScale = initialScale;
            Resume();
        }

        /// <summary>
        /// Stop the object behaviour, stopping process is specific to each object
        /// </summary>
        public void Stop()
        {
            OnStopped();
        }

        /// <summary>
        /// Opposed to Stop(), resumes the object behaviour, resume process is specific to each object
        /// </summary>
        public void Resume()
        {
            _transform.SetParent(null);
            OnResume();
        }

        #endregion

        #region Protected

        protected void ReceiveDamage(byte damage)
        {
         //   Debug.Log(name + " received " + damage + " damage versus health = " + currentHealth);
            if (damage > currentHealth)
                damage = currentHealth;
         //   Debug.Log("corrected: " + name + " received " + damage + " damage versus health = " + currentHealth);
            currentHealth -= damage;
            OnDamageReceived(damage);
        }

        protected bool SelfDestroy()
        {
            if (isDestroyed)
                return false;

            IObject[] childs = _transform.GetComponentsInChildren<IObject>();

            foreach (IObject item in childs)
            {
                item.OnAssimilationStopped();
            }
            ParticleSystem particles = Instantiate(objectData.explosionPrefab, _transform.position, Quaternion.identity);
            Destroy(particles.gameObject, particles.main.startLifetime.constantMax);
            isDestroyed = true;
            OnDestruction();
            return true;
        }
        
        #endregion

        #region Getter

        public int GetCurrentHealth()
        {
            return currentHealth;
        }

        public int GetCurrentMass()
        {
            int mass = currentMass;
            IObject tmp;

            if (!mainTransform)
                return mass;
            foreach (Transform t in mainTransform)
            {
                if (tmp = t.gameObject.GetComponent<IObject>())
                {
                    mass += tmp.GetCurrentMass();
                }
            }
            return mass;
        }

        public int GetRootMass()
        {
            return GetRootObj().GetCurrentMass();
        }

        public IObject GetRootObj()
        {
            Transform tmp = _transform.root;
            IObject rootObj;

            if (tmp == _transform)
                return this;
            rootObj = tmp.GetComponent<IObject>();
            if (rootObj)
                return (rootObj);
            else
            {
                Debug.LogWarning("root of " + tmp + " is not an IObject: " + rootObj);
                return this;
            }
        }

        #endregion

        #region Hooks
        
        /// <summary>
        /// Hook sent when the object is stopped (assimilation, pause, etc)
        /// </summary>
        protected virtual void OnStopped()
        {

        }

        /// <summary>
        /// Hook sent hen the object is resumed to a game state (deassimilation, intialisation,...)
        /// </summary>
        protected virtual void OnResume()
        {

        }

        /// <summary>
        /// Hook sent when damae is applied to this object. this Hook leads to destruction
        /// </summary>
        /// <param name="amount"> amount of damage</param>
        protected virtual void OnDamageReceived(byte amount)
        {
            if (currentHealth <= 0)
                SelfDestroy();
        }

        /// <summary>
        /// Hook sent when damage is applied to target object.
        /// </summary>
        /// <param name="target">object receiving the damage</param>
        protected virtual void OnDamageApplied(IObject target)
        {

        }

        /// <summary>
        /// Hook sent when the object is assimilated by an other object, defines default assimilation behavior
        /// </summary>
        /// <param name="newParent">Assimilating object, the new parent</param>
        protected virtual void OnAssimilation(IObject newParent)
        {
            _transform.SetParent(newParent.mainTransform);
            Stop();
            newParent.OnNewChild(this);
        }

        /// <summary>
        /// Hook sent when the object is no longer assimilated by an other object, defines default behavior
        /// </summary>
        protected virtual void OnAssimilationStopped()
        {
            if (!isDestroyed)
                Resume();
        }

        /// <summary>
        /// Calback sent when the objects assimilates an other object.
        /// </summary>
        /// <param name="child">The assimilated object</param>
        protected virtual void OnNewChild(IObject child)
        {
            IObject root = GetRootObj();
            if (root != this)
                root.OnNewChild(child);
        }

        /// <summary>
        /// Hook sent when the object is destroyed within the gameplay, implement pooling here
        /// </summary>
        protected virtual void OnDestruction()
        {
            IObject root = GetRootObj();
            if (root != this)
                root.OnLoseChild(this);
        }

        /// <summary>
        /// Hook sent when the object looses a child
        /// </summary>
        /// <param name="child"></param>
        protected virtual void OnLoseChild(IObject child)
        {

        }

        protected virtual void OnCollisionEnter2D(Collision2D coll)
        {
          //  if (coll.transform.tag == "Player")
          //      Debug.Log(name + " collided wth " + coll.gameObject.name);
            IObject obj = coll.gameObject.GetComponent<IObject>();

            if (!obj || !obj.gameObject.activeInHierarchy)
            {
                Debug.LogWarning("Wrong collision Detection");
                return;
            }
            if (obj.GetCurrentMass() == 0)
            {
                ReceiveDamage(obj.objectData.damage);
                obj.OnDamageApplied(this);
                return;
            }
            if (GetRootMass() > obj.GetRootMass())
            {
                obj.OnAssimilation(this);
                return;
            }
        }

        protected virtual void OnParticleCollision(GameObject other)
        {
		//	Debug.Log("In "+ name +", Particle collision with " + other.name);
		//	if (particleHitDestructor (other))
	//			return;
            IObject obj = other.GetComponent<IObject>();
            if (!obj)
                obj = other.GetComponentInParent<IObject>();
            if (obj)
            {
			//	Debug.Log("Particle emmited by " + obj.name + "hit " + name + " [-" + obj.objectData.damage + "]");
                ReceiveDamage(obj.objectData.damage);
                obj.OnDamageApplied(this);
            }
        }

        #endregion
    }
}