using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using FlappyBoiz.Scripts.Events;
using FlappyBoiz.Scripts.Features.Gameplay.Data;
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

        protected override UniTask OnGameStarted(GameStartedEvent ev, CancellationToken ct)
        {
            FlapTimer = 0;
            FlapsBuffered = 1;
            SetupEventSubs();

            return UniTask.CompletedTask;
        }

        protected override UniTask OnGameResumed(GameResumedEvent ev, CancellationToken ct)
        {
            SetupEventSubs();

            return UniTask.CompletedTask;
        }

        protected override UniTask OnGamePaused(GamePausedEvent ev, CancellationToken ct)
        {
            if (m_subscriptions != null)
            {
                m_subscriptions.Dispose();
                m_subscriptions = null;
            }

            return UniTask.CompletedTask;
        }

        protected override async UniTask OnGameEnded(GameEndedEvent ev, CancellationToken ct)
        {
            FlapsBuffered = 0;

            await FlapperPresenter.OnDead(ct);
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