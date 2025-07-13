using FlappyBoiz.Scripts.Events;
using MessagePipe;
using UnityEngine;
using UnityHFSM;
using VContainer;
using VContainer.Unity;

namespace FlappyBoiz.Scripts.Managers
{
    public class GameplayManager : IPostStartable, IFixedTickable, ITickable
    {
        [Inject] public IPublisher<GamePausedEvent> OnGamePaused { get; set; }

        [Inject] public IPublisher<GameResumedEvent> OnGameResumed { get; set; }

        private StateMachine m_fsm;

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
            m_fsm.AddState(name: GameStates.Playing);
            m_fsm.AddState(name: GameStates.Ready);
            m_fsm.AddState(
                name: GameStates.Paused,
                onEnter: _ => { Time.timeScale = 0; },
                onExit: _ => { Time.timeScale = 1; }
            );

            m_fsm.AddTriggerTransition("PlayerReady", new Transition(GameStates.Ready, GameStates.Playing));
            m_fsm.AddTriggerTransition("Pause", new Transition(GameStates.Playing, GameStates.Paused));
            m_fsm.AddTriggerTransition("Resume", new Transition(GameStates.Paused, GameStates.Playing));

            m_fsm.StateChanged += OnGameStateChanged;
            m_fsm.SetStartState(GameStates.Ready);
            m_fsm.Init();
        }

        private void OnGameStateChanged(StateBase<string> newState)
        {
            Debug.Log($"Game State: {newState.name}");

            switch (newState.name)
            {
                case GameStates.Paused:
                    OnGamePaused.Publish(GamePausedEvent.Empty);

                    break;
                case GameStates.Playing:
                    OnGameResumed.Publish(GameResumedEvent.Empty);

                    break;
            }
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