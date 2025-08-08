using FlappyBoiz.Scripts.Core.UI;
using UnityEngine;
using UnityEngine.UI;

namespace FlappyBoiz.Scripts.Features.Gameplay.UI.Screens
{
    public class PauseDialog : UIScreen
    {
        [field: SerializeField] public Button ResumeButton { get; set; }
        [field: SerializeField] public Button QuitButton { get; set; }
    }
}