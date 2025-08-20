using FlappyBoiz.Scripts.Core.Data;
using UnityEngine;

namespace FlappyBoiz.Scripts.Core.Services.SaveManager
{
    public class LocalDataService
    {
        private const string SaveKey = "GameSaveData";

        public void Save(GameSavedData data)
        {
            var json = JsonUtility.ToJson(data);
            PlayerPrefs.SetString(SaveKey, json);
            PlayerPrefs.Save();
        }

        public GameSavedData Load()
        {
            if (!PlayerPrefs.HasKey(SaveKey))
            {
                return new GameSavedData();
            }

            var json = PlayerPrefs.GetString(SaveKey);
            return JsonUtility.FromJson<GameSavedData>(json);
        }
    }
}