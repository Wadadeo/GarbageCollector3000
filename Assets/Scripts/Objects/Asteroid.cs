using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NObjects
{
    public class Asteroid : SpaceObject
    {
        public Vector2 initialDirection;

        [SerializeField]
        [Range(0.1f, 5f)]
        private float speed = 0.5f;
        bool stopped = false;

        #region Overrides

        public override void Init()
        {
            base.Init();
            stopped = false;
           // _body.velocity = initialDirection.normalized * speed;
        }

        protected override void OnResume()
        {
            base.OnResume();
            stopped = false;
        }

        protected override void OnStopped()
        {
            base.OnStopped();
            stopped = true;
        }

        #endregion

        #region MonoBehaviour

        private void Update()
        {
            if (stopped)
                return;
          //  _body.velocity = initialDirection.normalized * speed;
        }

     //  //[DEBUG] Draw the initial direction of the asteroid
     //  private void OnDrawGizmos()
     //  {
     //      Vector3 normalizedDir = initialDirection.normalized;
     //      Gizmos.DrawLine(transform.position, transform.position + normalizedDir);
     //  }
     //
        #endregion


    }
}