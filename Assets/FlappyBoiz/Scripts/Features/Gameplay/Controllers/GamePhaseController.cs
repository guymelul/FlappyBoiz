using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using FlappyBoiz.Scripts.Core.Controllers;
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

                        Time.timeScale = 0;
                        var pauseController = await StartChildAsync<GamePauseController>(ct);
                        Time.timeScale = 1;

                        if (pauseController.Result == PauseCommandResult.Continue)
                            continue;

                        if (pauseController.Result == PauseCommandResult.Quit)
                            break;
                    }

                    if (completedTask == 1)
                    {
                        return GameCommandResult.GameOver;
                    }
                } while (true);

                // TODO: wait back button

                // TODO: take care of pause

                return GameCommandResult.Quit;
            }
            finally
            {
                Time.timeScale = 1;
                await Screen.Hide(ct);
                m_disposables?.Dispose();
            }
        }
    }
}