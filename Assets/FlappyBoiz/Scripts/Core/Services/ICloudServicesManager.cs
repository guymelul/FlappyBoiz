using System.Threading;
using Cysharp.Threading.Tasks;
using FlappyBoiz.Scripts.Features.Gameplay.Data;

namespace FlappyBoiz.Scripts.Core.Services
{
    public interface ICloudServicesManager
    {
        UniTask Init(CancellationToken ct = default);

        UniTask<bool> Authenticate(CancellationToken ct = default);

        UniTask<GameSavedData> LoadSavedGame(CancellationToken ct = default);
    }
}