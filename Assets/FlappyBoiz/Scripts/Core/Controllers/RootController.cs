using System.Threading;
using Cysharp.Threading.Tasks;
using VContainer;
using VContainer.Unity;

namespace FlappyBoiz.Scripts.Core.Controllers
{
    public abstract class RootController : ControllerBase, IAsyncStartable
    {
        private readonly CancellationTokenSource m_shutdownTokenSource;

        protected RootController(IObjectResolver container) : base(container)
        {
            m_shutdownTokenSource = new CancellationTokenSource();
            m_shutdownTokenSource.Token.Register(OnTreeStopped);
        }

        private async UniTask Entrypoint(CancellationToken ct)
        {
            ct.Register(OnTreeStopped);

            var treeCt = CancellationTokenSource.CreateLinkedTokenSource(ct, m_shutdownTokenSource.Token).Token;

            await ((IController)this).InitAsync(treeCt);
            ct.ThrowIfCancellationRequested();
            await ((IController)this).StartAsync(treeCt);

            if (ct.IsCancellationRequested)
                return;

            await ((IController)this).StopAsync(treeCt);
        }

        public void Shutdown()
        {
            m_shutdownTokenSource.Cancel();
        }

        private void OnTreeStopped()
        {
            ((IController)this).StopAsync(CancellationToken.None).Forget();
        }

        public async UniTask StartAsync(CancellationToken cancellation = new CancellationToken())
        {
            await Entrypoint(cancellation);
        }
    }
}