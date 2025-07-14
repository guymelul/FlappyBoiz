using FlappyBoiz.Scripts.Data;
using FlappyBoiz.Scripts.Events;
using MessagePipe;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace FlappyBoiz.Scripts.Gameplay.Flapper
{
    public class FlapperLogic : IStartable, ITickable, IFixedTickable
    {
        [Inject] private FlapperView FlapperView { get; set; }
        [Inject] private FlapperSettings FlapperSettings { get; set; }
        [Inject] private ISubscriber<FlapperPrimaryActionRequest> FlapperPrimaryActionRequest { get; set; }

        private float FlapTimer { get; set; }
        private int FlapsBuffered { get; set; }
        private float FlapCooldown => FlapperSettings.FlapCooldown;
        private float FallAcceleration => FlapperSettings.FallAcceleration;
        private float MaxFallSpeed => FlapperSettings.MaxFallSpeed;
        private float FlapPower => FlapperSettings.FlapPower;

        public void Start()
        {
            FlapTimer = 0;
            FlapsBuffered = 0;

            FlapperPrimaryActionRequest.Subscribe(DoFlap);
        }

        private void DoFlap(FlapperPrimaryActionRequest ev) => FlapsBuffered++;

        public void Tick()
        {
            if (FlapTimer <= 0) return;
            FlapTimer -= Time.deltaTime;
        }

        public void FixedTick()
        {
            ApplyFlap();

            FlapperView.Move(-FallAcceleration * Time.fixedDeltaTime, MaxFallSpeed);
        }

        private void ApplyFlap()
        {
            if (FlapsBuffered <= 0 || !(FlapTimer <= 0)) return;

            FlapTimer = FlapCooldown;
            FlapsBuffered--;

            FlapperView.Flap(FlapPower);
        }
    }
}