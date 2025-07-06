using FlappyBoiz.Scripts.Data;
using UnityAtoms.BaseAtoms;
using UnityEditor;
using UnityEngine;
using UnityHFSM;

namespace FlappyBoiz.Scripts.Managers
{
    public static class GameEvents
    {
        public const string Restart = "ON_RESTART";
        public const string Action = "ON_ACTION";
        public const string Back = "ON_BACK";
        public const string UIBack = "ON_UI_BACK";
        public const string Exit = "ON_EXIT";
        public const string GameOver = "ON_GAME_OVER";
        public const string Authenticated = "ON_AUTH";
        public const string OperationSuccess = "ON_OP_SUCCESS";
    }

    public static class GameStates
    {
        public const string AutoLogin = "GS_AUTOLOGIN";
        public const string Login = "GS_LOGIN";
        public const string LoadData = "GS_LOAD_DATA";
        public const string MainMenu = "GS_MAIN_MENU";
        public const string Ready = "GS_READY";
        public const string Paused = "GS_PAUSED";
        public const string Playing = "GS_PLAYING";
        public const string GameOver = "GS_GAMEOVER";
        public const string Quit = "GS_QUIT";
    }

    [DisallowMultipleComponent]
    public class GameManager : MonoBehaviour
    {
        [SerializeField]
        private GameSettings gameSettings;

        [SerializeField]
        private StringEvent gameStateChanged;

        [SerializeField]
        private StringEvent onGameEvent;

        private StateMachine m_fsm;

        private void Start()
        {
            Debug.Log("Setting up game");

            SetupStateMachine();

            onGameEvent.Register(OnGameEvent);
        }

        private void FixedUpdate() => m_fsm.OnLogic();

        private void Update()
        {
            // Keep running fsm on game pause
            if (Time.timeScale > 0) return;

            m_fsm.OnLogic();
        }

        private void OnGameEvent(string gameEvent)
        {
            m_fsm.Trigger(gameEvent);
        }

        private void OnGameStateChanged(StateBase<string> newState)
        {
            gameStateChanged.Raise(newState.name);
        }

        private void SetupStateMachine()
        {
            m_fsm = new StateMachine();

            // States
            m_fsm.AddState(GameStates.AutoLogin);

            m_fsm.AddState(GameStates.Login);

            m_fsm.AddState(GameStates.LoadData);

            m_fsm.AddState(GameStates.MainMenu);

            m_fsm.AddState(GameStates.Playing);

            m_fsm.AddState(GameStates.GameOver);

            m_fsm.AddState(GameStates.Ready);

            m_fsm.AddState(GameStates.Paused,
                onEnter: _ => Time.timeScale = 0,
                onExit: _ => Time.timeScale = 1
            );

            m_fsm.AddState(GameStates.Quit, new State(onEnter: _ =>
            {
                Debug.Log("Good Bye!");
#if UNITY_EDITOR
                EditorApplication.isPlaying = false;
#endif
                Application.Quit();
            }));

            // Transitions

            m_fsm.AddTransition(GameStates.AutoLogin, GameStates.Login,
                condition: _ => !AuthenticationManager.Instance.IsAuthInProgress);

            m_fsm.AddTriggerTransition(GameEvents.Authenticated,
                new Transition(GameStates.AutoLogin, GameStates.LoadData));

            m_fsm.AddTriggerTransition(GameEvents.Authenticated, new Transition(GameStates.Login, GameStates.LoadData));

            m_fsm.AddTriggerTransition(GameEvents.OperationSuccess,
                new Transition(GameStates.LoadData, GameStates.MainMenu));
            m_fsm.AddTriggerTransition(GameEvents.Exit, new Transition(GameStates.LoadData, GameStates.Login));
            m_fsm.AddTriggerTransition(GameEvents.Back, new Transition(GameStates.LoadData, GameStates.Login));

            m_fsm.AddTriggerTransition(GameEvents.Restart, new Transition(GameStates.MainMenu, GameStates.Ready));
            m_fsm.AddTriggerTransition(GameEvents.Exit, new Transition(GameStates.MainMenu, GameStates.Quit));
            m_fsm.AddTriggerTransition(GameEvents.Back, new Transition(GameStates.MainMenu, GameStates.Quit));

            m_fsm.AddTriggerTransition(GameEvents.Back, new Transition(GameStates.Ready, GameStates.MainMenu));
            m_fsm.AddTriggerTransition(GameEvents.Action,
                new Transition(GameStates.Ready, GameStates.Playing,
                    onTransition: _ => gameSettings.Score.Value = 0)
            );

            m_fsm.AddTriggerTransition(GameEvents.Back, new Transition(GameStates.Playing, GameStates.Paused));
            m_fsm.AddTriggerTransition(GameEvents.GameOver, new Transition(GameStates.Paused, GameStates.GameOver));
            m_fsm.AddTriggerTransition(GameEvents.GameOver, new Transition(GameStates.Playing, GameStates.GameOver));

            m_fsm.AddTriggerTransition(GameEvents.Exit, new Transition(GameStates.Paused, GameStates.MainMenu));
            m_fsm.AddTriggerTransition(GameEvents.Restart, new Transition(GameStates.Paused, GameStates.Ready));
            m_fsm.AddTriggerTransition(GameEvents.Back, new Transition(GameStates.Paused, GameStates.Playing));

            m_fsm.AddTriggerTransition(GameEvents.Exit, new Transition(GameStates.GameOver, GameStates.MainMenu));
            m_fsm.AddTriggerTransition(GameEvents.Back, new Transition(GameStates.GameOver, GameStates.Ready));

            m_fsm.AddTriggerTransition(GameEvents.Restart, new Transition(GameStates.GameOver, GameStates.Ready));

            m_fsm.SetStartState(GameStates.AutoLogin);

            m_fsm.StateChanged += OnGameStateChanged;

            m_fsm.Init();
        }
    }
}