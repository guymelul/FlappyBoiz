using FlappyBoiz.Scripts.Gameplay.Managers;
using FlappyBoiz.Scripts.Gameplay.UI.Screens;
using VContainer;

namespace FlappyBoiz.Scripts.Gameplay.UI.Controllers
{
    public class GameOverController : BaseController<GameOverScreen>
    {
        [Inject] private GameSessionManager GameSession { get; set; }

        protected override void OnScreenInit()
        {
            Screen.Score = GameSession.Score.CurrentValue;
            Screen.EarnedCoins = GameSession.Coins;
            Screen.HighScore = 0;
        }
    }
}