using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NEngine;
using NObjects;

namespace NPlayer
{
    public class PlayerBehaviour : IObject
    {        
        [Header("Customization")]
        [SerializeField]        private float xSpeed = 1f;
        [SerializeField]        private float zRotationSpeed = 15f;
        [SerializeField]        private float ySpeed = 1f;
        [SerializeField]        private float massMultiplier = 0.3f;
        [SerializeField]        protected float maxVelocity = 15f;
        [Range(0f, 1f)]
        [SerializeField]        private float rotationRatio = 0.1f;
        [SerializeField]        private Transform mainObject = null;
        [Header("In Game Characteristics")]
        [SerializeField]        private bool paused = true;
        [SerializeField]        private int totalMass;
        private GameEngine engine;

        #region MonoBehaviour

        protected override void Awake()
        {
            base.Awake();
        }
        
        void FixedUpdate()
        {
            if (paused)
            {
                _body.velocity = Vector2.zero;
                return;
            }
            Vector2 next_pos = Vector2.zero;

            next_pos.x = Input.GetAxis("Horizontal");
            if (next_pos.x == 0 && Input.touchCount > 0)
            {
                Debug.LogWarning("Touch Input");
                next_pos.x = Input.touches[0].position.x - Screen.width / 2;
                next_pos.Normalize();
            }
            next_pos.y = ySpeed + (massMultiplier * totalMass);
            _body.velocity = Vector3.ClampMagnitude(_body.transform.right * next_pos.y + _body.transform.up * next_pos.x * xSpeed, maxVelocity);
            _transform.Rotate(Vector3.forward, next_pos.x * zRotationSpeed * rotationRatio * Time.fixedDeltaTime);
            mainObject.Rotate(Vector3.forward, next_pos.x * zRotationSpeed * Time.fixedDeltaTime);
        }

        #endregion

        #region Public

        public override void init()
        {
            base.init();
            mainTransform = mainObject;
            totalMass = getCurrentMass();
            this.engine = GameEngine.singleton;
            _transform.localScale = initialScale;
            engine.RefreshMass(totalMass);
            engine.RefreshHealth(currentHealth);
        }

        #endregion

        #region Main

        protected override void OnDamageReceived(int amount)
        {
            base.OnDamageReceived(amount);
            engine.RefreshHealth(currentHealth);
        }

        protected override void OnNewChild(IObject child)
        {
            base.OnNewChild(child);
            totalMass = getCurrentMass();
            engine.RefreshMass(totalMass);
            SpaceObject spaceTarget = child.GetComponent<SpaceObject>();

            if (spaceTarget != null)
                engine.addHostility(spaceTarget.GetHostilityModifier());
        }

        protected override void OnLoseChild(IObject child)
        {
            base.OnLoseChild(child);
            totalMass = getCurrentMass();
            engine.RefreshMass(totalMass);
        }

        protected override void OnDestruction()
        {
            base.OnDestruction();
            Destroy(mainObject.gameObject);
            GameEngine.singleton.EndGame();
        }

        protected override void OnResume()
        {
            base.OnResume();
            paused = false;
        }

        protected override void OnStopped()
        {
            base.OnStopped();
            paused = true;
        }

        protected override void OnDamageApplied(IObject target)
        {
            base.OnDamageApplied(target);
        }

        protected override void OnAssimilation(IObject newParent)
        {
            // Commplete override, no assimilation allowed
            return;
        }

        #endregion
    }
}
