using System.Threading;
using Cysharp.Threading.Tasks;
using FlappyBoiz.Scripts.Core.Controllers;
using VContainer;

namespace FlappyBoiz.Scripts.Core
{
    public class GameRootController : RootController
    {
        public GameRootController(IObjectResolver container) : base(container)
        {
        }

        protected override UniTask OnStartAsync(CancellationToken ct)
        {
            return UniTask.CompletedTask;
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