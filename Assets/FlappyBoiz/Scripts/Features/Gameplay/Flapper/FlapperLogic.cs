using System;
using FlappyBoiz.Scripts.Events;
using FlappyBoiz.Scripts.Features.Gameplay.ScriptableObjects;
using MessagePipe;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace FlappyBoiz.Scripts.Features.Gameplay.Flapper
{
    public class FlapperLogic : GameplayStateAware, IStartable, ITickable, IFixedTickable
    {
        [Inject] private FlapperPresenter FlapperPresenter { get; set; }
        [Inject] private FlapperSettings FlapperSettings { get; set; }
        [Inject] private ISubscriber<FlapperPrimaryActionRequest> FlapperPrimaryActionRequestSub { get; set; }

        private float FlapTimer { get; set; }
        private int FlapsBuffered { get; set; }
        private float FlapCooldown => FlapperSettings.FlapCooldown;

        private IDisposable m_subscriptions;

        public void Start()
        {
            FlapTimer = 0;
            FlapsBuffered = 0;
        }

        protected override void OnGameStarted(GameStartedEvent ev) => SetupEvents();

        protected override void OnGameResumed(GameResumedEvent ev) => SetupEvents();

        private void SetupEvents()
        {
            var bag = DisposableBag.CreateBuilder();
            FlapperPrimaryActionRequestSub.Subscribe(HandlePrimaryAction).AddTo(bag);

            m_subscriptions = bag.Build();
        }

        protected override void OnGamePaused(GamePausedEvent ev)
        {
            if (m_subscriptions == null) return;

            m_subscriptions.Dispose();
            m_subscriptions = null;
        }

        protected override void OnGameEnded(GameEndedEvent ev)
        {
            Start();

            FlapperPresenter.OnDead();
        }

        public void Tick()
        {
            if (!IsGameRunning || FlapTimer <= 0) return;

            FlapTimer -= Time.deltaTime;
        }

        public void FixedTick()
        {
            if (!IsGameRunning) return;

            FlapperPresenter.FixedTick();
            ApplyFlap();
        }

        private void HandlePrimaryAction(FlapperPrimaryActionRequest ev)
        {
            FlapsBuffered = Mathf.Clamp(FlapsBuffered + 1, 0, 2);
        }

        private void ApplyFlap()
        {
            if (FlapsBuffered <= 0 || !(FlapTimer <= 0)) return;

            FlapTimer = FlapCooldown;
            FlapsBuffered--;

            FlapperPresenter.Flap();
        }
    }
}