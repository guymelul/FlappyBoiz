using System.Threading;
using Cysharp.Threading.Tasks;
using FlappyBoiz.Scripts.Core.Controllers;
using FlappyBoiz.Scripts.Features.Lobby;
using VContainer;

namespace FlappyBoiz.Scripts.Features.Menu
{
    public sealed class MenuSceneController : SceneControllerBase<LobbyLifetimeScope>
    {
        protected override string SceneName => "MenuScene";

        public MenuSceneController(IObjectResolver container) : base(container)
        {
        }

        protected override async UniTask OnStartAsync(CancellationToken ct)
        {
            await StartChildAsync<MainMenuController>(ct);
        }
    }
}