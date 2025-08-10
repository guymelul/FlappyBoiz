using FlappyBoiz.Scripts.Core.Controllers;
using FlappyBoiz.Scripts.Core.Services;
using FlappyBoiz.Scripts.Features.Gameplay;
using FlappyBoiz.Scripts.Features.Lobby;
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
            // Entry point
            builder.RegisterEntryPoint<GameRootController>();
            
            // Register All Services 
            builder.Register<ICloudServicesManager, CloudServicesManager>(Lifetime.Singleton);
            builder.Register<ISceneLoaderService, SceneLoaderService>(Lifetime.Singleton);
            builder.Register<IScreenTransitionService, ScreenTransitionService>(Lifetime.Singleton);

            // Global Controllers
            builder.RegisterController<StartupSequenceController>();

            // Scene Controllers
            builder.RegisterController<SplashSceneController>();
            builder.RegisterController<GameplaySceneController>();
            builder.RegisterController<LobbySceneController>();

            // Global Views
            builder.RegisterComponentInHierarchy<ScreenFaderView>();

            builder.RegisterMessagePipe();
#if UNITY_EDITOR
            builder.RegisterBuildCallback(c => GlobalMessagePipe.SetProvider(c.AsServiceProvider()));
#endif
        }
    }
}