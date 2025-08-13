using System.Threading;
using Cysharp.Threading.Tasks;
using FlappyBoiz.Scripts.Core.Controllers;
using VContainer;

namespace FlappyBoiz.Scripts.Features.Gameplay.Controllers
{
    public enum PauseCommandResult
    {
        Continue,
        Quit
    }

    public class GamePauseController : ControllerBase<PauseCommandResult>
    {
        public GamePauseController(IObjectResolver container) : base(container)
        {
        }

        protected override async UniTask<PauseCommandResult> OnStartWithResultAsync(CancellationToken ct)
        {
            // TODO: wait for primary action or back button

            return PauseCommandResult.Quit;
        }
    }
}