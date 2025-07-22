using System;
using FlappyBoiz.Scripts.Events;
using MessagePipe;
using R3;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using DisposableBag = MessagePipe.DisposableBag;

namespace FlappyBoiz.Scripts.Gameplay.Managers
{
    public class GameSessionManager : IStartable
    {
        [Inject] private ISubscriber<FlapperCollisionEvent> OnFlapperCollisionEvent { get; set; }
        [Inject] private ISubscriber<FlapperCollectableEvent> OnFlapperCollectableEvent { get; set; }

        public ReactiveProperty<int> Score { get; } = new(0);

        public ReactiveProperty<bool> IsDead { get; } = new(false);

        public int Coins { get; set; } = 0;

        private IDisposable m_subscriptions;

        public void Start()
        {
            var bag = DisposableBag.CreateBuilder();
            OnFlapperCollectableEvent.Subscribe(ev => Score.Value += ev.Amount).AddTo(bag);
            OnFlapperCollisionEvent.Subscribe(OnFlapperHit).AddTo(bag);

            m_subscriptions = bag.Build();
        }

        private void OnFlapperHit(FlapperCollisionEvent ev)
        {
            IsDead.Value = true;

            // TODO: calculate amount of coins
            Coins = Mathf.FloorToInt(Score.CurrentValue * 0.75f);

            if (m_subscriptions == null) return;

            m_subscriptions.Dispose();
            m_subscriptions = null;
        }
    }
}