using System.Threading;
using Cysharp.Threading.Tasks;
using FlappyBoiz.Scripts.Core.Controllers;
using FlappyBoiz.Scripts.Core.Services;
using FlappyBoiz.Scripts.Features.Splash;
using UnityEngine;
using VContainer;

namespace FlappyBoiz.Scripts.Features.Startup
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

            Debug.Log("Starting splash scene controller");
            var splash = await StartChildAsync<SplashSceneController>(ct);
            
            // Start Fader
            
            splash.Dispose();

            // Switch to gameplay scene
            Debug.Log("Gameplay scene");
        }

        protected override UniTask OnStopAsync(CancellationToken ct) => UniTask.CompletedTask;

        protected override void OnDispose()
        {
        }
    }
}