using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using FlappyBoiz.Scripts.Core.Controllers;
using FlappyBoiz.Scripts.Events;
using FlappyBoiz.Scripts.Features.Gameplay.Managers;
using FlappyBoiz.Scripts.Features.Gameplay.UI.Screens;
using MessagePipe;
using R3;
using UnityEngine;
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
        [Inject] public IAsyncPublisher<GameEndedEvent> GameEndedEventPub { get; set; }
        [Inject] public IAsyncPublisher<GameStartedEvent> GameStartedEventPub { get; set; }

        [Inject] public HUDScreen Screen { get; set; }
        [Inject] public GameSessionManager GameSession { get; set; }

        private IDisposable m_disposables;

        public GamePhaseController(IObjectResolver container) : base(container)
        {
        }

        protected override async UniTask<GameCommandResult> OnStartWithResultAsync(CancellationToken ct)
        {
            try
            {
                await GameStartedEventPub.PublishAsync(GameStartedEvent.Empty, cancellationToken: ct);

                await Screen.Show(ct);

                var gameOverCompletionSource = new UniTaskCompletionSource();

                var builder = DisposableBag.CreateBuilder();

                GameSession.IsDead
                    .Where(b => b)
                    .Subscribe(_ => gameOverCompletionSource.TrySetResult())
                    .AddTo(builder);

                GameSession.Score
                    .Subscribe(score => Screen.ScoreText.text = score.ToString())
                    .AddTo(builder);

                m_disposables = builder.Build();

                do
                {
                    int completedTask;

                    using (var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(ct))
                    {
                        var linkedCt = linkedCts.Token;

                        var pauseButtonTask = Screen.PauseButton.OnClickAsync(linkedCt);
                        var gameOverTask = gameOverCompletionSource.Task;

                        completedTask = await UniTask.WhenAny(
                            pauseButtonTask,
                            gameOverTask
                        );

                        linkedCts.Cancel();
                    }

                    if (completedTask == 0)
                    {
                        Debug.Log("Game paused");

                        try
                        {
                            Screen.DisableInteraction();
                            Time.timeScale = 0;
                            var pauseController = await StartChildAsync<GamePauseController>(ct);

                            if (pauseController.Result == PauseCommandResult.Continue)
                                continue;

                            if (pauseController.Result == PauseCommandResult.Quit)
                                break;
                        }
                        finally
                        {
                            Time.timeScale = 1;
                            Screen.EnableInteraction();
                        }
                    }

                    if (completedTask != 1) continue;

                    Debug.Log("Game ending event");
                    await GameEndedEventPub.PublishAsync(GameEndedEvent.Empty, cancellationToken: ct);

                    return GameCommandResult.GameOver;
                } while (true);

                return GameCommandResult.Quit;
            }
            finally
            {
                m_disposables?.Dispose();
                Screen.Hide(ct).Forget();
            }
        }
    }
}