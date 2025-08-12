using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

namespace FlappyBoiz.Scripts.Core.Controllers
{
    public abstract class ControllerBase<TResult> : IControllerWithResult<TResult>
    {
        public TResult Result { get; protected set; }

        private IObjectResolver Container { get; }

        private readonly List<IController> m_children = new();
        IReadOnlyList<IController> IController.Children => m_children;

        protected ControllerBase(IObjectResolver container)
        {
            Container = container;
        }

        /// <summary>
        /// Spawns, initializes, and starts a child controller.
        /// </summary>
        protected virtual async UniTask<T> StartChildAsync<T>(CancellationToken ct, IObjectResolver container = null)
            where T : IController
        {
            // Use VContainer to instantiate the child and inject its dependencies
            var factory = container ?? Container;
            var child = factory.Resolve<T>();
            m_children.Add(child);

            try
            {
                await child.InitAsync(ct);
                await child.StartAsync(ct);
            }
            catch
            {
                // If something goes wrong, stop the child immediately
                Debug.LogError("Stopping controller due to exception");
                m_children.Remove(child);
                await child.StopAsync(ct);

                throw;
            }

            return child;
        }

        // To be overridden by concrete controllers
        async UniTask IController.InitAsync(CancellationToken ct) => await OnInitAsync(ct);

        protected virtual UniTask OnInitAsync(CancellationToken ct) => UniTask.CompletedTask;

        async UniTask IController.StartAsync(CancellationToken ct)
        {
            Result = await OnStartWithResultAsync(ct);
        }

        /// <summary>
        /// Stops all children in reverse order of creation before stopping itself.
        /// </summary>
        public async UniTask StopAsync(CancellationToken ct)
        {
            // Stop children in reverse order
            for (int i = m_children.Count - 1; i >= 0; i--)
            {
                await m_children[i].StopAsync(ct);
            }

            await OnStopAsync(ct);
            Dispose();
        }

        protected abstract UniTask<TResult> OnStartWithResultAsync(CancellationToken ct);

        protected virtual UniTask OnStopAsync(CancellationToken ct) => UniTask.CompletedTask;

        public void Dispose()
        {
            foreach (var child in m_children)
                child.Dispose();

            m_children.Clear();

            OnDispose();
        }

        protected virtual void OnDispose()
        {
        }
    }

    public abstract class ControllerBase : ControllerBase<EmptyControllerResult>
    {
        protected ControllerBase(IObjectResolver container) : base(container)
        {
        }

        protected abstract UniTask OnStartAsync(CancellationToken ct);

        protected override async UniTask<EmptyControllerResult> OnStartWithResultAsync(CancellationToken ct)
        {
            await OnStartAsync(ct);
            return new EmptyControllerResult();
        }
    }
}