using UnityEngine;
using UnityEngine.AddressableAssets;

namespace FlappyBoiz.Scripts.Utils
{
    public class SplashView : MonoBehaviour
    {
        [SerializeField]
        private AssetReference mainScene;
        
        public void LoadMainScene()
        {
            Addressables.LoadSceneAsync(mainScene.RuntimeKey).WaitForCompletion();
        }
    }
}