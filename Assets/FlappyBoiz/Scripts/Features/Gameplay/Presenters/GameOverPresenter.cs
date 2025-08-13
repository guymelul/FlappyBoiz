using FlappyBoiz.Scripts.Core.UI;
using FlappyBoiz.Scripts.Features.Gameplay.Managers;
using FlappyBoiz.Scripts.Features.Gameplay.UI.Screens;
using VContainer;

namespace FlappyBoiz.Scripts.Features.Gameplay.Presenters
{
    public class GameOverPresenter : BasePresenter<GameOverScreen>
    {
        [Inject] private GameSessionManager GameSession { get; set; }

        protected override void OnScreenInit()
        {
            Screen.Score = GameSession.Score.CurrentValue;
            Screen.HighScore = 0;
        }
    }
}