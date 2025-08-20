using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using FlappyBoiz.Scripts.Core.Data;
using Unity.Services.CloudSave;
using UnityEngine;

namespace FlappyBoiz.Scripts.Core.Services.SaveManager
{
    public class CloudDataService
    {
        public async UniTask<GameSavedData> Load(CancellationToken ct = default)
        {
            var savedData = await CloudSaveService.Instance.Data.Player.LoadAsync(new HashSet<string> { "GameSaveData" });

            if (savedData.TryGetValue("GameSaveData", out var data))
            {
                return JsonUtility.FromJson<GameSavedData>(data.Value.GetAsString());
            }

            return new GameSavedData();
        }

        public async UniTask Save(GameSavedData data, CancellationToken ct = default)
        {
            var dataToSave = new Dictionary<string, object>
            {
                { "GameSaveData", JsonUtility.ToJson(data) }
            };

            await Unity.Services.CloudSave.CloudSaveService.Instance.Data.Player.SaveAsync(dataToSave);
        }
    }
}