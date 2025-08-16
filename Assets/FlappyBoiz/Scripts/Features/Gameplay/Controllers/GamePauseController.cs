using System.Threading;
using Cysharp.Threading.Tasks;
using FlappyBoiz.Scripts.Core.Controllers;
using FlappyBoiz.Scripts.Core.UI;
using FlappyBoiz.Scripts.Features.Gameplay.UI.Screens;
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
        [Inject] public PauseDialogScreen Screen { get; set; }

        public GamePauseController(IObjectResolver container) : base(container)
        {
        }

        protected override async UniTask<PauseCommandResult> OnStartWithResultAsync(CancellationToken ct)
        {
            await Screen.Show(ct);

            using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(ct);

            var linkedToken = linkedCts.Token;

            var button = await UIControllerUtils.ButtonSelector(linkedToken,
                Screen.ResumeButton, Screen.QuitButton
            );

            linkedCts.Cancel();

            bool resuming = button == Screen.ResumeButton;

            var screenHide = Screen.Hide(ct);

            if (!resuming) return PauseCommandResult.Quit;

            await screenHide;

            return PauseCommandResult.Continue;

        }
    }
}