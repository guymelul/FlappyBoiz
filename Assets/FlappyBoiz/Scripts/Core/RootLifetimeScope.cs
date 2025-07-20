using MessagePipe;
using VContainer;
using VContainer.Unity;

namespace FlappyBoiz.Scripts.Scopes
{
    public class RootLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterMessagePipe();
        }
    }
}