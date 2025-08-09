using System.Threading;
using Cysharp.Threading.Tasks;

namespace FlappyBoiz.Scripts.Features.VFX
{
    public interface IScreenFaderService
    {
        /// <summary>
        /// Fades the entire screen to solid color
        /// </summary>
        UniTask FadeIn(CancellationToken ct = default);

        /// <summary>
        /// Fades the screen back to transparent.
        /// </summary>
        UniTask FadeOut(CancellationToken ct = default);
    }
}