using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace NObjects
{
	public class Missile : SpaceObject {

		private float rotateSpeed = 5f;
		private float moveSpeed = 8f;

		private Transform _target;

			
		void FixedUpdate ()
        {
			if (!_target)
            {
				MoveInCurrentOrientation (); //move forward
				return;
			}
			RotateTowardTarget (_target, rotateSpeed);
			MoveTowardTarget (_target, moveSpeed);
		}


		public void setTarget(Transform target) { _target = target; }

		void MoveInCurrentOrientation() 
		{
			float angle = transform.rotation.eulerAngles.z;

			angle = (angle + 90f) * Mathf.Deg2Rad;
			Vector2 dir = new Vector2 (Mathf.Cos (angle), Mathf.Sin (angle)).normalized;

			_body.velocity = dir * moveSpeed;
		}

		protected override void OnDamageApplied(IObject target)
		{
			base.OnStopped();
			SelfDestroy ();
		}
	}
}
