using System.Collections.Generic;
using UnityAtoms.BaseAtoms;
using UnityEngine;

namespace FlappyBoiz.Scripts.Data
{
    [CreateAssetMenu(fileName = "gameSettings", menuName = "FlappyBoiz/Game Settings", order = 0)]
    public class GameSettings : ScriptableObject
    {
        #region Game data

        [field: SerializeField]
        [field: Range(0.2f, 2.0f)]
        public float ConversionRate { get; private set; }

        [field: SerializeField] public List<ThemeData> Themes { get; private set; }

        [field: SerializeField] public List<FlapperData> Flappers { get; private set; }

        [field: SerializeField] public List<CosmeticData> Cosmetics { get; private set; }

        #endregion

        # region Player Data

        [field: SerializeField] public IntValueList FlapperUnlocks { private set; get; }

        [field: SerializeField] public IntValueList ThemeUnlocks { private set; get; }

        [field: SerializeField] public IntValueList CosmeticUnlocks { private set; get; }

        [field: SerializeField] public IntVariable HighScore { private set; get; }

        [field: SerializeField] public IntVariable Coins { private set; get; }

        [field: SerializeField] public IntVariable CoinsEarned { private set; get; }

        [field: SerializeField] public IntVariable EquippedTheme { private set; get; }

        [field: SerializeField] public IntVariable EquippedFlapper { private set; get; }

        [field: SerializeField] public IntVariable EquippedCosmetic { private set; get; }

        #endregion

        #region Runtime Data

        [field: SerializeField] public IntVariable Score { private set; get; }

        #endregion

        #region App Settings

        [field: SerializeField] public FloatVariable SFXVolume { private set; get; }

        [field: SerializeField] public FloatVariable BGMVolume { private set; get; }

        #endregion

        public FlapperData Flapper => Flappers[EquippedFlapper.Value];

        public CosmeticData FlapperCosmetic => Cosmetics[EquippedCosmetic.Value];

        public ThemeData Theme => Themes[EquippedTheme.Value];
        public SoundPackData SoundPack => Theme.SoundPack;

        public bool DarkModeEnabled { get; private set; }

        private const string SfxVolume = "SfxVolume";
        private const string BgmVolume = "BgmVolume";

        private void Awake()
        {
            SFXVolume.SetValue(PlayerPrefs.GetFloat(SfxVolume, 0.25f), true);
            BGMVolume.SetValue(PlayerPrefs.GetFloat(BgmVolume, 0.5f), true);
            DarkModeEnabled = false;
        }

        private void OnEnable()
        {
            SFXVolume.Changed.Register(PersistSfxVolume);
            BGMVolume.Changed.Register(PersistBgmVolume);
        }

        private void OnDisable()
        {
            SFXVolume.Changed.Unregister(PersistSfxVolume);
            BGMVolume.Changed.Unregister(PersistBgmVolume);
        }

        private static void PersistSfxVolume(float value) => PlayerPrefs.SetFloat(SfxVolume, value);

        private static void PersistBgmVolume(float value) => PlayerPrefs.SetFloat(BgmVolume, value);

        public bool TryUnlockFlapper(int index)
        {
            if (IsFlapperUnlocked(index)) return false;

            int flapperPrice = Flappers[index].Price;

            if (Coins.Value < flapperPrice) return false;

            Coins.Value -= flapperPrice;
            FlapperUnlocks.Add(index);

            return true;
        }

        public bool IsFlapperUnlocked(int index) => FlapperUnlocks.Contains(index);

        public bool IsFlapperLocked(int index) => !IsFlapperUnlocked(index);

        public void SwitchFlapper(int index)
        {
            if (index < 0 || index >= Flappers.Count)
            {
                Debug.LogError($"Invalid Flapper index {index}");

                return;
            }

            EquippedFlapper.Value = index;
        }

        public void SwitchTheme(int index)
        {
            if (index < 0 || index >= Themes.Count)
            {
                Debug.LogError($"Invalid Theme index {index}");

                return;
            }

            EquippedTheme.Value = index;
        }
    }
}