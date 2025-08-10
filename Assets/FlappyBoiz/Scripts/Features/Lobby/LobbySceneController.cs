using System.Threading;
using Cysharp.Threading.Tasks;
using FlappyBoiz.Scripts.Core.Controllers;
using VContainer;

namespace FlappyBoiz.Scripts.Features.Lobby
{
    public sealed class LobbySceneController : SceneControllerBase<LobbyLifetimeScope>
    {
        protected override string SceneName => "LobbyScene";

        public LobbySceneController(IObjectResolver container) : base(container)
        {
        }

        protected override async UniTask OnStartAsync(CancellationToken ct)
        {
            await StartChildAsync<LobbyController>(ct);
        }
    }
}