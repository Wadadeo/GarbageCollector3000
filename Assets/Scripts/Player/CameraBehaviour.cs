using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NEngine;

namespace NPlayer
{
    public class CameraBehaviour : MonoBehaviour
    {
        [Range(1f, 1000f)]
        [SerializeField]
        private float baseCameraDistance = 15;
        [SerializeField]
        private float maxDistanceScalingPerFrame = 0.01f;
        private float currentScale = 1;
        private float targetScale = 1;

        private Camera _cam;

        void Awake()
        {
            this._cam = this.GetComponent<Camera>();
            Vector3 pos = this._cam.transform.position;
            this._cam.transform.position = new Vector3(pos.x, pos.y, -baseCameraDistance);
        }

        void Update()
        {
            if (this.currentScale < this.targetScale)
            {
                if (this.targetScale - this.currentScale < this.maxDistanceScalingPerFrame)
                    this.currentScale = this.targetScale;
                else
                    this.currentScale += this.maxDistanceScalingPerFrame;
            }
            else if (this.currentScale > this.targetScale)
            {
                if (this.currentScale - this.targetScale < this.maxDistanceScalingPerFrame)
                    this.currentScale = this.targetScale;
                else
                    this.currentScale -= this.maxDistanceScalingPerFrame;
            }
            else
                return;
            Vector3 pos = this._cam.transform.position;
            this._cam.transform.position = new Vector3(pos.x, pos.y, -this.baseCameraDistance * this.currentScale);
        }

        public void changeCameraScale(float scale)
        {
            this.targetScale = scale;
        }
    }
}
