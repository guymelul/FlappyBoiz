using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace FlappyBoiz.Scripts.Core.Services
{
    public class SceneLoaderService : ISceneLoaderService
    {
        public async UniTask<SceneInstance> LoadSceneAsync(CancellationToken ct,
            string addressableName, bool additive = false)
        {
            return await Addressables.LoadSceneAsync(addressableName,
                loadMode: additive ? LoadSceneMode.Additive : LoadSceneMode.Single,
                activateOnLoad: true).ToUniTask(cancellationToken: ct);
        }

        public async UniTask UnloadSceneAsync(CancellationToken ct, SceneInstance scene)
        {
            await Addressables.UnloadSceneAsync(scene).ToUniTask(cancellationToken: ct);
        }
    }
}