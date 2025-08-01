using UnityEngine;
using UnityEngine.AddressableAssets;

namespace FlappyBoiz.Scripts.Core.UI
{
    public class SplashView : MonoBehaviour
    {
        public void LoadMainScene()
        {
            Addressables.LoadSceneAsync("GameplayScene").WaitForCompletion();
        }
    }
}