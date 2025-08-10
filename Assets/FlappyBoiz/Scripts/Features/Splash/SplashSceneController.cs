using System.Threading;
using Cysharp.Threading.Tasks;
using FlappyBoiz.Scripts.Core.Controllers;
using VContainer;

namespace FlappyBoiz.Scripts.Features.Splash
{
    public class SplashSceneController : SceneControllerBase<SplashLifetimeScope>
    {
        protected override string SceneName => "SplashScene";

        public SplashSceneController(IObjectResolver container) : base(container)
        {
        }

        protected override async UniTask OnStartAsync(CancellationToken ct)
        {
            await StartChildAsync<SplashController>(ct);
        }
    }
}