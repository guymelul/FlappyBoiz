using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.ResourceManagement.ResourceProviders;

namespace FlappyBoiz.Scripts.Core.Services
{
    public interface ISceneLoaderService
    {
        UniTask<SceneInstance> LoadSceneAsync(CancellationToken ct, string addressableName, bool additive = true);

        UniTask UnloadSceneAsync(CancellationToken ct, SceneInstance scene);
    }
}