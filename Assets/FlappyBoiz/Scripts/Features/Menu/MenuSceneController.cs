using System.Threading;
using Cysharp.Threading.Tasks;
using FlappyBoiz.Scripts.Core.Controllers;
using VContainer;

namespace FlappyBoiz.Scripts.Features.Menu
{
    public sealed class MenuSceneController : SceneControllerBase<MenuLifetimeScope, MainMenuChoiceResult>
    {
        protected override string SceneName => "MenuScene";

        public MenuSceneController(IObjectResolver container) : base(container)
        {
        }

        protected override async UniTask<MainMenuChoiceResult> OnStartWithResultAsync(CancellationToken ct)
        {
            var menuController = await StartChildAsync<MainMenuController>(ct);
            return menuController.Result;
        }
    }
}