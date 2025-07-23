using System.Collections;
using LitMotion;
using LitMotion.Animation;
using UnityEngine;

namespace FlappyBoiz.Scripts.Gameplay.UI.Screens
{
    [RequireComponent(typeof(CanvasGroup))]
    [RequireComponent(typeof(Canvas))]
    public class UIScreen : MonoBehaviour
    {
        private CanvasGroup m_canvasGroup;
        private Canvas m_canvas;
        private LitMotionAnimation m_screenAnimation;
        protected IEnumerator WaitForScreenAnimation { get; private set; }

        [SerializeField, Range(0.0f, 10.0f)] private float showDuration = 0.2f;

        [SerializeField, Range(0.0f, 10.0f)] private float showDelay = 0.0f;

        [SerializeField, Range(0.0f, 10.0f)] private float hideDuration = 0.1f;

        [SerializeField] private Ease ease = Ease.InCirc;

        private void Setup()
        {
            m_canvas = GetComponent<Canvas>();
            m_canvasGroup = GetComponent<CanvasGroup>();
            m_screenAnimation = GetComponent<LitMotionAnimation>();

            WaitForScreenAnimation = m_screenAnimation ? new WaitWhile(() => m_screenAnimation.IsPlaying) : null;
        }

        protected virtual void OnPreShown()
        {
        }

        protected virtual void OnPostShown()
        {
        }

        public void Show()
        {
            if (m_canvas == null)
            {
                Setup();
            }

            StartCoroutine(ShowCoroutine());
        }

        public void Hide()
        {
            bool animate = true;

            if (m_canvas == null)
            {
                Setup();
                animate = false;
            }

            StartCoroutine(HideCoroutine(animate));
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

        private IEnumerator ShowCoroutine()
        {
            DisableInteraction();

            OnPreShown();

            m_canvasGroup.alpha = 0;
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

        private IEnumerator HideCoroutine(bool animate)
        {
            DisableInteraction();

            if (animate && hideDuration > 0)
                yield return AnimateCanvasGroupAlpha(1f, 0f, hideDuration);

            m_canvas.enabled = false;
            m_canvasGroup.alpha = 0;
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