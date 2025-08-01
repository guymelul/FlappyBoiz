using System.Threading;
using Cysharp.Threading.Tasks;
using FlappyBoiz.Scripts.Core.UI;
using FlappyBoiz.Scripts.Events;
using FlappyBoiz.Scripts.Gameplay.UI.Screens;
using MessagePipe;
using UnityEngine.AddressableAssets;
using VContainer;
using VContainer.Unity;

namespace FlappyBoiz.Scripts.Gameplay.UI.Controllers
{
    public class PauseDialogController : BaseController<PauseDialog>, IAsyncStartable
    {
        [Inject] private IPublisher<GamePauseCommand> OnGamePauseCommand { get; set; }

        public async UniTask StartAsync(CancellationToken cancellation = new CancellationToken())
        {
            using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cancellation);

            var ct = linkedCts.Token;

            var button = await ButtonSelector(ct, Screen.ResumeButton, Screen.QuitButton);

            linkedCts.Cancel();

            if (button == Screen.ResumeButton) OnResumeGame();
            else if (button == Screen.QuitButton) OnQuitGame();
        }

        private void OnResumeGame()
        {
            OnGamePauseCommand.Publish(GamePauseCommand.Empty);
        }

        private void OnQuitGame()
        {
            Addressables.LoadSceneAsync("MainMenuScene").WaitForCompletion();
        }
    }
}