using System.Threading;
using Cysharp.Threading.Tasks;
using FlappyBoiz.Scripts.Core.Controllers;
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
            // TODO: wait for primary action or back button
            Screen.Score = GameSessionManager.Score.CurrentValue;
            Screen.HighScore = 0;

            await Screen.Show(ct);


            await UniTask.Never(ct);

            return GameOverCommandResult.MainMenu;
        }
    }
}