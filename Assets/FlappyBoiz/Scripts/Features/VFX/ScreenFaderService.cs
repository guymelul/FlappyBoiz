using System.Threading;
using Cysharp.Threading.Tasks;

namespace FlappyBoiz.Scripts.Features.VFX
{
    public class ScreenFaderService : IScreenFaderService
    {
        private readonly ScreenFaderView m_screenFaderView;

        public ScreenFaderService(ScreenFaderView screenFaderView)
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