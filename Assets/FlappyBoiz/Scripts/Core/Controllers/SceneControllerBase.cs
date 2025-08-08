using System.Threading;
using Cysharp.Threading.Tasks;
using FlappyBoiz.Scripts.Core.Services;
using UnityEngine;
using UnityEngine.ResourceManagement.ResourceProviders;
using VContainer;
using VContainer.Unity;

namespace FlappyBoiz.Scripts.Core.Controllers
{
    public abstract class SceneControllerBase : ControllerBase
    {
        private readonly LifetimeScope m_scope;
        private readonly ISceneLoaderService m_sceneLoader;
        private readonly string m_sceneName;

        private LifetimeScope m_childScope;
        private SceneInstance m_sceneInstance;

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
            await m_sceneInstance.ActivateAsync().ToUniTask(cancellationToken: ct);

            // Create scope and set as child to this scope
            m_childScope = LifetimeScope.Create(Configure, $"{m_sceneName} LifetimeScope");
            m_childScope.parentReference.Object = m_scope;
        }

        protected abstract void Configure(IContainerBuilder builder);

        protected override async UniTask<T> StartChildAsync<T>(CancellationToken ct, IObjectResolver container = null)
        {
            return await base.StartChildAsync<T>(ct, m_childScope.Container);
        }

        protected override async UniTask OnStopAsync(CancellationToken ct)
        {
            Debug.Log("Unloading scene: " + m_sceneName);
            m_childScope?.Dispose();
            m_childScope = null;
            await m_sceneLoader.UnloadSceneAsync(ct, m_sceneInstance);
        }

        protected override void OnDispose()
        {
        }
    }
}