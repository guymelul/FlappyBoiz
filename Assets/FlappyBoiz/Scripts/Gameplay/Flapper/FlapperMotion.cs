using System;
using UnityEngine;

namespace FlappyBoiz.Scripts.Gameplay.Flapper
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Rigidbody2D))]
    public class FlapperMotion : MonoBehaviour
    {
        #region Members

        private Rigidbody2D m_rigidbody;
        private Collider2D m_collider;

        private float m_jumpTimer;
        private Vector3 m_startingPosition;
        private Vector3 m_offscreenPosition;

        internal int JumpsBuffered { get; set; }
        internal float ForceMultiplier { get; set; }

        internal bool Moveable { get; set; }
        private bool Jumpable { get; set; }

        internal Vector2 Velocity => m_rigidbody.linearVelocity;

        #endregion

        #region Events

        public event Action OnJump;
        public event Action OnHit;

        #endregion

        #region Fields

        [Header("Falling")]
        [field: Range(0.0f, 20.0f)]
        [field: SerializeField]
        internal float MaxFallSpeed { get; private set; }

        [Range(0.0f, 50.0f)] [SerializeField] private float fallAcceleration;

        [Header("Jump")] [Range(1.0f, 5.0f)] [SerializeField]
        private float jumpPower;

        [Range(0.0f, 0.5f)] [SerializeField] private float jumpCooldown;

        #endregion

        private void Awake()
        {
            m_startingPosition = transform.position;

            m_rigidbody = GetComponent<Rigidbody2D>();
            m_collider = GetComponent<Collider2D>();

            ResetFlapper();
        }

        internal void ResetFlapper()
        {
            m_jumpTimer = 0;
            m_rigidbody.linearVelocity = Vector2.zero;
            m_rigidbody.angularVelocity = 0;
            ForceMultiplier = 1.0f;
            m_rigidbody.MovePositionAndRotation(m_startingPosition, Quaternion.identity);
            transform.SetPositionAndRotation(m_startingPosition, Quaternion.identity);
        }

        internal void ToggleBirdSimulated(bool active)
        {
            m_collider.enabled = active;
            Moveable = active;
            Jumpable = active;
        }

        public void PerformAction()
        {
            if (!Jumpable) return;

            JumpsBuffered++;
        }

        private void Update()
        {
            if (Jumpable && m_jumpTimer > 0)
            {
                m_jumpTimer -= Time.deltaTime;
            }
        }

        private void FixedUpdate()
        {
            ApplyJump();
            ApplyMotion();
        }

        private void ApplyJump()
        {
            if (!Jumpable) return;

            if (JumpsBuffered <= 0 || !(m_jumpTimer <= 0)) return;

            m_jumpTimer = jumpCooldown;
            JumpsBuffered--;

            m_rigidbody.linearVelocityY = jumpPower;

            OnJump?.Invoke();
        }

        private float Delta(float time)
        {
            return -fallAcceleration * ForceMultiplier * time;
        }

        private void ApplyMotion()
        {
            if (!Moveable) return;

            float nextFallSpeed = m_rigidbody.linearVelocityY + Delta(Time.deltaTime);
            m_rigidbody.linearVelocityY = Mathf.Clamp(nextFallSpeed, -MaxFallSpeed, MaxFallSpeed);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!Moveable) return;

            if (!(other.CompareTag("Ground") || other.CompareTag("Obstacle"))) return;

            OnHit?.Invoke();
        }
    }
}