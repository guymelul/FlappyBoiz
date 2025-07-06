using UnityEngine;

namespace FlappyBoiz.Scripts.Gameplay.VFX
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(SpriteRenderer))]
    public class BackgroundScroller : MonoBehaviour
    {
        private static readonly int SpeedProperty = Shader.PropertyToID("_Speed");
        private SpriteRenderer m_spriteRenderer;
        
        // TODO: fix
        // [SerializeField]
        // private StringEvent gameChangedEvent;
        
        [SerializeField]
        [Range(0.0f, 0.25f)]
        private float baseSpeed = 0.12f;
        
        [SerializeField]
        [Range(0.0f, 1.0f)]
        private float menuScale = 0.5f;
        
        [SerializeField]
        [Range(0.0f, 1.0f)]
        private float gameOverScale = 0.25f;
        
        [SerializeField]
        [Range(0.0f, 1.0f)]
        private float playingScale = 1.0f;

        private void Awake()
        {
            m_spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            ChangeScrollSpeed(0.5f);
        }

        private void OnEnable()
        {
            // gameChangedEvent.Register(OnGameStateChanged);
        }
        
        private void OnDisable()
        {
            // gameChangedEvent.Unregister(OnGameStateChanged);
        }

        private void ChangeScrollSpeed(float multiplier = 1.0f)
        {
            m_spriteRenderer.material.SetFloat(SpeedProperty, baseSpeed * multiplier);
        }

        // private void OnGameStateChanged(string state)
        // {
        //     switch (state)
        //     {
        //         case GameStates.MainMenu:
        //             ChangeScrollSpeed(menuScale);
        //             break;
        //         case GameStates.Ready:
        //         case GameStates.Playing:
        //             ChangeScrollSpeed(playingScale);
        //             break;
        //         case GameStates.Paused:
        //             ChangeScrollSpeed(0);
        //             break;
        //         case GameStates.GameOver:
        //             ChangeScrollSpeed(gameOverScale);
        //             break;
        //     }
        // }
    }
}