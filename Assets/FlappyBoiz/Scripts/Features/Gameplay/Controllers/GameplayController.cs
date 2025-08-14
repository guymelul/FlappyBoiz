using System.Threading;
using Cysharp.Threading.Tasks;
using FlappyBoiz.Scripts.Core.Controllers;
using FlappyBoiz.Scripts.Events;
using FlappyBoiz.Scripts.Features.Gameplay.Managers;
using MessagePipe;
using UnityEngine;
using VContainer;

namespace FlappyBoiz.Scripts.Features.Gameplay.Controllers
{
    public class GameplayController : ControllerBase<GameOverCommandResult>
    {
        [Inject] public IPublisher<GameStartedEvent> GameStartedEventPub { get; set; }
        [Inject] public IPublisher<GameEndedEvent> GameEndedEventPub { get; set; }

        [Inject] public GameSessionManager GameSession { get; set; }

        public GameplayController(IObjectResolver container) : base(container)
        {
        }

        protected override async UniTask<GameOverCommandResult> OnStartWithResultAsync(CancellationToken ct)
        {
            Debug.Log("Starting game...");

            var readyPhase = await StartChildAsync<ReadyPhaseController>(ct);

            Debug.Log("Ready phase done");

            if (readyPhase.Result == ReadyCommandResult.Quit) return GameOverCommandResult.MainMenu;

            GameStartedEventPub.Publish(GameStartedEvent.Empty);

            var gamePhase = await StartChildAsync<GamePhaseController>(ct);

            Debug.Log("Game phase done");

            if (gamePhase.Result == GameCommandResult.Quit) return GameOverCommandResult.MainMenu;
            
            Debug.Log("Game ending sequence");
            GameEndedEventPub.Publish(GameEndedEvent.Empty);
            
            // TODO: Start game ending sequence controller

            Debug.Log("Game over!");
            var gameOverPhase = await StartChildAsync<GameOverPhaseController>(ct);

            Debug.Log("Post Game over");

            return gameOverPhase.Result;
        }
    }
}