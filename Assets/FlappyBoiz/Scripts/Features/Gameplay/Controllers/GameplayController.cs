using System.Threading;
using Cysharp.Threading.Tasks;
using FlappyBoiz.Scripts.Core.Controllers;
using UnityEngine;
using VContainer;

namespace FlappyBoiz.Scripts.Features.Gameplay.Controllers
{
    public class GameplayController : ControllerBase<GameOverCommandResult>
    {
        public GameplayController(IObjectResolver container) : base(container)
        {
        }

        protected override async UniTask<GameOverCommandResult> OnStartWithResultAsync(CancellationToken ct)
        {
            Debug.Log("Starting game...");

            var readyPhase = await StartChildAsync<ReadyPhaseController>(ct);

            Debug.Log("Ready phase done");

            if (readyPhase.Result == ReadyCommandResult.Quit) return GameOverCommandResult.MainMenu;

            var gamePhase = await StartChildAsync<GamePhaseController>(ct);

            Debug.Log("Game phase done");

            if (gamePhase.Result == GameCommandResult.Quit) return GameOverCommandResult.MainMenu;

            Debug.Log("Game over!");
            
            var gameOverPhase = await StartChildAsync<GameOverPhaseController>(ct);

            Debug.Log("Post Game over");

            return gameOverPhase.Result;
        }
    }
}