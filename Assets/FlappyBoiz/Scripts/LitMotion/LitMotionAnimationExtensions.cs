using System.Threading;
using Cysharp.Threading.Tasks;
using LitMotion;
using LitMotion.Animation;

namespace FlappyBoiz.Scripts.LitMotion
{
    public static class LitMotionAnimationExtensions
    {
        public static async UniTask WaitCompletedLoop(this LitMotionAnimation animation, CancellationToken ct)
        {
            if (animation.Components.Count <= 0) return;

            var tasks = animation.Components.Select(c =>
                c.TrackedHandle.ToUniTask(
                    cancellationToken: ct,
                    cancelBehavior: CancelBehavior.Complete,
                    cancelAwaitOnMotionCanceled: true
                )
            );

            await UniTask.WhenAll(tasks);
        }
    }
}