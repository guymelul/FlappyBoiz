using System.Threading;
using Cysharp.Threading.Tasks;
using FlappyBoiz.Scripts.Core.Controllers;
using FlappyBoiz.Scripts.Core.Services;
using VContainer;

namespace FlappyBoiz.Scripts.Features.Lobby
{
    public enum LobbyStateResult
    {
        Success,
        AuthenticationError,
        SaveLoadingError
    }

    public class LobbyController : ControllerBase<LobbyStateResult>
    {
        [Inject] public ICloudServicesManager CloudServicesManager { get; set; }
        [Inject] public LobbyView LobbyView { get; set; }

        public LobbyController(IObjectResolver container) : base(container)
        {
        }

        protected override async UniTask<LobbyStateResult> OnStartWithResultAsync(CancellationToken ct)
        {
            try
            {
                await LobbyView.Show(ct);

                await CloudServicesManager.Init(ct);

                if (!await CloudServicesManager.Authenticate(ct))
                {
                    return LobbyStateResult.AuthenticationError;
                }

                //  Load player data

                return LobbyStateResult.Success;
            }
            finally
            {
                await LobbyView.Hide(ct);
                await UniTask.Delay(1000, cancellationToken: ct);
            }
        }
    }
}