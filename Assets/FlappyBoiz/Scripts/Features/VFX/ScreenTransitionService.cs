using System.Threading;
using Cysharp.Threading.Tasks;

namespace FlappyBoiz.Scripts.Features.VFX
{
    public class ScreenTransitionService : IScreenTransitionService
    {
        private readonly ScreenFaderView m_screenFaderView;

        public ScreenTransitionService(ScreenFaderView screenFaderView)
        {
            m_screenFaderView = screenFaderView;
        }

        public async UniTask FadeIn(CancellationToken ct = default)
        {
            await m_screenFaderView.FadeIn(ct);
        }

        public async UniTask FadeOut(CancellationToken ct = default)
        {
            await m_screenFaderView.FadeOut(ct);
        }
    }
}