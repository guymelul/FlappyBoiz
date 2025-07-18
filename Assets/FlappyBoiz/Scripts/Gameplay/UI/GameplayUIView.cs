using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace FlappyBoiz.Scripts.Gameplay.UI
{
    public class GameplayUIView : MonoBehaviour
    {
        private VisualElement m_readyScreen;
        private VisualElement m_hudScreen;
        private VisualElement m_pauseOverlay;

        private List<VisualElement> m_allVisualElements;

        private void OnEnable()
        {
            var root = GetComponent<UIDocument>().rootVisualElement;

            m_readyScreen = root.Q<VisualElement>("ready-screen");
            m_hudScreen = root.Q<VisualElement>("hud-screen");
            m_pauseOverlay = root.Q<VisualElement>("pause-overlay");

            m_allVisualElements = new List<VisualElement>
            {
                m_readyScreen, m_hudScreen, m_pauseOverlay
            };

            HideAll();
        }

        public void HideAll()
        {
            foreach (var ve in m_allVisualElements)
            {
                ve.style.display = DisplayStyle.None;
            }
        }

        public void ShowReadyScreen() => Show(m_readyScreen);

        public void ShowHUDScreen() => Show(m_hudScreen);

        public void ShowPauseOverlay() => Show(m_pauseOverlay);

        private void Show(VisualElement screen)
        {
            HideAll();

            Debug.Log($"Showing screen {screen.name}");

            screen.style.display = DisplayStyle.Flex;

            // TODO: animate fade-in using special visible class
        }
    }
}