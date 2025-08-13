using System;
using FlappyBoiz.Scripts.Events;
using MessagePipe;
using R3;
using VContainer;
using VContainer.Unity;
using DisposableBag = MessagePipe.DisposableBag;

namespace FlappyBoiz.Scripts.Features.Gameplay.Managers
{
    public class GameSessionManager : IStartable, IDisposable
    {
        [Inject] public ISubscriber<FlapperCollisionEvent> OnFlapperCollisionEvent { get; set; }
        [Inject] public ISubscriber<FlapperCollectableEvent> OnFlapperCollectableEvent { get; set; }

        public ReactiveProperty<int> Score { get; } = new(0);

        public ReactiveProperty<bool> IsDead { get; } = new(false);

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
            Dispose();
        }

        public void Dispose()
        {
            m_subscriptions?.Dispose();
            m_subscriptions = null;
        }
    }
}