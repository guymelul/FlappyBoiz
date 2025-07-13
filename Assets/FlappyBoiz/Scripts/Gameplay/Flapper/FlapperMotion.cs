using System;
using UnityEngine;

namespace FlappyBoiz.Scripts.Gameplay.Flapper
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Rigidbody2D))]
    public class FlapperMotion : MonoBehaviour
    {
        private Rigidbody2D m_rigidbody;
        private Collider2D m_collider;

        private Vector3 m_startingPosition;
        private Vector3 m_offscreenPosition;

        internal Vector2 Velocity => m_rigidbody.linearVelocity;
        
        public event Action OnJump;
        public event Action OnHit;

        private void Awake()
        {
            m_startingPosition = transform.position;

            m_rigidbody = GetComponent<Rigidbody2D>();
            m_collider = GetComponent<Collider2D>();

            ResetFlapper();
        }

        private void ResetFlapper()
        {
            m_rigidbody.linearVelocity = Vector2.zero;
            m_rigidbody.angularVelocity = 0;
            m_rigidbody.MovePositionAndRotation(m_startingPosition, Quaternion.identity);
            transform.SetPositionAndRotation(m_startingPosition, Quaternion.identity);
        }

        internal void ToggleBirdSimulated(bool active)
        {
            m_collider.enabled = active;
        }

        public void Flap(float power)
        {
            m_rigidbody.linearVelocityY = power;
            OnJump?.Invoke();
        }

        public void Move(float verticalMovement, float maxFallSpeed)
        {
            float nextFallSpeed = m_rigidbody.linearVelocityY + verticalMovement;
            m_rigidbody.linearVelocityY = Mathf.Clamp(nextFallSpeed, -maxFallSpeed, maxFallSpeed);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            // TODO: handle case where its already dead
            
            if (!(other.CompareTag("Ground") || other.CompareTag("Obstacle"))) return;
            
            OnHit?.Invoke();
        }
    }
}