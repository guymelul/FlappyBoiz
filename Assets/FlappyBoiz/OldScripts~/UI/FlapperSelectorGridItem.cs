using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FlappyBoiz.Scripts.UI
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Button))]
    public class FlapperSelectorGridItem : MonoBehaviour
    {
        public event Action<int> OnFlapperSelected;

        public bool IsLocked { get; private set; }
        private int m_flapperId;
        private Image m_previewImage;
        private CanvasGroup m_lockOverlay;
        private CanvasGroup m_priceDetails;
        private CanvasGroup m_selectPanel;
        private CanvasGroup m_equippedPanel;
        private TMP_Text m_priceText;
        private Color m_spriteLockedColor = new(0.23f, 0.23f, 0.23f, 1);

        private void Awake()
        {
            m_previewImage = transform.GetChild(0).gameObject.GetComponent<Image>();
            m_lockOverlay = transform.GetChild(1).gameObject.GetComponent<CanvasGroup>();
            m_priceDetails = transform.GetChild(2).gameObject.GetComponent<CanvasGroup>();
            m_selectPanel = transform.GetChild(3).gameObject.GetComponent<CanvasGroup>();
            m_equippedPanel = transform.GetChild(4).gameObject.GetComponent<CanvasGroup>();

            m_priceText = m_priceDetails.transform.GetChild(1).GetComponent<TMP_Text>();

            m_priceText.text = "";

            m_lockOverlay.interactable = false;
            m_lockOverlay.blocksRaycasts = false;

            m_priceDetails.interactable = false;
            m_priceDetails.blocksRaycasts = false;

            m_selectPanel.interactable = false;
            m_selectPanel.blocksRaycasts = false;

            m_equippedPanel.interactable = false;
            m_equippedPanel.blocksRaycasts = false;

            GetComponent<Button>().onClick.AddListener(OnSelected);
        }

        public void SetFlapper(int index, Sprite previewSprite, int price)
        {
            m_flapperId = index;
            m_previewImage.sprite = previewSprite;
            m_priceText.text = price.ToString();
        }

        public void SetState(bool locked, bool equipped, float progress = 0)
        {
            IsLocked = locked;

            // Locked
            m_spriteLockedColor = Color.HSVToRGB(1, 0, Mathf.Clamp01(progress + 0.25f));
            m_lockOverlay.alpha = locked ? 1 : 0;
            m_priceDetails.alpha = locked ? 1 : 0;
            m_previewImage.color = locked ? m_spriteLockedColor : Color.white;

            // Unlocked
            m_selectPanel.alpha = !locked && !equipped ? 1 : 0;
            m_equippedPanel.alpha = !locked && equipped ? 1 : 0;
        }

        private void OnSelected()
        {
            OnFlapperSelected?.Invoke(m_flapperId);
        }
    }
}