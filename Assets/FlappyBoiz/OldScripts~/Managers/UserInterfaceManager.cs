using System.Collections.Generic;
using FlappyBoiz.Scripts.UI;
using FlappyBoiz.Scripts.UI.Screen;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

namespace FlappyBoiz.Scripts.Managers
{
    public static class Dialogs
    {
        public const string SkinSelection = "DIAL_SKINSELCT";
        public const string Settings = "DIAL_SETTINGS";
        public const string Leaderboard = "DIAL_LEADERBOARD";
    }

    [RequireComponent(typeof(Canvas))]
    public class UserInterfaceManager : MonoBehaviour
    {
        #region Event Bus

        [SerializeField]
        private StringEvent gameStateChanged;

        [SerializeField]
        private StringEvent gameEventBus;

        [SerializeField]
        private StringEvent dialogShowBus;

        #endregion

        #region Screens

        [SerializeField]
        private GameScreen loadingScreen;

        [SerializeField]
        private GameScreen loginScreen;

        [SerializeField]
        private GameScreen mainMenuScreen;

        [SerializeField]
        private GameScreen readyScreen;

        [SerializeField]
        private GameScreen gameOverScreen;

        [SerializeField]
        private GameScreen hud;

        [SerializeField]
        private GameScreen pauseDialog;

        [SerializeField]
        private GameScreen skinSelectionDialog;

        [SerializeField]
        private GameScreen settingsDialog;

        [SerializeField]
        private GameScreen leaderboardDialog;

        #endregion

        private Stack<GameScreen> m_canvasStack;
        private InputAction m_cancel;

        private void Awake()
        {
            m_canvasStack = new Stack<GameScreen>(5);
        }

        private void OnEnable()
        {
            m_cancel = InputSystem.actions.FindActionMap("UI").FindAction("Cancel");
            gameStateChanged?.Register(OnGameStateChanged);
            gameEventBus?.Register(OnGameEvent);
            dialogShowBus?.Register(OnDialogShow);
            m_cancel.performed += OnCancel;
        }

        private void OnDisable()
        {
            gameStateChanged?.Unregister(OnGameStateChanged);
            gameEventBus?.Unregister(OnGameEvent);
            dialogShowBus?.Unregister(OnDialogShow);
            m_cancel.performed -= OnCancel;
        }

        #region Canvas Transitions

        private void Show(GameScreen gameScreen, bool overlay = false)
        {
            Assert.IsNotNull(gameScreen, "No game screen was chosen to show");
            
            if (m_canvasStack.Count > 0)
            {
                if (gameScreen == m_canvasStack.Peek()) return;

                if (!overlay)
                {
                    // Hide all GameScreen when switching screens
                    while (m_canvasStack.Count > 0)
                    {
                        HideActive();
                    }
                }
                else
                {
                    m_canvasStack.Peek().ChangeInteractable(false);
                }
            }

            m_canvasStack.Push(gameScreen);

            gameScreen.Show();
        }

        private void HideActive()
        {
            if (m_canvasStack.Count <= 0) return;

            var gameScreen = m_canvasStack.Pop();

            gameScreen.Hide();

            if (m_canvasStack.Count > 0)
            {
                m_canvasStack.Peek().ChangeInteractable(true);
            }
        }

        #endregion

        #region Event Handlers

        private void OnCancel(InputAction.CallbackContext ctx)
        {
            gameEventBus.Raise(GameEvents.UIBack);
        }

        private void OnGameEvent(string gameEvent)
        {
            if (gameEvent == GameEvents.UIBack) OnUIBack();
        }

        private void OnUIBack()
        {
            // Check if no dialog is open
            if (m_canvasStack.Count <= 1)
            {
                gameEventBus.Raise(GameEvents.Back);

                return;
            }

            // Other GameScreen is open we should close it
            HideActive();
        }

        private void OnGameStateChanged(string gameState)
        {
            GameScreen screenToShow;

            switch (gameState)
            {
                case GameStates.AutoLogin:
                case GameStates.LoadData:
                    screenToShow = loadingScreen;

                    break;
                case GameStates.Login:
                    Show(loginScreen, overlay: true);

                    return;
                case GameStates.MainMenu:
                    screenToShow = mainMenuScreen;

                    break;
                case GameStates.Ready:
                    screenToShow = readyScreen;

                    break;
                case GameStates.Playing:
                    screenToShow = hud;

                    break;
                case GameStates.Paused:
                    screenToShow = pauseDialog;

                    break;
                case GameStates.GameOver:
                    screenToShow = gameOverScreen;

                    break;
                default:
                    Debug.Log($"UI Manager Ignoring state: {gameState}");

                    return;
            }

            Show(screenToShow);
        }

        private void OnDialogShow(string dialogName)
        {
            GameScreen dialog;

            switch (dialogName)
            {
                case Dialogs.SkinSelection:
                    dialog = skinSelectionDialog;

                    break;
                case Dialogs.Settings:
                    dialog = settingsDialog;

                    break;
                case Dialogs.Leaderboard:
                    dialog = leaderboardDialog;

                    break;
                default:
                    return;
            }

            Show(dialog, overlay: true);
        }

        #endregion
    }
}