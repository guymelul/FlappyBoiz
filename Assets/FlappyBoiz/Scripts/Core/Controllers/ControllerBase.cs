using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using VContainer;

namespace FlappyBoiz.Scripts.Core.Controllers
{
    public abstract class ControllerBase : IController
    {
        private readonly IObjectResolver m_container;
        protected IObjectResolver Container => m_container;

        private readonly List<IController> m_children = new();
        IReadOnlyList<IController> IController.Children => m_children;

        protected ControllerBase(IObjectResolver container)
        {
            m_container = container;
        }

        /// <summary>
        /// Spawns, initializes, and starts a child controller.
        /// </summary>
        protected async UniTask<T> StartChildAsync<T>(CancellationToken ct) where T : IController
        {
            // Use VContainer to instantiate the child and inject its dependencies
            var child = Container.Resolve<T>();
            m_children.Add(child);

            await child.InitAsync(ct);
            await child.StartAsync(ct);

            return child;
        }

        // To be overridden by concrete controllers
        UniTask IController.InitAsync(CancellationToken ct) => UniTask.CompletedTask;

        async UniTask IController.StartAsync(CancellationToken ct)
        {
            await OnStartAsync(ct);
        }

        protected abstract UniTask OnStartAsync(CancellationToken ct);

        /// <summary>
        /// Stops all children in reverse order of creation before stopping itself.
        /// </summary>
        async UniTask IController.StopAsync(CancellationToken ct)
        {
            // Stop children in reverse order
            for (int i = m_children.Count - 1; i >= 0; i--)
            {
                await m_children[i].StopAsync(ct);
            }

            await OnStopAsync(ct);
        }

        protected abstract UniTask OnStopAsync(CancellationToken ct);

        public void Dispose()
        {
            foreach (var child in m_children)
                child.Dispose();

            m_children.Clear();

            OnDispose();
        }

        protected abstract void OnDispose();
    }
}