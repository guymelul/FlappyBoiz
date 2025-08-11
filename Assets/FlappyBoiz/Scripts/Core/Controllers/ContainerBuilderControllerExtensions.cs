using VContainer;

namespace FlappyBoiz.Scripts.Core.Controllers
{
    public static class ContainerBuilderControllerExtensions
    {
        public static void RegisterController<T>(this IContainerBuilder builder) where T : IController
        {
            builder.Register<T>(Lifetime.Transient);
        }
    }
}