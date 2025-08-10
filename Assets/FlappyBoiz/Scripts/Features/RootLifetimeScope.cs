using FlappyBoiz.Scripts.Core.Cloud;
using FlappyBoiz.Scripts.Core.Controllers;
using FlappyBoiz.Scripts.Core.Services;
using FlappyBoiz.Scripts.Features.Gameplay;
using FlappyBoiz.Scripts.Features.Splash;
using FlappyBoiz.Scripts.Features.Startup;
using FlappyBoiz.Scripts.Features.VFX;
using MessagePipe;
using VContainer;
using VContainer.Unity;

namespace FlappyBoiz.Scripts.Features
{
    public class RootLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            // Register All Services 
            builder.Register<UnityServicesManager>(Lifetime.Singleton);
            builder.Register<ISceneLoaderService, SceneLoaderService>(Lifetime.Singleton);
            builder.Register<IScreenFaderService, ScreenFaderService>(Lifetime.Singleton);

            // Entry point
            builder.RegisterEntryPoint<GameRootController>();

            // Global Controllers
            builder.RegisterController<StartupSequenceController>();
            builder.RegisterController<SplashSceneController>();
            builder.RegisterController<GameplaySceneController>();

            // Global Views
            builder.RegisterComponentInHierarchy<ScreenFaderView>();

            builder.RegisterMessagePipe();
#if UNITY_EDITOR
            builder.RegisterBuildCallback(c => GlobalMessagePipe.SetProvider(c.AsServiceProvider()));
#endif
        }
    }
}