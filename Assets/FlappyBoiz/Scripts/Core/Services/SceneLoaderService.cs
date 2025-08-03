using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace FlappyBoiz.Scripts.Core.Services
{
    public class SceneLoaderService : ISceneLoaderService
    {
        public async UniTask<SceneInstance> LoadSceneAsync(string addressableName, bool additive = false)
        {
            // TODO: unload all other scenes and leave on the Boot one?

            return await Addressables.LoadSceneAsync(addressableName,
                loadMode: additive ? LoadSceneMode.Additive : LoadSceneMode.Single,
                activateOnLoad: true).ToUniTask();
        }

        public async UniTask UnloadSceneAsync(SceneInstance scene)
        {
            await Addressables.UnloadSceneAsync(scene).ToUniTask();
        }
    }
}