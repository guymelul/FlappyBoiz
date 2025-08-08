using System.Threading;
using Cysharp.Threading.Tasks;
using FlappyBoiz.Scripts.Core.Controllers;
using FlappyBoiz.Scripts.Core.UI;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace FlappyBoiz.Scripts.Features.Splash
{
    public class SplashSceneController : SceneControllerBase
    {
        public SplashSceneController(IObjectResolver container) : base(container, "SplashScene")
        {
        }

        protected override async UniTask OnStartAsync(CancellationToken ct)
        {
            Debug.Log("Starting splash controller");
            await StartChildAsync<SplashController>(ct);
        }

        protected override void Configure(IContainerBuilder builder)
        {
            Debug.Log("SplashLifetimeScope Configure");
            builder.Register<SplashController>(Lifetime.Transient);
            builder.RegisterComponentInHierarchy<SplashView>();
        }

        protected override void OnDispose()
        {
        }
    }
}