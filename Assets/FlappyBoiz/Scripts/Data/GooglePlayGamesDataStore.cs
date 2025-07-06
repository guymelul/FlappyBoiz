using System;
using System.Text;
using System.Threading;
using Cysharp.Threading.Tasks;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using UnityEngine;

namespace FlappyBoiz.Scripts.Data
{
    public class GooglePlayGamesDataStore : IGameDataStore<GameSavedData>
    {
        private readonly ISavedGameClient m_savedGameClient = PlayGamesPlatform.Instance.SavedGame;
        private static readonly Encoding FileEncoding = Encoding.UTF8;
        private const string FileName = "flappyboiz.json";

        private UniTaskCompletionSource<GameSavedData> m_loadCompletionSource;
        private UniTaskCompletionSource m_saveCompletionSource;
        private CancellationToken m_cancellationToken = CancellationToken.None;
        private GameSavedData gameDataToSave;

        private ISavedGameMetadata m_savedGameMetadata;

        public UniTask Save(GameSavedData gameSavedData, CancellationToken token)
        {
            if (m_savedGameMetadata == null)
            {
                Debug.LogError("Attempted to save before saved game loaded");

                return UniTask.CompletedTask;
            }

            if (gameDataToSave != null)
            {
                Debug.LogError("Game save already in progress");

                return UniTask.FromCanceled(token);
            }

            m_saveCompletionSource = new UniTaskCompletionSource();

            if (!m_savedGameMetadata.IsOpen)
            {
                Debug.Log("Saved game file was closed, reopening to save...");

                gameDataToSave = gameSavedData;

                m_savedGameClient.OpenWithAutomaticConflictResolution(
                    FileName,
                    DataSource.ReadCacheOrNetwork,
                    ConflictResolutionStrategy.UseLongestPlaytime,
                    DoSaveAfterOpen
                );
            }
            else
            {
                Debug.Log("Saved game file is already opened");

                DoSave(gameSavedData);
            }

            return m_saveCompletionSource.Task;
        }

        private void DoSaveAfterOpen(SavedGameRequestStatus status, ISavedGameMetadata game)
        {
            if (status == SavedGameRequestStatus.Success)
            {
                Debug.Log("Game save file opened, saving to it...");

                m_savedGameMetadata = game;

                DoSave(gameDataToSave);
            }
            else
            {
                Debug.LogError($"Failed to open saved game: {status}");
                m_saveCompletionSource.TrySetCanceled(m_cancellationToken);
            }
        }

        private void DoSave(GameSavedData gameSavedData)
        {
            var updatedMetadata = new SavedGameMetadataUpdate.Builder()
                .WithUpdatedPlayedTime(TimeSpan.FromSeconds(gameSavedData.totalCoinsEarned))
                .Build();

            string json = JsonUtility.ToJson(gameSavedData);
            byte[] savedData = FileEncoding.GetBytes(json);

            Debug.Log($"Saving {savedData.Length} bytes to file. contents: {json}");

            m_savedGameClient.CommitUpdate(m_savedGameMetadata, updatedMetadata, savedData, OnSavedGameWritten);
        }

        private void OnSavedGameWritten(SavedGameRequestStatus status, ISavedGameMetadata game)
        {
            gameDataToSave = null;

            if (status == SavedGameRequestStatus.Success)
            {
                Debug.Log("Game was successfully saved in cloud");
                m_saveCompletionSource.TrySetResult();
            }
            else
            {
                m_saveCompletionSource.TrySetException(new Exception("Game failed to save in cloud"));
            }
        }

        public UniTask<GameSavedData> Load(CancellationToken token)
        {
            m_loadCompletionSource = new UniTaskCompletionSource<GameSavedData>();
            m_cancellationToken = token;

            token.ThrowIfCancellationRequested();

            Debug.Log($"Opening game save file {FileName}");

            m_savedGameClient.OpenWithAutomaticConflictResolution(
                FileName,
                DataSource.ReadCacheOrNetwork,
                ConflictResolutionStrategy.UseLongestPlaytime,
                OnSavedGameOpened
            );

            return m_loadCompletionSource.Task;
        }

        private void OnSavedGameOpened(SavedGameRequestStatus status, ISavedGameMetadata game)
        {
            if (status == SavedGameRequestStatus.Success)
            {
                Debug.Log("Game save file opened, reading it...");

                m_cancellationToken.ThrowIfCancellationRequested();
                m_savedGameClient.ReadBinaryData(game, OnSavedGameDataRead);

                m_savedGameMetadata = game;
            }
            else
            {
                Debug.LogError($"Failed to open saved game: {status}");
                m_loadCompletionSource.TrySetCanceled(m_cancellationToken);
            }
        }

        private void OnSavedGameDataRead(SavedGameRequestStatus status, byte[] data)
        {
            if (status == SavedGameRequestStatus.Success)
            {
                int size = data.Length;

                Debug.Log($"Got game save file with {size} bytes");
                GameSavedData savedData;

                if (size > 2)
                {
                    string contents = FileEncoding.GetString(data);

                    Debug.Log($"Save file contents: {contents}");

                    savedData = JsonUtility.FromJson<GameSavedData>(contents);
                }
                else
                {
                    Debug.Log("Got empty JSON creating a new save object");
                    savedData = new GameSavedData();
                }

                // handle processing the byte array data or create new game
                m_loadCompletionSource.TrySetResult(savedData);

                m_cancellationToken = CancellationToken.None;
            }
            else
            {
                Debug.LogError($"Failed to read saved game: {status}");
                m_loadCompletionSource.TrySetCanceled(m_cancellationToken);
            }
        }
    }
}