using FlappyBoiz.Scripts.Events;
using MessagePipe;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using VContainer;

namespace FlappyBoiz.Scripts.Gameplay.Managers
{
    [RequireComponent(typeof(PlayerInput))]
    public class GameplayInputManager : MonoBehaviour
    {
        [Inject]
        private readonly IPublisher<FlapperPrimaryActionRequest> m_actionPublisher;

        [Inject]
        private readonly IPublisher<GamePauseCommand> m_gamePauseCommand;

        private EventSystem m_currentEventSystem;

        private void Start()
        {
            m_currentEventSystem = EventSystem.current;
        }

        public void OnPrimaryAction(InputAction.CallbackContext context)
        {
            if (!context.performed) return;

            if (m_currentEventSystem.IsPointerOverGameObject())
            {
                // TODO: Fix this case
                Debug.Log("Primary action denied due to UI");

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