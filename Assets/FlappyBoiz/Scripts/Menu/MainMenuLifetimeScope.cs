using FlappyBoiz.Scripts.Menu.Managers;
using FlappyBoiz.Scripts.Menu.UI.Controllers;
using FlappyBoiz.Scripts.Menu.UI.Screens;
using VContainer;
using VContainer.Unity;

namespace FlappyBoiz.Scripts.Menu
{
    public class MainMenuLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<LobbyMenuController>(Lifetime.Scoped).AsSelf();
            builder.RegisterEntryPoint<MainMenuController>(Lifetime.Scoped).AsSelf();

            builder.RegisterComponentInHierarchy<MainMenuScreen>();
            builder.RegisterComponentInHierarchy<MenuLoadingScreen>();

            builder.RegisterEntryPoint<LobbyManager>(Lifetime.Scoped).AsSelf();
        }
    }
}