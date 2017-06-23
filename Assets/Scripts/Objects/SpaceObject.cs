using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NEngine;
using System;

namespace NObjects
{
    public class SpaceObject : IObject
    {
        protected Persistent.SpaceObjectData spaceData;
        protected float totalTimer;
        [Header("Debug")]
        [SerializeField]
        protected float currentTimer;
        [Header("Rigidbody Option")]
        [SerializeField]
        protected RigidbodyType2D stoppedBodyType = RigidbodyType2D.Kinematic;
        protected bool isStopped = false;
        protected RigidbodyType2D bodyType;

        #region MonoBehaviour

        protected override void Awake()
        {
            base.Awake();
            spaceData = objectData as Persistent.SpaceObjectData;
            totalTimer = spaceData.aliveTime;
            currentTimer = totalTimer;
            bodyType = _body.bodyType;
        }

        protected virtual void OnBecameVisible()
        {
            currentTimer = totalTimer;
            StopAllCoroutines();
        }

        protected virtual void OnBecameInvisible()
        {
            if (gameObject.activeInHierarchy && totalTimer != -1f)
                StartCoroutine(Cor_Timer());
        }

        #endregion

        #region Main

        protected virtual IEnumerator Cor_Timer()
        {
            while (currentTimer > 0f)
            {
                yield return new WaitForSeconds(1f);
                currentTimer -= 1f;
            }
         //   Debug.LogWarning("Object Timeout detruction");
            OnDestruction();
        }

		protected virtual void RotateTowardTarget(Transform target, float speed)
		{
            if (!target || isStopped)
            {
                return;
            }
			Vector3 dir = (target.position - transform.position).normalized;

			float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
			angle -= 90; //assume that sprite "face" is oriented top
			Quaternion lookRotation = Quaternion.AngleAxis(angle, Vector3.forward);

			transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.fixedDeltaTime * speed);

		}
        
		protected virtual void MoveTowardTarget(Transform target, float speed)
		{
			if (!target || isStopped)
                return;
			Vector3 direction = target.position - transform.position;
			_body.velocity = direction.normalized * speed;
		}

        #endregion

        #region overrides
        
        protected override void OnDestruction()
        {
            base.OnDestruction();
            ObjectHandler.singleton.PoolObject(this);
        }

        protected override void OnResume()
        {
            base.OnResume();
            _body.bodyType = bodyType;
            isStopped = false;
        //    _body.simulated = true;
        }

        protected override void OnStopped()
        {
            base.OnStopped();
            _body.bodyType = stoppedBodyType;
            isStopped = true;
         //   _body.simulated = false;
            _body.velocity = Vector2.zero;
            _body.angularVelocity = 0f;
        }

        #endregion

        #region Getters

        public float GetHostilityModifier()
        {
            return spaceData.HostilityModifier;
        }

        public string GetObjectName()
        {
            return spaceData.objectName;
        }

        public string GetObjectDescription()
        {
            return spaceData.objectDescription;
        }

        public eObjectType GetObjectType()
        {
            return spaceData.objectType;
        }
             
        #endregion
    }
}