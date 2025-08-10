using System.Threading;
using Cysharp.Threading.Tasks;
using FlappyBoiz.Scripts.Core.Controllers;
using VContainer;

namespace FlappyBoiz.Scripts.Features.Splash
{
    public class SplashController : ControllerBase
    {
        private readonly SplashView m_splashView;

        public SplashController(IObjectResolver container, SplashView splashView) : base(container)
        {
            m_splashView = splashView;
        }

        protected override async UniTask OnStartAsync(CancellationToken ct)
        {
            await m_splashView.StartSplashAnimation(ct);
        }

        protected override UniTask OnStopAsync(CancellationToken ct) => UniTask.CompletedTask;

        protected override void OnDispose()
        {
        }
    }
}