using System;
using FlappyBoiz.Scripts.Events;
using MessagePipe;
using R3;
using UnityEngine;
using UnityHFSM;
using VContainer;
using VContainer.Unity;

namespace FlappyBoiz.Scripts.Gameplay.Managers
{
    public class GameplayManager : IPostStartable, IFixedTickable, ITickable
    {
        [Inject] private ISubscriber<FlapperPrimaryActionRequest> OnPrimaryAction { get; set; }
        [Inject] private ISubscriber<GamePauseCommand> OnGamePauseCommand { get; set; }

        [Inject] private IPublisher<GamePausedEvent> GamePausedPub { get; set; }
        [Inject] private IPublisher<GameResumedEvent> GameResumedPub { get; set; }
        [Inject] private IPublisher<GameStartedEvent> GameStartedPub { get; set; }
        [Inject] private IPublisher<GameEndedEvent> GameEndedPub { get; set; }

        [Inject] private GameSessionManager GameSession { get; set; }

        private StateMachine m_fsm;
        private IDisposable m_disposableSub;
        private bool m_frameDelay;

        public void PostStart()
        {
            InitStateMachine();

            GameSession.IsDead.Subscribe(_ => m_fsm.Trigger("GameOver"));
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
                onEnter: _ => { m_disposableSub = OnPrimaryAction.Subscribe(_ => m_fsm.Trigger("PlayerReady")); },
                onExit: _ =>
                {
                    m_disposableSub.Dispose();
                    m_disposableSub = null;
                }
            );

            m_fsm.AddState(name: GameStates.Playing,
                needsExitTime: true,
                canExit: ExitFrameDelay,
                onEnter: _ => { m_disposableSub = OnGamePauseCommand.Subscribe(_ => m_fsm.Trigger("Pause")); },
                onExit: _ =>
                {
                    m_disposableSub.Dispose();
                    m_disposableSub = null;
                }
            );

            m_fsm.AddState(
                name: GameStates.Paused,
                needsExitTime: true,
                canExit: ExitFrameDelay,
                onEnter: _ =>
                {
                    Time.timeScale = 0;
                    m_disposableSub = OnGamePauseCommand.Subscribe(_ => m_fsm.Trigger("Resume"));
                },
                onExit: _ =>
                {
                    m_disposableSub.Dispose();
                    m_disposableSub = null;
                    Time.timeScale = 1;
                }
            );

            m_fsm.AddState(name: GameStates.GameOver);

            m_fsm.AddTriggerTransition("PlayerReady", new Transition(GameStates.Ready, GameStates.Playing,
                afterTransition: _ => GameStartedPub.Publish(GameStartedEvent.Empty))
            );

            m_fsm.AddTriggerTransition("Pause", new Transition(GameStates.Playing, GameStates.Paused,
                afterTransition: _ => GamePausedPub.Publish(GamePausedEvent.Empty))
            );

            m_fsm.AddTriggerTransition("Resume", new Transition(GameStates.Paused, GameStates.Playing,
                afterTransition: _ => GameResumedPub.Publish(GameResumedEvent.Empty))
            );

            m_fsm.AddTriggerTransition("GameOver", new Transition(GameStates.Playing, GameStates.GameOver,
                onTransition: _ => GameEndedPub.Publish(GameEndedEvent.Empty)
            ));

            m_fsm.SetStartState(GameStates.Ready);
            m_fsm.StateChanged += _ => m_frameDelay = true;
            m_fsm.Init();
        }

        private bool ExitFrameDelay(State<string, string> state)
        {
            if (!m_frameDelay) return true;

            m_frameDelay = false;

            return false;
        }

        #region States

        private static class GameStates
        {
            public const string Ready = "GS_READY";
            public const string Paused = "GS_PAUSED";
            public const string Playing = "GS_PLAYING";
            public const string GameOver = "GS_GAMEOVER";
        }

        #endregion
    }
}