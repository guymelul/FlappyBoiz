using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using FlappyBoiz.Scripts.Core.Controllers;
using FlappyBoiz.Scripts.Events;
using FlappyBoiz.Scripts.Features.Gameplay.Managers;
using FlappyBoiz.Scripts.Features.Gameplay.UI.Screens;
using MessagePipe;
using R3;
using VContainer;
using DisposableBag = MessagePipe.DisposableBag;

namespace FlappyBoiz.Scripts.Features.Gameplay.Controllers
{
    public enum GameCommandResult
    {
        GameOver,
        Quit
    }

    public class GamePhaseController : ControllerBase<GameCommandResult>
    {
        [Inject] public HUDScreen Screen { get; set; }
        [Inject] public IPublisher<GamePausedEvent> GamePausedPub { get; set; }
        [Inject] public IPublisher<GameResumedEvent> GameResumedPub { get; set; }
        [Inject] public GameSessionManager GameSession { get; set; }
        [Inject] public ISubscriber<GamePauseCommand> OnGamePauseCommand { get; set; }

        private IDisposable m_disposables;

        public GamePhaseController(IObjectResolver container) : base(container)
        {
        }

        protected override async UniTask<GameCommandResult> OnStartWithResultAsync(CancellationToken ct)
        {
            try
            {
                await Screen.Show(ct);

                var tcs = new UniTaskCompletionSource<GameCommandResult>();
                var builder = DisposableBag.CreateBuilder();

                GameSession.IsDead
                    .Where(b => b)
                    .Subscribe(_ => tcs.TrySetResult(GameCommandResult.GameOver))
                    .AddTo(builder);

                GameSession.Score
                    .Subscribe(score => Screen.ScoreText.text = score.ToString())
                    .AddTo(builder);

                // TODO: wait back button

                m_disposables = builder.Build();

                // TODO: take care of pause

                return await tcs.Task;
            }
            finally
            {
                await Screen.Hide(ct);
                m_disposables?.Dispose();
            }
        }
    }
}