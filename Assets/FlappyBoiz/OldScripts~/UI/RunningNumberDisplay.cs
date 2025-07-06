using LitMotion;
using LitMotion.Extensions;
using TMPro;
using UnityEngine;

namespace FlappyBoiz.Scripts.UI
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(TMP_Text))]
    public class RunningNumberDisplay : BaseNumberDisplay
    {
        private int m_endValue;

        [SerializeField]
        private int startingValue;

        protected override void OnValueChanged(int newValue, int oldValue)
        {
            m_endValue = newValue;
        }

        public void Play(float duration = 0.01f)
        {
            Play(duration, startingValue, m_endValue);
        }

        public void Play(float duration, int start, int end)
        {
            LMotion.Create(start, end, duration)
                .WithEase(Ease.OutSine)
                .BindToText(m_numberText, "{0:D}");
        }
    }
}