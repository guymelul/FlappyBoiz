using System.Threading;
using Cysharp.Threading.Tasks;
using FlappyBoiz.Scripts.Core.Controllers;
using UnityEngine;
using VContainer;

namespace FlappyBoiz.Scripts.Features.Startup
{
    public class GameRootController : RootController
    {
        public GameRootController(IObjectResolver container) : base(container)
        {
        }

        protected override async UniTask OnStartAsync(CancellationToken ct)
        {
            Debug.Log("Starting game");
            await StartChildAsync<StartupSequenceController>(ct);
        }

        protected override UniTask OnStopAsync(CancellationToken ct)
        {
            return UniTask.CompletedTask;
        }

        protected override void OnDispose()
        {
        }
    }
}