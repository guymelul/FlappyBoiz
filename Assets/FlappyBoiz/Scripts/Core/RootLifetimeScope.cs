using MessagePipe;
using VContainer;
using VContainer.Unity;

namespace FlappyBoiz.Scripts.Core
{
    public class RootLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterMessagePipe();
        }
    }
}