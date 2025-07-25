using FlappyBoiz.Scripts.Core.Cloud;
using MessagePipe;
using Unity.Services.LevelPlay;
using VContainer;
using VContainer.Unity;

namespace FlappyBoiz.Scripts.Core
{
    public class RootLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterMessagePipe();

            builder.Register<UnityServicesManager>(Lifetime.Singleton);

#if UNITY_EDITOR
            builder.RegisterBuildCallback(c => GlobalMessagePipe.SetProvider(c.AsServiceProvider()));

            LevelPlay.SetMetaData("is_test_suite", "enable");
            LevelPlay.LaunchTestSuite();
#endif
        }
    }
}