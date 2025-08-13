using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using FlappyBoiz.Scripts.Core.Controllers;
using FlappyBoiz.Scripts.Events;
using FlappyBoiz.Scripts.Features.Gameplay.UI.Screens;
using MessagePipe;
using VContainer;

namespace FlappyBoiz.Scripts.Features.Gameplay.Controllers
{
    public enum ReadyCommandResult
    {
        Play,
        Quit
    }

    public class ReadyPhaseController : ControllerBase<ReadyCommandResult>
    {
        [Inject] private ReadyScreen Screen { get; set; }
        [Inject] private ISubscriber<FlapperPrimaryActionRequest> OnPrimaryAction { get; set; }

        private IDisposable m_disposables;

        public ReadyPhaseController(IObjectResolver container) : base(container)
        {
        }

        protected override async UniTask<ReadyCommandResult> OnStartWithResultAsync(CancellationToken ct)
        {
            try
            {
                await Screen.Show(ct);

                var tcs = new UniTaskCompletionSource<ReadyCommandResult>();
                var builder = DisposableBag.CreateBuilder();

                OnPrimaryAction.Subscribe(_ => tcs.TrySetResult(ReadyCommandResult.Play)).AddTo(builder);
                // TODO: wait back button

                m_disposables = builder.Build();

                await tcs.Task;
            }
            finally
            {
                m_disposables?.Dispose();
                await Screen.Hide(CancellationToken.None);
            }

            return ReadyCommandResult.Quit;
        }
    }
}