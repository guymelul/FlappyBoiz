using System.Collections;
using LitMotion;
using LitMotion.Animation;
using TMPro;
using UnityEngine;

namespace FlappyBoiz.Scripts.UI
{
    [DisallowMultipleComponent]
    public class PopupMessage : MonoBehaviour
    {
        public static PopupMessage Instance { get; private set; }

        private RectTransform m_rectTransform;
        private CanvasGroup m_popupPanel;
        private TMP_Text m_popupText;
        private LitMotionAnimation m_animation;
        private WaitWhile m_waitForAnimation;

        [Range(0.1f, 10f)]
        [SerializeField]
        private float displayDuration = 2f;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);

                return;
            }

            Instance = this;

            m_rectTransform = GetComponent<RectTransform>();
            m_animation = GetComponent<LitMotionAnimation>();
            m_popupPanel = GetComponent<CanvasGroup>();
            m_popupText = GetComponentInChildren<TMP_Text>();

            m_waitForAnimation = new WaitWhile(() => m_animation.IsPlaying);

            m_animation.Stop();
            m_popupPanel.interactable = false;
            m_popupPanel.blocksRaycasts = false;
        }

        public void Show(string message, float? duration = null)
        {
            if (string.IsNullOrEmpty(message)) return;

            m_animation.Restart();
            m_popupText.text = message;

            StopAllCoroutines();
            StartCoroutine(PopupCoroutine(duration ?? displayDuration));
        }

        private IEnumerator PopupCoroutine(float duration)
        {
            m_animation.Play();

            yield return m_waitForAnimation;

            yield return new WaitForSecondsRealtime(duration);

            var pivot = m_rectTransform.pivot;

            m_animation.Stop();
            m_rectTransform.pivot = pivot;
            float targetPivot = pivot.y + 3;
            m_popupPanel.alpha = 1f;

            yield return LMotion.Create(pivot.y, targetPivot, 1f)
                .WithScheduler(MotionScheduler.UpdateRealtime)
                .WithEase(Ease.InOutQuad)
                .Bind(y =>
                {
                    m_rectTransform.pivot = new Vector2(pivot.x, y);
                    m_popupPanel.alpha = Mathf.Clamp(1.3f - y / targetPivot, 0f, 1f);
                })
                .ToYieldInstruction();
        }
    }
}