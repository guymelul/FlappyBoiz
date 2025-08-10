using FlappyBoiz.Scripts.Events;
using MessagePipe;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using VContainer;

namespace FlappyBoiz.Scripts.Features.Gameplay.Managers
{
    public class GameplayInputManager : MonoBehaviour
    {
        [Inject] private readonly IPublisher<FlapperPrimaryActionRequest> m_actionPublisher;

        [Inject] private readonly IPublisher<GamePauseCommand> m_gamePauseCommand;

        private InputAction m_primaryAction;
        private InputAction m_pauseAction;
        private EventSystem m_currentEventSystem;

        private void Awake()
        {
            var inputActions = new InputSystemActions();
            m_primaryAction = inputActions.Player.PrimaryAction;
            m_pauseAction = inputActions.Player.Pause;
        }

        private void OnEnable()
        {
            m_pauseAction.performed += OnPauseAction;
        }

        private void OnDisable()
        {
            m_pauseAction.performed -= OnPauseAction;
        }

        private void Start()
        {
            m_currentEventSystem = EventSystem.current;
        }

        private void Update()
        {
            if (!m_primaryAction.WasPerformedThisFrame()) return;

            if (m_currentEventSystem.IsPointerOverGameObject())
            {
                return;
            }

            m_actionPublisher.Publish(FlapperPrimaryActionRequest.Empty);
        }

        public void OnPauseAction(InputAction.CallbackContext context)
        {
            if (!context.performed) return;

            m_gamePauseCommand.Publish(GamePauseCommand.Empty);
        }
    }
}