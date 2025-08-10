using System.Threading;
using Cysharp.Threading.Tasks;

namespace FlappyBoiz.Scripts.Core.Services
{
    public interface ICloudServicesManager
    {
        UniTask Init(CancellationToken cancellation = new());

        UniTask<bool> Authenticate(CancellationToken cancellation = new());
    }
}