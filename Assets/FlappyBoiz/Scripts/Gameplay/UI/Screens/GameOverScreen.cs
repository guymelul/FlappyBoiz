using System.Collections;
using FlappyBoiz.Scripts.Gameplay.UI.Animations;
using LitMotion;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FlappyBoiz.Scripts.Gameplay.UI.Screens
{
    public class GameOverScreen : UIScreen
    {
        [field: SerializeField] public Button RestartButton { get; private set; }

        [field: SerializeField] public Button MainMenuButton { get; private set; }

        [field: SerializeField] public TMP_Text ScoreText { get; private set; }

        [field: SerializeField] public TMP_Text HighScoreText { get; private set; }

        [field: SerializeField] public TMP_Text EarnedCoinsText { get; private set; }

        public int Score { get; set; }
        public int HighScore { get; set; }
        public int EarnedCoins { get; set; }

        protected override void OnPreShown()
        {
            ScoreText.text = 0.ToString();
            EarnedCoinsText.text = 0.ToString();

            // Wont be animated
            HighScoreText.text = HighScore.ToString();
        }

        public void StartScoreTextAnimation()
        {
            StartCoroutine(ScoreTextAnimation());
        }

        private IEnumerator ScoreTextAnimation()
        {
            yield return TextAnimations
                .NumericTextMotion(ScoreText, 0, Score, 1.6f)
                .AddTo(gameObject)
                .ToYieldInstruction();
        }

        public void StartCoinsTextAnimation()
        {
            StartCoroutine(TextAnimations
                .NumericTextMotion(EarnedCoinsText, 0, EarnedCoins, 2f)
                .AddTo(gameObject)
                .ToYieldInstruction()
            );
        }
    }
}