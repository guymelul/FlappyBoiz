using FlappyBoiz.Scripts.Data;
using UnityEngine;
using UnityEngine.UI;

namespace FlappyBoiz.Scripts.UI
{
    public class AudioControlView : MonoBehaviour
    {
        [SerializeField]
        private Slider sfxSlider;

        [SerializeField]
        private Slider bgmSlider;

        [SerializeField]
        private GameSettings gameSettings;

        private bool m_discardUpdate;

        private void OnEnable()
        {
            sfxSlider.onValueChanged.AddListener(OnSfxSliderChanged);
            bgmSlider.onValueChanged.AddListener(OnBgmSliderChanged);

            gameSettings.SFXVolume.Changed.Register(OnSfxValueChanged);
            gameSettings.BGMVolume.Changed.Register(OnBgmValueChanged);
        }

        private void Start()
        {
            sfxSlider.SetValueWithoutNotify(gameSettings.SFXVolume.Value * 100);
            bgmSlider.SetValueWithoutNotify(gameSettings.BGMVolume.Value * 100);
        }

        private void OnDisable()
        {
            sfxSlider.onValueChanged.RemoveListener(OnSfxSliderChanged);
            bgmSlider.onValueChanged.RemoveListener(OnBgmSliderChanged);
            
            gameSettings.SFXVolume.Changed.Unregister(OnSfxValueChanged);
            gameSettings.BGMVolume.Changed.Unregister(OnBgmValueChanged);
        }

        private void OnSfxSliderChanged(float value)
        {
            m_discardUpdate = true;
            gameSettings.SFXVolume.Value = value / 100f;
        }

        private void OnSfxValueChanged(float value)
        {
            if (m_discardUpdate)
            {
                m_discardUpdate = false;

                return;
            }

            sfxSlider.SetValueWithoutNotify(gameSettings.SFXVolume.Value * 100);
        }

        private void OnBgmSliderChanged(float value)
        {
            m_discardUpdate = true;
            gameSettings.BGMVolume.Value = value / 100f;
        }

        private void OnBgmValueChanged(float value)
        {
            if (m_discardUpdate)
            {
                m_discardUpdate = false;

                return;
            }

            bgmSlider.SetValueWithoutNotify(gameSettings.BGMVolume.Value * 100);
        }
    }
}