using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NObjects
{
	public class Destroyer : SpaceShip
    {

		public GameObject missilePrefab;
		private Transform canon;


		void Start() 
		{
			canon = transform.GetChild (0);
		}

        // Update is called once per frame
        void FixedUpdate()
        {
			if (!_player) {
				//Debug.Log ("No player set");
				return;
			}
			RotateTowardTarget(_player, rotationSpeed);

			if (inRangeToFire (_player))
            {
				updateFireCoolDown();
				if (canFire)
                {
					FireSearchingMissile ();
					canFire = false;
				}
				_body.velocity = Vector2.zero;
			}
            else
            {
				MoveTowardTarget (_player, moveSpeed);
			}

        }

		void FireSearchingMissile()
		{
			GameObject missile = Instantiate (missilePrefab, canon.position, transform.rotation);
			missile.GetComponent<Missile> ().setTarget (_player);
			//do something else with missile ?
		}
    }
}
