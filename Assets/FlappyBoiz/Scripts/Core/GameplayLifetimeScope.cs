using FlappyBoiz.Scripts.Managers;
using VContainer;
using VContainer.Unity;

namespace FlappyBoiz.Scripts.Core
{
    public class GameplayLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<GameplayManager>(Lifetime.Scoped).AsSelf();
        }
    }
}