using System.Threading;
using Cysharp.Threading.Tasks;
using FlappyBoiz.Scripts.Events;
using FlappyBoiz.Scripts.Features.Gameplay.Obstacle;
using FlappyBoiz.Scripts.Features.Gameplay.ScriptableObjects;
using MessagePipe;
using UnityEngine;
using VContainer;

namespace FlappyBoiz.Scripts.Features.Gameplay.Flapper
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Animator))]
    public class FlapperPresenter : MonoBehaviour
    {
        private static readonly int AnimKeyFlapWings = Animator.StringToHash("FlapWings");

        [Inject] public FlapperSettings FlapperSettings { get; set; }
        [Inject] public IPublisher<FlapperCollisionEvent> FlapperCollisionEventPub { get; set; }
        [Inject] public IPublisher<FlapperCollectableEvent> FlapperCollectableEventPub { get; set; }

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
        }

        private void Start()
        {
            m_rigidbody.linearVelocity = Vector2.zero;
            m_rigidbody.angularVelocity = 0;
        }

        public void Flap()
        {
            m_rigidbody.linearVelocityY = FlapPower;
            Rotation = JumpRotation;

            m_animator.SetTrigger(AnimKeyFlapWings);
        }

        public void FixedTick()
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

        private void ApplyGravity()
        {
            float nextFallSpeed = -FallAcceleration * Time.deltaTime;
            m_rigidbody.linearVelocityY = Mathf.Clamp(
                m_rigidbody.linearVelocityY + nextFallSpeed,
                -MaxFallSpeed, MaxFallSpeed
            );
        }

        public async UniTask OnDead(CancellationToken ct)
        {
            Time.timeScale = 0;

            await UniTask.Delay(millisecondsDelay: 500, DelayType.Realtime, cancellationToken: ct);

            Time.timeScale = 1;

            m_rigidbody.bodyType = RigidbodyType2D.Dynamic;
            m_rigidbody.AddForce(Vector2.down, ForceMode2D.Impulse);

            await UniTask.Delay(millisecondsDelay: 2000, DelayType.Realtime, cancellationToken: ct);

            gameObject.SetActive(false);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Obstacle"))
            {
                FlapperCollisionEventPub.Publish(FlapperCollisionEvent.Empty);

                return;
            }

            if (other.CompareTag("Collectable"))
            {
                var collectable = other.GetComponent<Collectable>();
                FlapperCollectableEventPub.Publish(new FlapperCollectableEvent(collectable.Amount));

                return;
            }

            Debug.Log($"Flapper collides with unknown object {other.name}(Tag={other.tag})");
        }
    }
}