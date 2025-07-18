using System;
using FlappyBoiz.Scripts.Events;
using MessagePipe;
using UnityEngine;
using UnityHFSM;
using VContainer;
using VContainer.Unity;

namespace FlappyBoiz.Scripts.Gameplay.Managers
{
    public class GameplayManager : IPostStartable, IFixedTickable, ITickable
    {
        [Inject] private ISubscriber<FlapperPrimaryActionRequest> OnPrimaryAction { get; set; }
        [Inject] private IPublisher<GamePausedEvent> OnGamePaused { get; set; }
        [Inject] private IPublisher<GameResumedEvent> OnGameResumed { get; set; }
        [Inject] private IPublisher<GameStartedEvent> OnGameStartedEvent { get; set; }

        private StateMachine m_fsm;
        private IDisposable m_disposableSub;

        public void PostStart()
        {
            InitStateMachine();
        }

        public void FixedTick()
        {
            if (Time.timeScale <= 0) return;

            m_fsm.OnLogic();
        }

        public void Tick()
        {
            if (Time.timeScale > 0) return;

            m_fsm.OnLogic();
        }

        private void InitStateMachine()
        {
            m_fsm = new StateMachine();

            // States
            m_fsm.AddState(name: GameStates.Ready,
                onEnter: _ =>
                {
                    m_disposableSub = OnPrimaryAction.Subscribe(_ => m_fsm.Trigger("PlayerReady"));
                },
                onExit: _ =>
                {
                    m_disposableSub.Dispose();
                    m_disposableSub = null;
                }
            );

            m_fsm.AddState(name: GameStates.Playing);
            m_fsm.AddState(
                name: GameStates.Paused,
                onEnter: _ => { Time.timeScale = 0; },
                onExit: _ => { Time.timeScale = 1; }
            );

            m_fsm.AddTriggerTransition("PlayerReady", new Transition(GameStates.Ready, GameStates.Playing,
                afterTransition: _ => OnGameStartedEvent.Publish(GameStartedEvent.Empty))
            );

            m_fsm.AddTriggerTransition("Pause", new Transition(GameStates.Playing, GameStates.Paused,
                afterTransition: _ => OnGamePaused.Publish(GamePausedEvent.Empty))
            );

            m_fsm.AddTriggerTransition("Resume", new Transition(GameStates.Paused, GameStates.Playing,
                afterTransition: _ => OnGameResumed.Publish(GameResumedEvent.Empty))
            );

            m_fsm.SetStartState(GameStates.Ready);
            m_fsm.Init();
        }

        #region States

        private static class GameStates
        {
            public const string Ready = "GS_READY";
            public const string Paused = "GS_PAUSED";
            public const string Playing = "GS_PLAYING";
        }

        #endregion
    }
}