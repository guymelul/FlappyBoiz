using Cysharp.Threading.Tasks;
using UnityEngine.ResourceManagement.ResourceProviders;

namespace FlappyBoiz.Scripts.Core.Services
{
    public interface ISceneLoaderService
    {
        UniTask<SceneInstance> LoadSceneAsync(string addressableName, bool additive = false);

        UniTask UnloadSceneAsync(SceneInstance scene);
    }
}