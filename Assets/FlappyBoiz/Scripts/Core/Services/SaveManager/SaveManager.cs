using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using FlappyBoiz.Scripts.Core.Data;
using UnityEngine.Assertions;

namespace FlappyBoiz.Scripts.Core.Services.SaveManager
{
    public class SaveManager
    {
        private readonly LocalDataService m_localDataService;
        private readonly CloudDataService m_cloudDataService;

        public GameSavedData GameData { get; private set; }

        public SaveManager(LocalDataService localDataService, CloudDataService cloudDataService)
        {
            m_localDataService = localDataService;
            m_cloudDataService = cloudDataService;
        }

        public async UniTask LoadData(CancellationToken ct = default)
        {
            var localData = m_localDataService.Load();
            var cloudData = await m_cloudDataService.Load(ct);

            if (localData.lastUpdated > cloudData.lastUpdated)
            {
                GameData = localData;
                await m_cloudDataService.Save(localData, ct);
            }
            else
            {
                GameData = cloudData;
                m_localDataService.Save(cloudData);
            }
        }

        public async UniTask SaveData(CancellationToken ct = default)
        {
            Assert.IsNotNull(GameData, "GameData is null, should have been loaded at boot time");

            GameData.lastUpdated = DateTime.UtcNow;
            m_localDataService.Save(GameData);
            await m_cloudDataService.Save(GameData, ct);
        }
    }
}