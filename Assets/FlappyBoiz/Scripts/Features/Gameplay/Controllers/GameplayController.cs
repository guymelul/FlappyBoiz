using System.Threading;
using Cysharp.Threading.Tasks;
using FlappyBoiz.Scripts.Core.Controllers;
using FlappyBoiz.Scripts.Events;
using FlappyBoiz.Scripts.Features.Gameplay.Managers;
using MessagePipe;
using VContainer;

namespace FlappyBoiz.Scripts.Features.Gameplay.Controllers
{
    public class GameplayController : ControllerBase<GameOverCommandResult>
    {
        [Inject] private IPublisher<GameStartedEvent> GameStartedEventPub { get; set; }
        [Inject] private IPublisher<GameEndedEvent> GameEndedEventPub { get; set; }

        [Inject] private GameSessionManager GameSession { get; set; }

        public GameplayController(IObjectResolver container) : base(container)
        {
        }

        protected override async UniTask<GameOverCommandResult> OnStartWithResultAsync(CancellationToken ct)
        {
            var readyPhase = await StartChildAsync<ReadyPhaseController>(ct);

            if (readyPhase.Result == ReadyCommandResult.Quit) return GameOverCommandResult.MainMenu;

            var gamePhase = await StartChildAsync<GamePhaseController>(ct);

            if (gamePhase.Result == GameCommandResult.Quit) return GameOverCommandResult.MainMenu;

            GameEndedEventPub.Publish(GameEndedEvent.Empty);

            var gameOverPhase = await StartChildAsync<GameOverPhaseController>(ct);

            return gameOverPhase.Result;
        }
    }
}