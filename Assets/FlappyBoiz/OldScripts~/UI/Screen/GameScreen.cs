using System.Collections;
using System.Linq;
using LitMotion.Animation;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace FlappyBoiz.Scripts.UI.Screen
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Canvas))]
    [RequireComponent(typeof(CanvasGroup))]
    [RequireComponent(typeof(GraphicRaycaster))]
    public class GameScreen : MonoBehaviour
    {
        private Canvas m_canvas;
        private CanvasGroup m_canvasGroup;

        public UnityEvent onGameScreenShown;

        public UnityEvent onGameScreenHidden;

        [SerializeField]
        private GameObject firstFocus;

        [SerializeField]
        private LitMotionAnimation showAnimation;

        [SerializeField]
        private LitMotionAnimation hideAnimation;

        private void Awake()
        {
            m_canvas = GetComponent<Canvas>();
            m_canvasGroup = GetComponent<CanvasGroup>();

            Hide(animate: false);

            if (firstFocus) return;

            var elementToFocus = m_canvas
                .GetComponentsInChildren<Selectable>()
                .FirstOrDefault(btn => btn.CompareTag("FocusFirst"));

            firstFocus = elementToFocus ? elementToFocus.gameObject : null;
        }

        public void Show()
        {
            StopAllCoroutines();
            StartCoroutine(ShowSequence());
        }

        private IEnumerator ShowSequence(bool animate = true)
        {
            onGameScreenShown?.Invoke();

            if (EventSystem.current && firstFocus)
            {
                EventSystem.current.SetSelectedGameObject(firstFocus);
            }

            if (animate)
            {
                if (hideAnimation && hideAnimation.IsPlaying)
                {
                    hideAnimation.Stop();
                    yield return null;
                }

                if (showAnimation)
                {
                    Debug.Log("Show anim start");
                    
                    showAnimation.Restart();

                    yield return null;

                    m_canvas.enabled = true;
                    ChangeInteractable(false);

                    yield return new WaitWhile(() => showAnimation.IsPlaying);
                    
                    showAnimation.Stop();
                    Debug.Log("Show anim done");
                }
            }

            m_canvas.enabled = true;
            ChangeInteractable(true);
        }

        public void ChangeInteractable(bool interactable)
        {
            m_canvasGroup.interactable = interactable;
            m_canvasGroup.blocksRaycasts = interactable;
        }

        public void Hide(bool animate = true)
        {
            StopAllCoroutines();
            StartCoroutine(HideSequence(animate));
        }

        private IEnumerator HideSequence(bool animate = true)
        {
            m_canvasGroup.interactable = false;

            if (animate)
            {
                if (showAnimation && hideAnimation.IsPlaying)
                {
                    showAnimation.Stop();
                    yield return null;
                }

                if (hideAnimation)
                {
                    Debug.Log("Hide anim start");
                    
                    hideAnimation.Restart();

                    yield return new WaitWhile(() => hideAnimation.IsPlaying);

                    hideAnimation.Stop();
                    Debug.Log("Hide anim done");
                }
            }

            m_canvas.enabled = false;
            m_canvasGroup.enabled = false;

            onGameScreenHidden?.Invoke();
        }
    }
}