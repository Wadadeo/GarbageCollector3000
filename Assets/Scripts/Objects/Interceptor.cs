using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NObjects
{
	public class Interceptor : SpaceShip
    {
        /*[SerializeField]
        [Range(1f, 10f)]
        private float firePlayerDistance = 3f;

        [SerializeField]
        [Range(0.1f, 5f)]
        private float fireRate = 0.5f;

        [SerializeField]
        [Range(0.1f, 5f)]
        private float speed = 1f;

        [SerializeField]
        [Range(0.1f, 5f)]
        private float rotationSpeed = 1f;

        private float nextFireAt = 0;*/
//        private bool assimilated = false;


		//private Transform _player;
        private ParticleSystem[] _effects;

        #region MonoBehaviour

        protected override void Awake()
        {
            base.Awake();
            _effects = GetComponentsInChildren<ParticleSystem>();
            //init(); //[DEBUG] : for test scene
        }

        public override void Init()
        {
            base.Init();
            //GameObject instancePlayer = GameObject.FindGameObjectWithTag("Player");
            //if (instancePlayer)
            //    _player = instancePlayer.transform;
        }

        private void FixedUpdate()
        {
            /*if (!_player)
                return;
            float distToPlayer = Vector3.Distance(transform.position, _player.position);
            if (!assimilated)
                FacePlayer();
            if (distToPlayer > firePlayerDistance)
            {
                Vector3 direction = _player.position - transform.position;
                _body.velocity = direction.normalized * speed;
            }
            else
            {
                _body.velocity = Vector2.zero;
                FireAtPlayer();
            }*/

			if (!_player) {
				return;
			}
			updateFireCoolDown();

		//	if (!assimilated)
				RotateTowardTarget(_player, rotationSpeed);

			if (inRangeToFire (_player)) {
				if (canFire) {
					FireAtPlayer ();
					canFire = false;
				}
				_body.velocity = Vector2.zero;
			} else {

				MoveTowardTarget (_player, moveSpeed);
			}
        }


        #endregion


     // void FacePlayer()
     // {
     //     if (!_player) return;
     //     Vector3 dir = (_player.position - transform.position).normalized;
     //     //float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
     //     //angle -= 90; //assume that sprite "face" is oriented top
     //     //transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
     //
     //     transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z);
     //     //create the rotation we need to be in to look at the target
     //     Quaternion lookRotation = Quaternion.LookRotation(dir);
     //
     //     //rotate us over time according to speed until we are in the required rotation
     //     transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
     //
     // }

        void FireAtPlayer()
        {
            //if (Time.time > nextFireAt)
            //{
            //    nextFireAt = Time.time + fireRate;
            foreach (ParticleSystem effect in _effects)
            {
                Vector3 rotParticle = transform.rotation.eulerAngles;
                rotParticle.z *= -1;
                rotParticle *= Mathf.Deg2Rad;
                effect.Stop();
                ParticleSystem.MainModule main = effect.main;
                main.startRotationX = rotParticle.x;
                main.startRotationY = rotParticle.y;
                main.startRotationZ = rotParticle.z;
                effect.Play();
            }
            //}
        }

    //  protected override void OnAssimilation(IObject newParent)
    //  {
    //      base.OnAssimilation(newParent);
    //      if (newParent.tag == "Player")
    //          assimilated = true;
    //  }

        protected override void OnDamageApplied(IObject target)
        {
            base.OnDamageApplied(target);
        //    Debug.Log(objectData.damage + " damages applied by Interceptor : [" + name + "] to " + target.name);

        }


    }
}
