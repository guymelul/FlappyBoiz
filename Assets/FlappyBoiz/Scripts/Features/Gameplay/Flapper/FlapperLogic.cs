using System;
using FlappyBoiz.Scripts.Events;
using FlappyBoiz.Scripts.Features.Gameplay.ScriptableObjects;
using MessagePipe;
using UnityEngine;
using VContainer;

namespace FlappyBoiz.Scripts.Features.Gameplay.Flapper
{
    public class FlapperLogic : GameplayStateAware
    {
        [Inject] public FlapperPresenter FlapperPresenter { get; set; }
        [Inject] public FlapperSettings FlapperSettings { get; set; }
        [Inject] public ISubscriber<FlapperPrimaryActionRequest> FlapperPrimaryActionRequestSub { get; set; }

        private float FlapTimer { get; set; }
        private int FlapsBuffered { get; set; }
        private float FlapCooldown => FlapperSettings.FlapCooldown;

        private IDisposable m_subscriptions;

        #region Game Events

        protected override void OnGameStarted(GameStartedEvent ev)
        {
            FlapTimer = 0;
            FlapsBuffered = 1;
            SetupEventSubs();
        }

        protected override void OnGameResumed(GameResumedEvent ev) => SetupEventSubs();

        protected override void OnGamePaused(GamePausedEvent ev)
        {
            if (m_subscriptions == null) return;

            m_subscriptions.Dispose();
            m_subscriptions = null;
        }

        protected override void OnGameEnded(GameEndedEvent ev)
        {
            FlapsBuffered = 0;
            FlapperPresenter.OnDead();
        }

        #endregion

        private void SetupEventSubs()
        {
            var bag = DisposableBag.CreateBuilder();
            FlapperPrimaryActionRequestSub.Subscribe(HandlePrimaryAction).AddTo(bag);

            m_subscriptions = bag.Build();
        }

        private void HandlePrimaryAction(FlapperPrimaryActionRequest ev)
        {
            FlapsBuffered = Mathf.Clamp(FlapsBuffered + 1, 0, 2);
        }

        protected override void OnGameTick()
        {
            if (FlapTimer <= 0) return;

            FlapTimer -= Time.deltaTime;
        }

        protected override void OnGameFixedTick()
        {
            FlapperPresenter.FixedTick();

            if (FlapsBuffered <= 0 || !(FlapTimer <= 0)) return;

            FlapTimer = FlapCooldown;
            FlapsBuffered--;

            FlapperPresenter.Flap();
        }
    }
}