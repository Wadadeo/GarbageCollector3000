using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NEngine;

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
        protected int currentHealth;
        [SerializeField]
        protected int currentMass;
        protected Transform mainTransform;
        protected bool isDestroyed;

        #region MonoBehaviour

        protected virtual void Awake()
        {
            _collider = GetComponent<Collider2D>();
            _transform = transform;
            _body = GetComponent<Rigidbody2D>();
        }

        #endregion

        #region Main methods

        public virtual void init()
        {
            currentHealth = objectData.health;
            currentMass = objectData.mass;
            mainTransform = transform;
            isDestroyed = false;
        }

        public virtual void stop()
        {
            _body.velocity = Vector2.zero;
        }

        protected virtual void applyDamage(int damage)
        {
            currentHealth -= damage;
            if (currentHealth <= 0)
                selfDestroy();
        }

        protected virtual bool selfDestroy()
        {
            if (isDestroyed)
                return false;
            int n = mainTransform.childCount;

            for (int i = 0; i < n; i++)
            {
                IObject obj = mainTransform.GetChild(i).GetComponent<IObject>();
                if (obj)
                    obj.selfDestroy();
            }
            ParticleSystem particles = Instantiate(objectData.explosionPrefab, mainTransform.position, Quaternion.identity);
            Destroy(particles.gameObject, particles.main.startLifetime.constantMax);
            isDestroyed = true;
            return true;
        }

        protected virtual void getAssimilated(IObject obj)
        {
            _transform.SetParent(obj.mainTransform);
            stop();
            Debug.Log(objectData.name + " got assimilated by " + obj.objectData.name);
        }

        public virtual void addChild(IObject newChild)
        {
            currentMass += newChild.currentMass;
        }

        #endregion

        #region Getter

        public int getCurrentMass()
        {
            return currentMass;
        }

        #endregion

        #region CallBacks

        protected virtual void OnCollisionEnter2D(Collision2D coll)
        {
            Debug.Log(name + " collided wth " + coll.gameObject.name);
            IObject obj = coll.gameObject.GetComponent<IObject>();

            if (!obj)
            {
                Debug.LogError("Wrong collision Detection");
                return;
            }
            if (obj.currentMass >= currentMass * 2)
            {
                IObject newFather = coll.contacts[coll.contacts.Length - 1].rigidbody.gameObject.GetComponent<IObject>();
                getAssimilated(newFather);
            }
            else if (obj.currentMass * 2 > currentMass)
                applyDamage(obj.objectData.damage);
        }

        protected virtual void OnParticleCollision(GameObject other)
        {
            IObject obj = other.GetComponentInParent<IObject>();
            if (obj)
            {
                Debug.Log("Particle emmited by " + obj.name + "hit " + name);
                applyDamage(obj.objectData.damage);
            }
        }

        #endregion
    }
}