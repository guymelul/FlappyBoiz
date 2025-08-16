using System;
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

        private CancellationTokenSource m_cancellationTokenSource;
        protected CancellationToken CancellationToken => m_cancellationTokenSource.Token;

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
            catch (Exception e)
            {
                if (e is OperationCanceledException)
                {
                    Debug.Log("Stopping controller due to cancellation");
                }
                else
                {
                    // If something goes wrong, stop the child immediately
                    Debug.LogError($"Stopping controller due to exception [{e.GetType().Name}]");
                }

                m_children.Remove(child);
                await child.StopAsync(ct);

                throw;
            }

            return child;
        }

        // To be overridden by concrete controllers
        async UniTask IController.InitAsync(CancellationToken ct)
        {
            m_cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(ct);
            await OnInitAsync(CancellationToken);
        }

        protected virtual UniTask OnInitAsync(CancellationToken ct) => UniTask.CompletedTask;

        async UniTask IController.StartAsync(CancellationToken ct)
        {
            Result = await OnStartWithResultAsync(CancellationToken);
        }

        /// <summary>
        /// Stops all children in reverse order of creation before stopping itself.
        /// </summary>
        public async UniTask StopAsync(CancellationToken ct)
        {
            if (m_cancellationTokenSource == null || m_cancellationTokenSource.IsCancellationRequested)
            {
                return;
            }

            // Immediately signal cancellation to all background tasks managed by this controller.
            m_cancellationTokenSource.Cancel();

            try
            {
                // Stop children in reverse order. This operation as a whole is cancellable by the external `ct` token.
                for (int i = m_children.Count - 1; i >= 0; i--)
                {
                    await m_children[i].StopAsync(ct);
                }
            }
            finally
            {
                // This controller's own cleanup is guaranteed to run and is not cancellable.
                await OnStopAsync(CancellationToken.None);
                Dispose();
            }
        }

        protected abstract UniTask<TResult> OnStartWithResultAsync(CancellationToken ct);

        protected virtual UniTask OnStopAsync(CancellationToken ct) => UniTask.CompletedTask;

        public void Dispose()
        {
            foreach (var child in m_children)
                child.Dispose();

            m_children.Clear();

            m_cancellationTokenSource?.Dispose();
            m_cancellationTokenSource = null;

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