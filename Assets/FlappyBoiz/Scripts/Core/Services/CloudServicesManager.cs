using System.Threading;
using Cysharp.Threading.Tasks;
using FlappyBoiz.Scripts.Core.Data;
using FlappyBoiz.Scripts.Core.Services.Authentication;
using FlappyBoiz.Scripts.Core.Services.SaveManager;
using Unity.Services.Core;
using Unity.Services.LevelPlay;
using UnityEngine;

namespace FlappyBoiz.Scripts.Core.Services
{
    public class CloudServicesManager
    {
        private readonly AuthenticationStrategy _authenticationStrategy;
        private readonly CloudDataService _cloudSaveService;

        public CloudServicesManager()
        {
            _authenticationStrategy = new AuthenticationStrategy();
            _cloudSaveService = new CloudDataService();
        }

        public async UniTask Init(CancellationToken ct = default)
        {
            if (UnityServices.State == ServicesInitializationState.Initialized)
                return;

#if UNITY_EDITOR
            LevelPlay.SetMetaData("is_test_suite", "enable");
            LevelPlay.LaunchTestSuite();
#endif

            await UnityServices.InitializeAsync();
            Debug.Log("Unity Services Initialized");
        }

        public UniTask<bool> Authenticate(CancellationToken ct = default)
        {
            var authenticationService = _authenticationStrategy.GetAuthenticationService();

            return authenticationService.AuthenticateAsync(ct);
        }

        public UniTask<GameSavedData> LoadSavedGame(CancellationToken ct = default)
        {
            return _cloudSaveService.Load(ct);
        }
    }
}