using System.Threading;
using Cysharp.Threading.Tasks;
using FlappyBoiz.Scripts.Core.Controllers;
using FlappyBoiz.Scripts.Core.Services;
using UnityEngine;
using VContainer;

namespace FlappyBoiz.Scripts.Gameplay.Controllers
{
    public class StartupSequenceController : ControllerBase
    {
        private readonly ISceneLoaderService m_sceneLoader;

        public StartupSequenceController(
            IObjectResolver container,
            ISceneLoaderService sceneLoader) : base(container)
        {
            m_sceneLoader = sceneLoader;
        }

        protected override async UniTask OnStartAsync(CancellationToken ct)
        {
            // Show Splash Screen
            Debug.Log("Startup sequence");
            var splashScene = await m_sceneLoader.LoadSceneAsync("SplashScene");
            await StartChildAsync<SplashController>(ct);
            await m_sceneLoader.UnloadSceneAsync(splashScene);

            // Switch to gameplay scene
        }

        protected override UniTask OnStopAsync(CancellationToken ct) => UniTask.CompletedTask;

        protected override void OnDispose()
        {
        }
    }
}