using System.Threading;
using Cysharp.Threading.Tasks;
using FlappyBoiz.Scripts.Core.Services;
using UnityEngine;
using UnityEngine.ResourceManagement.ResourceProviders;
using VContainer;
using VContainer.Unity;

namespace FlappyBoiz.Scripts.Core.Controllers
{
    public abstract class SceneControllerBase<TLifetimeScope> : ControllerBase where TLifetimeScope : LifetimeScope
    {
        private readonly LifetimeScope m_scope;
        private readonly ISceneLoaderService m_sceneLoader;
        private readonly string m_sceneName;

        public bool IsSceneLoaded { get; private set; } = false;

        private TLifetimeScope m_childScope;
        private SceneInstance? m_sceneInstance;

        protected SceneControllerBase(IObjectResolver container, string sceneName) : base(container)
        {
            m_sceneLoader = container.Resolve<ISceneLoaderService>();
            m_scope = container.Resolve<LifetimeScope>();
            m_sceneName = sceneName;
        }

        protected override async UniTask OnInitAsync(CancellationToken ct)
        {
            Debug.Log("Loading scene: " + m_sceneName);

            // Load scene and activate
            m_sceneInstance = await m_sceneLoader.LoadSceneAsync(ct, m_sceneName);
            await m_sceneInstance.Value.ActivateAsync().ToUniTask(cancellationToken: ct);

            // Create scope and set as child to this scope
            m_childScope = Object.FindFirstObjectByType<TLifetimeScope>();
            m_childScope.parentReference.Object = m_scope;

            IsSceneLoaded = true;
        }

        protected override async UniTask<T> StartChildAsync<T>(CancellationToken ct, IObjectResolver container = null)
        {
            return await base.StartChildAsync<T>(ct, m_childScope.Container);
        }

        protected override async UniTask OnStopAsync(CancellationToken ct)
        {
            if (!m_sceneInstance.HasValue) return;

            Debug.Log("Unloading scene: " + m_sceneName);
            await m_sceneLoader.UnloadSceneAsync(ct, m_sceneInstance.Value);
            m_sceneInstance = null;

            IsSceneLoaded = false;
        }

        protected override void OnDispose()
        {
            m_childScope?.Dispose();
            m_childScope = null;
        }
    }
}