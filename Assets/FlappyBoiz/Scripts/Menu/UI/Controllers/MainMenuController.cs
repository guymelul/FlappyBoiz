using FlappyBoiz.Scripts.Core.UI;
using FlappyBoiz.Scripts.Menu.UI.Screens;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using VContainer;

namespace FlappyBoiz.Scripts.Menu.UI.Controllers
{
    public class MainMenuController : BaseController<MainMenuScreen>
    {
        
        protected override void OnScreenShow()
        {
            Screen.StartButton.onClick.AddListener(OnStartGame);
        }
        
        protected override void OnScreenHide()
        {
            Screen.StartButton.onClick.RemoveAllListeners();
        }

        private void OnStartGame()
        {
            Addressables.LoadSceneAsync("GameplayScene").WaitForCompletion();
        }
    }
}