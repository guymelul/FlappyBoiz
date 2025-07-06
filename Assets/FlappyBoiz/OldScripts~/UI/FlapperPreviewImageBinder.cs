using FlappyBoiz.Scripts.Data;
using UnityEngine;
using UnityEngine.UI;

namespace FlappyBoiz.Scripts.UI
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Image))]
    public class FlapperPreviewImageBinder : MonoBehaviour
    {
        [SerializeField]
        private GameSettings gameSettings;

        private Image m_image;

        private void Awake()
        {
            m_image = GetComponent<Image>();
        }

        private void OnEnable()
        {
            gameSettings.EquippedFlapper.Changed.Register(SetFlapperPreview);
        }

        private void OnDisable()
        {
            gameSettings.EquippedFlapper.Changed.Unregister(SetFlapperPreview);
        }

        private void SetFlapperPreview(int index = -1)
        {
            m_image.sprite = gameSettings.Flapper.Preview;
        }
    }
}