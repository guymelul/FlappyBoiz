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
    }
}