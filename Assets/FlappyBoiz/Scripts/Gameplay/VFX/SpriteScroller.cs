using UnityEngine;

namespace FlappyBoiz.Scripts.Gameplay.VFX
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteScroller : MonoBehaviour
    {
        private static readonly int SpeedProperty = Shader.PropertyToID("_Speed");
        private SpriteRenderer m_spriteRenderer;

        [SerializeField, Range(0.0f, 0.25f)] private float baseSpeed = 0.12f;

        private void Awake()
        {
            m_spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            ChangeScrollSpeed(0.5f);
        }

        private void ChangeScrollSpeed(float multiplier = 1.0f)
        {
            m_spriteRenderer.material.SetFloat(SpeedProperty, baseSpeed * multiplier);
        }
    }
}