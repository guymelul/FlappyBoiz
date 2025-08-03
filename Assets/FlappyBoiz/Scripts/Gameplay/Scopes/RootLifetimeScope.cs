using FlappyBoiz.Scripts.Core.Cloud;
using FlappyBoiz.Scripts.Core.Services;
using FlappyBoiz.Scripts.Gameplay.Controllers;
using MessagePipe;
using VContainer;
using VContainer.Unity;

namespace FlappyBoiz.Scripts.Gameplay.Scopes
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
            builder.Register<StartupSequenceController>(Lifetime.Transient);

            builder.RegisterMessagePipe();
#if UNITY_EDITOR
            builder.RegisterBuildCallback(c => GlobalMessagePipe.SetProvider(c.AsServiceProvider()));
#endif
        }
    }
}