using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NObjects
{
	public class SpaceShip : SpaceObject {

		[SerializeField]
		[Range(0.1f, 5f)]
		private float fireRate = 0.5f;
		private float nextFireAt;


		[SerializeField]
		[Range(1f, 50f)]
		private float fireRange = 10f;

		[SerializeField]
		[Range(0.1f, 5f)]
		protected float moveSpeed = 1f;

		[SerializeField]
		[Range(0.1f, 5f)]
		protected float rotationSpeed = 1f;

		protected Transform _player;

		protected bool canFire = false;


		protected override void Awake()
		{
			base.Awake();
			GameObject instancePlayer = GameObject.FindGameObjectWithTag("Player");
			if (instancePlayer)
				_player = instancePlayer.transform;
			nextFireAt = fireRate;
		}

		protected void updateFireCoolDown()
		{
			if (Time.time >= nextFireAt && !canFire)
			{
				nextFireAt = Time.time + fireRate;
				canFire = true;
			}
		}

		protected bool inRangeToFire(Transform target)
		{
			float distToTarget = Vector3.Distance(transform.position, target.position);
			return (distToTarget <= fireRange);
		}

	}
}
