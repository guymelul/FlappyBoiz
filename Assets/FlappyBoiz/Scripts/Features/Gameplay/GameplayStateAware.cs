using FlappyBoiz.Scripts.Events;
using MessagePipe;
using VContainer;
using VContainer.Unity;

namespace FlappyBoiz.Scripts.Features.Gameplay
{
    public abstract class GameplayStateAware : IPostInitializable, ITickable, IFixedTickable
    {
        [Inject] public ISubscriber<GameStartedEvent> GameStarted { get; set; }
        [Inject] public ISubscriber<GameResumedEvent> GameResumed { get; set; }
        [Inject] public ISubscriber<GamePausedEvent> GamePaused { get; set; }
        [Inject] public ISubscriber<GameEndedEvent> GameEnded { get; set; }

        private bool IsGameRunning { get; set; }

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

        protected virtual void OnGameTick()
        {
        }

        protected virtual void OnGameFixedTick()
        {
        }
    }
}