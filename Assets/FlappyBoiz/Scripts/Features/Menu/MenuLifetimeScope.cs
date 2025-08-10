using FlappyBoiz.Scripts.Core.Controllers;
using FlappyBoiz.Scripts.Features.Lobby;
using VContainer;
using VContainer.Unity;

namespace FlappyBoiz.Scripts.Features.Menu
{
    public class MenuLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterController<LobbyController>();
            builder.RegisterComponentInHierarchy<LobbyLoadingScreen>();
        }
    }
}