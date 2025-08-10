using System.Threading;
using Cysharp.Threading.Tasks;
using FlappyBoiz.Scripts.Core.Cloud;
using FlappyBoiz.Scripts.Core.Controllers;
using UnityEngine;
using VContainer;

namespace FlappyBoiz.Scripts.Features.Lobby
{
    public class LobbyController : ControllerBase
    {
        private readonly UnityServicesManager m_unityServicesManager;
        private readonly LobbyView m_lobbyView;

        public LobbyController(IObjectResolver container,
            UnityServicesManager unityServicesManager,
            LobbyView lobbyView) : base(container)
        {
            m_unityServicesManager = unityServicesManager;
            m_lobbyView = lobbyView;
        }

        protected override async UniTask OnStartAsync(CancellationToken ct)
        {
            Debug.Log("Loading Unity Services");
            //LobbyView.ShowLoading();

            await UniTask.Delay(200, cancellationToken: ct);

            await UnityServicesManager.Init(ct);
            await UnityServicesManager.Authenticate(ct);

            Debug.Log("Loaded Unity Services");
            await UniTask.Delay(1000, cancellationToken: ct);
        }
    }
}