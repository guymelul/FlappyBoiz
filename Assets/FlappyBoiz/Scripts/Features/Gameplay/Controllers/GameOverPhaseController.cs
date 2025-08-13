using System.Threading;
using Cysharp.Threading.Tasks;
using FlappyBoiz.Scripts.Core.Controllers;
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
        public GameOverPhaseController(IObjectResolver container) : base(container)
        {
        }

        protected override async UniTask<GameOverCommandResult> OnStartWithResultAsync(CancellationToken ct)
        {
            // TODO: wait for primary action or back button

            return GameOverCommandResult.MainMenu;
        }
    }
}