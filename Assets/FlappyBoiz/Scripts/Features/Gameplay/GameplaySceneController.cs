using System.Threading;
using Cysharp.Threading.Tasks;
using FlappyBoiz.Scripts.Core.Controllers;
using VContainer;

namespace FlappyBoiz.Scripts.Features.Gameplay
{
    public sealed class GameplaySceneController : SceneControllerBase<GameplayLifetimeScope>
    {
        protected override string SceneName => "GameplayScene";

        public GameplaySceneController(IObjectResolver container) : base(container)
        {
        }

        protected override async UniTask OnStartAsync(CancellationToken ct)
        {
            await UniTask.Never(ct);
        }
    }
}