using R3;
using UnityEngine;

namespace FlappyBoiz.Scripts.Features.Gameplay.VFX
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteScroller : MonoBehaviour
    {
        private static readonly int SpeedProperty = Shader.PropertyToID("_Speed");
        private SpriteRenderer m_spriteRenderer;

        [SerializeField]
        private SerializableReactiveProperty<float> scrollingSpeed;

        private void Awake()
        {
            m_spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            scrollingSpeed.Subscribe(ChangeScrollSpeed).AddTo(this);
        }

        private void ChangeScrollSpeed(float speed = 1.0f) => m_spriteRenderer.material.SetFloat(SpeedProperty, speed);
    }
}