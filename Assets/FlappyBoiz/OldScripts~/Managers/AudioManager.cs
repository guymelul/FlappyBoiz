using FlappyBoiz.Scripts.Data;
using UnityEngine;
using UnityEngine.Assertions;

namespace FlappyBoiz.Scripts.Managers
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(AudioSource))]
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }

        private AudioSource m_audioBGM;
        private AudioSource m_audioSFX;

        [SerializeField]
        private GameSettings gameSettings;

        private SoundPackData Sfx => gameSettings.SoundPack;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Debug.LogWarning("Multiple audio managers detected!");
                Destroy(gameObject);

                return;
            }

            Instance = this;

            var audioSources = GetComponents<AudioSource>();

            Assert.AreEqual(audioSources.Length, 2, "Audio manager should have 2 Audio Sources");

            m_audioBGM = audioSources[0];
            m_audioBGM.loop = true;
            m_audioBGM.playOnAwake = false;
            m_audioBGM.clip = null;

            m_audioSFX = audioSources[1];
            m_audioSFX.loop = false;
            m_audioSFX.playOnAwake = false;
            m_audioSFX.clip = null;
        }

        private void OnEnable()
        {
            gameSettings.SFXVolume.Changed.Register(OnSfxChanged);
            gameSettings.BGMVolume.Changed.Register(OnBgmChanged);
        }

        private void OnDisable()
        {
            gameSettings.SFXVolume.Changed.Unregister(OnSfxChanged);
            gameSettings.BGMVolume.Changed.Unregister(OnBgmChanged);
        }

        private void OnSfxChanged(float value) => m_audioSFX.volume = value;

        private void OnBgmChanged(float value) => m_audioBGM.volume = value;

        public void OnGameStateChanged(string state)
        {
            switch (state)
            {
                case GameStates.Playing:
                    m_audioBGM.Stop();
                    m_audioSFX.UnPause();

                    if (!Sfx.BgmPlaying) break;

                    m_audioBGM.clip = Sfx.BgmPlaying;
                    m_audioBGM.Play();

                    break;
                case GameStates.MainMenu:
                    m_audioSFX.Stop();

                    if (!Sfx.BgmMenu) break;

                    if (m_audioBGM.clip != Sfx.BgmMenu)
                    {
                        m_audioBGM.Stop();
                        m_audioBGM.clip = Sfx.BgmMenu;
                        m_audioBGM.Play();
                    }

                    break;
                case GameStates.Paused:
                    m_audioSFX.Pause();

                    break;
            }
        }

        public void OnScoreChanged(int score)
        {
            if (score <= 0) return;

            if (Sfx.Point)
            {
                m_audioSFX.PlayOneShot(Sfx.Point);
            }
        }

        public void UISelect() => m_audioSFX.PlayOneShot(Sfx.UISelect);

        public void UIUnavaiable() => m_audioSFX.PlayOneShot(Sfx.UIUnavailable);
    }
}