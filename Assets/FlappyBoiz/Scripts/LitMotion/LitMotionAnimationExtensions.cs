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
            var tasks = animation.Components.Select(c =>
                c.TrackedHandle.ToUniTask(cancellationToken: ct)
            );

            await UniTask.WhenAll(tasks);
        }
    }
}