using System.Threading;
using Cysharp.Threading.Tasks;
using FlappyBoiz.Scripts.Core.Controllers;
using VContainer;

namespace FlappyBoiz.Scripts.Features.VFX
{
    public class ScreenFaderController : ControllerBase
    {
        private readonly ScreenFaderView m_screenFaderView;

        public ScreenFaderController(IObjectResolver container, ScreenFaderView screenFaderView) : base(container)
        {
            m_screenFaderView = screenFaderView;
        }

        protected override async UniTask OnStartAsync(CancellationToken ct)
        {
            await UniTask.CompletedTask;
        }

        protected override void OnDispose()
        {
            
        }
    }
}