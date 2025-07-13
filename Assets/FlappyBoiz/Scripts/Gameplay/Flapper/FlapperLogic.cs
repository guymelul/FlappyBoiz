using FlappyBoiz.Scripts.Data;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace FlappyBoiz.Scripts.Gameplay.Flapper
{
    public class FlapperLogic : IStartable, ITickable, IFixedTickable
    {
        [Inject] private FlapperMotion FlapperMotion { get; set; }
        [Inject] private FlapperSettings FlapperSettings { get; set; }

        public float FlapTimer { get; private set; }
        public int FlapsBuffered { get; private set; }
        public bool CanFlap { get; private set; }
        private float FlapCooldown => FlapperSettings.FlapCooldown;
        private float FallAcceleration => FlapperSettings.FallAcceleration;
        private float MaxFallSpeed => FlapperSettings.MaxFallSpeed;
        private float FlapPower => FlapperSettings.FlapPower;

        public void Start()
        {
            FlapTimer = 0;
            FlapsBuffered = 0;
            CanFlap = true;
        }

        public void DoFlap()
        {
            if (!CanFlap) return;

            FlapsBuffered++;
        }

        public void Tick()
        {
            if (CanFlap && FlapTimer > 0)
            {
                FlapTimer -= Time.deltaTime;
            }
        }

        public void FixedTick()
        {
            ApplyFlap();

            float verticalDelta = -FallAcceleration * Time.deltaTime;
            FlapperMotion.Move(verticalDelta, MaxFallSpeed);
        }

        private void ApplyFlap()
        {
            if (!CanFlap) return;

            if (FlapsBuffered <= 0 || !(FlapTimer <= 0)) return;

            FlapTimer = FlapCooldown;
            FlapsBuffered--;

            FlapperMotion.Flap(FlapPower);
        }
    }
}