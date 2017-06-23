using System.Collections;
using UnityEngine;

namespace NEngine
{
    public class SkyboxHandler : MonoBehaviour
    {
        public float rotationSpeed = 1f;

        public float minExposure = 1f;
        public float maxExposure = 2f;
        public float exposureRefreshTime = 2f;
        [Range(0.1f, 10f)]
        public float exposureSpeed = 1f;

        [Header("Debug Print")]
        [SerializeField]
        private float targetExposure;
        [SerializeField]
        private float currentExposure;
        private float speed;
        private float lastExposure;
        private float expDelta;

        #region MonoBehaviour

        void Start()
        {
            currentExposure = RenderSettings.skybox.GetFloat("_Exposure");
            StartCoroutine(Cor_HandleExposure());
            expDelta = Time.fixedDeltaTime * exposureSpeed;
        }

        void FixedUpdate()
        {
            lastExposure = currentExposure;

            RenderSettings.skybox.SetFloat("_Rotation", Time.time * rotationSpeed);
            if (currentExposure > targetExposure)
            {
                currentExposure -= expDelta;
                if (currentExposure < targetExposure)
                    currentExposure = targetExposure;
            }
            else if (currentExposure < targetExposure)
            {
                currentExposure += expDelta;
                if (currentExposure > targetExposure)
                currentExposure = targetExposure;
            }
            if (lastExposure != currentExposure)
                RenderSettings.skybox.SetFloat("_Exposure", currentExposure);
        }

        #endregion

        #region Private

        private IEnumerator Cor_HandleExposure()
        {
            while (true)
            {
                targetExposure = Random.Range(minExposure, maxExposure);
                yield return new WaitForSeconds(exposureRefreshTime);
            }
        }

        #endregion
    }
}