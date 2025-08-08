using System.Threading;
using Cysharp.Threading.Tasks;
using FlappyBoiz.Scripts.Core.Controllers;
using FlappyBoiz.Scripts.Core.UI;
using R3;
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
            m_splashView.StartSplashAnimation();
            await m_splashView.OnSplashAnimationEnded.FirstAsync(ct).AsUniTask();
        }

        protected override UniTask OnStopAsync(CancellationToken ct) => UniTask.CompletedTask;

        protected override void OnDispose()
        {
        }
    }
}