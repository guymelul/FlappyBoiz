using FlappyBoiz.Scripts.Gameplay.UI.Screens;
using VContainer;

namespace FlappyBoiz.Scripts.Gameplay.Presenters
{
    public class LobbyMenuController
    {
        [Inject] public MainMenuPresenter MainMenuPresenter { get; set; }
        [Inject] public MenuLoadingScreen MenuLoadingScreen { get; set; }

        public void ShowLoading()
        {
            MainMenuPresenter.Disable();
            MenuLoadingScreen.Show();
        }

        public void ShowMainMenu()
        {
            MenuLoadingScreen.Hide();
            MainMenuPresenter.Show();
        }
    }
}