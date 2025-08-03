using FlappyBoiz.Scripts.Core.UI;
using VContainer;
using VContainer.Unity;

namespace FlappyBoiz.Scripts.Gameplay.Scopes
{
    public class SplashLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponentInHierarchy<SplashView>();
        }
    }
}