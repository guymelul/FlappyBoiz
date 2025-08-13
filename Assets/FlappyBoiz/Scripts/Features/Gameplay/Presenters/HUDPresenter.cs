using System.Threading;
using Cysharp.Threading.Tasks;
using FlappyBoiz.Scripts.Core.UI;
using FlappyBoiz.Scripts.Events;
using FlappyBoiz.Scripts.Features.Gameplay.Managers;
using FlappyBoiz.Scripts.Features.Gameplay.UI.Screens;
using MessagePipe;
using R3;
using VContainer;
using VContainer.Unity;

namespace FlappyBoiz.Scripts.Features.Gameplay.Presenters
{
    public class HUDPresenter : BasePresenter<HUDScreen>, IAsyncStartable
    {
        [Inject] public IPublisher<GamePauseCommand> GamePauseCommandPub { get; set; }

        [Inject] public GameSessionManager GameSessionManager { get; set; }

        public async UniTask StartAsync(CancellationToken cancellation = new())
        {
            SetScore(0);
            GameSessionManager.Score.Subscribe(SetScore);

            using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cancellation);

            var button = await ButtonSelector(linkedCts.Token, Screen.PauseButton);

            linkedCts.Cancel();

            if (button == Screen.PauseButton) OnPauseGame();
        }

        private void OnPauseGame()
        {
            GamePauseCommandPub.Publish(GamePauseCommand.Empty);
        }

        private void SetScore(int score)
        {
            Screen.ScoreText.text = score.ToString();
        }
    }
}