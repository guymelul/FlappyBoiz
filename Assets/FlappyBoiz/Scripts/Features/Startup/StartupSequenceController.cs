using System.Threading;
using Cysharp.Threading.Tasks;
using FlappyBoiz.Scripts.Core.Controllers;
using FlappyBoiz.Scripts.Features.Gameplay;
using FlappyBoiz.Scripts.Features.Lobby;
using FlappyBoiz.Scripts.Features.Menu;
using FlappyBoiz.Scripts.Features.Splash;
using FlappyBoiz.Scripts.Features.VFX;
using VContainer;

namespace FlappyBoiz.Scripts.Features.Startup
{
    public class StartupSequenceController : ControllerBase
    {
        private readonly IScreenTransitionService m_screenTransition;

        public StartupSequenceController(
            IObjectResolver container,
            IScreenTransitionService screenTransition) : base(container)
        {
            m_screenTransition = screenTransition;
        }

        protected override async UniTask OnStartAsync(CancellationToken ct)
        {
            // Show Splash Screen
            var splash = await StartChildAsync<SplashSceneController>(ct);

            // Start Fader
            await m_screenTransition.FadeIn(ct);

            // Unload Splash
            await splash.StopAsync(ct);

            // Start loading lobby scene
            var lobbyTask = StartChildAsync<LobbySceneController>(ct);
            await m_screenTransition.FadeOut(ct);

            // Wait for lobby to finish
            var lobby = await lobbyTask;

            // Unload lobby
            await m_screenTransition.FadeIn(ct);
            await lobby.StopAsync(ct);

            while (!ct.IsCancellationRequested)
            {
                // Load menu scene
                var menuSceneTask = StartChildAsync<MenuSceneController>(ct);
                await m_screenTransition.FadeOut(ct);

                // Listen
                var menuScene = await menuSceneTask;

                await m_screenTransition.FadeIn(ct);
                await menuScene.StopAsync(ct);

                if (menuScene.Result != MainMenuChoiceResult.Play) continue;

                var gameSceneTask = StartChildAsync<GameplaySceneController>(ct);
                await m_screenTransition.FadeOut(ct);

                var gameScene = await gameSceneTask;

                await gameScene.StopAsync(ct);
            }
        }
    }
}