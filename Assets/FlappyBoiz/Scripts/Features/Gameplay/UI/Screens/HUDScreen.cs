using FlappyBoiz.Scripts.Core.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FlappyBoiz.Scripts.Features.Gameplay.UI.Screens
{
    public class HUDScreen : UIScreen
    {
        [field: SerializeField] public TMP_Text ScoreText { get; set; }

        [field: SerializeField] public Button PauseButton { get; set; }
    }
}