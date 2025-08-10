using System.Threading;
using Cysharp.Threading.Tasks;
using FlappyBoiz.Scripts.Core.Controllers;
using FlappyBoiz.Scripts.Features.Gameplay;
using FlappyBoiz.Scripts.Features.VFX;
using UnityEngine;
using VContainer;

namespace FlappyBoiz.Scripts.Features.Startup
{
    public class GameRootController : RootController
    {
        private readonly IScreenTransitionService m_screenTransition;

        public GameRootController(
            IObjectResolver container,
            IScreenTransitionService screenTransition) : base(container)
        {
            m_screenTransition = screenTransition;
        }

        protected override async UniTask OnStartAsync(CancellationToken ct)
        {
            Debug.Log("Starting game");

            await StartChildAsync<StartupSequenceController>(ct);

            Debug.Log("Quiting game");

            Application.Quit();
        }

        protected override UniTask OnStopAsync(CancellationToken ct)
        {
            return UniTask.CompletedTask;
        }

        protected override void OnDispose()
        {
        }
    }
}