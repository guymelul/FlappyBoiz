using System;
using FlappyBoiz.Scripts.Events;
using MessagePipe;
using R3;
using VContainer;
using VContainer.Unity;
using DisposableBag = MessagePipe.DisposableBag;

namespace FlappyBoiz.Scripts.Gameplay.Managers
{
    public class GameSessionManager : IStartable
    {
        [Inject] private ISubscriber<FlapperCollisionEvent> OnFlapperCollisionEvent { get; set; }
        [Inject] private ISubscriber<FlapperCollectableEvent> OnFlapperCollectableEvent { get; set; }

        private readonly ReactiveProperty<int> m_score = new(0);
        public Observable<int> Score => m_score;

        private readonly ReactiveProperty<bool> m_isDead = new(false);
        public Observable<bool> IsDead => m_isDead;

        private IDisposable subscriptions;

        public void Start()
        {
            var bag = DisposableBag.CreateBuilder();
            OnFlapperCollectableEvent.Subscribe(ev => m_score.Value += ev.Amount).AddTo(bag);
            OnFlapperCollisionEvent.Subscribe(OnFlapperHit).AddTo(bag);

            subscriptions = bag.Build();
        }

        private void OnFlapperHit(FlapperCollisionEvent ev)
        {
            m_isDead.Value = true;

            if (subscriptions == null) return;

            subscriptions.Dispose();
            subscriptions = null;
        }
    }
}