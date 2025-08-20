using System.Threading;
using Cysharp.Threading.Tasks;

namespace FlappyBoiz.Scripts.Core.Services.Authentication
{
    public interface IAuthenticationService
    {
        UniTask<bool> AuthenticateAsync(CancellationToken ct = default);
    }
}
