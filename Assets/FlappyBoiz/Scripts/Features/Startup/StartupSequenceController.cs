using System.Threading;
using Cysharp.Threading.Tasks;
using FlappyBoiz.Scripts.Core.Controllers;
using FlappyBoiz.Scripts.Features.Gameplay;
using FlappyBoiz.Scripts.Features.Splash;
using FlappyBoiz.Scripts.Features.VFX;
using UnityEngine;
using VContainer;

namespace FlappyBoiz.Scripts.Features.Startup
{
    public class StartupSequenceController : ControllerBase
    {
        private readonly IScreenFaderService m_screenFader;

        public StartupSequenceController(
            IObjectResolver container,
            IScreenFaderService screenFader) : base(container)
        {
            m_screenFader = screenFader;
        }

        protected override async UniTask OnStartAsync(CancellationToken ct)
        {
            // Show Splash Screen
            Debug.Log("Startup sequence");

            var splash = await StartChildAsync<SplashSceneController>(ct);

            // Start Fader
            await m_screenFader.FadeIn(ct);

            await splash.StopAsync(ct);

            // Switch to gameplay scene
            var gameplay = StartChildAsync<GameplaySceneController>(ct);

            await m_screenFader.FadeOut(ct);

            Debug.Log("Gameplay Starting");
            await gameplay;
        }
    }
}