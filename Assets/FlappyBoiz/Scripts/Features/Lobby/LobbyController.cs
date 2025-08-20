using System.Threading;
using Cysharp.Threading.Tasks;
using FlappyBoiz.Scripts.Core.Controllers;
using FlappyBoiz.Scripts.Core.Services;
using FlappyBoiz.Scripts.Core.Services.SaveManager;
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
        [Inject] public CloudServicesManager CloudServicesManager { get; set; }
        [Inject] public SaveManager SaveManager { get; set; }
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

                try
                {
                    await SaveManager.LoadData(ct);
                }
                catch
                {
                    return LobbyStateResult.SaveLoadingError;
                }

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