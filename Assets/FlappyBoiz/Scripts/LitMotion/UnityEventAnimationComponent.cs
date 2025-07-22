using System;
using LitMotion;
using LitMotion.Animation;
using UnityEngine;
using UnityEngine.Events;

namespace FlappyBoiz.Scripts.LitMotion
{
    [Serializable]
    [LitMotionAnimationComponentMenu("Events/Unity Event")]
    public class UnityEventAnimationComponent : LitMotionAnimationComponent
    {
        [SerializeField]
        private int delay;

        [SerializeField]
        private UnityEvent action;

        private readonly IntegerOptions m_activationOptions = new()
        {
            RoundingMode = RoundingMode.ToZero
        };

        public override MotionHandle Play()
        {
            return LMotion.Create(0, 1, 0.001f)
                .WithOptions(m_activationOptions)
                .WithDelay(delay)
                .WithOnComplete(() => action?.Invoke())
                .RunWithoutBinding();
        }
    }
}