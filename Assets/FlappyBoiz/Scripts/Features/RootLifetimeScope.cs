using FlappyBoiz.Scripts.Core.Cloud;
using FlappyBoiz.Scripts.Core.Services;
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

            // Entry point
            builder.RegisterEntryPoint<GameRootController>();

            // Controllers
            builder.Register<StartupSequenceController>(Lifetime.Transient);
            builder.Register<SplashSceneController>(Lifetime.Transient);
            builder.Register<ScreenFaderController>(Lifetime.Transient);

            // Views
            // builder.RegisterComponentOnNewGameObject<ScreenFaderView>(Lifetime.Singleton,
            //     newGameObjectName: "ScreenFader");


            builder.RegisterMessagePipe();
#if UNITY_EDITOR
            builder.RegisterBuildCallback(c => GlobalMessagePipe.SetProvider(c.AsServiceProvider()));
#endif
        }
    }
}