using System.Threading;
using Cysharp.Threading.Tasks;
using FlappyBoiz.Scripts.Core.UI;
using FlappyBoiz.Scripts.Features.Gameplay.UI.Screens;
using UnityEngine.AddressableAssets;
using VContainer.Unity;

namespace FlappyBoiz.Scripts.Features.Gameplay.Presenters
{
    public class MainMenuPresenter : BasePresenter<MainMenuScreen>, IAsyncStartable
    {
        public async UniTask StartAsync(CancellationToken cancellation = new())
        {
            using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cancellation);

            var button = await ButtonSelector(linkedCts.Token, Screen.StartButton);

            linkedCts.Cancel();

            if (button == Screen.StartButton) OnStartGame();
        }

        private void OnStartGame()
        {
            Addressables.LoadSceneAsync("GameplayScene").WaitForCompletion();
        }
    }
}