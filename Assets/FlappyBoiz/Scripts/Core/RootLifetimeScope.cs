using FlappyBoiz.Scripts.Cloud;
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

            builder.RegisterEntryPoint<UnityServicesManager>().AsSelf();

#if UNITY_EDITOR
            builder.RegisterBuildCallback(c => GlobalMessagePipe.SetProvider(c.AsServiceProvider()));

            LevelPlay.SetMetaData("is_test_suite", "enable");
            LevelPlay.LaunchTestSuite();
#endif
        }
    }
}