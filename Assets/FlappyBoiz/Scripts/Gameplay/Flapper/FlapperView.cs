using System.Collections;
using FlappyBoiz.Scripts.Data;
using FlappyBoiz.Scripts.Events;
using MessagePipe;
using UnityEngine;
using VContainer;

namespace FlappyBoiz.Scripts.Gameplay.Flapper
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Animator))]
    public class FlapperView : MonoBehaviour
    {
        private static readonly int AnimKeyFlapWings = Animator.StringToHash("FlapWings");

        [Inject] private FlapperSettings FlapperSettings { get; set; }
        [Inject] private IPublisher<FlapperCollisionEvent> FlapperCollisionEventPub { get; set; }

        private Rigidbody2D m_rigidbody;
        private Animator m_animator;

        private Vector2 Velocity => m_rigidbody.linearVelocity;
        private float FallAcceleration => FlapperSettings.FallAcceleration;
        private float MaxFallSpeed => FlapperSettings.MaxFallSpeed;
        private float FlapPower => FlapperSettings.FlapPower;
        private float FallMaxRotation => FlapperSettings.FallMaxRotation;
        private float AirborneTargetRotation => FlapperSettings.AirborneTargetRotation;
        private float FallRotationSpeed => FlapperSettings.FallRotationSpeed;
        private float JumpRotation => FlapperSettings.JumpRotation;

        private float Rotation
        {
            get => transform.localEulerAngles.z;
            set => transform.localEulerAngles = new Vector3(0, 0, value);
        }

        private void Awake()
        {
            m_animator = GetComponent<Animator>();
            m_rigidbody = GetComponent<Rigidbody2D>();

            m_rigidbody.linearVelocity = Vector2.zero;
            m_rigidbody.angularVelocity = 0;
        }

        private void FixedUpdate()
        {
            ApplyRotation();
            ApplyGravity();
        }

        private void ApplyRotation()
        {
            Rotation = Mathf.MoveTowardsAngle(
                Rotation,
                Velocity.y < 0 ? FallMaxRotation : AirborneTargetRotation,
                FallRotationSpeed * Time.deltaTime
            );
        }

        public void Flap()
        {
            m_rigidbody.linearVelocityY = FlapPower;
            Rotation = JumpRotation;

            m_animator.SetTrigger(AnimKeyFlapWings);
        }

        private void ApplyGravity()
        {
            float nextFallSpeed = -FallAcceleration * Time.deltaTime;
            m_rigidbody.linearVelocityY = Mathf.Clamp(
                m_rigidbody.linearVelocityY + nextFallSpeed,
                -MaxFallSpeed, MaxFallSpeed
            );
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!(other.CompareTag("Ground") || other.CompareTag("Obstacle"))) return;

            FlapperCollisionEventPub.Publish(FlapperCollisionEvent.Empty);
        }
    }
}