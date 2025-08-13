using System.Threading;
using Cysharp.Threading.Tasks;
using FlappyBoiz.Scripts.Core.Services;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.ResourceManagement.ResourceProviders;
using VContainer;
using VContainer.Unity;

namespace FlappyBoiz.Scripts.Core.Controllers
{
    public abstract class SceneControllerBase<TLifetimeScope, TResult> : ControllerBase<TResult>
        where TLifetimeScope : LifetimeScope
    {
        private readonly LifetimeScope m_scope;
        private readonly ISceneLoaderService m_sceneLoader;

        private TLifetimeScope m_childScope;
        private SceneInstance? m_sceneInstance;
        protected abstract string SceneName { get; }

        protected SceneControllerBase(IObjectResolver container) : base(container)
        {
            m_sceneLoader = container.Resolve<ISceneLoaderService>();
            m_scope = container.Resolve<LifetimeScope>();
        }

        protected sealed override async UniTask OnInitAsync(CancellationToken ct) => await LoadScene(ct);

        protected override async UniTask<T> StartChildAsync<T>(CancellationToken ct, IObjectResolver container = null)
        {
            return await base.StartChildAsync<T>(ct, m_childScope.Container);
        }

        protected override async UniTask OnStopAsync(CancellationToken ct) => await UnloadScene();

        private async UniTask LoadScene(CancellationToken ct)
        {
            Debug.Log("Loading scene: " + SceneName);

            // Load scene and activate
            using (LifetimeScope.EnqueueParent(m_scope))
            {
                m_sceneInstance = await m_sceneLoader.LoadSceneAsync(ct, SceneName);
                await m_sceneInstance.Value.ActivateAsync().ToUniTask(cancellationToken: ct);
            }

            // Create scope and set as child to this scope
            m_childScope = Object.FindFirstObjectByType<TLifetimeScope>();

            Assert.IsNotNull(m_childScope, $"Scene [{SceneName}] does not container a LifetimeScope");
            Assert.IsTrue(m_childScope.Parent == m_scope, "Lifetime scope parent not set");
        }

        private async UniTask UnloadScene()
        {
            if (!m_sceneInstance.HasValue) return;

            Debug.Log("Unloading scene: " + SceneName);
            await m_sceneLoader.UnloadSceneAsync(CancellationToken.None, m_sceneInstance.Value);
            m_sceneInstance = null;
        }

        protected async UniTask ReloadScene(CancellationToken ct)
        {
            await UnloadScene();
            await LoadScene(ct);
        }

        protected override void OnDispose()
        {
            m_childScope?.Dispose();
            m_childScope = null;
        }
    }

    public abstract class SceneControllerBase<TLifetimeScope> :
        SceneControllerBase<TLifetimeScope, EmptyControllerResult>
        where TLifetimeScope : LifetimeScope
    {
        protected SceneControllerBase(IObjectResolver container) : base(container)
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