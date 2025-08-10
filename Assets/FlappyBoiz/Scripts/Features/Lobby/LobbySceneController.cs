using System.Threading;
using Cysharp.Threading.Tasks;
using FlappyBoiz.Scripts.Core.Controllers;
using VContainer;

namespace FlappyBoiz.Scripts.Features.Lobby
{
    public class LobbySceneController : SceneControllerBase<LobbyLifetimeScope>
    {
        protected override string SceneName => "LobbyScene";

        public LobbySceneController(IObjectResolver container) : base(container)
        {
        }

        protected override async UniTask OnStartAsync(CancellationToken ct)
        {
            // TODO: Start auth controller (wont stop until user is authenticated)

            // TODO: If auth is success
            //  Start saved game loader controller
        }
    }
}