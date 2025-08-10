using System.Threading;
using Cysharp.Threading.Tasks;
using FlappyBoiz.Scripts.LitMotion;
using LitMotion.Animation;
using UnityEngine;

namespace FlappyBoiz.Scripts.Features.Splash
{
    public class SplashView : MonoBehaviour
    {
        private LitMotionAnimation m_animation;

        private void Awake()
        {
            m_animation = GetComponent<LitMotionAnimation>();
        }

        public async UniTask StartSplashAnimation(CancellationToken ct)
        {
            m_animation.Restart();
            await m_animation.WaitCompletedLoop(ct);
        }
    }
}