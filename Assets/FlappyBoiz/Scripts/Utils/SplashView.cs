using UnityEngine;
using UnityEngine.AddressableAssets;

namespace FlappyBoiz.Scripts.Utils
{
    public class SplashView : MonoBehaviour
    {
        public void LoadMainScene()
        {
            Addressables.LoadSceneAsync("MainMenuScene");
        }
    }
}