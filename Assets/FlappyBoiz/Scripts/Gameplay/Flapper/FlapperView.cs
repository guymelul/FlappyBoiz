using System.Collections;
using UnityEngine;

namespace FlappyBoiz.Scripts.Gameplay.Flapper
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Animator))]
    public class FlapperView : MonoBehaviour
    {
        private static readonly int Reset = Animator.StringToHash("Reset");
        private static readonly int AnimKeyFlapWings = Animator.StringToHash("FlapWings");

        private Rigidbody2D m_rigidbody;
        private Animator m_animator;
        private SpriteRenderer m_flapperSprite;
        private SpriteRenderer m_cosmeticSprite;

        private Vector2 Velocity => m_rigidbody.linearVelocity;

        private float Rotation
        {
            get => transform.localEulerAngles.z;
            set => transform.localEulerAngles = new Vector3(0, 0, value);
        }

        [Range(0, 90)] [SerializeField] private int jumpRotation = 35;

        [Range(-90, 0)] [SerializeField] private int fallMaxRotation = -35;

        [Range(1, 360)] [SerializeField] private int fallRotationSpeed = 75;

        [Range(0.5f, 3.0f)] [SerializeField] private float freeFallDuration = 2.0f;

        [Range(0.1f, 5.0f)] [SerializeField] private float hitStopDuration = 0.5f;

        // private SoundPackData Sfx => gameSettings.SoundPack;

        private void Awake()
        {
            m_flapperSprite = GetComponent<SpriteRenderer>();
            m_animator = GetComponent<Animator>();
            m_rigidbody = GetComponent<Rigidbody2D>();
            m_cosmeticSprite = transform.GetChild(0).GetComponent<SpriteRenderer>();

            m_rigidbody.linearVelocity = Vector2.zero;
            m_rigidbody.angularVelocity = 0;
        }

        private void OnEnable()
        {
            //onGameStateChanged.Register(OnGameStateChange);
            // gameSettings.SFXVolume.Changed.Register(OnSfxVolumeChanged);
            // gameSettings.EquippedFlapper.Changed.Register(OnFlapperChanged);
            // gameSettings.EquippedCosmetic.Changed.Register(OnCosmeticChanged);
        }

        private void OnDisable()
        {
            //onGameStateChanged.Unregister(OnGameStateChange);
            // gameSettings.SFXVolume.Changed.Unregister(OnSfxVolumeChanged);
            // gameSettings.EquippedFlapper.Changed.Unregister(OnFlapperChanged);
            // gameSettings.EquippedCosmetic.Changed.Unregister(OnCosmeticChanged);
        }
        
        
        private void FixedUpdate()
        {
            // TODO: ApplyRotation();
        }

        private void ApplyRotation()
        {
            Rotation = Mathf.MoveTowards(
                Rotation,
                Velocity.y < 0 ? fallMaxRotation : 0,
                fallRotationSpeed * Time.deltaTime
            );
        }

        public void Flap(float power)
        {
            m_rigidbody.linearVelocityY = power;
            // OnJump?.Invoke();
        }

        public void Move(float delta, float maxFallSpeed)
        {
            float nextFallSpeed = m_rigidbody.linearVelocityY + delta;
            m_rigidbody.linearVelocityY = Mathf.Clamp(nextFallSpeed, -maxFallSpeed, maxFallSpeed);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            // TODO: handle case where its already dead

            if (!(other.CompareTag("Ground") || other.CompareTag("Obstacle"))) return;

            // OnHit?.Invoke();
        }

        private void OnFlapperChanged(int index)
        {
            // m_animator.runtimeAnimatorController = gameSettings.Flapper.AnimatorOverride
            //     ? gameSettings.Flapper.AnimatorOverride
            //     : m_defaultAnimatorController;
            //
            // m_flapperSprite.sprite = gameSettings.Flapper.Preview;
            m_animator.SetTrigger(Reset);
        }

        private void OnCosmeticChanged(int index)
        {
            //m_cosmeticSprite.sprite = gameSettings.FlapperCosmetic ? gameSettings.FlapperCosmetic.Sprite : null;
        }

        private void OnFlapperJump()
        {
            Rotation = jumpRotation;

            m_animator.SetTrigger(AnimKeyFlapWings);

            // if (Sfx.WingFlap)
            // {
            //     m_audio.PlayOneShot(Sfx.WingFlap);
            // }
        }

        private void OnFlapperHit() => StartCoroutine(FlapperDeathCoroutine());

        private IEnumerator FlapperDeathCoroutine()
        {
            // Inactivate but let it fall
            // TODO: disable collider

            // Hit stop
            // if (Sfx.Hit)
            // {
            //     m_audio.PlayOneShot(Sfx.Hit);
            // }

            yield return FlapperDeathSequence();
        }

        // TODO: export to scriptable object
        private IEnumerator FlapperDeathSequence()
        {
            Time.timeScale = 0;

            yield return new WaitForSecondsRealtime(hitStopDuration);

            Time.timeScale = 1;

            // TODO: flapperMotion.CanMove = true;

            // if (Sfx.Die)
            // {
            //     m_audio.PlayOneShot(Sfx.Die);
            // }

            yield return new WaitForSecondsRealtime(freeFallDuration);
        }
    }
}