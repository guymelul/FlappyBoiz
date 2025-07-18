using FlappyBoiz.Scripts.Events;
using MessagePipe;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace FlappyBoiz.Scripts.Core
{
    public abstract class GameplayStateAware: IPostInitializable
    {
        [Inject] protected ISubscriber<GameStartedEvent> GameStarted { get; set; }
        [Inject] protected ISubscriber<GameResumedEvent> GameResumed { get; set; }
        [Inject] protected ISubscriber<GamePausedEvent> GamePaused { get; set; }

        protected bool IsGameStarted { get; private set; }

        public void PostInitialize()
        {
            GameStarted.Subscribe(ev =>
            {
                IsGameStarted = true;
                OnGameStarted(ev);
            });
            GameResumed.Subscribe(OnGameResumed);
            GamePaused.Subscribe(OnGamePaused);
        }

        protected virtual void OnGameStarted(GameStartedEvent ev)
        {
            Debug.Log("Game Started");
        }

        protected virtual void OnGameResumed(GameResumedEvent ev)
        {
            Debug.Log("Game Resumed");
        }

        protected virtual void OnGamePaused(GamePausedEvent ev)
        {
            Debug.Log("Game Paused");
        }
    }
}