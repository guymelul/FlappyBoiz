using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using FlappyBoiz.Scripts.Events;
using MessagePipe;
using VContainer;
using VContainer.Unity;

namespace FlappyBoiz.Scripts.Features.Gameplay
{
    public abstract class GameplayStateAware : IPostInitializable, ITickable, IFixedTickable, IDisposable
    {
        [Inject] public IAsyncSubscriber<GameStartedEvent> GameStarted { get; set; }
        [Inject] public IAsyncSubscriber<GameResumedEvent> GameResumed { get; set; }
        [Inject] public IAsyncSubscriber<GamePausedEvent> GamePaused { get; set; }
        [Inject] public IAsyncSubscriber<GameEndedEvent> GameEnded { get; set; }

        private bool IsGameRunning { get; set; }

        private IDisposable m_disposables;

        public void PostInitialize()
        {
            var disposeBag = DisposableBag.CreateBuilder();

            GameStarted.Subscribe(OnGameStartedInternal).AddTo(disposeBag);

            GameResumed.Subscribe(OnGameResumed).AddTo(disposeBag);

            GamePaused.Subscribe(OnGamePaused).AddTo(disposeBag);

            GameEnded.Subscribe(OnGameEndedInternal).AddTo(disposeBag);

            m_disposables = disposeBag.Build();
        }

        public void Tick()
        {
            if (!IsGameRunning) return;

            OnGameTick();
        }

        public void FixedTick()
        {
            if (!IsGameRunning) return;

            OnGameFixedTick();
        }

        private async UniTask OnGameStartedInternal(GameStartedEvent ev, CancellationToken ct)
        {
            IsGameRunning = true;
            await OnGameStarted(ev, ct);
        }

        private async UniTask OnGameEndedInternal(GameEndedEvent ev, CancellationToken ct)
        {
            IsGameRunning = false;
            await OnGameEnded(ev, ct);
        }

        protected virtual async UniTask OnGameStarted(GameStartedEvent ev, CancellationToken ct) =>
            await UniTask.CompletedTask;


        protected virtual async UniTask OnGameResumed(GameResumedEvent ev, CancellationToken ct) =>
            await UniTask.CompletedTask;


        protected virtual async UniTask OnGamePaused(GamePausedEvent ev, CancellationToken ct) =>
            await UniTask.CompletedTask;


        protected virtual async UniTask OnGameEnded(GameEndedEvent ev, CancellationToken ct) =>
            await UniTask.CompletedTask;


        protected virtual void OnGameTick()
        {
        }

        protected virtual void OnGameFixedTick()
        {
        }

        public void Dispose()
        {
            m_disposables?.Dispose();
        }
    }
}