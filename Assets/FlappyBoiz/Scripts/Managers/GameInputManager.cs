using FlappyBoiz.Scripts.Events;
using MessagePipe;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using VContainer;

namespace FlappyBoiz.Scripts.Managers
{
    public class GameInputManager : MonoBehaviour
    {
        [Inject]
        private readonly IPublisher<FlapperPrimaryActionRequest> m_actionPublisher;

        // You need a reference to the UIDocument that hosts your UI.
        [SerializeField]
        private UIDocument uiHudDocument;

        [SerializeField]
        private InputActionAsset inputActionAsset;

        private IPanel m_uiPanel;
        private InputAction m_primaryContactAction;
        private InputAction m_pointerPositionAction;

        private void Awake()
        {
            var playerActionMap = inputActionAsset.FindActionMap("Player", true);
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
            m_uiPanel = uiHudDocument.runtimePanel;
        }

        private void OnPrimaryContact(InputAction.CallbackContext context)
        {
            var pointerPosition = m_pointerPositionAction.ReadValue<Vector2>();

            pointerPosition = new Vector2(pointerPosition.x, Screen.height - pointerPosition.y);

            var panelPosition = RuntimePanelUtils.ScreenToPanel(m_uiPanel, pointerPosition);

            var pickedElement = m_uiPanel.Pick(panelPosition);

            if (pickedElement != null)
            {
                // If we picked any VisualElement, it means the pointer is over the UI.
                // Do nothing; the UI will handle its own click/touch events.
                return;
            }

            m_actionPublisher.Publish(FlapperPrimaryActionRequest.Empty);
        }
    }
}