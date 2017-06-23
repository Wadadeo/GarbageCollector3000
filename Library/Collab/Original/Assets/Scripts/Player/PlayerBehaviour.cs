using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NEngine;
using NObjects;

namespace NPlayer
{
    public class PlayerBehaviour : IObject
    {
        private Rigidbody2D _body;
        private Transform _transform;

        private GameEngine engine;

        [Header("Customization")]
        [SerializeField]        private float xSpeed = 1f;
        [SerializeField]        private float ySpeed = 1f;
        [SerializeField]        private float massMultiplier = 0.3f;
        [Range(0f, 1f)]
        [SerializeField]        private float rotationRatio = 0.1f;
        [SerializeField]        private Transform mainObject = null;
        [Header("In Game Characteristics")]
        [SerializeField]        private int stickedObjectLength = 0;
        [SerializeField]        private bool paused = true;

        #region MonoBehaviour

        void Awake()
        {
            _body = GetComponent<Rigidbody2D>();
            _transform = GetComponent<Transform>();
            engine = GameEngine.singleton;
        }
        
        void Update()
        {
            if (paused)
                return;
            Vector2 next_pos = Vector2.zero;

            next_pos.x = Input.GetAxis("Horizontal");
            if (next_pos.x == 0 && Input.touchCount > 0)
            {
                Debug.LogWarning("Touch Input");
                next_pos.x = Input.touches[0].position.x - Screen.width / 2;
                next_pos.Normalize();
            }
            next_pos.x *= xSpeed;
            next_pos.y = ySpeed + (massMultiplier * stickedObjectLength);
            _body.velocity = _body.transform.right * next_pos.y + _body.transform.up * next_pos.x;
            _transform.Rotate(Vector3.forward, next_pos.x * rotationRatio);
            mainObject.Rotate(Vector3.forward, next_pos.x);
        }

        #endregion

        #region Public

        public override void init()
        {
            base.init();
            resume();
        }

        public void resume()
        {
            paused = false;
        }

        public void stop()
        {
            paused = true;
        }

        #endregion

        #region Main

        protected override void applyDamage(int damage)
        {
            base.applyDamage(damage);
            engine.pauseGame();
        }

        #endregion
    }
}
