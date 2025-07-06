using System;
using System.Collections;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using FlappyBoiz.Scripts.Data;
using FlappyBoiz.Scripts.UI;
using UnityAtoms.BaseAtoms;
using UnityEngine;

namespace FlappyBoiz.Scripts.Managers
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(GameEventInvoker))]
    public class GameDataManager : MonoBehaviour
    {
        public static GameDataManager Instance { get; private set; }

        private readonly WaitForSecondsRealtime m_periodicSaveInterval = new(1f);

        [SerializeField]
        private GameSettings gameSettings;

        [SerializeField]
        private StringEvent gameStateChanged;

        private GameEventInvoker m_gameEventInvoker;

        private IGameDataStore<GameSavedData> m_gameDataStore;
        private GameSavedData m_savedData;
        private bool m_isDirty;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Debug.LogWarning("Multiple game data managers detected!");
                Destroy(gameObject);

                return;
            }

            Instance = this;
            m_gameEventInvoker = GetComponent<GameEventInvoker>();
        }

        private void OnEnable()
        {
            gameStateChanged.Register(OnGameStateChanged);
        }

        private void OnDisable()
        {
            gameStateChanged.Unregister(OnGameStateChanged);
        }

        private void OnGameStateChanged(string newState)
        {
            switch (newState)
            {
                case GameStates.LoadData:
                    Debug.Log("Starting to load data");
                    // Starts loading in the background.
                    LoadInitData().Forget();

                    break;
                case GameStates.GameOver:
                    OnGameOver();

                    break;
            }
        }

        public static string GetLeaderboardId()
        {
            switch (AuthenticationManager.Instance.ActiveSocialPlatform)
            {
                case SocialPlatform.Anonymous:
                    return "Leaderboard01";
                case SocialPlatform.GooglePlayGames:
                    return GPGSIds.leaderboard_classic;
                default:
                    Debug.LogError("Unknown Social Platform");

                    return null;
            }
        }

        private void OnGameOver()
        {
            if (gameSettings.Score.Value > gameSettings.HighScore.Value)
            {
                Debug.Log($"New High Score: {gameSettings.Score.Value}");
                gameSettings.HighScore.Value = gameSettings.Score.Value;

                if (Social.localUser.authenticated)
                {
                    int newHighScore = gameSettings.HighScore.Value;
                    string leaderboardId = GetLeaderboardId();

                    Debug.Log($"Updating leaderboard {leaderboardId} with score {newHighScore}");
                    Social.ReportScore(newHighScore, leaderboardId,
                        success => PopupMessage.Instance.Show(success
                            ? "Leaderboard Updated"
                            : "Failed to update Leaderboard")
                    );
                }
                else
                {
                    Debug.Log("User not authenticated, skipping leaderboard update");
                }
            }

            if (gameSettings.Score.Value <= 0) return;

            int coinsEarned = Mathf.FloorToInt(gameSettings.Score.Value * gameSettings.ConversionRate);
            gameSettings.CoinsEarned.Value = coinsEarned;
            gameSettings.Coins.Add(coinsEarned);
            m_savedData.totalCoinsEarned += coinsEarned;

            Debug.Log($"Coin Balance: {gameSettings.Coins.Value}");
        }

        // TODO: turn into event
        private IEnumerator SaveData()
        {
            yield return m_periodicSaveInterval;

            while (true)
            {
                yield return m_periodicSaveInterval;

                if (!m_isDirty) continue;

                yield return null;

                using (var saveCancellationToken =
                       CancellationTokenSource.CreateLinkedTokenSource(destroyCancellationToken))
                {
                    saveCancellationToken.CancelAfter(60000);

                    m_savedData.coinsBalance = gameSettings.Coins.Value;
                    m_savedData.highScore = gameSettings.HighScore.Value;
                    m_savedData.equippedTheme = gameSettings.EquippedTheme.Value;
                    m_savedData.equippedFlapper = gameSettings.EquippedFlapper.Value;
                    m_savedData.equippedCosmetic = gameSettings.EquippedCosmetic.Value;
                    m_savedData.flappersUnlocked = gameSettings.FlapperUnlocks.List;
                    m_savedData.themesUnlocked = gameSettings.ThemeUnlocks.List;
                    m_savedData.cosmeticsUnlocked = gameSettings.CosmeticUnlocks.List;

                    yield return m_gameDataStore.Save(m_savedData, saveCancellationToken.Token).ToCoroutine(
                        exc => { Debug.LogError(exc.Message); }
                    );

                    m_isDirty = false;
                }

                Debug.Log("Game saved");
            }
        }

        private async UniTaskVoid LoadInitData()
        {
            m_gameDataStore = AuthenticationManager.Instance.ActiveSocialPlatform switch
            {
                SocialPlatform.Anonymous => new PlayerPrefsDataStore(),
                SocialPlatform.GooglePlayGames => new GooglePlayGamesDataStore(),
                _ => throw new NotImplementedException(),
            };

            using var ct = new CancellationTokenSource(30000);

            m_savedData = await m_gameDataStore.Load(ct.Token);

            if (m_savedData.flappersUnlocked.Count == 0)
            {
                m_savedData.flappersUnlocked.Add(0);
            }

            if (m_savedData.themesUnlocked.Count == 0)
            {
                m_savedData.themesUnlocked.Add(0);
            }

            if (m_savedData.equippedFlapper >= gameSettings.Flappers.Count)
            {
                m_savedData.equippedFlapper = 0;
            }

            if (m_savedData.equippedTheme >= gameSettings.Themes.Count)
            {
                m_savedData.equippedTheme = 0;
            }

            gameSettings.HighScore.SetValue(m_savedData.highScore, true);
            gameSettings.Coins.SetValue(m_savedData.coinsBalance, true);
            gameSettings.EquippedTheme.SetValue(m_savedData.equippedTheme, true);
            gameSettings.EquippedFlapper.SetValue(m_savedData.equippedFlapper, true);
            gameSettings.EquippedCosmetic.SetValue(m_savedData.equippedCosmetic, true);

            gameSettings.FlapperUnlocks.Clear();
            gameSettings.FlapperUnlocks.List.AddRange(m_savedData.flappersUnlocked);

            gameSettings.CosmeticUnlocks.Clear();
            gameSettings.CosmeticUnlocks.List.AddRange(m_savedData.cosmeticsUnlocked);

            gameSettings.ThemeUnlocks.Clear();
            gameSettings.ThemeUnlocks.List.AddRange(m_savedData.themesUnlocked);

            ListenToChanges();

            StartCoroutine(SaveData());

            Debug.Log("Game data loaded");

            m_gameEventInvoker.OperationSuccess();
        }

        private void ListenToChanges()
        {
            gameSettings.EquippedTheme.Changed.Register(SetSaveDataDirty);
            gameSettings.EquippedFlapper.Changed.Register(SetSaveDataDirty);
            gameSettings.EquippedCosmetic.Changed.Register(SetSaveDataDirty);
            gameSettings.HighScore.Changed.Register(SetSaveDataDirty);
            gameSettings.Coins.Changed.Register(SetSaveDataDirty);
            gameSettings.FlapperUnlocks.Added.Register(SetSaveDataDirty);
            gameSettings.ThemeUnlocks.Added.Register(SetSaveDataDirty);
            gameSettings.CosmeticUnlocks.Added.Register(SetSaveDataDirty);
        }

        private void SetSaveDataDirty() => m_isDirty = true;
    }
}