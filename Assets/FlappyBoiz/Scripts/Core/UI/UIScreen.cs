using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using FlappyBoiz.Scripts.LitMotion;
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

            DisableInteraction();
        }

        private void OnDestroy()
        {
            m_canvasGroup = null;
        }

        protected virtual void OnPreShown()
        {
        }

        protected virtual void OnPostShown()
        {
        }

        public void EnableInteraction()
        {
            m_canvasGroup.interactable = true;
            m_canvasGroup.blocksRaycasts = true;
        }

        public void DisableInteraction()
        {
            if (!m_canvasGroup) return;
            
            m_canvasGroup.interactable = false;
            m_canvasGroup.blocksRaycasts = false;
        }

        private void MakeInvisible()
        {
            m_canvas.enabled = false;
            m_canvasGroup.alpha = 0;
        }

        public async UniTask Show(CancellationToken ct = default)
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
                await AnimateCanvasGroupAlpha(ct, 0f, 1f, showDuration, showDelay);

            m_canvasGroup.alpha = 1;

            OnPostShown();

            if (!m_screenAnimation) return;

            m_screenAnimation.Restart();
            await m_screenAnimation.WaitCompletedLoop(ct);
        }

        public async UniTask Hide(CancellationToken ct = default)
        {
            if (!m_canvasGroup) return;
            
            DisableInteraction();

            if (hideDuration > 0 && !ct.IsCancellationRequested)
                await AnimateCanvasGroupAlpha(ct, 1f, 0f, hideDuration);

            MakeInvisible();
        }

        private UniTask AnimateCanvasGroupAlpha(
            CancellationToken ct,
            float from, float to, float duration, float delay = 0)
        {
            return LMotion.Create(from, to, duration)
                .WithScheduler(MotionScheduler.UpdateRealtime)
                .WithCancelOnError()
                .WithEase(ease)
                .WithDelay(delay, skipValuesDuringDelay: false)
                .Bind(n => m_canvasGroup.alpha = n)
                .AddTo(gameObject)
                .ToUniTask(ct);
        }
    }
}