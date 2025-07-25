using FlappyBoiz.Scripts.Menu.UI.Screens;
using VContainer;

namespace FlappyBoiz.Scripts.Menu.UI.Controllers
{
    public class LobbyMenuController
    {
        [Inject] public MainMenuController MainMenuController { get; set; }
        [Inject] public MenuLoadingScreen MenuLoadingScreen { get; set; }

        public void ShowLoading()
        {
            MainMenuController.Disable();
            MenuLoadingScreen.Show();
        }

        public void ShowMainMenu()
        {
            MenuLoadingScreen.Hide();
            MainMenuController.Show();
        }
    }
}