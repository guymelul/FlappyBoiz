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
        [Inject] private ISubscriber<FlapperPrimaryActionRequest> FlapperPrimaryActionRequestSub { get; set; }
        [Inject] private ISubscriber<FlapperCollisionEvent> FlapperCollisionEventSub { get; set; }

        private float FlapTimer { get; set; }
        private int FlapsBuffered { get; set; }
        private float FlapCooldown => FlapperSettings.FlapCooldown;

        public void Start()
        {
            FlapTimer = 0;
            FlapsBuffered = 0;

            FlapperPrimaryActionRequestSub.Subscribe(DoFlap);
            FlapperCollisionEventSub.Subscribe(OnCollision);
        }

        public void Tick()
        {
            if (FlapTimer <= 0) return;
            FlapTimer -= Time.deltaTime;
        }

        public void FixedTick()
        {
            ApplyFlap();
        }
        
        private void DoFlap(FlapperPrimaryActionRequest ev) => FlapsBuffered++;

        private void ApplyFlap()
        {
            if (FlapsBuffered <= 0 || !(FlapTimer <= 0)) return;

            FlapTimer = FlapCooldown;
            FlapsBuffered--;

            FlapperView.Flap();
        }

        private void OnCollision(FlapperCollisionEvent ev)
        {
            Debug.Log("Flapper is dead!");
        }
    }
}