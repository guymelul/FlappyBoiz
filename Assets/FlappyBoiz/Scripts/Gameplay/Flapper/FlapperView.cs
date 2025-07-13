using System.Collections;
using UnityEngine;

namespace FlappyBoiz.Scripts.Gameplay.Flapper
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(FlapperMotion))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(AudioSource))]
    public class FlapperView : MonoBehaviour
    {
        private static readonly int Reset = Animator.StringToHash("Reset");
        private static readonly int AnimKeyFlapWings = Animator.StringToHash("FlapWings");

        private Animator m_animator;
        private RuntimeAnimatorController m_defaultAnimatorController;

        private AudioSource m_audio;
        private FlapperMotion m_flapperMotion;
        private SpriteRenderer m_flapperSprite;
        private SpriteRenderer m_cosmeticSprite;

        //[Header("Event")]
        // [SerializeField]
        // private StringEvent onGameStateChanged;
        //
        // [SerializeField]
        // private StringEvent gameEvent;

        // [Header("Settings")]
        // [SerializeField]
        // private GameSettings gameSettings;

        [Header("Rotation")]
        [Range(0, 90)]
        [SerializeField]
        private int jumpRotation = 35;

        [Range(-90, 0)]
        [SerializeField]
        private int fallMaxRotation = -35;

        [Range(1, 360)]
        [SerializeField]
        private int fallRotationSpeed = 75;

        [Header("Hit")]
        [Range(0.5f, 3.0f)]
        [SerializeField]
        private float freeFallDuration;

        [Range(0.1f, 5.0f)]
        [SerializeField]
        private float hitStopDuration;

        // private SoundPackData Sfx => gameSettings.SoundPack;

        private void Awake()
        {
            m_flapperSprite = GetComponent<SpriteRenderer>();
            m_cosmeticSprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
            m_animator = GetComponent<Animator>();
            m_audio = GetComponent<AudioSource>();
            m_flapperMotion = GetComponent<FlapperMotion>();

            m_defaultAnimatorController = m_animator.runtimeAnimatorController;
        }

        private void FixedUpdate()
        {
            ApplyRotation();
        }

        private float Rotation
        {
            get => transform.localEulerAngles.z;
            set => transform.localEulerAngles = new Vector3(0, 0, value);
        }

        private void ApplyRotation()
        {
            Rotation = Mathf.MoveTowards(
                Rotation,
                m_flapperMotion.Velocity.y < 0 ? fallMaxRotation : 0,
                fallRotationSpeed * Time.deltaTime
            );
        }

        private void OnEnable()
        {
            //onGameStateChanged.Register(OnGameStateChange);
            m_flapperMotion.OnJump += OnFlapperJump;
            m_flapperMotion.OnHit += OnFlapperHit;
            // gameSettings.SFXVolume.Changed.Register(OnSfxVolumeChanged);
            // gameSettings.EquippedFlapper.Changed.Register(OnFlapperChanged);
            // gameSettings.EquippedCosmetic.Changed.Register(OnCosmeticChanged);
        }

        private void OnDisable()
        {
            //onGameStateChanged.Unregister(OnGameStateChange);
            m_flapperMotion.OnJump -= OnFlapperJump;
            m_flapperMotion.OnHit -= OnFlapperHit;
            // gameSettings.SFXVolume.Changed.Unregister(OnSfxVolumeChanged);
            // gameSettings.EquippedFlapper.Changed.Unregister(OnFlapperChanged);
            // gameSettings.EquippedCosmetic.Changed.Unregister(OnCosmeticChanged);
        }

        // private void OnGameStateChange(string state)
        // {
        //     switch (state)
        //     {
        //         case GameStates.Ready:
        //             m_flapperSprite.enabled = true;
        //             m_cosmeticSprite.enabled = true;
        //             m_flapperMotion.ToggleBirdSimulated(false);
        //             m_flapperMotion.ResetFlapper();
        //             m_flapperMotion.JumpsBuffered = 1;
        //
        //             break;
        //         case GameStates.Playing:
        //             m_flapperMotion.ToggleBirdSimulated(true);
        //
        //             break;
        //         case GameStates.Paused:
        //             m_flapperMotion.ToggleBirdSimulated(false);
        //
        //             break;
        //         default:
        //             m_flapperSprite.enabled = false;
        //             m_cosmeticSprite.enabled = false;
        //             m_flapperMotion.ToggleBirdSimulated(false);
        //             m_flapperMotion.ResetFlapper();
        //
        //             break;
        //     }
        // }

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

        private void OnSfxVolumeChanged(float value) => m_audio.volume = value;

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
            m_flapperMotion.ToggleBirdSimulated(active: false);

            // Hit stop
            // if (Sfx.Hit)
            // {
            //     m_audio.PlayOneShot(Sfx.Hit);
            // }

            yield return FlapperDeathSequence(m_flapperMotion);

            //gameEvent.Raise(GameEvents.GameOver);
        }

        // TODO: export to scriptable object
        private IEnumerator FlapperDeathSequence(FlapperMotion flapperMotion)
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