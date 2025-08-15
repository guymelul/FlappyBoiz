using System.Threading;
using Cysharp.Threading.Tasks;
using FlappyBoiz.Scripts.Core.Controllers;
using FlappyBoiz.Scripts.Features.Gameplay.Controllers;
using UnityEngine;
using VContainer;

namespace FlappyBoiz.Scripts.Features.Gameplay
{
    public sealed class GameplaySceneController : SceneControllerBase<GameplayLifetimeScope>
    {
        protected override string SceneName => "GameplayScene";

        public GameplaySceneController(IObjectResolver container) : base(container)
        {
        }

        protected override async UniTask OnStartAsync(CancellationToken ct)
        {
            do
            {
                var gameplay = await StartChildAsync<GameplayController>(ct);

                await gameplay.StopAsync(ct);

                if (gameplay.Result == GameOverCommandResult.MainMenu) break;

                Debug.Log("Restarting game");
                await ReloadScene(ct);
            } while (true);

            Debug.Log("Quiting game");
        }
    }
}