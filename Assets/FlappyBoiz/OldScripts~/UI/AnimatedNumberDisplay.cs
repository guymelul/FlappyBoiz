using LitMotion.Animation;
using TMPro;
using UnityEngine;

namespace FlappyBoiz.Scripts.UI
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(TMP_Text))]
    public class AnimatedNumberDisplay : BaseNumberDisplay
    {
        private LitMotionAnimation m_animation;

        protected override void Awake()
        {
            base.Awake();
            m_animation = GetComponent<LitMotionAnimation>();
        }

        protected override void OnValueChanged(int newValue, int oldValue)
        {
            m_numberText.text = newValue.ToString();

            if (!m_animation) return;

            m_animation.Restart();
        }
    }
}