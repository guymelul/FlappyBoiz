using FlappyBoiz.Scripts.Events;
using MessagePipe;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using VContainer;

namespace FlappyBoiz.Scripts.Gameplay.Managers
{
    public class GameInputManager : MonoBehaviour
    {
        [Inject]
        private readonly IPublisher<FlapperPrimaryActionRequest> m_actionPublisher;

        [Inject]
        private UIDocument m_uiDocument;

        [Inject]
        private InputActionAsset m_inputActionAsset;

        private IPanel m_uiPanel;
        private InputAction m_primaryContactAction;
        private InputAction m_pointerPositionAction;

        private void Awake()
        {
            var playerActionMap = m_inputActionAsset.FindActionMap("Player", true);
            m_primaryContactAction = playerActionMap.FindAction("PrimaryAction", true);
            m_pointerPositionAction = playerActionMap.FindAction("PrimaryActionPosition", true);
        }

        private void OnEnable()
        {
            m_primaryContactAction.performed += OnPrimaryContact;
        }

        private void OnDisable()
        {
            m_primaryContactAction.performed -= OnPrimaryContact;
        }

        private void Start()
        {
            m_uiPanel = m_uiDocument.runtimePanel;
        }

        private void OnPrimaryContact(InputAction.CallbackContext context)
        {
            if (m_pointerPositionAction.IsInProgress())
            {
                var pointerPosition = m_pointerPositionAction.ReadValue<Vector2>();

                pointerPosition = new Vector2(pointerPosition.x, Screen.height - pointerPosition.y);

                var panelPosition = RuntimePanelUtils.ScreenToPanel(m_uiPanel, pointerPosition);

                var pickedElement = m_uiPanel.Pick(panelPosition);

                if (pickedElement != null)
                {
                    // If we picked any VisualElement, it means the pointer is over the UI.
                    // Do nothing; the UI will handle its own click/touch events.
                    Debug.Log($"Pointer is over UI element: {pickedElement.name}");

                    return;
                }
            }

            m_actionPublisher.Publish(FlapperPrimaryActionRequest.Empty);
        }
    }
}