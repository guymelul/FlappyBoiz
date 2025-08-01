using System.Threading;
using Cysharp.Threading.Tasks;
using FlappyBoiz.Scripts.Core.Cloud;
using FlappyBoiz.Scripts.Menu.UI.Controllers;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace FlappyBoiz.Scripts.Menu.Managers
{
    public class LobbyManager : IAsyncStartable
    {
        [Inject] public LobbyMenuController LobbyMenuController { get; set; }
        [Inject] public UnityServicesManager UnityServicesManager { get; set; }

        public async UniTask StartAsync(CancellationToken cancellation = new())
        {
            Debug.Log("Loading Unity Services");
            LobbyMenuController.ShowLoading();
            
            await UniTask.Delay(200, cancellationToken: cancellation);
            
            await UnityServicesManager.Init(cancellation);
            await UnityServicesManager.Authenticate(cancellation);

            Debug.Log("Loaded Unity Services");
            await UniTask.Delay(1000, cancellationToken: cancellation);
            LobbyMenuController.ShowMainMenu();
        }
    }
}