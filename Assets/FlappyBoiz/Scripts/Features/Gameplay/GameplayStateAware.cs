using FlappyBoiz.Scripts.Events;
using MessagePipe;
using VContainer;
using VContainer.Unity;

namespace FlappyBoiz.Scripts.Features.Gameplay
{
    public abstract class GameplayStateAware : IPostInitializable
    {
        [Inject] protected ISubscriber<GameStartedEvent> GameStarted { get; set; }
        [Inject] protected ISubscriber<GameResumedEvent> GameResumed { get; set; }
        [Inject] protected ISubscriber<GamePausedEvent> GamePaused { get; set; }
        [Inject] protected ISubscriber<GameEndedEvent> GameEnded { get; set; }

        protected bool IsGameRunning { get; private set; }

        public void PostInitialize()
        {
            GameStarted.Subscribe(ev =>
            {
                IsGameRunning = true;
                OnGameStarted(ev);
            });

            GameResumed.Subscribe(OnGameResumed);

            GamePaused.Subscribe(OnGamePaused);

            GameEnded.Subscribe(ev =>
            {
                IsGameRunning = false;
                OnGameEnded(ev);
            });
        }

        protected virtual void OnGameStarted(GameStartedEvent ev)
        {
        }

        protected virtual void OnGameResumed(GameResumedEvent ev)
        {
        }

        protected virtual void OnGamePaused(GamePausedEvent ev)
        {
        }

        protected virtual void OnGameEnded(GameEndedEvent ev)
        {
        }
    }
}