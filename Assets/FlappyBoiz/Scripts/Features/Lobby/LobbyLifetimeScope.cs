using FlappyBoiz.Scripts.Core.Controllers;
using VContainer;
using VContainer.Unity;

namespace FlappyBoiz.Scripts.Features.Lobby
{
    public class LobbyLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterController<LobbyController>();
            builder.RegisterComponentInHierarchy<LobbyView>();
        }
    }
}
