using LitMotion.Animation;
using UnityAtoms.BaseAtoms;
using UnityEngine;

namespace FlappyBoiz.Scripts.UI
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(LitMotionAnimation))]
    public class HighScoreBeatenAnim : MonoBehaviour
    {
        private LitMotionAnimation m_animation;

        [SerializeField]
        protected IntVariable scoreVariable;

        [SerializeField]
        protected IntVariable highScoreVariable;

        private void Awake()
        {
            m_animation = GetComponent<LitMotionAnimation>();
        }

        private void OnEnable()
        {
            scoreVariable.Changed.Register(OnValueChanged);
        }

        private void OnDisable()
        {
            scoreVariable.Changed.Unregister(OnValueChanged);
        }

        private void OnValueChanged(int newValue)
        {
            if (newValue <= highScoreVariable.Value || highScoreVariable.Value == 0) return;

            m_animation.Restart();
        }
    }
}