using System;
using FlappyBoiz.Scripts.Data;
using UnityEngine;

namespace FlappyBoiz.Scripts.Managers
{
    public class ThemeManager : MonoBehaviour
    {
        [SerializeField]
        private GameSettings gameSettings;

        [SerializeField]
        private GameObject background;

        [SerializeField]
        private GameObject ground;
        
        private ThemeData ActiveTheme => gameSettings.Theme;

        private void Start()
        {
            gameSettings.EquippedTheme.Changed.Register(OnThemeChange);
        }

        private void OnThemeChange(int index)
        {
            background.GetComponent<SpriteRenderer>().sprite =
                gameSettings.DarkModeEnabled ? ActiveTheme.LightBackground : ActiveTheme.DarkBackground;

            ground.GetComponent<SpriteRenderer>().sprite = ActiveTheme.Ground;
        }
    }
}