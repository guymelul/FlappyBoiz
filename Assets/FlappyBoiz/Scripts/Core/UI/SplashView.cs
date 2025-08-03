using System;
using LitMotion.Animation;
using R3;
using UnityEngine;

namespace FlappyBoiz.Scripts.Core.UI
{
    public class SplashView : MonoBehaviour
    {
        private readonly Subject<Unit> m_onCompleteAnimationSubject = new();
        public Observable<Unit> OnSplashAnimationEnded;

        private LitMotionAnimation m_animation;
        
        private void Awake()
        {
            m_animation = GetComponent<LitMotionAnimation>();
        }

        public void StartSplashAnimation()
        {
            
        }
        
        public void SplashCompleted()
        {
            m_onCompleteAnimationSubject.OnNext(Unit.Default);
        }

        private void OnDestroy()
        {
            m_onCompleteAnimationSubject.Dispose();
        }
    }
}