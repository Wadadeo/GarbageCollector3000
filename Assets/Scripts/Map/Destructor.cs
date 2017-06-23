using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NObjects;
using NEngine;

namespace NMap
{
    [System.Obsolete]
    public class Destructor : MonoBehaviour
    {
        void OnTriggerEnter2D(Collider2D coll)
        {
            SpaceObject obj = coll.gameObject.GetComponent<SpaceObject>();
         //   Debug.Log(coll.name + " trying to destroy....");
            ObjectHandler.singleton.PoolObject(obj);
    //        Debug.Log(obj.name + " was destroyed by the map");
        }

        public void zoomIn(float z)
        {
            transform.Translate(z, 0, 0);
        }

        public void zoomOut(float z)
        {
            transform.Translate(-z, 0, 0);
        }
    }
}