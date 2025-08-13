using System.Threading;
using Cysharp.Threading.Tasks;
using LitMotion;
using LitMotion.Animation;
using UnityEngine;
using UnityEngine.Assertions;

namespace FlappyBoiz.Scripts.Features.VFX
{
    public class ScreenFaderView : MonoBehaviour
    {
        [SerializeField]
        private LitMotionAnimation fadeInAnimation;

        [SerializeField]
        private LitMotionAnimation fadeOutAnimation;

        private void OnValidate()
        {
            Assert.IsNotNull(fadeInAnimation);
            Assert.AreNotEqual(fadeInAnimation.Components.Count, 0);

            Assert.IsNotNull(fadeOutAnimation);
            Assert.AreNotEqual(fadeOutAnimation.Components.Count, 0);
        }

        public async UniTask FadeIn(CancellationToken ct)
        {
            fadeOutAnimation.Stop();
            fadeInAnimation.Restart();
            await fadeInAnimation.Components[0].TrackedHandle.ToUniTask(cancellationToken: ct,
                cancelBehavior: CancelBehavior.Complete,
                cancelAwaitOnMotionCanceled: true);
        }

        public async UniTask FadeOut(CancellationToken ct)
        {
            fadeInAnimation.Stop();
            fadeOutAnimation.Restart();
            await fadeOutAnimation.Components[0].TrackedHandle
                .ToUniTask(cancellationToken: ct,
                    cancelBehavior: CancelBehavior.Complete, cancelAwaitOnMotionCanceled: true);
        }
    }
}