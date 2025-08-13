using System.Collections;
using System.Threading;
using Cysharp.Threading.Tasks;
using LitMotion;
using LitMotion.Animation;
using UnityEngine;

namespace FlappyBoiz.Scripts.Core.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    [RequireComponent(typeof(Canvas))]
    public class UIScreen : MonoBehaviour
    {
        private CanvasGroup m_canvasGroup;
        private Canvas m_canvas;
        private LitMotionAnimation m_screenAnimation;
        private IEnumerator WaitForScreenAnimation { get; set; }

        [SerializeField, Range(0.0f, 10.0f)]
        private float showDuration = 0.2f;

        [SerializeField, Range(0.0f, 10.0f)]
        private float showDelay;

        [SerializeField, Range(0.0f, 10.0f)]
        private float hideDuration = 0.1f;

        [SerializeField]
        private Ease ease = Ease.InCirc;

        private void Awake()
        {
            m_canvas = GetComponent<Canvas>();
            m_canvasGroup = GetComponent<CanvasGroup>();
            m_screenAnimation = GetComponent<LitMotionAnimation>();
            WaitForScreenAnimation = m_screenAnimation ? new WaitWhile(() => m_screenAnimation.IsPlaying) : null;

            DisableInteraction();
        }

        protected virtual void OnPreShown()
        {
        }

        protected virtual void OnPostShown()
        {
        }

        public async UniTask Show(CancellationToken ct = default)
        {
            StopAllCoroutines();

            await ShowCoroutine().ToUniTask(cancellationToken: ct);
        }

        public async UniTask Hide(CancellationToken ct = default)
        {
            StopAllCoroutines();

            await HideCoroutine().ToUniTask(cancellationToken: ct);
        }

        public void EnableInteraction()
        {
            m_canvasGroup.interactable = true;
            m_canvasGroup.blocksRaycasts = true;
        }

        public void DisableInteraction()
        {
            m_canvasGroup.interactable = false;
            m_canvasGroup.blocksRaycasts = false;
        }

        private void MakeInvisible()
        {
            m_canvas.enabled = false;
            m_canvasGroup.alpha = 0;
        }

        private IEnumerator ShowCoroutine()
        {
            // Used to hide the screen

            OnPreShown();

            m_canvas.enabled = true;

            // Trigger immediate bindings
            if (m_screenAnimation)
            {
                m_screenAnimation.Play();
                m_screenAnimation.Pause();
            }

            EnableInteraction();

            if (showDuration > 0)
                // Animate
                yield return AnimateCanvasGroupAlpha(0f, 1f, showDuration, showDelay);

            m_canvasGroup.alpha = 1;

            OnPostShown();

            if (!m_screenAnimation) yield break;

            m_screenAnimation.Restart();

            yield return WaitForScreenAnimation;
        }

        private IEnumerator HideCoroutine()
        {
            DisableInteraction();

            if (hideDuration > 0)
                yield return AnimateCanvasGroupAlpha(1f, 0f, hideDuration);

            MakeInvisible();
        }

        private IEnumerator AnimateCanvasGroupAlpha(float from, float to, float duration, float delay = 0)
        {
            yield return LMotion.Create(from, to, duration)
                .WithScheduler(MotionScheduler.UpdateRealtime)
                .WithEase(ease)
                .WithDelay(delay, skipValuesDuringDelay: false)
                .Bind(n => m_canvasGroup.alpha = n)
                .AddTo(gameObject)
                .ToYieldInstruction();
        }
    }
}