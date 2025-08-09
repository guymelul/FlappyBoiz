using FlappyBoiz.Scripts.Core.Controllers;
using FlappyBoiz.Scripts.Core.UI;
using VContainer;
using VContainer.Unity;

namespace FlappyBoiz.Scripts.Features.Splash
{
    public class SplashLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterController<SplashController>();
            builder.RegisterComponentInHierarchy<SplashView>();
        }
    }
}