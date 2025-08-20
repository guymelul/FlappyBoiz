using System;
using FlappyBoiz.Scripts.Core.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

namespace FlappyBoiz.Scripts.Features.Gameplay.UI.Screens
{
    public class ReadyScreen : UIScreen
    {
        [field: SerializeField] public TMP_Text HighScoreText { get; set; }

        private void OnValidate()
        {
            Assert.IsNotNull(HighScoreText);
        }

        protected override void OnPreShown()
        {
            SetHighScore(0);
        }

        public void SetHighScore(int n)
        {
            HighScoreText.text = n.ToString("D3");
        }
    }
}