using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace FlappyBoiz.Scripts.Data
{
    public static class PlayerPrefKeys
    {
        public const string ListSeparator = ",";

        public const string Coins = "Coins";
        public const string TotalCoins = "TotalCoins";
        public const string HighScore = "HighScore";
        public const string FlapperIndex = "FlapperIndex";
        public const string ThemeIndex = "ThemeIndex";
        public const string CosmeticIndex = "CosmeticIndex";
        public const string FlapperUnlocks = "FlapperUnlocks";
        public const string ThemeUnlocks = "ThemeUnlocks";
        public const string CosmeticsUnlocks = "CosmeticsUnlocks";
    }


    public class PlayerPrefsDataStore : IGameDataStore<GameSavedData>
    {
        private static string CollectionToString(ICollection<int> values)
        {
            return string.Join(PlayerPrefKeys.ListSeparator, values);
        }

        private static List<int> StringToCollection(string key)
        {
            return PlayerPrefs.GetString(key, "")
                .Split(PlayerPrefKeys.ListSeparator, StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToList();
        }

        public UniTask Save(GameSavedData gameSavedData, CancellationToken token)
        {
            PlayerPrefs.SetString(PlayerPrefKeys.FlapperUnlocks, CollectionToString(gameSavedData.flappersUnlocked));
            PlayerPrefs.SetString(PlayerPrefKeys.ThemeUnlocks, CollectionToString(gameSavedData.themesUnlocked));
            PlayerPrefs.SetString(PlayerPrefKeys.CosmeticsUnlocks, CollectionToString(gameSavedData.cosmeticsUnlocked));

            PlayerPrefs.SetInt(PlayerPrefKeys.HighScore, gameSavedData.highScore);
            PlayerPrefs.SetInt(PlayerPrefKeys.Coins, gameSavedData.coinsBalance);
            PlayerPrefs.SetInt(PlayerPrefKeys.TotalCoins, gameSavedData.totalCoinsEarned);
            PlayerPrefs.SetInt(PlayerPrefKeys.ThemeIndex, gameSavedData.equippedTheme);
            PlayerPrefs.SetInt(PlayerPrefKeys.FlapperIndex, gameSavedData.equippedFlapper);
            PlayerPrefs.SetInt(PlayerPrefKeys.CosmeticIndex, gameSavedData.equippedCosmetic);

            PlayerPrefs.Save();

            return UniTask.CompletedTask;
        }

        public UniTask<GameSavedData> Load(CancellationToken token)
        {
            var gameData = new GameSavedData
            {
                totalCoinsEarned = PlayerPrefs.GetInt(PlayerPrefKeys.TotalCoins, 0),
                coinsBalance = PlayerPrefs.GetInt(PlayerPrefKeys.Coins, 0),
                highScore = PlayerPrefs.GetInt(PlayerPrefKeys.HighScore, 0),
                equippedTheme = PlayerPrefs.GetInt(PlayerPrefKeys.ThemeIndex, 0),
                equippedFlapper = PlayerPrefs.GetInt(PlayerPrefKeys.FlapperIndex, 0),
                equippedCosmetic = PlayerPrefs.GetInt(PlayerPrefKeys.CosmeticIndex, 0),

                flappersUnlocked = StringToCollection(PlayerPrefKeys.FlapperUnlocks),
                themesUnlocked = StringToCollection(PlayerPrefKeys.ThemeUnlocks),
                cosmeticsUnlocked = StringToCollection(PlayerPrefKeys.CosmeticIndex),
            };

            return UniTask.FromResult(gameData);
        }
    }
}