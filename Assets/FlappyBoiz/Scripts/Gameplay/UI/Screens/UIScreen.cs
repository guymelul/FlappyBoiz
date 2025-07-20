using System.Collections;
using LitMotion;
using UnityEngine;

namespace FlappyBoiz.Scripts.Gameplay.UI.Screens
{
    [RequireComponent(typeof(CanvasGroup))]
    [RequireComponent(typeof(Canvas))]
    public class UIScreen : MonoBehaviour
    {
        private CanvasGroup m_canvasGroup;
        private Canvas m_canvas;

        [SerializeField, Range(0.0f, 10.0f)]
        private float showDuration = 0.2f;

        [SerializeField, Range(0.0f, 1.0f)]
        private float hideDuration = 0.1f;
        
        [SerializeField]
        private Ease ease = Ease.InCirc;
        
        public virtual void Show()
        {
            if (m_canvas == null)
            {
                m_canvas = GetComponent<Canvas>();
                m_canvasGroup = GetComponent<CanvasGroup>();
            }

            StartCoroutine(ShowCoroutine());
        }

        public virtual void Hide()
        {
            bool animate = true;

            if (m_canvas == null)
            {
                m_canvas = GetComponent<Canvas>();
                m_canvasGroup = GetComponent<CanvasGroup>();
                animate = false;
            }

            StartCoroutine(HideCoroutine(animate));
        }
        
        public void Enable()
        {
            m_canvasGroup.interactable = true;
            m_canvasGroup.blocksRaycasts = true;
        }
        
        public void Disable()
        {
            m_canvasGroup.interactable = false;
            m_canvasGroup.blocksRaycasts = false;
        }

        private IEnumerator ShowCoroutine()
        {
            Disable();
            m_canvasGroup.alpha = 0;
            m_canvas.enabled = true;

            if (showDuration > 0)
                // Animate
                yield return AnimateCanvasGroupAlpha(0f, 1f, showDuration);

            Enable();
            m_canvasGroup.alpha = 1;
        }

        private IEnumerator HideCoroutine(bool animate)
        {
            Disable();

            if (animate && hideDuration > 0)
                yield return AnimateCanvasGroupAlpha(1f, 0f, hideDuration);

            m_canvas.enabled = false;
            m_canvasGroup.alpha = 0;
        }

        private IEnumerator AnimateCanvasGroupAlpha(float from, float to, float duration)
        {
            yield return LMotion.Create(from, to, duration)
                .WithScheduler(MotionScheduler.UpdateRealtime)
                .WithEase(ease)
                .Bind(n => m_canvasGroup.alpha = n)
                .AddTo(gameObject)
                .ToYieldInstruction();
        }
    }
}