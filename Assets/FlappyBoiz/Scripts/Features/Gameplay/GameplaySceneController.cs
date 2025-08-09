using System.Threading;
using Cysharp.Threading.Tasks;
using FlappyBoiz.Scripts.Core.Controllers;
using VContainer;

namespace FlappyBoiz.Scripts.Features.Gameplay
{
    public class GameplaySceneController : SceneControllerBase<GameplayLifetimeScope>
    {
        public GameplaySceneController(IObjectResolver container) : base(container,
            sceneName: "GameplayScene")
        {
        }

        protected override async UniTask OnStartAsync(CancellationToken ct)
        {
            // TODO: start gameplay controller
        }
    }
}