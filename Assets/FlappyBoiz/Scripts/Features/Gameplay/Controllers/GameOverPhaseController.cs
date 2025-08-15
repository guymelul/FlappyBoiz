using System.Threading;
using Cysharp.Threading.Tasks;
using FlappyBoiz.Scripts.Core.Controllers;
using FlappyBoiz.Scripts.Core.UI;
using FlappyBoiz.Scripts.Features.Gameplay.Managers;
using FlappyBoiz.Scripts.Features.Gameplay.UI.Screens;
using VContainer;

namespace FlappyBoiz.Scripts.Features.Gameplay.Controllers
{
    public enum GameOverCommandResult
    {
        MainMenu,
        Restart
    }

    public class GameOverPhaseController : ControllerBase<GameOverCommandResult>
    {
        [Inject] public GameOverScreen Screen { get; set; }
        [Inject] public GameSessionManager GameSessionManager { get; set; }

        public GameOverPhaseController(IObjectResolver container) : base(container)
        {
        }

        protected override async UniTask<GameOverCommandResult> OnStartWithResultAsync(CancellationToken ct)
        {
            Screen.Score = GameSessionManager.Score.CurrentValue;
            Screen.HighScore = 0;
            Screen.EarnedCoins = 5;

            await Screen.Show(ct);

            using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(ct);

            var selectedButton = await UIControllerUtils.ButtonSelector(linkedCts.Token,
                Screen.MainMenuButton, Screen.RestartButton
            );

            linkedCts.Cancel();

            return selectedButton == Screen.RestartButton
                ? GameOverCommandResult.Restart
                : GameOverCommandResult.MainMenu;
        }
    }
}