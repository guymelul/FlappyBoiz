using FlappyBoiz.Scripts.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace FlappyBoiz.Scripts.UI
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(RectTransform))]
    public class UISoundEffect : MonoBehaviour
    {
        private void Start()
        {
            var button = GetComponent<Button>();

            if (button != null)
            {
                button.onClick.AddListener(OnSelect);
            }
        }

        private static void OnSelect()
        {
            AudioManager.Instance.UISelect();
        }
    }
}