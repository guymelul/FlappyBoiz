using System;
using LitMotion;
using LitMotion.Animation;
using UnityEngine;
using UnityEngine.Events;

namespace FlappyBoiz.Scripts.LitMotion
{
    public abstract class ActionLitMotionAnimationComponent : LitMotionAnimationComponent
    {
        [SerializeField]
        private float delay;

        private readonly IntegerOptions m_activationOptions = new()
        {
            RoundingMode = RoundingMode.ToZero
        };

        public override MotionHandle Play()
        {
            return LMotion.Create(0, 1, 0.001f)
                .WithOptions(m_activationOptions)
                .WithDelay(delay)
                .WithOnComplete(Invoke)
                .RunWithoutBinding();
        }

        protected abstract void Invoke();
    }

    [Serializable]
    [LitMotionAnimationComponentMenu("Events/Unity Event")]
    public class UnityEventAnimationComponent : ActionLitMotionAnimationComponent
    {
        [SerializeField]
        private UnityEvent action;

        protected override void Invoke()
        {
            action?.Invoke();
        }
    }

    [Serializable]
    [LitMotionAnimationComponentMenu("Particle System/Particle System - Play")]
    public class ParticleSystemAnimationComponent : ActionLitMotionAnimationComponent
    {
        [SerializeField]
        private Transform transform;

        [SerializeField]
        private ParticleSystem particleSystem;

        protected override void Invoke()
        {
            if (transform)
                particleSystem.transform.position = transform.position;

            if (!particleSystem)
            {
                Debug.LogError("Particle system is null in lit motion animation component");

                return;
            }

            particleSystem.Play();
        }
    }

    [Serializable]
    [LitMotionAnimationComponentMenu("Particle System/Particle System - Stop")]
    public class ParticleSystemStopperAnimationComponent : ActionLitMotionAnimationComponent
    {
        [SerializeField]
        private ParticleSystem particleSystem;

        protected override void Invoke()
        {
            if (!particleSystem)
            {
                Debug.LogError("Particle system is null in lit motion animation component");

                return;
            }

            particleSystem.Stop();
        }
    }
}