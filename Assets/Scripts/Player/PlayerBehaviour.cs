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
        [SerializeField]        protected float maxYVelocity = 15f;
        [Range(0f, 1f)]
        [SerializeField]        private float rotationRatio = 0.1f;
        [SerializeField]        private Transform mainObject = null;
        [SerializeField]        private ParticleSystem damageExplosion = null;
        [Header("In Game Characteristics")]
        [SerializeField]        private bool paused = true;
        [SerializeField]        private int totalMass;
        private GameEngine engine;

        #region MonoBehaviour
                
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
            next_pos.y = Mathf.Clamp(ySpeed + (massMultiplier * totalMass), 0f, maxYVelocity);
            _body.velocity = _body.transform.right * next_pos.y + _body.transform.up * next_pos.x * xSpeed;
            _transform.Rotate(Vector3.forward, next_pos.x * zRotationSpeed * rotationRatio * Time.fixedDeltaTime);
            mainObject.Rotate(Vector3.forward, next_pos.x * zRotationSpeed * Time.fixedDeltaTime);
        }

        #endregion

        #region Public Overrides

        public override void Init()
        {
            base.Init();
            mainTransform = mainObject;
            totalMass = GetCurrentMass();
            this.engine = GameEngine.singleton;
            _transform.localScale = initialScale;
            engine.RefreshMass(totalMass);
            engine.RefreshHealth(currentHealth);
        }

        #endregion

        #region Protectd Overrides

        protected override void OnDamageReceived(byte amount)
        {
            base.OnDamageReceived(amount);
            engine.RefreshHealth(currentHealth);
            ParticleSystem particles = Instantiate(damageExplosion, _transform.position, Quaternion.identity);
            Destroy(particles.gameObject, particles.main.startLifetime.constantMax);
        }

        protected override void OnNewChild(IObject child)
        {
            base.OnNewChild(child);
            totalMass = GetCurrentMass();
            engine.RefreshMass(totalMass);
            SpaceObject spaceTarget = child.GetComponent<SpaceObject>();

            if (spaceTarget != null)
                engine.AddHostility(spaceTarget.GetHostilityModifier());
        }

        protected override void OnLoseChild(IObject child)
        {
            base.OnLoseChild(child);
            totalMass = GetCurrentMass();
            engine.RefreshMass(totalMass);
        }

        protected override void OnDestruction()
        {
            base.OnDestruction();
            engine.RefreshHealth(currentHealth);
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
