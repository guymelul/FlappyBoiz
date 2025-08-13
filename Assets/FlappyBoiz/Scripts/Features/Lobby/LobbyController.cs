using System.Threading;
using Cysharp.Threading.Tasks;
using FlappyBoiz.Scripts.Core.Controllers;
using FlappyBoiz.Scripts.Core.Services;
using UnityEngine;
using VContainer;

namespace FlappyBoiz.Scripts.Features.Lobby
{
    public class LobbyController : ControllerBase
    {
        private readonly ICloudServicesManager m_cloudServicesManager;
        private readonly LobbyLoadingScreen m_lobbyLoadingScreen;

        // ICloudServicesManager cloudServicesManager
        public LobbyController(IObjectResolver container,
            ICloudServicesManager cloudServicesManager,
            LobbyLoadingScreen lobbyLoadingScreen) : base(container)
        {
            m_cloudServicesManager = cloudServicesManager;
            m_lobbyLoadingScreen = lobbyLoadingScreen;
        }

        protected override async UniTask OnStartAsync(CancellationToken ct)
        {
            await m_lobbyLoadingScreen.Show(ct);

            Debug.Log("Cloud Services Init");
            await m_cloudServicesManager.Init(ct);

            Debug.Log("Authenticating");
            bool isAuthenticated = await m_cloudServicesManager.Authenticate(ct);

            // TODO: If auth is success
            //  Load player data

            await m_lobbyLoadingScreen.Hide(ct);

            await UniTask.Delay(1000, cancellationToken: ct);
        }
    }
}