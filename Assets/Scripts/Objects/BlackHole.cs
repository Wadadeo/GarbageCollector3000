using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NObjects
{
    public class BlackHole : SpaceObject
    {
        private Coroutine runningCoroutine;

        private List<GameObject> absorbingObjects;
        private List<Vector3> originalScales;
        private List<float> startDistances;

        #region MonoBehaviour

        protected override void Awake()
        {
            base.Awake();
            absorbingObjects = new List<GameObject>();
            originalScales = new List<Vector3>();
            startDistances = new List<float>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
         //   Debug.Log(other.gameObject.name + " enter black hole");
            absorbingObjects.Add(other.gameObject);
            originalScales.Add(other.gameObject.transform.localScale);
            startDistances.Add(Vector3.Distance(other.gameObject.transform.position, _transform.position));
            //  if (runningCoroutine != null)
            //      StopCoroutine(runningCoroutine);
            //  runningCoroutine = StartCoroutine(AbsorbPlayer(other.gameObject));
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            int i = absorbingObjects.FindIndex(x => x == collision.gameObject);
            if (i >= 0)
                ClearItem(i);
        }

        private void FixedUpdate()
        {
            if (absorbingObjects.Count <= 0)
                return;

            float distance;
            float ratio;

            for (int i = 0; i < absorbingObjects.Count; i++)
            {
                if (!absorbingObjects[i] || !absorbingObjects[i].activeInHierarchy)
                {
                    ClearItem(i);
                    continue;
                }
                distance = Vector3.Distance(absorbingObjects[i].transform.position, _transform.position);
                if (distance > startDistances[i])
                {
                    ClearItem(i);
                    continue;
                }
                ratio = Mathf.Clamp(distance / startDistances[i], 0.2f, 1f);
                absorbingObjects[i].transform.localScale = originalScales[i] * ratio;
            }
        }

        #endregion

        private void ClearItem(int i)
        {
       //     if (absorbingObjects[i])
       //         absorbingObjects[i].transform.localScale = originalScales[i];
            absorbingObjects.RemoveAt(i);
            startDistances.RemoveAt(i);
            originalScales.RemoveAt(i);
        }

        protected override void OnCollisionEnter2D(Collision2D coll)
        {
         //   Debug.LogWarning("BlackHole collided with " + coll.gameObject.name);
            base.OnCollisionEnter2D(coll);
        }

        //IEnumerator AbsorbPlayer(GameObject objectToAbsorb)
        //{
        //    float startDistance = Vector3.Distance(objectToAbsorb.transform.position, transform.position);
        //    Vector3 startScale = objectToAbsorb.transform.localScale;
        //    float ratio = 1f;
        //    float distance = startDistance;
        //
        //    while (distance <= startDistance && ratio <= 1f)
        //    {
        //        if (!objectToAbsorb.activeInHierarchy)
        //        {
        //            objectToAbsorb.transform.localScale = startScale;
        //            break;
        //        }
        //        Vector3 scale = startScale * ratio;
        //        objectToAbsorb.transform.localScale = scale;
        //
        //        distance = Vector3.Distance(objectToAbsorb.transform.position, transform.position);
        //        ratio = distance / startDistance;
        //        yield return null;
        //    }
        //    if (objectToAbsorb.transform.localScale != startScale)
        //    {
        //        // Debug.LogError(objectToAbsorb.name + " not correctly scaled");
        //        objectToAbsorb.transform.localScale = startScale;
        //    }
        //}

    }
}
