using System;
using FlappyBoiz.Scripts.Core.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace FlappyBoiz.Scripts.Gameplay.UI.Screens
{
    public class HUDScreen : UIScreen
    {
        [SerializeField] private TMP_Text scoreText;

        [SerializeField] private Button pauseButton;

        public Action OnPauseButtonClick;

        private void Awake()
        {
            scoreText.text = 0.ToString();
        }

        private void OnValidate()
        {
            Assert.IsNotNull(scoreText);
            Assert.IsNotNull(pauseButton);
        }

        private void OnEnable()
        {
            pauseButton.onClick.AddListener(OnPauseClick);
        }

        private void OnDisable()
        {
            pauseButton.onClick.RemoveAllListeners();
        }

        private void OnPauseClick()
        {
            Debug.Log("Game Pause");
            OnPauseButtonClick?.Invoke();
        }

        public void SetScore(int score)
        {
            scoreText.text = score.ToString();
        }
    }
}